using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class CreateBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private CreateOrderModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public const string MANUAL_DOCUMENT_PREFIX = "RA-";
		public CreateBlanketHandler(CreateOrderModel data, Identity.Models.UserModel user)
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
			botransaction.beginTransaction();
			var maxAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetMaxAngebotNrByTypeAndPrefix(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONTRACT, (int)Core.Common.Enums.INSEnums.INSOrderTypesAngebotNrPrefix.RA);
			//check for double angebotNr souilmi 12-05-2025
			var checkAngebotNrExist = Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.GetByAngebotNr(maxAngebotNr);
			if(checkAngebotNrExist != null && checkAngebotNrExist.Count > 0)
			{
				return ResponseModel<int>.FailureResponse("Another Rahmen is in creation, please try again in a moment .");
			}
			Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.Insert(new Infrastructure.Data.Entities.Tables.CRP.__crp_FertigungsnummerEntity
			{
				angebotNr = maxAngebotNr,
				User = $"{_user.Username}-{_user.Id}",
			});
			lock(Locks.Locks.OrdersLock)
			{
				try
				{
					var insertedNr = -1;
					var orderDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity();
					Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressDb = null;
					if(_data.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale)
					{
						// - 2023-07-21 - set supplier as PSZ for sale RA
						_data.SupplierId = 318;
						//get customer
						var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
						var customerId = customerDb.Nr;
						var customerNummer = customerDb.Nummer;
						adressDb = customerDb.Nummer.HasValue
							? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
							: null;
						var mailBoxIsPreferred = adressDb.Postfach_bevorzugt == true;
						var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
								? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
								: null;
						var lieferadressDb = new Infrastructure.Data.Entities.Tables.PRS.AdressenEntity();
						if(customerDb.LSADR.HasValue && customerDb.LSADR.Value > 0)
							lieferadressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int)customerDb.LSADR);
						else
							lieferadressDb = customerDb.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
						: null;

						var supplierDb = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_data.SupplierId);
						var supplieradrresDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierDb?.Nummer ?? -1);

						orderDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity
						{
							Bezug = string.IsNullOrEmpty(_data.DocumentCustomer) || string.IsNullOrWhiteSpace(_data.DocumentCustomer) ? getUniqueDocumentName(customerNummer ?? -1, Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONTRACT, Enums.BlanketEnums.Types.sale) : _data.DocumentCustomer,
							EDI_Dateiname_CSV = "",
							ABSENDER = adressDb.Name1,
							Kunden_Nr = adressDb.Nr,
							Typ = Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract),
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
							Neu_Order = null,
							Angebot_Nr = maxAngebotNr,
							Projekt_Nr = $"{maxAngebotNr}",
							Erledigt = false
						};

						// >>>>>> Logging
						Infrastructure.Services.Logging.Logger.LogInfo($" OrderImport[Purchase] >>>>>> insert orderDb:{JsonConvert.SerializeObject(orderDb)} ");

						insertedNr = orderDb.Nr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.InsertWithTransaction(orderDb, /*MaxCurrentValue, MinNewValue, Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract),*/ botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateAbIdWithTransaction(orderDb.Nr, orderDb.Nr, botransaction.connection, botransaction.transaction);

						var blanketExtensionDb = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(orderDb.Nr);
						if(blanketExtensionDb == null)
						{
							Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity()
							{
								Id = -1,
								AngeboteNr = orderDb.Nr,
								CreateTime = DateTime.Now,
								CreateUserId = _user.Id,
								CustomerId = _data.CustomerId,
								CustomerName = adressDb.Name1,
								SupplierId = _data.SupplierId,
								SupplierName = supplieradrresDb.Name1,
								Auftraggeber = supplieradrresDb.Name1,
								Warenemfanger = adressDb.Name1,
								//"PSZ electronic GmbH",
								BlanketTypeId = _data.BlanketTypeId,
								BlanketTypeName = ((Enums.BlanketEnums.Types)_data.BlanketTypeId).GetDescription(),
								StatusId = (int)Enums.BlanketEnums.RAStatus.Draft,
								StatusName = Enums.BlanketEnums.RAStatus.Draft.GetDescription()
							}, botransaction.connection, botransaction.transaction);
						}
						else
						{
							blanketExtensionDb.AngeboteNr = orderDb.Nr;
							blanketExtensionDb.CustomerId = _data.CustomerId;
							blanketExtensionDb.SupplierId = _data.SupplierId;
							blanketExtensionDb.Auftraggeber = supplieradrresDb.Name1;
							blanketExtensionDb.Warenemfanger = adressDb.Name1;
							blanketExtensionDb.LastEditUserId = _user.Id;
							blanketExtensionDb.LastEditTime = DateTime.Now;
							Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.UpdateWithTransaction(blanketExtensionDb, botransaction.connection, botransaction.transaction);
						}
					}
					if(_data.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase)
					{
						// - 2022-11-30 - Purchase blanket are stored in Bestellung not Angebote table
						var supplierDb = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_data.SupplierId);
						var supplieradrresDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierDb?.Nummer ?? -1);

						var supplierNummer = supplierDb.Nummer;
						var mailBoxIsPreferred = supplieradrresDb.Postfach_bevorzugt == true;
						var conditionAssignementTableDb = supplierDb.Konditionszuordnungs_Nr.HasValue
								? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(supplierDb.Konditionszuordnungs_Nr.Value)
								: null;
						var conditionAssignementTableDB = supplierDb.Konditionszuordnungs_Nr.HasValue;

						//get customer (PSZ)
						var customerdb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
						var customeradrresDb = customerdb.Nummer.HasValue
							? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerdb.Nummer.Value) : null;

						orderDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity
						{
							Bezug = string.IsNullOrEmpty(_data.DocumentCustomer) || string.IsNullOrWhiteSpace(_data.DocumentCustomer) ? getUniqueDocumentName(supplieradrresDb.Nr, Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONTRACT, Enums.BlanketEnums.Types.purchase) : _data.DocumentCustomer,
							EDI_Dateiname_CSV = "",
							ABSENDER = supplieradrresDb.Name1,
							Kunden_Nr = supplieradrresDb.Nr,
							Typ = Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract),
							Mandant = "PSZ electronic",
							EDI_Order_Neu = false,
							Vorname_NameFirma = supplieradrresDb.Name1,
							Name2 = supplieradrresDb.Name2,
							Name3 = supplieradrresDb.Name3,
							Ansprechpartner = supplieradrresDb.Abteilung,
							Abteilung = supplieradrresDb.Abteilung,
							Straße_Postfach = mailBoxIsPreferred
									   ? $"Postfach {supplieradrresDb.Postfach}"
									   : $"{supplieradrresDb.StraBe}",
							Land_PLZ_Ort = mailBoxIsPreferred
									   ? $"{supplieradrresDb.PLZ_Postfach} {supplieradrresDb.Ort}"
									   : $"{supplieradrresDb.PLZ_StraBe} {supplieradrresDb.Ort} ",
							Versandart = supplierDb.Versandart,
							Zahlungsweise = supplierDb.Zahlungsweise,
							Konditionen = conditionAssignementTableDb?.Text,
							Unser_Zeichen = supplieradrresDb.Kundennummer.HasValue ? supplieradrresDb.Kundennummer.ToString() : "",
							Ihr_Zeichen = supplierDb.Kundennummer_Lieferanten,
							USt_Berechnen = supplierDb.Umsatzsteuer_berechnen,
							Falligkeit = DateTime.Now.AddDays(+30),
							Datum = DateTime.Now,
							Briefanrede = supplieradrresDb.Briefanrede,
							Personal_Nr = _data.Converted.HasValue && _data.Converted.Value ? -1 : 0,
							Freitext = $"USt - ID - Nr.: {supplierDb.EG_Identifikationsnummer}",
							Lieferadresse = "0",
							Reparatur_nr = 0,
							Ab_id = -1,
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
							LAnrede = supplieradrresDb.Anrede,
							LVorname_NameFirma = supplieradrresDb.Name1,
							LName2 = supplieradrresDb.Name2,
							LName3 = supplieradrresDb.Name3,
							LAnsprechpartner = supplieradrresDb.Abteilung,
							LAbteilung = supplieradrresDb.Abteilung,
							LStraße_Postfach = $"{supplieradrresDb.StraBe}",
							LLand_PLZ_Ort = $"{supplieradrresDb.PLZ_StraBe}, {supplieradrresDb.Ort}",
							LBriefanrede = supplieradrresDb.Briefanrede,
							Neu_Order = null,
							Angebot_Nr = maxAngebotNr,
							Projekt_Nr = $"{maxAngebotNr}",
							Erledigt = false,
						};

						// >>>>>> Logging
						Infrastructure.Services.Logging.Logger.Log(level: Infrastructure.Services.Logging.Logger.Levels.Trace, $" OrderImport[Purchase] >>>>>> insert orderDb:{JsonConvert.SerializeObject(orderDb)} ");

						insertedNr = orderDb.Nr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.InsertWithTransaction(orderDb, /*MaxCurrentValue, MinNewValue, Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract),*/ botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateAbIdWithTransaction(orderDb.Nr, orderDb.Nr, botransaction.connection, botransaction.transaction);

						var blanketExtensionDb = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(orderDb.Nr);
						if(blanketExtensionDb == null)
						{
							Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity()
							{
								Id = -1,
								AngeboteNr = orderDb.Nr,
								CreateTime = DateTime.Now,
								CreateUserId = _user.Id,
								SupplierId = _data.SupplierId,
								SupplierName = supplieradrresDb.Name1,
								CustomerId = _data.CustomerId,
								CustomerName = customeradrresDb.Name1,
								Warenemfanger = customeradrresDb.Name1,
								Auftraggeber = supplieradrresDb.Name1,
								//"PSZ electronic GmbH",
								BlanketTypeId = _data.BlanketTypeId,
								BlanketTypeName = ((Enums.BlanketEnums.Types)_data.BlanketTypeId).GetDescription(),
								StatusId = (int)Enums.BlanketEnums.RAStatus.Draft,
								StatusName = Enums.BlanketEnums.RAStatus.Draft.GetDescription()

							}, botransaction.connection, botransaction.transaction);
						}
						else
						{
							blanketExtensionDb.LastEditUserId = _user.Id;
							blanketExtensionDb.LastEditTime = DateTime.Now;
							Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.UpdateWithTransaction(blanketExtensionDb, botransaction.connection, botransaction.transaction);
						}
					}
					if(botransaction.commit())
					{
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
						//Logging
						var rahmenAfterInsert = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(insertedNr);
						var _log = new LogHelper(rahmenAfterInsert.Nr, rahmenAfterInsert.Angebot_Nr ?? -1,
							int.TryParse(rahmenAfterInsert.Projekt_Nr, out var val) ? val : 0, rahmenAfterInsert.Typ, LogHelper.LogType.CREATIONOBJECT, "CTS", _user)
							.LogCTS(null, null, null, 0);
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
						// - 2022-05-04 - allow same DokumentNumber
						var warnings = new List<string> { };
						if(!string.IsNullOrEmpty(_data.DocumentCustomer) && !string.IsNullOrWhiteSpace(_data.DocumentCustomer))
						{
							var _exsistDocument = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTypAndDocumentAndCustomer(
												  Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract),
												  _data.DocumentCustomer, adressDb?.Nr ?? -1);
							if(_exsistDocument > 1)
								warnings.Add($"Document [{_data.DocumentCustomer}] already exists !");
						}
						return new ResponseModel<int>
						{
							Success = true,
							Body = insertedNr,
							Warnings = warnings.Count > 0 ? warnings : null
						};
					}
					else
					{
						Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
						botransaction.rollback();
						return ResponseModel<int>.FailureResponse("Transaction diden't commit .");
					}

				} catch(Exception e)
				{
					Infrastructure.Data.Access.Tables.CRP.__crp_FertigungsnummerAccess.DeleteByAngebotNr(maxAngebotNr);
					botransaction.rollback();
					throw;
				}
			}
		}
		public ResponseModel<int> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			if(_data.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale)
			{
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);

				if(customerDb == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Customer not found");


				var adressDb = customerDb.Nummer.HasValue
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
					: null;

				if(adressDb == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Address not found");
			}
			if(_data.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase)
			{
				var supplierDb = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_data.SupplierId);

				if(supplierDb == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Supplier not found");


				var adressDb = supplierDb.Nummer.HasValue
					? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierDb.Nummer.Value)
					: null;
				if(adressDb == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Address not found");

				if(!string.IsNullOrEmpty(_data.DocumentCustomer) && !string.IsNullOrWhiteSpace(_data.DocumentCustomer))
				{
					//var docNumber = _data.DocumentCustomer?.Trim()?.ToLower();
					//var _exsistDocument = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByTypeAndSupplier(_data.DocumentCustomer, (int)Enums.OrderEnums.Types.Contract, _data.SupplierId);
					//var _existingDocSameNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_exsistDocument?.Select(x => x.AngeboteNr)?.ToList())
					//	?.Where(x => x.Bezug?.Trim()?.ToLower() == docNumber)?.ToList();

					int _existingDocSameNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetBezugByRahmenCount(_data.DocumentCustomer);
					if(_existingDocSameNumber > 0)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Document [{_data.DocumentCustomer}] Already Exist !");
				}

			}

			return ResponseModel<int>.SuccessResponse();
		}
		private static string getUniqueDocumentName(int customerId, string documentType, Enums.BlanketEnums.Types type = Enums.BlanketEnums.Types.purchase)
		{
			var lastId = Infrastructure.Data.Access.Joins.CTS.Divers.GetMaxDocumentNumberRaPurchase(type == Enums.BlanketEnums.Types.purchase);
			return $"{MANUAL_DOCUMENT_PREFIX}{(lastId + 1).ToString("00000")}";
		}
		public static string getNextAngebotNr(Enums.OrderEnums.Types type)
		{
			var maxAngebotNrString = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.MaxAngebotNrByTyp(Enums.OrderEnums.TypeToData(type));
			return $"{Convert.ToInt32(Convert.ToDecimal(maxAngebotNrString)) + 1}";
		}
	}
}