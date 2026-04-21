using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class SendRechnungEmailHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public SendRechnungEmailHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<byte[]> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var INVOICE_ADDRESS = "Invoice@psz-electronic.com";
				var rechung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_data, botransaction.connection, botransaction.transaction);
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
					mailSubject = rechnungCustomer.Betreff.StringIsNullOrEmptyOrWhiteSpaces() ? $"{rechnungConfig.EmailSubject} {rechung.Angebot_Nr}_PO# {rechung.Bezug}" : $"{rechnungCustomer.Betreff} {rechung.Angebot_Nr}_PO# {rechung.Bezug}";
					body = rechnungCustomer.EmailVermerk.StringIsNullOrEmptyOrWhiteSpaces() ? rechnungConfig.EmailBody : rechnungCustomer.EmailVermerk;
					rechnungName = rechnungCustomer.Rechnung_Name;
					receiver = rechnungCustomer.Email.StringIsNullOrEmptyOrWhiteSpaces() ? INVOICE_ADDRESS : rechnungCustomer.Email;
				}
				var fileName = $"Rechnung_{rechnungName}_{DateTime.Now.ToString("yyyyMMDDHHmmss")}.eml";
				var RechnungDocResponse = new GetRechnungReportHandler(
						_user,
						new Models.E_Rechnung.RechnungReportRequestModel
						{
							LanguageId = customerEntity?.Sprache ?? 4,
							TypeId = 7,
							RechnungId = _data
						})
					.Handle(botransaction);
				var attachementPath = Path.Combine(Path.GetTempPath(), $"Rechnung_{rechnungName}_{DateTime.Now.ToString("yyyyMMDDHHmmss")}.pdf");
				File.WriteAllBytes(attachementPath, RechnungDocResponse.Body);
				var response = Helpers.SaveMailHelper.SaveMailMessage(fileName, sender, new List<string> { receiver }, mailSubject, body, attachementPath);
				Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetByRechnungAngobotNr(_data, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<byte[]>.SuccessResponse(response);
				}
				else
				{
					return ResponseModel<byte[]>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

	}
}
