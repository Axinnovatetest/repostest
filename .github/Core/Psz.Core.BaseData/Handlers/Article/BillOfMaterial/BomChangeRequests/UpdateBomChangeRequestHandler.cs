using Psz.Core.BaseData.Models.Article.BillOfMaterial.BomChangeRequests;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.BomChangeRequests
{
	public class UpdateBomChangeRequestHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly ChangeRequestStatusRequestmodel _data;

		public UpdateBomChangeRequestHandler(Identity.Models.UserModel user, ChangeRequestStatusRequestmodel data)
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
			/// Transaction-based
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var BomChangeRequestEntity = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.GetWithTransaction(_data.Idrequest, botransaction.connection, botransaction.transaction);
				BomChangeRequestEntity.Status = ((Enums.BomChangeEnums.Status)_data.IdStatus).GetDescription();
				//var validatorUser = Infrastructure.Data.Access.Tables.COR.UserAccess.GetWithTransaction(BomChangeRequestEntity.Validator_id, botransaction.connection, botransaction.transaction);

				var accessProfilesValidatorsIds = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get().Where(x => x.ValidateBCR == true).Select(x => x.Id).ToList();

				List<Infrastructure.Data.Entities.Tables.BSD.AccessProfileUsersEntity> usersValidators = Infrastructure.Data.Access.Tables.BSD.AccessProfileUsersAccess.GetByAccessProfileIds(accessProfilesValidatorsIds).ToList();

				List<string> validatorsEmails = usersValidators.Select(x => x.UserEmail).ToList();
				if(_data.IdStatus == (int)Enums.BomChangeEnums.Status.Accepted)
				{
					BomChangeRequestEntity.AcceptanceDate = DateTime.Now;
					SendEmailNotification(botransaction, usersValidators, BomChangeRequestEntity.Id, true, "Accepted");
					//SendEmailNotification(botransaction, BomChangeRequestEntity.Requester_email, BomChangeRequestEntity.Id, false, "Accepted");

				}
				else if(_data.IdStatus == (int)Enums.BomChangeEnums.Status.Rejected)
				{
					BomChangeRequestEntity.RejectionReason = _data.Reason;
					BomChangeRequestEntity.RejectionDate = DateTime.Now;
					SendEmailNotification(botransaction, usersValidators, BomChangeRequestEntity.Id, true, "Rejected", _data.Reason);
					//SendEmailNotification(botransaction, BomChangeRequestEntity.Requester_email, BomChangeRequestEntity.Id, false, "Rejected", _data.Reason);

				}
				var response = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.UpdateWithTransaction(BomChangeRequestEntity, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null /*|| !this._user.Access.MasterData.ValidateBCR.Value*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();

			}
			return ResponseModel<int>.SuccessResponse();
		}

		private async void SendEmailNotification(Infrastructure.Services.Utils.TransactionsManager botransaction, List<Infrastructure.Data.Entities.Tables.BSD.AccessProfileUsersEntity> validators, int requestId, bool toValidator, string status, string rejectionReason = "")
		{
			string emailTitle = $"BOM Change Request {status}";
			string greeting = DateTime.Now.Hour <= 12 ? "morning" : "afternoon";
			string currentDate = DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"));
			string statusMessage = status == "Accepted" ? "has been accepted" : "has been rejected";

			var BomChangeRequestEntity = Infrastructure.Data.Access.Tables.BSD.BomChangeRequests.__BSD_BomChangesRequestsAccess.GetWithTransaction(_data.Idrequest, botransaction.connection, botransaction.transaction);

			string emailContent = $@"
            <div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:1000px;'>
                {currentDate}<br/>
                <span style='font-size:1.15em;'>Good {greeting},</span><br/>";

			if(toValidator)
			{
				emailContent += $@"
                <br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has just {statusMessage} a BOM change request with the following details:</strong><br/>";
			}
			else
			{
				emailContent += $@"
                <br/><span style='font-size:1.15em;'>This is to notify you that your BOM Change Request {statusMessage}.</span><br/>";
			}

			emailContent += $@"
               
                        - <strong>Article:</strong> {BomChangeRequestEntity.Artikelnummer}
                        - <strong>Reason:</strong> {BomChangeRequestEntity.Reason}
                        - <strong>Comments:</strong> {BomChangeRequestEntity.Comments}
                        - <strong>{status} Date:</strong> {currentDate}";

			if(status == "Rejected")
			{
				emailContent += $"<br/>- <strong>Reason for Rejection:</strong> {rejectionReason}";
			}

			emailContent += $@"
                <br/><br/>
                You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}#/bomchanges?id={requestId}'>here</a>
                <br/><br/>Regards, <br/>IT Department </div>";

			try
			{
				await Module.EmailingService.SendEmailAsync(
					emailTitle,
					emailContent,
					validators.Select(el => el.UserEmail).ToList(), null, null,
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
