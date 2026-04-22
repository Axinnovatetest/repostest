using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class UpdateBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Blanket.BlanketModel _data { get; set; }
		public UpdateBlanketHandler(Identity.Models.UserModel user, Models.Blanket.BlanketModel data)
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
				var Blanketdb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Nr);
				if(_data.BlanketTypeId == (int)Enums.BlanketEnums.Types.purchase)
				{
					var supplierDb1 = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_data.SupplierId);
					var supplieradrresDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierDb1?.Nummer ?? -1);
					var supplierNummer = supplierDb1.Nummer;
					var adressDb = supplierDb1.Nummer.HasValue
								? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierDb1.Nummer.Value)
								: null;
					var customerdb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
					var customeradrresDb = customerdb.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerdb.Nummer.Value) : null;
					Blanketdb.Kunden_Nr = supplieradrresDb.Nr;//this._data.SupplierId;
					Blanketdb.Vorname_NameFirma = adressDb.Name1;
					Blanketdb.Freitext = _data.Freitext;


					// -
					var mailBoxIsPreferred = supplieradrresDb.Postfach_bevorzugt == true;
					var conditionAssignementTableDb = supplierDb1.Konditionszuordnungs_Nr.HasValue
							? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(supplierDb1.Konditionszuordnungs_Nr.Value)
							: null;
					Blanketdb.Versandart = supplierDb1.Versandart;
					Blanketdb.Zahlungsweise = supplierDb1.Zahlungsweise;
					Blanketdb.Konditionen = conditionAssignementTableDb?.Text;
					Blanketdb.Ihr_Zeichen = supplierDb1.Kundennummer_Lieferanten;
					Blanketdb.USt_Berechnen = supplierDb1.Umsatzsteuer_berechnen;
					Blanketdb.Freitext = $"USt - ID - Nr.: {supplierDb1.EG_Identifikationsnummer}";

					Blanketdb.ABSENDER = supplieradrresDb.Name1;
					Blanketdb.Kunden_Nr = supplieradrresDb.Nr;
					Blanketdb.Vorname_NameFirma = supplieradrresDb.Name1;
					Blanketdb.Name2 = supplieradrresDb.Name2;
					Blanketdb.Name3 = supplieradrresDb.Name3;
					Blanketdb.Ansprechpartner = supplieradrresDb.Abteilung;
					Blanketdb.Abteilung = supplieradrresDb.Abteilung;
					Blanketdb.Straße_Postfach = mailBoxIsPreferred ? $"Postfach {supplieradrresDb.Postfach}" : $"{supplieradrresDb.StraBe}";
					Blanketdb.Land_PLZ_Ort = mailBoxIsPreferred ? $"{supplieradrresDb.PLZ_Postfach} {supplieradrresDb.Ort}" : $"{supplieradrresDb.PLZ_StraBe} {supplieradrresDb.Ort} ";
					Blanketdb.Unser_Zeichen = supplieradrresDb.Kundennummer.HasValue ? supplieradrresDb.Kundennummer.ToString() : "";
					Blanketdb.Briefanrede = supplieradrresDb.Briefanrede;
					Blanketdb.LAnrede = supplieradrresDb.Anrede;
					Blanketdb.LVorname_NameFirma = supplieradrresDb.Name1;
					Blanketdb.LName2 = supplieradrresDb.Name2;
					Blanketdb.LName3 = supplieradrresDb.Name3;
					Blanketdb.LAnsprechpartner = supplieradrresDb.Abteilung;
					Blanketdb.LAbteilung = supplieradrresDb.Abteilung;
					Blanketdb.LStraße_Postfach = $"{supplieradrresDb.StraBe}";
					Blanketdb.LLand_PLZ_Ort = $"{supplieradrresDb.PLZ_StraBe}, {supplieradrresDb.Ort}";
					Blanketdb.LBriefanrede = supplieradrresDb.Briefanrede;
				}
				else if(_data.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale)
				{
					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
					var customerId = customerDb.Nr;
					var customerNummer = customerDb.Nummer;
					var adressDb = customerDb.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb.Nummer.Value)
						: null;
					var supplieradrresDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(_data.SupplierId);
					var supplierDb2 = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(supplieradrresDb?.Nr ?? -1);
					Blanketdb.Kunden_Nr = adressDb.Nr; //this._data.CustomerId;
					Blanketdb.Vorname_NameFirma = adressDb.Name1;
					Blanketdb.Freitext = _data.Freitext;

					// - 
					var mailBoxIsPreferred = adressDb.Postfach_bevorzugt == true;
					var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
							? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
							: null;
					Blanketdb.Versandart = customerDb.Versandart;
					Blanketdb.Zahlungsweise = customerDb.Zahlungsweise;
					Blanketdb.Konditionen = conditionAssignementTableDb?.Text;
					Blanketdb.Freitext = $"USt - ID - Nr.: {customerDb.EG___Identifikationsnummer}";
					Blanketdb.Ihr_Zeichen = customerDb.Lieferantenummer__Kunden_;
					Blanketdb.USt_Berechnen = customerDb.Umsatzsteuer_berechnen;

					Blanketdb.ABSENDER = adressDb.Name1;
					Blanketdb.Kunden_Nr = adressDb.Nr;
					Blanketdb.Vorname_NameFirma = adressDb.Name1;
					Blanketdb.Name2 = adressDb.Name2;
					Blanketdb.Name3 = adressDb.Name3;
					Blanketdb.Ansprechpartner = adressDb.Abteilung;
					Blanketdb.Abteilung = adressDb.Abteilung;
					Blanketdb.Straße_Postfach = mailBoxIsPreferred ? $"Postfach {adressDb.Postfach}" : $"{adressDb.StraBe}";
					Blanketdb.Land_PLZ_Ort = mailBoxIsPreferred ? $"{adressDb.PLZ_Postfach} {adressDb.Ort}" : $"{adressDb.PLZ_StraBe} {adressDb.Ort} ";
					Blanketdb.Unser_Zeichen = adressDb.Kundennummer.HasValue ? adressDb.Kundennummer.ToString() : "";
					Blanketdb.Briefanrede = adressDb.Briefanrede;
					Blanketdb.LAnrede = adressDb.Anrede;
					Blanketdb.LVorname_NameFirma = adressDb.Name1;
					Blanketdb.LName2 = adressDb.Name2;
					Blanketdb.LName3 = adressDb.Name3;
					Blanketdb.LAnsprechpartner = adressDb.Abteilung;
					Blanketdb.LAbteilung = adressDb.Abteilung;
					Blanketdb.LStraße_Postfach = $"{adressDb.StraBe}";
					Blanketdb.LLand_PLZ_Ort = $"{adressDb.PLZ_StraBe}, {adressDb.Ort}";
					Blanketdb.LBriefanrede = adressDb.Briefanrede;
				}
				var _oldOrder = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Nr);
				if(_oldOrder.Freitext != _data.Freitext)
				{
					LogHelper _log = new LogHelper(_oldOrder.Nr, (int)_oldOrder.Angebot_Nr, int.TryParse(_oldOrder.Projekt_Nr, out var v) ? v : 0, "Rahmenauftrag", LogHelper.LogType.MODIFICATIONBLANKET, "CTS", _user);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log.LogCTS("Freitext", _oldOrder.Freitext, _data.Freitext, 0));
				}
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(Blanketdb);
				var BlanketExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(Blanketdb.Nr);
				var _oldBlanket = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data.Nr);
				var supplierDb = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(_data.SupplierId);
				var adressDb1 = supplierDb.Nummer.HasValue
			  ? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(supplierDb.Nummer.Value)
			  : null;

				var customerDb1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(_data.CustomerId);
				var adressDb2 = customerDb1.Nummer.HasValue
				   ? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customerDb1.Nummer.Value)
				   : null;


				//order extension
				if(BlanketExtension == null)
				{
					Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity()
					{
						Anhage = 0,
						ArchiveTime = DateTime.Now,
						Auftraggeber = adressDb1.Name1,
						Warenemfanger = adressDb2.Name1,
						SupplierId = _data.SupplierId,
						CustomerId = _data.CustomerId
					});
				}
				else
				{
					BlanketExtension.AngeboteNr = _data.Nr;
					BlanketExtension.SupplierId = _data.SupplierId;
					BlanketExtension.CustomerId = _data.CustomerId;
					BlanketExtension.Auftraggeber = adressDb1.Name1;
					BlanketExtension.Warenemfanger = adressDb2.Name1;



					//BlanketExtension.Gesamtpreis = _data.Gesamtpreis; /// - decimal(20,7)
					Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.Update(BlanketExtension);
				}
				Blanketdb.Freitext = _data.Freitext;
				//logging
				var _logs = GetLogs(_oldBlanket, BlanketExtension, _user);
				if(_logs != null && _logs.Count > 0)
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_logs);
				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
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
			var rahmenEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.Nr);
			var rahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data.Nr);
			var rahmenPositionsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data.Nr, false);

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

				// - 2023-11-13 - check if has orders
				var orders = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetbyRahmenPositions(rahmenPositionsEntities?.Select(x => x.Nr)?.ToList());
				if(orders?.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"One or more Rahmen postion(s) are linked to BE, cannot change supplier.");
				}

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

				if(rahmenPositionsEntities != null && rahmenPositionsEntities.Count > 0)
				{
					if(rahmenExtensionEntity.BlanketTypeId.HasValue && rahmenExtensionEntity.BlanketTypeId.Value == (int)Enums.BlanketEnums.Types.sale && _data.CustomerId != rahmenExtensionEntity.CustomerId)
					{
						var abLinkCheck = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRahmenPositions(rahmenPositionsEntities.Select(x => x.Nr).ToList());
						if(abLinkCheck != null && abLinkCheck.Count > 0)
							return ResponseModel<int>.FailureResponse("One or more Rahmen postion(s) are linked to AB, cannot change customer.");
					}
				}

			}
			var BlanketExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(this._data.Nr);
			if(BlanketExtension.StatusId != 0)
			{
				return ResponseModel<int>.FailureResponse("update denied: Rahmen is not draft.");
			}
			return ResponseModel<int>.SuccessResponse();
		}
		public static List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> GetLogs(Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity _old,
 Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity _new, Core.Identity.Models.UserModel user)
		{
			List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity> _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
			var rahmenAngebotEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_old.AngeboteNr);
			LogHelper _log = new LogHelper(_old.AngeboteNr, (int)rahmenAngebotEntity.Angebot_Nr, int.TryParse(rahmenAngebotEntity.Projekt_Nr, out var v) ? v : 0, "Rahmenauftrag", LogHelper.LogType.MODIFICATIONBLANKET, "CTS", user);
			if(_old.Auftraggeber != _new.Auftraggeber)
			{
				_logs.Add(_log.LogCTS("Auftraggeber", _old.Auftraggeber, _new.Auftraggeber, _new.AngeboteNr));
			}
			if(_old.Warenemfanger != _new.Warenemfanger)
			{
				_logs.Add(_log.LogCTS("Warenemfanger", _old.Warenemfanger, _new.Warenemfanger, _new.AngeboteNr));
			}
			if(_old.Gesamtpreis != _new.Gesamtpreis)
			{
				_logs.Add(_log.LogCTS("Gesamtpreis", _old.Gesamtpreis.ToString(), _new.Gesamtpreis.ToString(), _new.AngeboteNr));
			}
			if(_old.CustomerName != _new.CustomerName)
			{
				_logs.Add(_log.LogCTS("CustomerName", _old.CustomerName.ToString(), _new.CustomerName.ToString(), _new.AngeboteNr));
			}
			if(_old.SupplierName != _new.SupplierName)
			{
				_logs.Add(_log.LogCTS("Supplier", _old.SupplierName.ToString(), _new.SupplierName.ToString(), _new.AngeboteNr));
			}
			if(_old.StatusName != _new.StatusName)
			{
				_logs.Add(_log.LogCTS("Status", _old.StatusName.ToString(), _new.StatusName.ToString(), _new.AngeboteNr));
			}
			return _logs;
		}
	}
}