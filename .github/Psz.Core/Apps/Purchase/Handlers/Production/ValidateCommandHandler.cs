using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.CustomerService.Helpers;
	using Psz.Core.SharedKernel.Interfaces;
	public class ValidateCommandHandler//: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		//		private Identity.Models.UserModel _user { get; set; }
		//		private Models.Production.ValidateCommandModel _data { get; set; }
		//		public ValidateCommandHandler(Identity.Models.UserModel user, Models.Production.ValidateCommandModel command)
		//		{
		//			this._user = user;
		//			this._data = command;
		//		}
		//		public ResponseModel<int> Handle()
		//		{
		//			lock(Locks.ProductionLock)
		//			{

		//				try
		//				{
		//					var validationResponse = this.Validate();
		//					if(!validationResponse.Success)
		//					{
		//						return validationResponse;
		//					}

		//					var validationErrors = new List<ResponseModel<int>.ResponseError>();

		//					var LagerWithVersionning = Program.LagersWithVersionning ?? new List<int>();
		//					var orderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data.PositionId);
		//					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntity?.ArtikelNr ?? -1);
		//					var orderEntity = orderItemEntity.AngebotNr.HasValue
		//						? Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderItemEntity.AngebotNr.Value)
		//						: null;
		//					var itemEntity = orderItemEntity.ArtikelNr.HasValue
		//						? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntity.ArtikelNr.Value)
		//						: null;
		//					var URSarticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_data.OriginalArticleId);
		//					var priceGroupEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr((int)orderItemEntity.ArtikelNr);

		//					var priceType = getPriceType(priceGroupEntity, Convert.ToDecimal(orderItemEntity.Anzahl ?? 0));

		//					Infrastructure.Services.Logging.Logger.LogTrace("itemEntity.ArtikelNr: " + itemEntity.ArtikelNr);
		//					Infrastructure.Services.Logging.Logger.LogTrace("priceType: " + priceType);

		//					var staffPriceEntity = Infrastructure.Data.Access.Tables.PRS.StaffelpreisKonditionzuordnungAccess.GetByArtikelNrAndType(itemEntity.ArtikelNr, priceType);

		//					// - 
		//					var stucklistenEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(itemEntity.ArtikelNr);
		//					var bomSnapshotCountByArticle = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetBOMVersionByArticle_Count(itemEntity?.ArtikelNr ?? -1);

		//					// - if BOM activated for versioning
		//					if(LagerWithVersionning.Contains(this._data.ManufacturingFacilityId))
		//					{
		//						//var bomExtension = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(itemEntity?.ArtikelNr ?? -1);
		//						//int? bomVersion = null;
		//						//if (bomExtension != null)
		//						//{
		//						//    if (bomExtension.BomStatusId == (int)Core.BaseData.Enums.ArticleEnums.BomStatus.Approved)
		//						//    {
		//						//        bomVersion = bomExtension.BomVersion;
		//						//    }
		//						//    else
		//						//    {
		//						//        if (bomExtension.BomVersion > 0)
		//						//        {
		//						//            bomVersion = bomExtension.BomVersion - 1;
		//						//        }
		//						//    }
		//						//}

		//						// - If not first BOM for current Article, check Snapshot w Index
		//						if(bomSnapshotCountByArticle > 0)
		//						{
		//							//var stucklistenSnapshot = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetByArticleAndVersion(itemEntity?.ArtikelNr ?? -1, bomVersion)
		//							var stucklistenSnapshot = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticleAndIndex(itemEntity?.ArtikelNr ?? -1, orderItemEntity.Index_Kunde) // - 2022-05-18 - take the BOM for Pos Index
		//								?.Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity
		//								{
		//									Anzahl = (float?)x.Anzahl,
		//									Artikelnummer = x.Artikelnummer,
		//									Artikel_Nr = x.Artikel_Nr,
		//									Artikel_Nr_des_Bauteils = x.Artikel_Nr_des_Bauteils,
		//									Bezeichnung_des_Bauteils = x.Bezeichnung_des_Bauteils,
		//									DocumentId = x.DocumentId,
		//									Nr = -1,
		//									Position = x.Position,
		//									Variante = x.Variante,
		//									Vorgang_Nr = x.Vorgang_Nr
		//								})?.ToList();

		//							if(/*articleEntity.Index_Kunde?.Trim() != orderItemEntity.Index_Kunde?.Trim() && */
		//								(stucklistenSnapshot == null || stucklistenSnapshot.Count <= 0))
		//							{
		//								validationErrors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = $"Validated BOM not found for Artikel [{articleEntity.ArtikelNummer}] and Index [{orderItemEntity.Index_Kunde}]" });
		//							}
		//							// - 
		//							if(stucklistenSnapshot != null && stucklistenSnapshot.Count > 0)
		//							{
		//								stucklistenEntities = stucklistenSnapshot;
		//							}
		//						}
		//						else
		//						{
		//							// 
		//							var stucklistenSnapshot = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(itemEntity?.ArtikelNr ?? -1);
		//							/////////// 
		//							//var stucklistenSnapshot = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticleAndIndex(itemEntity?.ArtikelNr ?? -1)
		//							////var stucklistenSnapshot = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticleAndIndex(itemEntity?.ArtikelNr ?? -1, orderItemEntity.Index_Kunde) // - 2022-05-18 - take the BOM for Pos Index
		//							//    ?.Select(x => new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity
		//							//    {
		//							//        Anzahl = (float?)x.Anzahl,
		//							//        Artikelnummer = x.Artikelnummer,
		//							//        Artikel_Nr = x.Artikel_Nr,
		//							//        Artikel_Nr_des_Bauteils = x.Artikel_Nr_des_Bauteils,
		//							//        Bezeichnung_des_Bauteils = x.Bezeichnung_des_Bauteils,
		//							//        DocumentId = x.DocumentId,
		//							//        Nr = -1,
		//							//        Position = x.Position,
		//							//        Variante = x.Variante,
		//							//        Vorgang_Nr = x.Vorgang_Nr
		//							//    })?.ToList();

		//							if(stucklistenSnapshot == null || stucklistenSnapshot.Count <= 0)
		//							{
		//								validationErrors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = $"BOM not found for Artikel [{articleEntity.ArtikelNummer}]" });
		//							}
		//							// - 
		//							if(stucklistenSnapshot != null && stucklistenSnapshot.Count > 0)
		//							{
		//								stucklistenEntities = stucklistenSnapshot;
		//							}
		//						}
		//					}

		//					if(stucklistenEntities.Count == 0)
		//					{
		//						validationErrors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "BOM not found" });
		//					}

		//					if(validationErrors.Count > 0)
		//					{
		//						return new ResponseModel<int>() { Errors = validationErrors };
		//					}

		//					var itemCalculatoryCostsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetByArtikelNr(itemEntity.ArtikelNr);
		//					var storageLocationEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(this._data.ManufacturingFacilityId/* (int)orderItemEntity.Lagerort_id*/);
		//					var nextFertigungsnummer = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetMaxFertigungsnummer("PSZ Electronic"/*orderEntity.Mandant*/)
		//						+ 1;

		//					var completionDeadlineDate = orderItemEntity.Liefertermin.HasValue ? orderItemEntity.Liefertermin.Value.AddDays(-3) : new DateTime(1970, 1, 1);
		//					var appointmentConfirmedDate1 = this._data.ProductionDate;

		//					var notes = $"{storageLocationEntity?.Lagerort}, {orderEntity.Vorname_NameFirma}: {orderEntity.Bezug}, {this._data.TypeRemarks} ,{this._data.Customer}";
		//					var creationNotes = $"Erstellt: {_user.Name}, {DateTime.Now.ToString("dddd, dd MMMM yyyy")}";
		//					var notesWithoutSite = $"Eigenfertigung, {orderEntity.Vorname_NameFirma}: {orderEntity.Bezug} ,{this._data.Customer}.";

		//					var price = this._data.FirstSample
		//						? (itemCalculatoryCostsEntity.Betrag ?? 0m) + Convert.ToDecimal(this._data.Price)
		//						: (priceType == "S0" || staffPriceEntity == null)
		//							? (itemCalculatoryCostsEntity.Betrag ?? 0m)
		//							: (staffPriceEntity.Betrag ?? 0m);

		//					var time = (priceType == "S0" || staffPriceEntity == null)
		//						? (itemEntity.Produktionszeit ?? 0m)
		//						: (staffPriceEntity.ProduKtionzeit ?? 0m);

		//					if(itemCalculatoryCostsEntity.Kostenart.ToLower() == "arbeitskosten")
		//					{
		//						price = this._data.FirstSample
		//							? (itemCalculatoryCostsEntity.Betrag ?? 0m) + Convert.ToDecimal(this._data.Price)
		//							: itemCalculatoryCostsEntity.Betrag ?? 0m;

		//						time = itemEntity.Produktionszeit ?? 0m;
		//					}

		//					// > Insert Production > Queries: 1, 2, 3, 4, 6, 7, 8, 10 and 11
		//					var fertigungEntity = new Infrastructure.Data.Entities.Tables.PRS.FertigungEntity()
		//					{
		//						Angebot_nr = orderItemEntity.AngebotNr,
		//						Angebot_Artikel_Nr = orderItemEntity.Nr,
		//						Artikel_Nr = itemEntity.ArtikelNr,
		//						Anzahl = (int)orderItemEntity.Anzahl,
		//						Lagerort_id = storageLocationEntity?.LagerortId,
		//						Fertigungsnummer = nextFertigungsnummer,
		//						Datum = DateTime.Now,
		//						Termin_Fertigstellung = completionDeadlineDate.Date,

		//						Termin_Bestatigt1 = appointmentConfirmedDate1.Date,
		//						//Gebucht = false, // 1
		//						//Kennzeichen = "gesperrt", // 1
		//						Bemerkung = notes, // 1, 6, 7 and 8
		//						Originalanzahl = (int)orderItemEntity.Anzahl,
		//						Bemerkung_Planung = creationNotes,
		//						Mandant = "PSZ Electronic"/*orderEntity.Mandant*/,

		//						Lagerort_id_zubuchen = orderItemEntity.Lagerort_id,
		//						Techniker = this._data.TechnicianName,
		//						Erstmuster = this._data.FirstSample,
		//						Technik = this._data.TechnicalCommand,
		//						Bemerkung_ohne_statte = notesWithoutSite,
		//						Termin_Ursprunglich = this._data.ProductionDate.Date,

		//						KundenIndex = orderItemEntity.Index_Kunde,
		//						Kunden_Index_Datum = orderItemEntity.Index_Kunde_Datum,
		//						Urs_Artikelnummer = URSarticleEntity?.ArtikelNummer ?? "-",
		//						//this._data.OriginalArticleId.ToString(),
		//						UBG = this._data.Storage_Subassembly,
		//						UBGTransfer = false,

		//						Preis = price,  // 1, 3, 4 and 10
		//						Zeit = time, // 3, 4 and 10

		//						Gebucht = true, // 6, 7 and 8
		//						Kennzeichen = "Offen", // 6, 7 and 8

		//						// > Missing
		//						Anzahl_erledigt = 0,
		//						Anzahl_aktuell = 0,
		//						ID_Hauptartikel = 0,
		//						ID_Rahmenfertigung = 0,
		//						Planungsstatus = "A",
		//						Tage_Abweichung = 0,
		//						Letzte_Gebuchte_Menge = 0,

		//						Gedruckt = false, // issue #81

		//						Kabel_geschnitten = false,//souilmi 21/06/2022
		//						Check_Kabelgeschnitten = false,//souilmi 21/06/2022
		//						HBGFAPositionId = this._data.HBGFAPositionId
		//					};

		//					// - set BOM & CP Version - Update only BETN - for now
		//					var logEntity = new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity();
		//					if(LagerWithVersionning.Contains(fertigungEntity.Lagerort_id ?? -1))
		//					{
		//						var articleSnapshotEntity = new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity();
		//						var cpSnapshotEntity = new Infrastructure.Data.Entities.Tables.BSD.CP_snapshot_positionsEntity();
		//						var articleExtension = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(itemEntity.ArtikelNr);
		//						//var articleSnapshotEntity = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticle(itemEntity.ArtikelNr);
		//						if(bomSnapshotCountByArticle > 0)
		//						{
		//							articleSnapshotEntity = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetLastByArticleAndIndex(itemEntity.ArtikelNr, orderItemEntity.Index_Kunde)[0]; // - Already checked that it has at least 1 elmt
		//							cpSnapshotEntity = Infrastructure.Data.Access.Tables.BSD.CP_snapshot_positionsAccess.GetLastByArticle(itemEntity.ArtikelNr, articleSnapshotEntity?.BomVersion ?? -1);
		//						}
		//						else
		//						{
		//							// - fake Snapshot - just to have BOM & CP info
		//							articleSnapshotEntity = new Infrastructure.Data.Entities.Tables.BSD.Stucklisten_SnapshotEntity
		//							{
		//								KundenIndex = articleEntity.Index_Kunde,
		//								KundenIndexDate = articleEntity.Index_Kunde_Datum,
		//								BomVersion = articleExtension?.BomVersion ?? 0
		//							};
		//						}


		//						fertigungEntity.BomVersion = articleSnapshotEntity?.BomVersion;
		//						fertigungEntity.KundenIndex = articleSnapshotEntity?.KundenIndex;
		//						fertigungEntity.CPVersion = cpSnapshotEntity?.CP_version;
		//						fertigungEntity.Kunden_Index_Datum = articleSnapshotEntity?.KundenIndexDate;

		//						// -
		//						logEntity =
		//							Psz.Core.BaseData.Handlers.ObjectLogHelper.getLog(this._user, -1, $"[FA:{fertigungEntity.Fertigungsnummer}] BOM || CP || IndexKunde",
		//							$"{fertigungEntity.BomVersion} || {cpSnapshotEntity?.CP_version} || {fertigungEntity.KundenIndex}",
		//							$"{articleSnapshotEntity?.BomVersion} || {articleSnapshotEntity?.KundenIndex}",
		//							"Fertigung",
		//							Core.BaseData.Enums.ObjectLogEnums.LogType.Edit);
		//					}

		//					fertigungEntity.ID = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Insert(fertigungEntity);

		//					if(LagerWithVersionning.Contains(fertigungEntity.Lagerort_id ?? -1)) // -- logging // - Update only BETN - for now
		//					{
		//						logEntity.LogObjectId = fertigungEntity.ID;
		//						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logEntity);
		//					}

		//					Infrastructure.Services.Logging.Logger.LogTrace("STEP 1 COMPLETED");

		//					// > Insert Production Item > Query: 5
		//					foreach(var stucklistenEntity in stucklistenEntities)
		//					{
		//						Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity
		//						{
		//							ID_Fertigung_HL = fertigungEntity.ID,
		//							ID_Fertigung = fertigungEntity.ID,
		//							Artikel_Nr = stucklistenEntity.Artikel_Nr_des_Bauteils,
		//							Anzahl = fertigungEntity.Anzahl * stucklistenEntity.Anzahl,
		//							Lagerort_ID = fertigungEntity.Lagerort_id,
		//							Buchen = true,
		//							Vorgang_Nr = stucklistenEntity.Vorgang_Nr,
		//							ME_gebucht = false
		//						});
		//					}

		//					Infrastructure.Services.Logging.Logger.LogTrace("STEP 2 COMPLETED");

		//					// > Update Order Item > Query: 9
		//					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateFertigungsnummer(orderItemEntity.Nr,
		//						nextFertigungsnummer);

		//					Infrastructure.Services.Logging.Logger.LogTrace("STEP 3 COMPLETED");

		//					// > WorkArea > Queries: 12, 13 and 14
		//					var list_Gewerk_Fertigungsnummer = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get_Gewerk_Fertigungsnummer_Query11(nextFertigungsnummer);

		//					var gewerk1 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 1")
		//						? "False"
		//						: "No";
		//					var gewerk2 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 2")
		//						? "False"
		//						: "No";
		//					var gewerk3 = list_Gewerk_Fertigungsnummer.Exists(e => e.Item1.ToLower() == "gewerk 3")
		//						? "False"
		//						: "No";
		//					Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateGewerk(nextFertigungsnummer, gewerk1, gewerk2, gewerk3);
		//					//update article production place
		//					var articleExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(itemEntity.ArtikelNr);
		//					var _prodPlace = -1;
		//					// - 2022-12-22 - return EnumValue
		//					foreach(int i in Enum.GetValues(typeof(Common.Enums.ArticleEnums.ArticleProductionPlace)))
		//					{
		//						if(i == storageLocationEntity.LagerortId)
		//						{
		//							_prodPlace = i;
		//							break;
		//						}
		//					}
		//					//switch(storageLocationEntity.LagerortId)
		//					//{
		//					//	case 60:
		//					//		_prodPlace = (int)Common.Enums.ArticleEnums.ArticleProductionPlace.BETN;
		//					//		break;
		//					//	case 7:
		//					//		_prodPlace = (int)Common.Enums.ArticleEnums.ArticleProductionPlace.TN;
		//					//		break;
		//					//	case 42:
		//					//		_prodPlace = (int)Common.Enums.ArticleEnums.ArticleProductionPlace.WS;
		//					//		break;
		//					//	case 6:
		//					//		_prodPlace = (int)Common.Enums.ArticleEnums.ArticleProductionPlace.CZ;
		//					//		break;
		//					//	case 26:
		//					//		_prodPlace = (int)Common.Enums.ArticleEnums.ArticleProductionPlace.AL;
		//					//		break;
		//					//	case 15:
		//					//		_prodPlace = (int)Common.Enums.ArticleEnums.ArticleProductionPlace.DE;
		//					//		break;
		//					//	default:
		//					//		_prodPlace = -1;
		//					//		break;
		//					//}
		//					if(articleExtensionEntity != null)
		//					{
		//						if(_prodPlace != -1 && !articleExtensionEntity.ProductionPlace1_Id.HasValue)
		//						{
		//							articleExtensionEntity.ProductionPlace1_Id = _prodPlace;
		//							articleExtensionEntity.UpdateTime = DateTime.Now;
		//							articleExtensionEntity.UpdateUserId = _user.Id;
		//							Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Update(articleExtensionEntity);
		//						}

		//					}
		//					else
		//					{
		//						Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity
		//						{
		//							ArticleId = itemEntity.ArtikelNr,
		//							Id = -1,
		//							CreateTime = DateTime.Now,
		//							CreateUserId = _user.Id,
		//							ProductionPlace1_Id = _prodPlace != -1 ? _prodPlace : null,
		//							AlternativeProductionPlace = false,
		//						});
		//					}
		//					//logging
		//					var _log = new LogHelper(orderEntity.Nr, (int)orderEntity.Angebot_Nr, int.TryParse(orderEntity.Projekt_Nr, out var v) ? v : 0, orderEntity.Typ, LogHelper.LogType.VALIDATEPRODUCTION, "CTS", _user)
		//						.LogCTS(null, null, null, (int)orderItemEntity.Position);
		//					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
		//					SpecialHelper.Perform(fertigungEntity);

		//					return ResponseModel<int>.SuccessResponse(fertigungEntity.ID);
		//				} catch(Exception e)
		//				{
		//					Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, e.StackTrace);
		//					throw;
		//				}
		//			}
		//		}
		//		public ResponseModel<int> Validate()
		//		{
		//			if(this._user == null/*|| this._user.Access.____*/)
		//			{
		//				return ResponseModel<int>.AccessDeniedResponse();
		//			}

		//			var errors = new List<ResponseModel<int>.ResponseError>();


		//			#region >>> fertigung data
		//			if(this._data.ManufacturingFacilityId <= 0)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Storage location invalid" });
		//				return new ResponseModel<int>() { Errors = errors };
		//			}
		//			var storageLoacation = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(this._data.ManufacturingFacilityId);
		//			if(storageLoacation == null)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Storage location not found" });
		//				return new ResponseModel<int>() { Errors = errors };
		//			}

		//			var orderItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data.PositionId);
		//			if(orderItemEntity == null)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position not found" });
		//				return new ResponseModel<int>() { Errors = errors };
		//			}

		//			if(orderItemEntity.Fertigungsnummer.HasValue && orderItemEntity.Fertigungsnummer.Value > 0)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position already validated" });
		//				return new ResponseModel<int>() { Errors = errors };
		//			}

		//			var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntity.ArtikelNr.HasValue ? (int)orderItemEntity.ArtikelNr : -1);
		//			if(itemDb == null)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item not found" });
		//			}
		//			if(itemDb.Freigabestatus.ToUpper() == "O")
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Item is 'Obsolete'" });
		//			}

		//			var orderEntity = orderItemEntity.AngebotNr.HasValue
		//				? Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderItemEntity.AngebotNr.Value)
		//				: null;
		//			if(orderEntity == null)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position order not found" });
		//			}

		//			var itemEntity = orderItemEntity.ArtikelNr.HasValue
		//				? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(orderItemEntity.ArtikelNr.Value)
		//				: null;
		//			if(itemEntity == null)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position article not found" });
		//			}

		//			var itemCalculatoryCostsEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.GetByArtikelNr(itemEntity.ArtikelNr);
		//			if(itemCalculatoryCostsEntity == null)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position article cost record not found" });
		//			}

		//			if(!orderItemEntity.Liefertermin.HasValue) // delivery date
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position delivery date invalid" });
		//			}

		//			if(!orderItemEntity.Lagerort_id.HasValue || int.TryParse(orderItemEntity.Lagerort_id.ToString(), out var storageId) == false)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position storage location invalid" });
		//			}
		//			else
		//			{
		//				var storageLocationEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(storageId);
		//				if(storageLocationEntity == null)
		//				{
		//					errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position storage location not found" });
		//				}
		//			}

		//			var position = new Models.Order.Element.OrderItemModel(orderItemEntity, itemEntity);
		//			if(position.OpenQuantity_Quantity <= 0)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position quantity invalid" });
		//			}

		//			// - 2022-06-03 - KI - Index Pos <> Index Article AND Index Pos not in BOM Index
		//			var bomSnapshotIndexes = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetKundenIndexByArticle(itemEntity.ArtikelNr)
		//				?? new List<string>();
		//			if(itemEntity.Index_Kunde?.Trim() != orderItemEntity.Index_Kunde?.Trim() && !bomSnapshotIndexes.Exists(x => x?.Trim() == orderItemEntity.Index_Kunde?.Trim()))
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position Index Kunde invalid" });
		//			}
		//			//var technicArticles = Program.BSD.TechnicArticleIds;
		//			var horizonCheck = HorizonsHelper.userHasFaCreateHorizonRight(_data.ProductionDate, _user, out List<string> messages);
		//			if(!horizonCheck && !HorizonsHelper.ArticleIsTechnic(itemEntity.ArtikelNr))
		//				errors.AddRange(messages.Select(m => new ResponseModel<int>.ResponseError { Key = "", Value = m }).ToList());
		//			#endregion fertigung data

		//			#region >>> Pricing group
		//			var preisgruppenEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemEntity.ArtikelNr);
		//			if(preisgruppenEntity == null)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group invalid" });
		//				return new ResponseModel<int>() { Errors = errors };
		//			}
		//			else
		//			{
		//				//if(!preisgruppenEntity.ME1.HasValue || decimal.TryParse(preisgruppenEntity.ME1.Value.ToString(), out var me1) == false)
		//				//{
		//				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group ME1 invalid" });
		//				//}
		//				//if (!preisgruppenEntity.ME2.HasValue || decimal.TryParse(preisgruppenEntity.ME2.Value.ToString(), out var me2) == false)
		//				//{
		//				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group ME2 invalid" });
		//				//}
		//				//if (!preisgruppenEntity.ME3.HasValue || decimal.TryParse(preisgruppenEntity.ME3.Value.ToString(), out var me3) == false)
		//				//{
		//				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group ME3 invalid" });
		//				//}
		//				//if (!preisgruppenEntity.ME4.HasValue || decimal.TryParse(preisgruppenEntity.ME4.Value.ToString(), out var me4) == false)
		//				//{
		//				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group ME4 invalid" });
		//				//}

		//				//// >>> 
		//				//if (!preisgruppenEntity.Staffelpreis1.HasValue || decimal.TryParse(preisgruppenEntity.Staffelpreis1.Value.ToString(), out var staffelpreis1) == false)
		//				//{
		//				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group Staffelpreis1 invalid" });
		//				//}
		//				//if (!preisgruppenEntity.Staffelpreis2.HasValue || decimal.TryParse(preisgruppenEntity.Staffelpreis2.Value.ToString(), out var staffelpreis2) == false)
		//				//{
		//				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group Staffelpreis2 invalid" });
		//				//}
		//				//if (!preisgruppenEntity.Staffelpreis3.HasValue || decimal.TryParse(preisgruppenEntity.Staffelpreis3.Value.ToString(), out var staffelpreis3) == false)
		//				//{
		//				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group Staffelpreis3 invalid" });
		//				//}
		//				//if (!preisgruppenEntity.Staffelpreis4.HasValue || decimal.TryParse(preisgruppenEntity.Staffelpreis4.Value.ToString(), out var staffelpreis4) == false)
		//				//{
		//				//    errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Princing group Staffelpreis4 invalid" });
		//				//}
		//			}
		//			#endregion Pricing group

		//			if(this._data.Price < 0)
		//			{
		//				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Price is invalid" });
		//			}


		//			// >>>
		//			if(errors.Count > 0)
		//			{
		//				return new ResponseModel<int>() { Errors = errors };
		//			}

		//			// - 2023-02-07 - DateProductionFA = DateAB - 14days
		//			if(!Program.BSD.TechnicArticleIds.Exists(x => x == itemDb.ArtikelNr) && itemEntity?.Hubmastleitungen != true && orderItemEntity.Liefertermin.Value.AddDays(-14) != this._data.ProductionDate)
		//			{
		//				return ResponseModel<int>.FailureResponse($"Date invalid: production date [{this._data.ProductionDate.ToString("dd/MM/yyyy")}] must be [{orderItemEntity.Liefertermin.Value.AddDays(-14).ToString("dd/MM/yyyy")}], 2KW before AB.");
		//			}

		//			// - 203-02-03
		//			var frZone = DateTime.Today.AddDays(Program.CTS.FAHorizons.H1LengthInDays); // 2024-01-25 - Khelil change H1 to 41 days
		//			if(this._user?.Access?.CustomerService?.FAWerkWunshAdmin != true && this._user.Access?.CustomerService?.FaAdmin != true && this._data.ManufacturingFacilityId != 6 && this._data.TechnicalCommand != true && !Program.BSD.TechnicArticleIds.Exists(x => x == itemDb.ArtikelNr))
		//			{
		//				var _newDate = this._data.ProductionDate;
		//				if(_newDate < DateTime.Today)
		//				{
		//					return ResponseModel<int>.FailureResponse($"Production date invalid: cannot add FA [{_newDate.ToString("dd/MM/yyyy")}] in the past.");
		//				}

		//				if(_newDate <= frZone)
		//				{
		//					return ResponseModel<int>.FailureResponse($"Production date invalid: cannot add FA before Frozen Zone limit [{frZone.ToString("dd/MM/yyyy")}].");
		//				}
		//			}

		//			return ResponseModel<int>.SuccessResponse();
		//		}
		public static string getPriceType(Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity preisgruppenEntity, decimal amount)
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

		/*
		 CASE WHEN Preisgruppen.ME4>0 And @productionAmount <= Preisgruppen.ME4 And @productionAmount > Preisgruppen.ME3 And Not Preisgruppen.Staffelpreis4 IS NULL THEN 'S4' 
		 ELSE 
			CASE WHEN Preisgruppen.ME3>0 And @productionAmount <= Preisgruppen.ME3 And @productionAmount > Preisgruppen.ME2 And Not Preisgruppen.Staffelpreis3 IS NULL THEN 'S3' 
			ELSE 
				CASE WHEN Preisgruppen.ME2>0 And @productionAmount <= Preisgruppen.ME2 And @productionAmount > Preisgruppen.ME1 And Not Preisgruppen.Staffelpreis2 IS NULL THEN 'S2'
				ELSE 
					CASE WHEN Preisgruppen.ME1>0 And @productionAmount <= Preisgruppen.ME1 And Not Preisgruppen.Staffelpreis1  IS NULL THEN 'S1' 
					ELSE 'S0' 
					END
				END 
			END
		END
		 */

		//if (preisgruppenEntity.Staffelpreis4 != null && preisgruppenEntity.Staffelpreis3 != null && preisgruppenEntity.Staffelpreis2 != null && preisgruppenEntity.Staffelpreis1 != null)
		//{
		//    return "S0";
		//}

		//if (preisgruppenEntity.Staffelpreis4 != null && preisgruppenEntity.ME4 > 0 && amount <= (decimal)preisgruppenEntity.ME4 && amount > (decimal)preisgruppenEntity.ME3)
		//{
		//    return "S4";
		//}
		//else
		//{
		//    if (preisgruppenEntity.Staffelpreis3 != null && preisgruppenEntity.ME3 > 0 && amount <= (decimal)preisgruppenEntity.ME3 && amount > (decimal)preisgruppenEntity.ME2)
		//    {
		//        return "S3";
		//    }
		//    else
		//    {
		//        if (preisgruppenEntity.Staffelpreis2 != null && preisgruppenEntity.ME2 > 0 && amount <= (decimal)preisgruppenEntity.ME2 && amount > (decimal)preisgruppenEntity.ME1)
		//        {
		//            return "S2";
		//        }
		//        else
		//        {
		//            if (preisgruppenEntity.ME1 > 0 && amount <= (decimal)preisgruppenEntity.ME1 && preisgruppenEntity.Staffelpreis1 != null)
		//            {
		//                return "S1";
		//            }
		//            else
		//            {
		//                return "S0";
		//            }
		//        }
		//    }
		//}
	}
	}
}