using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.CustomerService.Helpers;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class AddOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Delfor.Production.ValidateCommandModel _data { get; set; }
		public AddOrderHandler(Identity.Models.UserModel user, Models.Delfor.Production.ValidateCommandModel command)
		{
			this._user = user;
			this._data = command;
		}
		public ResponseModel<int> Handle()
		{
			lock(Locks.DLF_ProductionLock)
			{
				try
				{
					Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.AddOrder(this._data.PositionId, 11, 12, this._user.Id, DateTime.Now);
					return ResponseModel<int>.SuccessResponse(10);

					//var validationResponse = this.Validate();
					//if(!validationResponse.Success)
					//{
					//	return validationResponse;
					//}


					//var validationErrors = new List<ResponseModel<int>.ResponseError>();

					//var orderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data.PositionId);
					//var orderEntity = orderItemEntity.AngebotNr.HasValue
					//	? Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderItemEntity.AngebotNr.Value)
					//	: null;
					//var itemEntity = orderItemEntity.ArtikelNr.HasValue
					//	? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntity.ArtikelNr.Value)
					//	: null;
					//var URSArticleEntiy = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.OriginalArticleId ?? -1);
					//var priceGroupEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr((int)orderItemEntity.ArtikelNr);

					//var priceType = getPriceType(priceGroupEntity, Convert.ToDecimal(orderItemEntity.Anzahl ?? 0));

					//Infrastructure.Services.Logging.Logger.LogTrace("itemEntity.ArtikelNr: " + itemEntity.ArtikelNr);
					//Infrastructure.Services.Logging.Logger.LogTrace("priceType: " + priceType);

					//var staffPriceEntity = Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.GetByArtikelNrAndType(itemEntity.ArtikelNr, priceType);

					//var stucklistenEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(itemEntity.ArtikelNr);
					//if(stucklistenEntities.Count == 0)
					//{
					//	validationErrors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Stock list not found" });
					//}

					//if(validationErrors.Count > 0)
					//{
					//	return new ResponseModel<int>() { Errors = validationErrors };
					//}

					//if(validationErrors.Count > 0)
					//{
					//	return new ResponseModel<int>() { Errors = validationErrors };
					//}

					//var itemCalculatoryCostsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetByArtikelNr(itemEntity.ArtikelNr);
					//var storageLocationEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(this._data.ManufacturingFacilityId/* (int)orderItemEntity.Lagerort_id*/);
					//var nextFertigungsnummer = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetMaxFertigungsnummer("PSZ Electronic"/*orderEntity.Mandant*/)
					//	+ 1;

					//var completionDeadlineDate = orderItemEntity.Liefertermin.HasValue ? orderItemEntity.Liefertermin.Value.AddDays(-3) : new DateTime(1970, 1, 1);
					//var appointmentConfirmedDate1 = this._data.ProductionDate;

					//var notes = $"{storageLocationEntity?.Lagerort}, {orderEntity.Vorname_NameFirma}: {orderEntity.Bezug}, {this._data.TypeRemarks}";
					//var creationNotes = $"Erstellt: {_user.Name}, {DateTime.Now.ToString("dddd, dd MMMM yyyy")}";
					//var notesWithoutSite = $"Eigenfertigung, {orderEntity.Vorname_NameFirma}: {orderEntity.Bezug}.";

					//var price = this._data.FirstSample
					//	? (itemCalculatoryCostsEntity.Betrag ?? 0m) + Convert.ToDecimal(this._data.Price)
					//	: (priceType == "S0" || staffPriceEntity == null)
					//		? (itemCalculatoryCostsEntity.Betrag ?? 0m)
					//		: (staffPriceEntity.Betrag ?? 0m);

					//var time = (priceType == "S0" || staffPriceEntity == null)
					//	? (itemEntity.Produktionszeit ?? 0m)
					//	: (staffPriceEntity.ProduKtionzeit ?? 0m);

					//if(itemCalculatoryCostsEntity.Kostenart.ToLower() == "arbeitskosten")
					//{
					//	price = this._data.FirstSample
					//		? (itemCalculatoryCostsEntity.Betrag ?? 0m) + Convert.ToDecimal(this._data.Price)
					//		: itemCalculatoryCostsEntity.Betrag ?? 0m;

					//	time = itemEntity.Produktionszeit ?? 0m;
					//}

					//// > Insert Production > Queries: 1, 2, 3, 4, 6, 7, 8, 10 and 11
					//var fertigungEntity = new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity()
					//{
					//	Angebot_nr = orderItemEntity.AngebotNr,
					//	Angebot_Artikel_Nr = orderItemEntity.Nr,
					//	Artikel_Nr = itemEntity.ArtikelNr,
					//	Anzahl = (int)orderItemEntity.Anzahl,
					//	Lagerort_id = storageLocationEntity?.LagerortId,
					//	Fertigungsnummer = nextFertigungsnummer,
					//	Datum = DateTime.Now,
					//	Termin_Fertigstellung = completionDeadlineDate.Date,

					//	Termin_Bestatigt1 = appointmentConfirmedDate1.Date,
					//	//Gebucht = false, // 1
					//	//Kennzeichen = "gesperrt", // 1
					//	Bemerkung = notes, // 1, 6, 7 and 8
					//	Originalanzahl = (int)orderItemEntity.Anzahl,
					//	Bemerkung_Planung = creationNotes,
					//	Mandant = "PSZ Electronic"/*orderEntity.Mandant*/,

					//	Lagerort_id_zubuchen = orderItemEntity.Lagerort_id,
					//	Techniker = this._data.TechnicianName,
					//	Erstmuster = this._data.FirstSample,
					//	Technik = this._data.TechnicalCommand,
					//	Bemerkung_ohne_statte = notesWithoutSite,
					//	Termin_Ursprunglich = this._data.ProductionDate.Date,

					//	KundenIndex = orderItemEntity.Index_Kunde,
					//	Kunden_Index_Datum = orderItemEntity.Index_Kunde_Datum,
					//	Urs_Artikelnummer = URSArticleEntiy?.ArtikelNummer ?? "-",
					//	//this._data.OriginalArticleId.ToString(),
					//	UBG = this._data.Storage_Subassembly,
					//	UBGTransfer = false,

					//	Preis = price,  // 1, 3, 4 and 10
					//	Zeit = time, // 3, 4 and 10

					//	Gebucht = true, // 6, 7 and 8
					//	Kennzeichen = "Offen", // 6, 7 and 8

					//	// > Missing
					//	Anzahl_erledigt = 0,
					//	Anzahl_aktuell = 0,
					//	ID_Hauptartikel = 0,
					//	ID_Rahmenfertigung = 0,
					//	Planungsstatus = "A",
					//	Tage_Abweichung = 0,
					//	Letzte_Gebuchte_Menge = 0,

					//	Gedruckt = false, // issue #81

					//	Kabel_geschnitten = false,//souilmi 21/06/2022
					//	Check_Kabelgeschnitten = false,//souilmi 21/06/2022
					//};

					//fertigungEntity.ID = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Insert(fertigungEntity);

					//Infrastructure.Services.Logging.Logger.LogTrace("STEP 1 COMPLETED");

					//// > Insert Production Item > Query: 5
					//foreach(var stucklistenEntity in stucklistenEntities)
					//{
					//	Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
					//	{
					//		ID_Fertigung_HL = fertigungEntity.ID,
					//		ID_Fertigung = fertigungEntity.ID,
					//		Artikel_Nr = stucklistenEntity.Artikel_Nr_des_Bauteils,
					//		Anzahl = fertigungEntity.Anzahl * stucklistenEntity.Anzahl,
					//		Lagerort_ID = fertigungEntity.Lagerort_id,
					//		Buchen = true,
					//		Vorgang_Nr = stucklistenEntity.Vorgang_Nr,
					//		ME_gebucht = false
					//	});
					//}

					//Infrastructure.Services.Logging.Logger.LogTrace("STEP 2 COMPLETED");

					//// > Update Order Item > Query: 9
					//Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateFertigungsnummer(orderItemEntity.Nr,
					//	nextFertigungsnummer);

					//Infrastructure.Services.Logging.Logger.LogTrace("STEP 3 COMPLETED");

					//// > WorkArea > Queries: 12, 13 and 14
					//var list_Gewerk_Fertigungsnummer = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get_Gewerk_Fertigungsnummer_Query11(nextFertigungsnummer);

					//var gewerk1 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 1")
					//	? "False"
					//	: "No";
					//var gewerk2 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 2")
					//	? "False"
					//	: "No";
					//var gewerk3 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 3")
					//	? "False"
					//	: "No";
					//Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateGewerk(nextFertigungsnummer, gewerk1, gewerk2, gewerk3);
					//SpecialHelper.Perform(fertigungEntity);
					////logging
					//var Log = new LogHelper((int)fertigungEntity.Fertigungsnummer, (int)fertigungEntity.Angebot_nr, 0, "Fertigung", LogHelper.LogType.CREATIONFA, "EDI", _user)
					//	.LogCTS(null, null, null, 0);
					//Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(Log);

					//return ResponseModel<int>.SuccessResponse(fertigungEntity.ID);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, e.StackTrace);
					throw;
				}
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var errors = new List<ResponseModel<int>.ResponseError>();


			#region >>> fertigung data
			if(this._data.ManufacturingFacilityId <= 0)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Storage location invalid" });
				return new ResponseModel<int>() { Errors = errors };
			}
			var storageLoacation = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(this._data.ManufacturingFacilityId);
			if(storageLoacation == null)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Storage location not found" });
				return new ResponseModel<int>() { Errors = errors };
			}

			var orderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data.PositionId);
			if(orderItemEntity == null)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position not found" });
				return new ResponseModel<int>() { Errors = errors };
			}

			if(orderItemEntity.Fertigungsnummer.HasValue && orderItemEntity.Fertigungsnummer.Value > 0)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position already validated" });
				return new ResponseModel<int>() { Errors = errors };
			}

			var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntity.ArtikelNr.HasValue ? (int)orderItemEntity.ArtikelNr : -1);
			if(itemDb == null)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item not found" });
			}
			if(itemDb.Freigabestatus.ToUpper() == "O")
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item is 'Obsolete'" });
			}

			var orderEntity = orderItemEntity.AngebotNr.HasValue
				? Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderItemEntity.AngebotNr.Value)
				: null;
			if(orderEntity == null)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position order not found" });
			}

			var itemEntity = orderItemEntity.ArtikelNr.HasValue
				? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntity.ArtikelNr.Value)
				: null;
			if(itemEntity == null)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position article not found" });
			}

			var itemCalculatoryCostsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetByArtikelNr(itemEntity.ArtikelNr);
			if(itemCalculatoryCostsEntity == null)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position article cost record not found" });
			}

			//if (!orderItemEntity.Liefertermin.HasValue) // delivery date
			//{
			//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position delivery date invalid" });
			//}

			if(!orderItemEntity.Lagerort_id.HasValue || int.TryParse(orderItemEntity.Lagerort_id.ToString(), out var storageId) == false)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position storage location invalid" });
			}
			else
			{
				var storageLocationEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(storageId);
				if(storageLocationEntity == null)
				{
					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position storage location not found" });
				}
			}

			//var position = new Models.Order.Element.OrderItemModel(orderItemEntity, itemEntity);
			//if (position.OpenQuantity_Quantity <= 0)
			//{
			//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position quantity invalid" });
			//}
			//var technicArticles = Program.BSD.TechnicArticleIds;
			var horizonCheck = HorizonsHelper.userHasFaCreateHorizonRight(_data.ProductionDate, _user, out List<string> messages);
			if(!horizonCheck && !HorizonsHelper.ArticleIsTechnic(itemDb.ArtikelNr))
				errors.AddRange(messages.Select(m => new ResponseModel<int>.ResponseError { Key = "", Value = m }).ToList());

			#endregion fertigung data

			#region >>> Pricing group
			var preisgruppenEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemEntity.ArtikelNr);
			if(preisgruppenEntity == null)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group invalid" });
				return new ResponseModel<int>() { Errors = errors };
			}
			else
			{
			}
			#endregion Pricing group

			if(this._data.Price < 0)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Price is invalid" });
			}


			// >>>
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}

			return ResponseModel<int>.SuccessResponse();
		}
		internal string getPriceType(Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity preisgruppenEntity, decimal amount)
		{
			var amountFloat = amount;

			if(preisgruppenEntity.ME4 > 0 && amountFloat <= preisgruppenEntity.ME4 && amountFloat > preisgruppenEntity.ME3 && preisgruppenEntity.Staffelpreis4 != null)
			{
				return "S4";
			}
			else if(preisgruppenEntity.ME3 > 0 && amountFloat <= preisgruppenEntity.ME3 && amountFloat > preisgruppenEntity.ME2 && preisgruppenEntity.Staffelpreis3 != null)
			{
				return "S3";
			}
			else if(preisgruppenEntity.ME2 > 0 && amountFloat <= preisgruppenEntity.ME2 && amountFloat > preisgruppenEntity.ME1 && preisgruppenEntity.Staffelpreis2 != null)
			{
				return "S2";
			}
			else if(preisgruppenEntity.ME1 > 0 && amountFloat <= preisgruppenEntity.ME1 && preisgruppenEntity.Staffelpreis1 != null)
			{
				return "S1";
			}
			else
			{
				return "S0";
			}
		}
	}
}