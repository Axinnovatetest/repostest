using Psz.Core.BaseData.Models.Article.BillOfMaterial.BomChangeRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;



namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.BomChangeRequests
{
	public class CreateBomChangeRequestsHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private List<BomChangeRequestsModel> _data { get; set; }
		public CreateBomChangeRequestsHandler(Identity.Models.UserModel user, List<BomChangeRequestsModel> data)
		{

			this._user = user;
			this._data = data;

		}

		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				int insertedId = -1;
				var bomRequestElements = _data.Select(x => x.ToEntity()).ToList();
				botransaction.beginTransaction();
				var insertedtransaction = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.InsertWithTransaction(bomRequestElements, botransaction.connection, botransaction.transaction);

				var accessProfilesValidatorsIds = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get().Where(x => x.ValidateBCR == true).Select(x => x.Id).ToList();

				if(accessProfilesValidatorsIds.Count == 0)
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "There is no validator ");
				}

				List<Infrastructure.Data.Entities.Tables.BSD.AccessProfileUsersEntity> usersValidators = Infrastructure.Data.Access.Tables.BSD.AccessProfileUsersAccess.GetByAccessProfileIds(accessProfilesValidatorsIds).DistinctBy(el => el.UserEmail).ToList();

				List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity> savedBCRIds = null;
				if(this._data.Count > 0)
				{
					savedBCRIds = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.GetAllByWithTransaction(this._data.Select(el => el.Artikel_Nr).ToList(), botransaction.connection, botransaction.transaction);

				}
				/*Send Email to users who has access profile Validate BCR */

				SendEmailNotificationOfCreation(usersValidators, true, savedBCRIds);

				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insertedId);
				}
				else
				{
					botransaction.rollback();
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}


			} catch(Exception ex)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/* || !this._user.Access.MasterData.AddBCR.Value*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();

			}
			return ResponseModel<int>.SuccessResponse();
		}
		public async void SendEmailNotificationOfCreation(List<Infrastructure.Data.Entities.Tables.BSD.AccessProfileUsersEntity> validators, bool toValidator, List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEntity>? savedBCRIds)
		{
			var emailTitle = "BOM Change Request";
			var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:1000px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
			 + $"<br><span style='font-size:1.15em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";
			if(toValidator)
				emailContent += $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just created a BOM change request with the following details :</strong><br/>";
			else
				emailContent += $"<br/><span style='font-size:1.15em;'><strong>This is to confirm that your BOM Change Request has been successfully submitted.<span/></strong><br/>";
			savedBCRIds.ForEach(x =>
		   {
			   string emailLink = $"{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}#/bomchanges?id={x.Id}";
			   emailContent += $@"
						<span style='font-size:1rem;font-weight:bold;'>Details of the Change Request:</span>
                                        -  <strong>Article:</strong> {x.Artikelnummer}
                                        - <strong> Reason:</strong> {x.Reason}
                                        -  <strong>Comments:</strong> {x.Comments}
                                        -  <strong>Submission Date:</strong> {DateTime.Now}
										You can login to check it <a href='{emailLink}'>here</a>
						";
		   });


			//emailContent += "<br/><br/>";
			//emailContent += $"</span><br/><br/>You can login to check it <a href='{emailLink}'>here</a>";

			//emailContent += $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/bomchanges?id={requestId}'>here</a>";
			emailContent += $"<br/><br/>Regards, <br/>IT Department </div>";

			try
			{
				await Module.EmailingService.SendEmailAsync(
					emailTitle,
					emailContent,
					validators.Select(x => x.UserEmail).ToList(), null, null,
				   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);


				List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity> bomChangesRequestsEmailsHistory = new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity>();


				foreach(var validator in validators)
				{
					bomChangesRequestsEmailsHistory.Add(new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity
					{
						Mail_subject = emailTitle,
						Validator_email = validator.UserEmail,
						Validator_id = validator.UserId,
						Status = "Sent",
						Requester_name = _user.Name,
						Requester_email = _user.Email,
						Sending_date = DateTime.Now,
						Requester_id = _user.Id,
					});
				}
				Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryAccess.Insert(bomChangesRequestsEmailsHistory);

			} catch(Exception ex)
			{
				List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity> bcrHistoriesErrors = new List<Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity>();
				foreach(var validator in validators)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", validator.UserEmail)}]"));
					bcrHistoriesErrors.Add(new Infrastructure.Data.Entities.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryEntity
					{
						Mail_subject = emailTitle,
						Validator_email = validator.UserEmail,
						Validator_id = validator.UserId,
						Status = "Not Sent",
						Requester_name = _user.Name,
						Requester_email = _user.Email,
						Sending_date = DateTime.Now,
						Requester_id = _user.Id,
					});
				}
				Infrastructure.Services.Logging.Logger.Log(ex);
				Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsEmailHistoryAccess.Insert(bcrHistoriesErrors);
			}
		}
	}


}
