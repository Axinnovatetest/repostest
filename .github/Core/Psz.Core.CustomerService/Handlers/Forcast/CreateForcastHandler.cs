using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.Forcast
{
	public class CreateForcastHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private CreateOrderModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public const string MANUAL_DOCUMENT_PREFIX = "BD-";
		public CreateForcastHandler(Identity.Models.UserModel user, CreateOrderModel data)
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
				//var MaxCurrentValue = Module.CTS.bvMaxCurrentValue;
				//var MinNewValue = Module.CTS.bvMinNewValue;

				var maxAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndPrefix(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST, (int)Core.Common.Enums.INSEnums.INSOrderTypesAngebotNrPrefix.BV);
				//Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndSettingsValues(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST, MaxCurrentValue, MinNewValue);
				//check for double angebotNr souilmi 12-05-2025
				var checkAngebotNrExist = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetByAngebotNr(maxAngebotNr);
				if(checkAngebotNrExist != null && checkAngebotNrExist.Count > 0)
				{
					return ResponseModel<int>.FailureResponse("Another Forecast is in creation, please try again in a moment.");
				}
				lock(Locks.Locks.OrdersLock)
				{
					var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
					try
					{
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.Insert(new Infrastructure.Data.Entities.Tables.CRP.__crp_FertigungsnummerEntity
						{
							angebotNr = maxAngebotNr,
							User = $"{_user.Username}-{_user.Id}",
						});
						var block = Psz.Core.Common.Helpers.blockHelper.GetBlockState().BV;
						if(block)
							return ResponseModel<int>.FailureResponse("Another Forecast is in creation, please try again in a moment.");
						Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.BV, true);

						int response = -1;
						var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
						var customerNummer = customerDb.Nummer;
						var lieferadressDb = new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity();

						if(customerDb.LSADR.HasValue && customerDb.LSADR.Value > 0)
							lieferadressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)customerDb.LSADR);
						else
							lieferadressDb = customerDb.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
						: null;

						var adressDb = customerDb.Nummer.HasValue
							? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
							: null;
						var mailBoxIsPreferred = adressDb?.Postfach_bevorzugt == true;
						var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
								? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
								: null;

						//opening sql transaction
						botransaction.beginTransaction();

						var orderDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity()
						{
							Bezug = string.IsNullOrEmpty(_data.DocumentCustomer) || string.IsNullOrWhiteSpace(_data.DocumentCustomer) ? getUniqueDocumentName(customerNummer ?? -1, Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST) : _data.DocumentCustomer,
							EDI_Dateiname_CSV = "",

							ABSENDER = adressDb.Name1,
							Kunden_Nr = adressDb.Nr,
							Typ = Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.forecast), // "bedarfsvorschau",
							Mandant = "PSZ electronic",
							EDI_Order_Neu = false,

							Vorname_NameFirma = adressDb.Name1,
							Name2 = adressDb.Name2,
							Name3 = adressDb.Name3,

							Ansprechpartner = adressDb.Abteilung,
							Abteilung = adressDb.Abteilung,

							Straße_Postfach = mailBoxIsPreferred
									? $"Postfach {adressDb.Postfach}"
									: $"{adressDb.StraBe}",
							Land_PLZ_Ort = mailBoxIsPreferred
									? $"{adressDb.PLZ_Postfach} {adressDb.Ort}"
									: $"{adressDb.PLZ_StraBe} {adressDb.Ort} ",

							Versandart = customerDb.Versandart,
							Zahlungsweise = customerDb.Zahlungsweise,

							Konditionen = conditionAssignementTableDb?.Text,

							Unser_Zeichen = adressDb.Kundennummer.HasValue ? adressDb.Kundennummer.ToString() : "",
							Ihr_Zeichen = customerDb.Lieferantenummer__Kunden_,
							USt_Berechnen = customerDb.Umsatzsteuer_berechnen,
							Falligkeit = DateTime.Now.AddDays(+30),
							Datum = DateTime.Now,
							Briefanrede = adressDb.Briefanrede,
							Personal_Nr = 0,

							Freitext = $"USt - ID - Nr.: {customerDb.EG___Identifikationsnummer}",

							Lieferadresse = "0",
							Reparatur_nr = 0,
							Ab_id = -1, // update after insert
							Nr_BV = 0,
							Nr_RA = 0,
							Nr_Kanban = 0,
							Nr_auf = 0,
							Nr_lie = 0,
							Nr_rec = 0,
							Nr_pro = 0,
							Nr_gut = 0,
							Nr_sto = 0,
							Belegkreis = 0,
							Wunschtermin = new DateTime(2999, 12, 31),
							Neu = -1,

							LAnrede = lieferadressDb?.Anrede,
							LVorname_NameFirma = lieferadressDb?.Name1,
							LName2 = lieferadressDb?.Name2,
							LName3 = lieferadressDb?.Name3,
							LAnsprechpartner = lieferadressDb?.Abteilung,
							LAbteilung = lieferadressDb?.Abteilung,
							LStraße_Postfach = $"{lieferadressDb?.StraBe}",
							LLand_PLZ_Ort = $"{lieferadressDb?.PLZ_StraBe}, {lieferadressDb?.Ort}".Trim(new char[] { ',', ' ' }),
							LBriefanrede = lieferadressDb?.Briefanrede,
							Neu_Order = null, //(!data.IsManualCreation),

							// souilmi 12-05-2025 new double angebotNr prevention process
							Angebot_Nr = maxAngebotNr,
							Projekt_Nr = $"{maxAngebotNr}",
							Erledigt = false,
							Benutzer = $"Gebucht, {_user.Username}, {DateTime.Now}",
						};

						orderDb.Nr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.InsertWithTransaction(orderDb,/* MaxCurrentValue, MinNewValue, orderDb.Typ,*/
							botransaction.connection, botransaction.transaction);
						response = orderDb.Nr;

						if(botransaction.commit())
						{
							Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.BV, false);
							//Logging
							var _log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr,
								int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.CREATIONOBJECT, "CTS", _user)
								.LogCTS(null, null, null, 0);
							Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

							// - 2022-05-04 - allow same DokumentNumber
							var warnings = new List<string> { };
							if(!string.IsNullOrEmpty(_data.DocumentCustomer) && !string.IsNullOrWhiteSpace(_data.DocumentCustomer))
							{
								var _exsistDocument = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTypAndDocumentAndCustomer(
													  Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.forecast),
													  _data.DocumentCustomer, customerDb.Nummer.Value);
								if(_exsistDocument > 1)
									warnings.Add($"Document [{_data.DocumentCustomer}] already exists !");
							}

							Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
							return new ResponseModel<int>
							{
								Success = true,
								Body = response,
								Warnings = warnings.Count > 0 ? warnings : null
							};
						}
						else
						{
							botransaction.rollback();
							Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.BV, false);
							Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
							return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction error");
						}
					} catch(Exception e)
					{
						botransaction.rollback();
						Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.BV, false);
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
						Infrastructure.Services.Logging.Logger.Log(e);
						throw;
					}
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
			if(customerDb == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Customer not found");


			var adressDb = customerDb.Nummer.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
				: null;
			if(adressDb == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Address not found");

			return ResponseModel<int>.SuccessResponse();
		}
		private static string getUniqueDocumentName(int customerId, string documentType)
		{
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetUniqueByKundenkNr(customerId, documentType, MANUAL_DOCUMENT_PREFIX);
			if(orderDb == null)
			{
				return MANUAL_DOCUMENT_PREFIX + "1";
			}

			// > Extract and increment last Id
			var lastIdDb = orderDb.Bezug.TrimStart(MANUAL_DOCUMENT_PREFIX.ToCharArray());
			if(int.TryParse(lastIdDb, out int lastId))
			{
				return MANUAL_DOCUMENT_PREFIX + (lastId + 1);
			}

			return MANUAL_DOCUMENT_PREFIX + lastIdDb + "1";
		}
	}
}