using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using SendGrid.Helpers.Mail;
	using System.Linq;

	public class PlaceHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Budget.Order.PlaceModel _data { get; set; }
		public PlaceHandler(Models.Budget.Order.PlaceModel validateData, UserModel user)
		{
			_user = user;
			_data = validateData;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
				var orderPlacementEntity = new Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity
				{
					Id = -1,
					OrderId = orderEntity.OrderId,
					OrderPlacedEmailMessage = this._data.EmailBody,
					OrderPlacedReportFileId = -1,
					OrderPlacedEmailTitle = this._data.EmailTitle,
					OrderPlacedSendingEmail = this._data.IssuerEmail,
					OrderPlacedSupplierEmail = this._data.SupplierEmail,
					OrderPlacedTime = DateTime.Now,
					OrderPlacedUserEmail = this._user.Email,
					OrderPlacedUserId = this._user.Id,
					OrderPlacedUserName = this._user.Username,
					SupplierEmail = orderEntity.SupplierEmail,
					SupplierNummer = orderEntity.SupplierNummer,
					OrderPlacementCCEmail = this._data.OrderPlacementCCEmail
				};
				// - update order
				orderEntity.OrderPlacedUserId = this._user.Id;
				orderEntity.OrderPlacedUserName = this._user.Username;
				orderEntity.OrderPlacedTime = DateTime.Now;
				orderEntity.OrderPlacedUserEmail = this._user.Email;
				orderEntity.OrderPlacedSupplierEmail = this._data.SupplierEmail;
				orderEntity.OrderPlacedSendingEmail = this._data.IssuerEmail;
				orderEntity.OrderPlacedReportFileId = -1;
				orderEntity.OrderPlacedEmailMessage = this._data.EmailBody;
				orderEntity.OrderPlacedEmailTitle = this._data.EmailTitle;
				orderEntity.OrderPlacementCCEmail = this._data.OrderPlacementCCEmail;

				var placementFiles = new List<Infrastructure.Data.Entities.Tables.FNC.OrderPlacementFileEntity> { };
				var attachments = new List<KeyValuePair<string, System.IO.Stream>> { };
				var attachmentIds = new List<int> { };

				#region >>>> Order Report <<<<
				try
				{
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity?.CompanyId ?? -1);
					var langId = (companyExtensionEntity?.ReportDefaultLanguageId) ?? 0;
					var report = ReportHandler.generateReportData(this._data.OrderId, langId, true);
					if(report != null)
					{
						var fileName = $"{orderEntity.OrderNumber}.pdf";
						var fileId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(-1, report, ".pdf");
						orderEntity.OrderPlacedReportFileId = fileId;
						placementFiles.Add(new Infrastructure.Data.Entities.Tables.FNC.OrderPlacementFileEntity
						{
							Date = DateTime.Now,
							FileId = fileId,
							FileName = fileName,
							Id = -1,
							OrderId = orderEntity.OrderId,
							OrderPlacementId = -1,
							UserId = this._user.Id,
							UserName = this._user.Username
						});
						attachments.Add(new KeyValuePair<string, System.IO.Stream>(fileName, new System.IO.MemoryStream(report) as System.IO.Stream));
					}
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
				#endregion Order Report 

				#region >>>> Order Other files (attached by user) <<<<
				try
				{
					if(this._data.Files != null && this._data.Files.Count > 0)
					{
						foreach(var fileItem in this._data.Files)
						{
							var fileName = fileItem.actionFile;
							var fileId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(-1, fileItem.DocumentData, ".pdf");
							placementFiles.Add(new Infrastructure.Data.Entities.Tables.FNC.OrderPlacementFileEntity
							{
								Date = DateTime.Now,
								FileId = fileId,
								FileName = fileName,
								Id = -1,
								OrderId = orderEntity.OrderId,
								OrderPlacementId = -1,
								UserId = this._user.Id,
								UserName = this._user.Username
							});
							attachments.Add(new KeyValuePair<string, System.IO.Stream>(fileName, new System.IO.MemoryStream(fileItem.DocumentData)));
							attachmentIds.Add(fileId);
						}
					}

					var attachedFiles = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdOrder(this._data.OrderId);
					if(attachedFiles != null && attachedFiles.Count > 0)
					{
						int n = 1;
						foreach(var fileItem in attachedFiles)
						{
							var file = Psz.Core.Common.Helpers.ImageFileHelper.getFileData(fileItem.FileId);
							if(file != null)
							{
								attachments.Add(new KeyValuePair<string, System.IO.Stream>($"attachment{n}{file.FileExtension}", new System.IO.MemoryStream(file.FileBytes)));
								attachmentIds.Add(fileItem.FileId);
							}
							n++;
						}
					}
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
				#endregion Order Other files

				#region >>>> Save changes <<<<
				Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(orderEntity);
				var insertedId = Infrastructure.Data.Access.Tables.FNC.OrderPlacementAccess.Insert(orderPlacementEntity);
				if(placementFiles.Count > 0)
				{
					for(int i = 0; i < placementFiles.Count; i++)
					{
						placementFiles[i].OrderPlacementId = insertedId;
					}
					Infrastructure.Data.Access.Tables.FNC.OrderPlacementFileAccess.Insert(placementFiles);
				}
				#endregion Save changes


				#region >>> Email notif <<<
				try
				{
					var ccEmailaddresses = new List<string> { };
					// - Always cc coonnected user
					//if (this._data.CcIssuer.HasValue && this._data.CcIssuer.Value)
					ccEmailaddresses.Add(this._user.Email);

					if(!string.IsNullOrWhiteSpace(this._data.OrderPlacementCCEmail))
						ccEmailaddresses.AddRange(this._data.OrderPlacementCCEmail.Split(new char[] { ';', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));

					// - CC Finance Service Guys on Leasing Placement
					if(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
					{
						var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);
						var financeEmployees = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByAccessProfilesIds(new List<int> { companyExtensionEntity?.FinanceProfileId ?? -1 });
						if(financeEmployees != null && financeEmployees.Count > 0)
							ccEmailaddresses.AddRange(financeEmployees.Select(x => x.Email.Trim())?.ToList());
					}
					var sendgridmodeltmeplate = new Infrastructure.Services.Email.GlobalModel()
					{
						TemplateID = "d-a39e1169322849a8baa331da536a10a4"
					};
					var templateData = new
					{
						subject = this._data.EmailTitle,
						firstebody = this._data.EmailBody,
					};

					Module.EmailingService.SendEmailSendGridWithStaticTemplate(this._data.EmailBody, this._data.EmailTitle,
					   this._data.SupplierEmail?.Split(';')?.ToList(),
							attachments,
						ccEmailaddresses,
					   true, this._user.Email, this._user.Username, this._user.Id, this._data.CcIssuer, attachmentIds);

					/*Module.EmailingService.SendEmailSendGridWithDynamicTmeplate(
						 templateData,
						 this._data.EmailTitle,
						 this._data.EmailBody,
						  sendgridmodeltmeplate,
						"houssem.hcini@psz-electronic.com",
						this._data.SupplierEmail?.Split(';')?.ToList(),
						attachments,
						ccEmailaddresses,
						true, this._user.Email, this._user.Username, this._user.Id, this._data.CcIssuer, attachmentIds);*/

					/*Module.EmailingService.SendEmailAsync(this._data.EmailTitle,
						this._data.EmailBody,
						this._data.SupplierEmail?.Split(';')?.ToList(),
						attachments,
						ccEmailaddresses,
						true, this._user.Email, this._user.Username, this._user.Id, this._data.CcIssuer, attachmentIds);*/
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", this._data.SupplierEmail)}]", ex));
				}
				#endregion Email notif 


				// - workflow history
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderEntity, Enums.BudgetEnums.OrderWorkflowActions.Place, this._user, $"Sender:{this._user.Email} | Supplier:{this._data.SupplierEmail}");

				return ResponseModel<int>.SuccessResponse(insertedId);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not found");

			if(orderEntity.Archived.HasValue && orderEntity.Archived.Value)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Order [{orderEntity?.OrderNumber}] is archived");

			var orderArticleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderArticleEntities == null || orderArticleEntities.Count <= 0)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order articles list empty");

			#region >>>> Project <<<<
			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);

			// - External order
			if(orderEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower())
			{
				if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is archived");

				// - Global Director Approval
				if(projectEntity.Id_State != (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active)
				{
					if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject)
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] has been rejected");
					}

					if(projectEntity.ApprovalUserId > 0)
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] approval has been withdrawn");
					}
					else
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is pending approval");
					}
				}

				// - PM Approval
				if(projectEntity.ProjectStatus != (int)Enums.BudgetEnums.ProjectStatuses.Active)
				{
					if(projectEntity.ApprovalUserId > 0)
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is not active");
					}
					else
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is on-hold");
					}
				}
			}
			else // internal project
			{

				if(projectEntity != null && projectEntity.Archived.HasValue && projectEntity.Archived.Value)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is archived");

			}
			#endregion >>> Project

			// - 
			if(string.IsNullOrWhiteSpace(this._data.EmailTitle))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Email title [{this._data.EmailTitle}] is invalid");

			if(string.IsNullOrWhiteSpace(this._data.EmailBody))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Email message [{this._data.EmailBody}] is invalid");

			if(!Infrastructure.Services.Email.Helpers.IsValidEmail(this._data.SupplierEmail))
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Supplier email [{this._data.SupplierEmail}] is invalid");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
