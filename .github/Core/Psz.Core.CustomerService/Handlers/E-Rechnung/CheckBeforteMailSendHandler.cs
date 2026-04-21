using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class CheckBeforteMailSendHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CheckBeforteMailSendHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
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

				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				try
				{
					botransaction.beginTransaction();

					#region // -- transaction-based logic -- //
					//DONE: - insert process here

					var rechung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_data, botransaction.connection, botransaction.transaction);
					var rechnungCreated = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetByRechnungNr(_data, botransaction.connection, botransaction.transaction);
					var rechnungCustomer = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.GetByKundennummer(rechung.Kunden_Nr ?? -1, botransaction.connection, botransaction.transaction);
					var rechnungConfig = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_ConfigAccess.GetWithTransaction(botransaction.connection, botransaction.transaction)?[0];

					if(rechnungCreated == null)
					{
						var ls = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(rechung.Nr_lie ?? -1, botransaction.connection, botransaction.transaction);
						var invoiceType = Infrastructure.Data.Access.Joins.CTS.Divers.GetInvoiceType(rechung.Kunden_Nr ?? -1, botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.__E_rechnung_CreatedEntity
						{
							CreationTime = DateTime.Now,
							CustomerNr = rechung.Kunden_Nr,
							CustomerName = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(rechung.Kunden_Nr ?? -1)?.Name1,
							CustomerRechnungType = ((Enums.E_RechnungEnums.RechnungTyp)invoiceType).GetDescription(),
							LsAngebotNr = ls?.Angebot_Nr,
							LSNr = rechung.Nr_lie,
							RechnungForfallNr = rechung.Angebot_Nr,
							RechnungNr = rechung.Nr,
							RechnungProjectNr = int.TryParse(rechung.Projekt_Nr, out var s) ? s : 0,
						}, botransaction.connection, botransaction.transaction);
						rechnungCreated = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetByRechnungNr(_data, botransaction.connection, botransaction.transaction);
					}

					if(rechnungCreated != null)
					{
						rechnungCreated.SentTime = DateTime.Now;
						Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.UpdateWithTransaction(rechnungCreated, botransaction.connection, botransaction.transaction);
						//in case of sammel rechnung 
						if(rechnungCreated.CustomerRechnungType == Enums.E_RechnungEnums.RechnungTyp.Sammelrechnung.GetDescription())
						{
							var created = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetByRechnungAngobotNr(rechnungCreated.RechnungForfallNr ?? -1, botransaction.connection, botransaction.transaction);
							var angebotEntites = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByAngebotNr(rechnungCreated.RechnungForfallNr.ToString(), botransaction.connection, botransaction.transaction);

							created.ForEach(x => x.SentTime = DateTime.Now);
							angebotEntites?.ForEach(x => { x.rec_sent = true; x.Gebucht = true; });
							Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.UpdateWithTransaction(created, botransaction.connection, botransaction.transaction);
							Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(angebotEntites, botransaction.connection, botransaction.transaction);
						}
					}

					rechung.rec_sent = true;
					rechung.Gebucht = true;
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(rechung, botransaction.connection, botransaction.transaction);

					//logging 
					var _log = new LogHelper(rechung.Nr, rechung.Angebot_Nr ?? -1,
								int.TryParse(rechung.Projekt_Nr, out var val) ? val : 0, rechung.Typ, LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user)
								.LogCTS("Sent", "false", "true", 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);

					#endregion

					//DONE: handle transaction state (success or failure)
					if(botransaction.commit())
					{
						return ResponseModel<int>.SuccessResponse(1);
					}
					else
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
					}
				} catch(Exception e)
				{
					botransaction.rollback();
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}

				// -
				//return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var rechung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(rechung == null)
				return ResponseModel<int>.FailureResponse("Rechung not found .");
			return ResponseModel<int>.SuccessResponse();
		}

	}
}
