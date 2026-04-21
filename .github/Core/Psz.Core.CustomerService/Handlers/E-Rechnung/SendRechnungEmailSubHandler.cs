using System;
using System.Collections.Generic;
using Psz.Core.Common.Models;
using System.IO;
using Psz.Core.SharedKernel.Interfaces;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class SendRechnungEmailSubHandler: IHandleAsync<Identity.Models.UserModel, ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public SendRechnungEmailSubHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public async Task<ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>> HandleAsync()
		{
			var result = new Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel();
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var validationResponse = await this.ValidateAsync();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var INVOICE_ADDRESS = Module.CTS.InvoiceSenderEmail;
				var rechung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_data, botransaction.connection, botransaction.transaction);
				var rechungAmount = Infrastructure.Data.Access.Joins.CTS.Divers.GetInvoiceAmount(rechung?.Angebot_Nr ?? -1, botransaction.connection, botransaction.transaction);

				if(rechungAmount == 0)
				{
					result.Subject = "";
					return await ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>.SuccessResponseAsync(result);
				}
				var rechnungCustomer = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.GetByKundennummer(rechung.Kunden_Nr ?? -1, botransaction.connection, botransaction.transaction);
				var rechnungConfig = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_ConfigAccess.GetWithTransaction(botransaction.connection, botransaction.transaction)?[0];
				var customerEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(rechung?.Kunden_Nr ?? -1, botransaction.connection, botransaction.transaction);
				var sender = INVOICE_ADDRESS;
				var receiver = INVOICE_ADDRESS;
				var mailSubject = rechnungConfig.EmailSubject;
				var body = rechnungConfig.EmailBody;
				var rechnungName = rechung.Vorname_NameFirma;
				if(rechnungCustomer != null)
				{
					mailSubject = rechnungCustomer.Betreff.StringIsNullOrEmptyOrWhiteSpaces() ? $"{rechnungConfig.EmailSubject} {rechung.Angebot_Nr}_PO#{rechung.Bezug}" : $"{rechnungCustomer.Betreff} {rechung.Angebot_Nr}_PO#{rechung.Bezug}";
					body = rechnungCustomer.EmailVermerk.StringIsNullOrEmptyOrWhiteSpaces() ? rechnungConfig.EmailBody : rechnungCustomer.EmailVermerk;
					rechnungName = rechnungCustomer.Rechnung_Name;
					receiver = rechnungCustomer.Email.StringIsNullOrEmptyOrWhiteSpaces() ? INVOICE_ADDRESS : rechnungCustomer.Email;
				}
				//var fileName = $"Rechnung_{rechnungName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.eml";
				var RechnungDocResponse = new GetRechnungReportHandler(
						_user,
						new Models.E_Rechnung.RechnungReportRequestModel
						{
							LanguageId = customerEntity?.Sprache ?? 4,
							TypeId = 7,
							RechnungId = _data
						})
					.Handle(botransaction);
				// -
				List<KeyValuePair<string, Stream>> attachments = new List<KeyValuePair<string, System.IO.Stream>> { };
				attachments.Add(new KeyValuePair<string, System.IO.Stream>($"Rechnung_{rechnungName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf", new System.IO.MemoryStream(RechnungDocResponse.Body) as System.IO.Stream));

				// - 
				result.Sent = (await Module.EmailingService.SendEmailSendGridWithStaticTemplate(
				   content: body,
				   subject: mailSubject,
				   toAddresses: new List<string> { receiver },
				   attachments: (attachments.Count > 0) ? attachments : null,
				   toAddressesCC: new List<string> { sender },
				   true,
				   senderEmail: sender,
				   this._user.Username,
				   senderId: this._user.Id,
				   senderCC: false,
				   attachmentIds: null
				  )).Item1;
				result.Subject = mailSubject;

				//var response = Helpers.SaveMailHelper.SaveMailMessage(fileName, sender, new List<string> { receiver }, mailSubject, body, attachementPath);
				Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetByRechnungAngobotNr(_data, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					//updating sent status and timestamp
					if(result.Sent)
					{
						var sentUpdate = new Psz.Core.CustomerService.Handlers.E_Rechnung.CheckBeforteMailSendHandler(_user, _data).Handle();
					}
					return await ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>.SuccessResponseAsync(result);
				}
				else
				{
					return await ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>.FailureResponseAsync($"Email sent ?  : {result} , " + "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				return await ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>.SuccessResponseAsync(result);
			}
		}
		public async Task<ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>> ValidateAsync()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return await ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>.AccessDeniedResponseAsync();
			}

			return await ResponseModel<Psz.Core.CustomerService.Models.E_Rechnung.SendRechnungEmailModel>.SuccessResponseAsync();
		}

	}
}
