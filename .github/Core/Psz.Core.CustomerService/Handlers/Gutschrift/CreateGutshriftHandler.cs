using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class CreateGutshriftHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private GutshriftCreateModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public const string MANUAL_DOCUMENT_PREFIX = "GS-";
		public CreateGutshriftHandler(Identity.Models.UserModel user, GutshriftCreateModel data)
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
			//var MaxCurrentValue = Module.CTS.gsMaxCurrentValue;
			//var MinNewValue = Module.CTS.gsMinNewValue;

			var maxAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndPrefix(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CREDIT, (int)Core.Common.Enums.INSEnums.INSOrderTypesAngebotNrPrefix.GS);
			//Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndSettingsValues(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CREDIT, MaxCurrentValue, MinNewValue);
			var checkAngebotNrExist = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetByAngebotNr(maxAngebotNr);
			if(checkAngebotNrExist != null && checkAngebotNrExist.Count > 0)
			{
				return ResponseModel<int>.FailureResponse("Another Gutshrift is in creation, please try again in a moment .");
			}
			Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.Insert(new Infrastructure.Data.Entities.Tables.CRP.__crp_FertigungsnummerEntity
			{
				angebotNr = maxAngebotNr,
				User = $"{_user.Username}-{_user.Id}",
			});
			lock(Locks.Locks.OrdersLock)
			{
				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				try
				{
					int response = -1;
					var warnings = new List<string>();
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
					var rechungEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.RechnungId);
					var rechnungItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data.RechnungId);
					if(rechnungItemsEntities == null || rechnungItemsEntities.Count == 0)
						warnings.Add($"chosen rechnung [{rechungEntity.Angebot_Nr}] has no positions .");
					botransaction.beginTransaction();
					var creditDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity()
					{
						Bezug = string.IsNullOrEmpty(_data.DocumentCustomer) || string.IsNullOrWhiteSpace(_data.DocumentCustomer) ? getUniqueDocumentName(customerNummer ?? -1, Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Credit)) : _data.DocumentCustomer,
						EDI_Dateiname_CSV = "",

						ABSENDER = rechungEntity?.Vorname_NameFirma,
						Kunden_Nr = adressDb.Nr,
						Typ = Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Credit), // "Gutshrift",
						Mandant = "PSZ electronic",
						EDI_Order_Neu = false,

						Vorname_NameFirma = rechungEntity?.Vorname_NameFirma,
						Name2 = rechungEntity?.Name2,
						Name3 = rechungEntity?.Name3,

						Ansprechpartner = rechungEntity?.Ansprechpartner,
						Abteilung = rechungEntity?.Abteilung,

						Straße_Postfach = rechungEntity?.Straße_Postfach,
						Land_PLZ_Ort = rechungEntity?.Land_PLZ_Ort,

						Versandart = customerDb.Versandart,
						Zahlungsweise = customerDb.Zahlungsweise,

						Konditionen = conditionAssignementTableDb?.Text,

						Unser_Zeichen = rechungEntity?.Unser_Zeichen,
						Ihr_Zeichen = rechungEntity?.Ihr_Zeichen,
						USt_Berechnen = rechungEntity?.USt_Berechnen,
						Falligkeit = DateTime.Now.AddDays(+30),
						Datum = DateTime.Now,
						Briefanrede = rechungEntity?.Briefanrede,
						Personal_Nr = 0,

						Freitext = rechungEntity?.Freitext,

						Lieferadresse = "0",
						Reparatur_nr = 0,
						Ab_id = -1, // update after insert
						Nr_BV = 0,
						Nr_RA = 0,
						Nr_Kanban = 0,
						Nr_auf = 0,
						Nr_lie = 0,
						Nr_rec = _data.RechnungId,
						Nr_pro = 0,
						Nr_gut = 0,
						Nr_sto = 0,
						Belegkreis = 0,
						Wunschtermin = new DateTime(2999, 12, 31),
						Neu = -1,

						LAnrede = rechungEntity?.LAnrede,
						LVorname_NameFirma = rechungEntity?.LVorname_NameFirma,
						LName2 = rechungEntity?.LName2,
						LName3 = rechungEntity?.LName3,
						LAnsprechpartner = rechungEntity?.LAnsprechpartner,
						LAbteilung = rechungEntity?.LAbteilung,
						LStraße_Postfach = rechungEntity?.LStraße_Postfach,
						LLand_PLZ_Ort = rechungEntity?.LLand_PLZ_Ort,
						LBriefanrede = rechungEntity?.LBriefanrede,
						Neu_Order = null, //(!data.IsManualCreation),

						// souilmi 12-05-2025 new double angebotNr prevention process
						Angebot_Nr = maxAngebotNr,
						Projekt_Nr = rechungEntity.Projekt_Nr,
						Erledigt = false,
						Benutzer = $"Gebucht, {_user.Username}, {DateTime.Now}",
					};

					creditDb.Nr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.InsertWithTransaction(creditDb, /*MaxCurrentValue, MinNewValue, creditDb.Typ,*/
						botransaction.connection, botransaction.transaction);
					response = creditDb.Nr;
					var creditDbAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(response, botransaction.connection, botransaction.transaction);
					//creditDbAfterInsert.Projekt_Nr = rechungEntity.Projekt_Nr;
					//Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(creditDbAfterInsert, botransaction.connection, botransaction.transaction);
					if(botransaction.commit())
					{
						//Logging
						var _log = new LogHelper(creditDbAfterInsert.Nr, (int)creditDbAfterInsert.Angebot_Nr,
							int.TryParse(creditDbAfterInsert.Projekt_Nr, out var val) ? val : 0, creditDbAfterInsert.Typ, LogHelper.LogType.CREATIONOBJECT, "CTS", _user)
							.LogCTS(null, null, null, 0);
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

						// - 2022-05-04 - allow same DokumentNumber
						if(!string.IsNullOrEmpty(_data.DocumentCustomer) && !string.IsNullOrWhiteSpace(_data.DocumentCustomer))
						{
							var _exsistDocument = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTypAndDocumentAndCustomer(
												  Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Credit),
												  _data.DocumentCustomer, customerDb.Nummer.Value);
							if(_exsistDocument > 1)
								warnings.Add($"Document [{_data.DocumentCustomer}] already exists !");
						}
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
						return new ResponseModel<int> { Warnings = warnings, Body = response, Success = true, Errors = null };
					}
					else
					{
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
						botransaction.rollback();
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Transaction error");
					}
				} catch(Exception e)
				{
					botransaction.rollback();
					Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}

			//return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
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

			var rechnungEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.RechnungId);
			if(rechnungEntity == null || (rechnungEntity != null && rechnungEntity.Typ != Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Invoice)))
				return ResponseModel<int>.FailureResponse("Rechnung not found .");
			if(string.IsNullOrEmpty(rechnungEntity.Projekt_Nr) || string.IsNullOrWhiteSpace(rechnungEntity.Projekt_Nr))
				return ResponseModel<int>.FailureResponse("Rechnung is not validated .");
			return ResponseModel<int>.SuccessResponse();
		}
		private static string getUniqueDocumentName(int customerId, string typ)
		{
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetUniqueByKundenkNr(customerId, typ);
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
		public static string getNextAngebotNr(Enums.OrderEnums.Types type)
		{
			var maxAngebotNrString = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.MaxAngebotNrByTyp(Enums.OrderEnums.TypeToData(type));
			return $"{Convert.ToInt32(Convert.ToDecimal(maxAngebotNrString)) + 1}";
		}
	}
}