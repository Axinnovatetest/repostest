using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	public class ResendPlacementmails_TempHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<List<int>>>
	{
		public UserModel _user { get; }
		public ResendPlacementmails_TempHandler(Identity.Models.UserModel user)
		{
			_user = user;
		}
		public async Task<ResponseModel<List<int>>> HandleAsync()
		{
			var validateResponse = await this.ValidateAsync();
			if(!validateResponse.Success)
			{
				return validateResponse;
			}

			try
			{
				var sentWithoutAttachment = Infrastructure.Data.Access.Tables.FNC.OrderPlacementAccess.GetSentWithoutAttachment_temp();
				var response = new List<int>();
				if(sentWithoutAttachment != null && sentWithoutAttachment.Count > 0)
				{
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity?.CompanyId ?? -1);
					var langId = (companyExtensionEntity?.ReportDefaultLanguageId) ?? 0;

					foreach(var item in sentWithoutAttachment)
					{
						//prep report
						var report = ReportHandler.generateReportData(item.OrderId, langId, true);
						var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(item.OrderId);
						List<KeyValuePair<string, System.IO.Stream>> attachments = new List<KeyValuePair<string, System.IO.Stream>> { };
						var attachmentIds = new List<int> { };

						if(report != null)
						{
							var fileName = $"{orderEntity.OrderNumber}.pdf";
							var fileId = Psz.Core.Common.Helpers.ImageFileHelper.updateImage(-1, report, ".pdf");
							orderEntity.OrderPlacedReportFileId = fileId;
							attachments.Add(new KeyValuePair<string, System.IO.Stream>(fileName, new System.IO.MemoryStream(report) as System.IO.Stream));
						}
						var attachedFiles = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdOrder(item.OrderId);
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
						//send mail
						var ccEmailaddresses = new List<string> { };
						// - Always cc coonnected user
						//if (this._data.CcIssuer.HasValue && this._data.CcIssuer.Value)
						ccEmailaddresses.Add(this._user.Email);

						if(!string.IsNullOrWhiteSpace(item.OrderPlacementCCEmail))
							ccEmailaddresses.AddRange(item.OrderPlacementCCEmail.Split(new char[] { ';', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));

						// - CC Finance Service Guys on Leasing Placement
						if(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
						{
							var companyExtensionEntityOrder = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);
							var financeEmployees = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByAccessProfilesIds(new List<int> { companyExtensionEntityOrder?.FinanceProfileId ?? -1 });
							if(financeEmployees != null && financeEmployees.Count > 0)
								ccEmailaddresses.AddRange(financeEmployees.Select(x => x.Email.Trim())?.ToList());
						}
						var sendgridmodeltmeplate = new Infrastructure.Services.Email.GlobalModel()
						{
							TemplateID = "d-a39e1169322849a8baa331da536a10a4"
						};
						var templateData = new
						{
							subject = item.OrderPlacedEmailTitle,
							firstebody = item.OrderPlacedEmailMessage,
						};

						await Module.EmailingService.SendEmailSendGridWithStaticTemplate(item.OrderPlacedEmailMessage, item.OrderPlacedEmailTitle,
							   item.SupplierEmail?.Split(';')?.ToList(),
									attachments,
								ccEmailaddresses,
							   true, this._user.Email, this._user.Username, this._user.Id, true, attachmentIds);
						response.Add(item.OrderId);
					}
				}
				return await ResponseModel<List<int>>.SuccessResponseAsync(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public async Task<ResponseModel<List<int>>> ValidateAsync()
		{
			if(_user == null/*|| this._user.Access.____*/)
			{
				return await ResponseModel<List<int>>.AccessDeniedResponseAsync();
			}
			return await ResponseModel<List<int>>.SuccessResponseAsync();
		}
	}
}
