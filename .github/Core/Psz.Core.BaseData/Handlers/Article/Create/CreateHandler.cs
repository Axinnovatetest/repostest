using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Infrastructure.Services.Utils;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class CreateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.CreateRequestModel _data { get; set; }

		public CreateHandler(Identity.Models.UserModel user, Models.Article.CreateRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{

				#region // -- transaction-based logic -- //

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();


				var countryEntity = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(this._data.ProductionCountryId);
				var siteEntity = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(this._data.ProductionSiteId);
				var rohGoodsGroupEntity = (Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity>()).Find(x => x.Warengruppe.Trim().ToLower() == "roh");
				var efGoodsGroupEntity = (Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity>()).Find(x => x.Warengruppe.Trim().ToLower() == "ef");
				var itemParts = this._data.ArticleNumber?.Trim().Split('-')?.ToList();
				int customerNumberSeq = 0;
				int customerIndexSeq = 0;

				#region >>> Data sequences <<<
				// - current Index does not exist
				if(this._data.IsArticleNumberSpecial)
				{
					customerIndexSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNextIndexSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber, this._data.IsArticleNumberSpecial);
				}
				var maxCustomerSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerMaxNumberSequence(this._data.CustomerNumber, itemParts[0]);
				if(maxCustomerSeq < 0)
				{
					// - no articles for Customer
				}
				else
				{
					// - current ItemNumber
					customerNumberSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNumberSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber);
					if(customerNumberSeq < 0)
					{
						// - current ItemNumber does not exist
						customerNumberSeq = maxCustomerSeq + 1;
					}
					else
					{
						// - current Index
						customerIndexSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerIndexSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber, this._data.CustomerItemIndex, this._data.IsArticleNumberSpecial);
						if(customerIndexSeq < 0)
						{
							// - current Index does not exist
							customerIndexSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNextIndexSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber, this._data.IsArticleNumberSpecial);
						}
					}
				}
				#endregion data sequences

				if(!this._data.IsArticleNumberCustom && this._data.GoodsGroupName?.Trim()?.ToLower() == "ef")
				{
					var kreis = itemParts[0];//?.Substring(0, 3);
					this._data.ArticleNumber = Data.UpdateCustomerIndexHandler.getNewArticleNumber(kreis, customerNumberSeq,
						customerIndexSeq, this._data.ProductionSiteCode, this._data.ProductionCountryCode);
				}

				// - 2022-11-09 - default index to '-' when NOT EF - Khelil
				if(this._data.GoodsGroupName.Trim().ToLower() != efGoodsGroupEntity?.Warengruppe?.Trim().ToLower())
				{
					// - 2022-10-26 - default ROH index to '-'
					// - 2023-04-27 - allow index for ROH - Sax
					if(this._data.GoodsGroupName.Trim().ToLower() != rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower()
						|| string.IsNullOrWhiteSpace(this._data.CustomerItemIndex))
					{
						this._data.CustomerItemIndex = "-";
					}
				}


				// - 2023-01-22 - remove old EdiDefault if any
				if(this._data.GoodsGroupName?.Trim()?.ToLower() == "ef" && this._data.IsEdiDefault == true)
				{
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.ResetCustomerEdiDefaultWithTransaction(this._data.CustomerNumber, this._data.CustomerItemNumber, botransaction.connection, botransaction.transaction);
				}
				// - 2024-03-06 Task :00024 PM - FG1(back)
				int DELKunden = 0;
				if(this._data.DELFixiert && this._data.GoodsGroupName?.Trim()?.ToLower() == "ef")
				{
					DELKunden = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDELKunden(_data.CustomerNumber, botransaction.connection, botransaction.transaction);
				}
				// - 2024-06-04 Task :set classification auto from selected level one if
				var classification = new Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity();
				if(this._data.IdLevelOne is not null)
				{
					var levelOneEntity = Infrastructure.Data.Access.Tables.BSD.Roh_Artikelnummer_Level1Access.Get(_data.IdLevelOne ?? -1);
					classification = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Get(levelOneEntity?.ClassificationId ?? -1);
				}
				var insertedNr = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
					{
						ArtikelNummer = this._data.ArticleNumber,
						Bezeichnung1 = this._data.Designation,
						aktiv = true,
						Umsatzsteuer = 0.19m,
						// - 2021-10-22 Khelil, default values
						Sysmonummer = "-",
						Lagerartikel = this._data.GoodsGroupName?.Trim()?.ToLower() == "ef" ? true : false, //-2024-03-05 - 00024 PM - FG1(back)
						Rabattierfähig = false,
						Stuckliste = this._data.GoodsGroupName?.Trim()?.ToLower() == "ef" ? true : false, // - 2022-09-28 - Sax set to true if EF
						fakturierenStückliste = false,
						Seriennummernverwaltung = false,
						Anfangsbestand = 0,
						DatumAnfangsbestand = DateTime.Today,
						Wert_Anfangsbestand = 0,
						Preiseinheit = 1,
						aktualisiert = DateTime.Today,
						Gewicht = 7.5m, // - 2023-02-17 - Sax
						Größe = 0,
						Exportgewicht = 0,
						Lagerhaltungskosten = 0,
						ArtikelAusEigenerProduktion = false,
						Sonderrabatt = 0,
						Provisionsartikel = true,
						ArtikelFürWeitereBestellungenSperren = false,
						Langtext_drucken_AB = false,
						Langtext_drucken_BW = false,
						fibu_rahmen = 0,
						Barverkauf = false,
						Webshop = false,
						Freigabestatus = "N",
						Produktionszeit = 0,
						Stundensatz = 0,
						Kupferzahl = this._data.CopperWeight,
						Kupferbasis = 150,// - 2023-01-18 - Khelil
						DEL = DELKunden,// - 2024-03-06 Task :00024 PM - FG1(back)
						DELFixiert = _data.DELFixiert,// - 2024-03-06 Task :00024 PM - FG1(back)
						Rahmen = false,
						Rahmenmenge = 0,
						PrufstatusTNWare = "T", // - 2022-09-05 - Sax
						ULzertifiziert = false,
						FreigabestatusTNIntern = "N",
						Artikelfamilie_Kunde = "-",
						CuGewicht = 0,
						VKFestpreis = _data.VKFestpreis, // - 2022-06-14 - issue:2099 Sax - // - 2024-03-06 Task :00024 PM - FG1(back)
						Standard_Lagerort_id = 0,
						ULEtikett = false,
						Materialkosten_Alt = 0,
						Werkzeug = "-",
						Halle = "0",
						ESD_Schutz = false,
						Kanban = false,
						Verpackungsmenge = 1,
						Losgroesse = (this._data.Losgroesse == null) ? 1 : this._data.Losgroesse,
						ROHSEEEConfirmity = false,
						MineralsConfirmity = false,
						REACHSVHCConfirmity = false,
						MHD = false,
						Rahmen2 = false,
						Rahmenmenge2 = 0,
						EMPB = false,
						EMPB_Freigegeben = false,
						Hubmastleitungen = false,
						Dienstelistung = 0,
						VDA_1 = false,
						VDA_2 = false,
						ESD_Schutz_Text = "",
						Blokiert_Status = false,
						CP_required = false,
						Warengruppe = this._data.GoodsGroupName,
						Warentyp = this._data.GoodsTypeId,
						UBG = false,
						// - 2022-08-11 - new concept
						Index_Kunde = this._data.CustomerItemIndex,
						Index_Kunde_Datum = this._data.CustomerItemIndexDate,
						CustomerNumber = this._data.CustomerNumber,
						CustomerPrefix = itemParts[0],
						CustomerItemNumber = this._data.CustomerItemNumber,
						CustomerItemNumberSequence = customerNumberSeq,
						CustomerIndex = this._data.CustomerItemIndex,
						CustomerIndexSequence = customerIndexSeq,
						ProductionCountryCode = countryEntity?.Designation,
						ProductionCountryName = countryEntity?.Name,
						ProductionCountrySequence = countryEntity?.MtdArticleSequence ?? 0,
						ProductionSiteCode = $"{(siteEntity?.LagerortId ?? 0).ToString("D2")}",
						ProductionSiteName = siteEntity?.Name,
						ProductionSiteSequence = siteEntity?.LagerortId ?? 0,
						ArticleNumber = this._data.ArticleNumber,
						IsArticleNumberSpecial = this._data.IsArticleNumberSpecial,
						EdiDefault = this._data.GoodsGroupName?.Trim()?.ToLower() == "ef" ? this._data.IsEdiDefault : false, // - 2023-01-22 - Sax & Schremmer
						Zolltarif_nr = this._data.CustomsNumber,
						Ursprungsland = this._data.OriginCountry,
						artikelklassifizierung = this._data.ProjectClassification,
						// - 2023-08-24
						COF_Pflichtig = this._data.CocActive,
						CocVersion = this._data.CocActive == true ? this._data.CocVersion : null,
						IsEDrawing = this._data.IsEDrawing,
						Artikelbezeichnung = this._data.Artikelbezeichnung,// - 2022-06-14 - issue:2099 Sax - // - 2024-03-06 Task :00024 PM - FG1(back)
						OrderNumber = this._data.OrderNumber,
						Consumption12Months = this._data.Consumption12Months,

						Projektname = this._data.Projektname,
						// souilmi roh artikelummer
						Manufacturer = this._data.Manufacturer,
						ManufacturerNumber = this._data.ManufacturerNumber,
						Einheit = this._data.Unit,
						ID_Klassifizierung = classification != null ? classification.ID : null,
						Klassifizierung = classification != null ? classification.Klassifizierung : null,
						//
						// - elimintaed after request on feed back 29-10-2024
						//Bezeichnung2 = this._data.IdLevelOne is not null ? _data.Designation : ""

					}, botransaction.connection, botransaction.transaction);


				var kabelsatzEntity = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.GetByName(_data.ProjektartFG);
				var insertedExtension = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity
					{
						ArtikelNr = insertedNr,
						CreatorID = this._user.Id,
						DateCreation = DateTime.Now,
						ProjectTypeId = this._data.GoodsGroupName?.Trim()?.ToLower() == "ef" ? (kabelsatzEntity?.Count > 0 ? kabelsatzEntity[0].Id : -1) : null, // - 2024-03-04 - Sax - only for EF artilcles
						OrderNumber = _data.OrderNumber,
						Consumption12Months = _data.Consumption12Months
					}, botransaction.connection, botransaction.transaction);


				#region // - BOM - //
				Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
					{
						ArticleId = insertedNr,
						ArticleDesignation = this._data.Designation,
						ArticleNumber = this._data.ArticleNumber,
						BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(),
						BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation,
						BomVersion = 0,
						BomValidFrom = null
					}, botransaction.connection, botransaction.transaction);
				#endregion BOM

				#region // - Prod - //
				//if (this._data.GoodsGroupName?.Trim()?.ToLower() == "ef")
				{
					Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity
						{
							ArticleId = insertedNr,
							CreateTime = DateTime.Now,
							CreateUserId = this._user.Id,
							ProductionPlace1_Id = int.TryParse(this._data.ProductionSiteCode, out var v) ? v : 0,
							ProductionPlace1_Name = this._data.ProductionSiteName,
						}, botransaction.connection, botransaction.transaction);
				}
				#endregion Prod

				#region // - Quality - // 
				Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity
					{
						ArticleId = insertedNr,
						CreateTime = DateTime.Now,
						CreateUserId = this._user.Id,
					}, botransaction.connection, botransaction.transaction);
				#endregion Quality

				#region // - Logistics - // 
				Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity
					{
						ArticleId = insertedNr,
						CreateTime = DateTime.Now,
						CreateUserId = this._user.Id,
					}, botransaction.connection, botransaction.transaction);
				#endregion Logistics

				// - save History - for AV auswertung
				Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.Insert(
					new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity
					{
						Artikel_Nr = insertedNr,
						Datum_Anderung = DateTime.Now,
						ID = -1, // not needed coz AutoIncrement
						Anderungsbereich = $"Artikelnummer",
						Anderungsbeschreibung = $"NEW-NEU-NOUVEAU",
						Anderung_von = this._user.Username,
					});
				// - logs
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, insertedNr, "Article", "", this._data.ArticleNumber, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Add),
					botransaction.connection, botransaction.transaction);

				#endregion transaction-based logic

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-04-22 - async 
					System.Threading.Tasks.Task.Factory.StartNew(() => addToLagers(insertedNr));

					// - 2022-03-30
					generateFileDAT(insertedNr, isNew: true);

					// -
					return ResponseModel<int>.SuccessResponse(insertedNr);
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
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			#region all Articles
			if(this._data.IsArticleNumberCustom)
			{
				if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumberCustom) != null)
				{
					return ResponseModel<int>.FailureResponse($"Article Number [{this._data.ArticleNumberCustom}] exists.");
				}
				// -
				this._data.ArticleNumber = this._data.ArticleNumberCustom;
			}
			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber) != null)
			{
				return ResponseModel<int>.FailureResponse($"Article Number [{this._data.ArticleNumber}] exists.");
			}
			if(Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get(this._data.GoodsGroupId) == null)
			{
				return ResponseModel<int>.FailureResponse($"Goods Group (Warengruppe) [{this._data.GoodsGroupName}] not found.");
			}
			var itemParts = this._data.ArticleNumber?.Trim().Split('-')?.ToList();
			if(itemParts == null || itemParts.Count < 3)
			{
				return ResponseModel<int>.FailureResponse($"Article Number structure invalid.");
			}
			#endregion - all Articles

			#region ROH
			var rohGoodsGroupEntity = (Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get()
				?? new List<Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity>()).Find(x => x.Warengruppe.Trim().ToLower() == "roh");
			if(this._data.GoodsGroupName.Trim().ToLower() == rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower()
				&& !this._data.GoodsTypeId.HasValue)
				return ResponseModel<int>.FailureResponse("Article ROH must have a [Waren Type].");
			if(this._data.GoodsGroupName.Trim().ToLower() == rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower()
				&& string.IsNullOrWhiteSpace(this._data.CustomsNumber))
			{
				return ResponseModel<int>.FailureResponse("Article ROH must have a [Customs Number (Zolltariffnummer)].");
			}
			if(this._data.GoodsGroupName.Trim().ToLower() == rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower()
				&& this._data.CustomsNumber.Length != 11)
			{
				return ResponseModel<int>.FailureResponse("[Customs Number (Zolltariffnummer)]: invalid data length. Length must be 11 characters.");
			}
			if(this._data.GoodsGroupName.Trim().ToLower() == rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower()
				&& string.IsNullOrWhiteSpace(this._data.OriginCountry))
			{
				return ResponseModel<int>.FailureResponse("Article ROH must have a [Origin Country (Ursprungsland)].");
			}
			if(this._data.GoodsGroupName.Trim().ToLower() == rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower()
				&& string.IsNullOrWhiteSpace(this._data.ProjectClassification))
			{
				return ResponseModel<int>.FailureResponse("Article ROH must have a [ProjectClassification (Projektklassifizierung)].");
			}
			// souilmi 03/06/2024

			#endregion - ROH

			// -
			var kreis = itemParts[0];//?.Substring(0, 3);
			var customers = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(kreis);

			// - 2022-06-08
			if(this._data.GoodsGroupName?.Trim()?.ToLower() == "ef")
			{
				#region --- prems ---
				if(string.IsNullOrWhiteSpace(this._data.CustomsNumber))
				{
					return ResponseModel<int>.FailureResponse("Article EF must have a [Customs Number (Zolltariffnummer)].");
				}

				if(this._data.CustomsNumber.Length != 11)
				{
					return ResponseModel<int>.FailureResponse("[Customs Number (Zolltariffnummer)]: invalid data length. Length must be 11 characters.");
				}
				if(string.IsNullOrWhiteSpace(this._data.OriginCountry))
				{
					return ResponseModel<int>.FailureResponse("Article EF must have an [Origin Country (Ursprungsland)].");
				}
				// - 2023-08-20 - Original country slinked to production country for EF
				//if(_data.ProductionCountryCode?.ToLower()?.Trim() != _data.OriginCountry?.ToLower()?.Trim())
				//{
				//	return ResponseModel<int>.FailureResponse($"[Origin Country (Ursprungsland)]: selected value [{_data.OriginCountry?.Trim()}] is different from production country [{_data.ProductionCountryCode?.Trim()}].");
				//}
				_data.OriginCountry = _data.ProductionCountryCode;

				if(string.IsNullOrWhiteSpace(this._data.ProjectClassification))
				{
					return ResponseModel<int>.FailureResponse("Article EF must have a [Project Classification (Projektklassifizierung)].");
				}
				if(Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(this._data.ProductionCountryId) == null)
				{
					return ResponseModel<int>.FailureResponse($"Country [{this._data.ProductionCountryName}] not found.");
				}
				if(Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(this._data.ProductionSiteId) == null)
				{
					return ResponseModel<int>.FailureResponse($"Hall [{this._data.ProductionSiteName}] not found.");
				}

				if(customers == null || customers.Count <= 0)
				{
					return ResponseModel<int>.FailureResponse($"EF Article Nummerschlüssel [{kreis}] does not exist.");
				}

				// - 1 - Customer
				if(Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data.CustomerId) == null)
				{
					return ResponseModel<int>.FailureResponse("Customer not found in Nummerschlüssel.");
				}

				if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(this._data.CustomerNumber) == null)
				{
					return ResponseModel<int>.FailureResponse("Customer not found in Adressen.");
				}

				// - 2 - ItemNumber
				if(string.IsNullOrWhiteSpace(this._data.CustomerItemNumber))
				{
					return ResponseModel<int>.FailureResponse($"Customer Item Number invalid value [{this._data.CustomerItemNumber}].");
				}
				// - 3 - Index
				if(string.IsNullOrWhiteSpace(this._data.CustomerItemIndex))
				{
					return ResponseModel<int>.FailureResponse($"Customer Item Index invalid value [{this._data.CustomerItemIndex}]");
				}
				#endregion prems

				var itemNumberSeq = itemParts[1];
				var itemIndexSeq = itemParts[2]?.Substring(0, 2);
				var warehouseSeq = itemParts[2]?.Substring(2, (itemParts[2]?.Length ?? 0) - 4);
				//var countrySeq = itemParts[2]?.Substring(4);
				// - Customer ItemNumber
				var sameCustomerItems = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber, this._data.IsArticleNumberSpecial);
				if(sameCustomerItems == null || sameCustomerItems.Count <= 0)
				{
					// - new CustomerItem - do nothing
				}
				else // - old CustomerItem
				{
					// - check CustomerIndex
					var sameCustomerIndex = sameCustomerItems.Where(x => x.CustomerIndex == this._data.CustomerItemIndex).ToList();
					if(sameCustomerIndex == null || sameCustomerIndex.Count <= 0)
					{
						// - new CustomerIndex - do nothing
					}
					else // - old CustomerIndex
					{
						// - checks unique <Warehouse-Country>
						var sameData = sameCustomerIndex.FirstOrDefault(x => x.ProductionCountryName?.Trim()?.ToLower() == this._data.ProductionCountryName?.Trim()?.ToLower()
							&& x.ProductionSiteName?.Trim()?.ToLower() == this._data.ProductionSiteName?.Trim()?.ToLower());
						if(sameData != null)
						{
							return ResponseModel<int>.FailureResponse($"Another article with same values exists [{sameData.ArtikelNummer}]");
						}
					}
				}
			}
			else
			{
				// - not EF article

				// - should not start with a Customer kreis
				if(customers != null && customers.Count > 0)
				{
					return ResponseModel<int>.FailureResponse(new List<string> { $"Kreis [{kreis}] exists for customers [{string.Join(", ", customers.Take(5).Select(x => $"{x.Kundennummer} | {x.Kunde}"))}].", $"Change article to EF or use another Nummerkreis." });
				}
			}
			// - 2023-08-24 - CoC
			if(_data.CocActive == null)
			{
				return ResponseModel<int>.FailureResponse(new List<string> { $"CoC : invalid value [{_data.CocActive}]" });
			}
			else
			{
				if(_data.CocActive == true && string.IsNullOrEmpty(_data.CocVersion))
				{
					return ResponseModel<int>.FailureResponse(new List<string> { $"CoC Version: invalid value [{_data.CocVersion}]" });
				}
			}

			// - 2025-02-10
			if(this._data.DELFixiert && this._data.GoodsGroupName?.Trim()?.ToLower() == "ef")
			{
				var adresse = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(_data.CustomerNumber);
				if(adresse == null)
				{
					return ResponseModel<int>.FailureResponse(new List<string> { $"DEL Fixiert: invalid value Cutomer [{_data.CustomerNumber}] for DEL Fixiert active." });
				}
				else
				{
					var kunde = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adresse.Nr);
					if(kunde == null)
					{
						return ResponseModel<int>.FailureResponse(new List<string> { $"DEL Fixiert: invalid value Cutomer [{_data.CustomerNumber}] for DEL Fixiert active." });
					}
				}
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
		public static void generateFileDAT(int articleId, bool isNew = false)
		{
			try
			{
				var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleId);
				if(artikelEntity != null)
				{
					//TODO: Update to use CustomerItemNumber
					var customerItemNumber = string.IsNullOrWhiteSpace(artikelEntity.CustomerItemNumber) ? artikelEntity.Bezeichnung1 : artikelEntity.CustomerItemNumber;
					var fileContent = "";
					var B2 = customerItemNumber?.Substring(0, Math.Min(customerItemNumber.Length, 37));

					// -
					if(isNew)
					{
						fileContent = $"A;{artikelEntity.ArtikelNr};;1;{artikelEntity.ArtikelNummer};{B2};STK;1;;0;0;;;;;;;;;;";
					}
					else
					{
						fileContent = $"M;{artikelEntity.ArtikelNr};;1;{artikelEntity.ArtikelNummer};{B2};STK;1;;0;0;;;;;;;;;;";
					}

					// -
					var filePath = System.IO.Path.Combine(Module.AppSettings.WMSArticleDATPath, $"AST{DateTime.Now.ToString("yyyyMMddHHmmss")}.dat");
					System.IO.File.WriteAllTextAsync(filePath, fileContent);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
		}
		public static void generateFileDAT(int articleId, bool isNew/* = false*/, TransactionsManager botransaction)
		{
			try
			{
				var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(articleId, botransaction.connection, botransaction.transaction);
				if(artikelEntity != null)
				{
					//TODO: Update to use CustomerItemNumber
					var customerItemNumber = string.IsNullOrWhiteSpace(artikelEntity.CustomerItemNumber) ? artikelEntity.Bezeichnung1 : artikelEntity.CustomerItemNumber;
					var fileContent = "";
					var B2 = customerItemNumber?.Substring(0, Math.Min(customerItemNumber.Length, 37));

					// -
					if(isNew)
					{
						fileContent = $"A;{artikelEntity.ArtikelNr};;1;{artikelEntity.ArtikelNummer};{B2};STK;1;;0;0;;;;;;;;;;";
					}
					else
					{
						fileContent = $"M;{artikelEntity.ArtikelNr};;1;{artikelEntity.ArtikelNummer};{B2};STK;1;;0;0;;;;;;;;;;";
					}

					// -
					var filePath = System.IO.Path.Combine(Module.AppSettings.WMSArticleDATPath, $"AST{DateTime.Now.ToString("yyyyMMddHHmmss")}.dat");
					System.IO.File.WriteAllTextAsync(filePath, fileContent);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
		}

		public static void addToLagers(int articleId)
		{
			try
			{
				var mTransaction = new Infrastructure.Services.Utils.TransactionsManager();
				mTransaction.beginTransaction();

				// -
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(articleId, mTransaction.connection, mTransaction.transaction);
				if(articleEntity == null)
					return;

				// -
				var lagerorteEntities = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetCreationLagers()
					?? new List<Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity>();
				var standardLagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetStandardByArticleAndId(articleEntity.ArtikelNr, lagerorteEntities.Select(x => x.Lagerort_id)?.ToList())
					?? new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();

				// -
				var newLagers = new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
				foreach(var lagerortItem in lagerorteEntities)
				{
					// - not existing ones
					if(standardLagerEntities.FindIndex(x => x.Lagerort_id == lagerortItem.Lagerort_id) < 0)
					{
						newLagers.Add(
							new Infrastructure.Data.Entities.Tables.PRS.LagerEntity
							{
								Artikel_Nr = articleEntity.ArtikelNr,
								Lagerort_id = lagerortItem.Lagerort_id,
								Bestand = 0,
								letzte_Bewegung = DateTime.Now,
								Durchschnittspreis = 0,
								CCID = false,
								Bestand_reserviert = 0,
								Mindestbestand = 0,
								GesamtBestand = 0,
							});
					}
				}
				// -
				var id = Infrastructure.Data.Access.Tables.PRS.LagerAccess.InsertWithTransaction(newLagers, mTransaction.connection, mTransaction.transaction);
				Infrastructure.Data.Access.Tables.BSD.PSZ_Eingangskontrolle_PrufungenAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.BSD.PSZ_Eingangskontrolle_PrufungenEntity
					{
						Artikelnummer = articleEntity.ArtikelNummer,
						Hilfsmittel = "Visuell",
						Prufung = "Viz příchozí složky"
					}, mTransaction.connection, mTransaction.transaction);

				//
				if(mTransaction.commit() != true)
				{
					throw new Exception("Error in transaction");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
			}
		}
		string getArticleNumber(
			Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity customerEntity,
			List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> customerItemNumberEntities,
			List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> customerIndexEntities,
			Infrastructure.Data.Entities.Tables.WPL.CountryEntity countryEntity,
			Infrastructure.Data.Entities.Tables.WPL.HallEntity hallEntity,
			List<string> errors)
		{
			customerEntity = customerEntity ?? Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(this._data.CustomerId) ?? new Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity();
			customerItemNumberEntities = customerItemNumberEntities ?? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(this._data.CustomerId, customerEntity.Nummerschlüssel, this._data.CustomerItemNumber, this._data.IsArticleNumberSpecial, true) ?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			customerIndexEntities = customerIndexEntities ?? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemIndex(this._data.CustomerId, customerEntity.Nummerschlüssel, this._data.CustomerItemNumber, this._data.CustomerItemIndex, true) ?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
			countryEntity = countryEntity ?? Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(this._data.ProductionCountryId) ?? new Infrastructure.Data.Entities.Tables.WPL.CountryEntity();
			hallEntity = hallEntity ?? Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(this._data.ProductionSiteId) ?? new Infrastructure.Data.Entities.Tables.WPL.HallEntity();


			// -
			return $"{customerEntity.Kundennummer?.ToString().Trim()}-{(customerItemNumberEntities[0]?.CustomerItemNumberSequence ?? 0).ToString().PadLeft(6)}-{(customerItemNumberEntities[0]?.CustomerIndexSequence ?? 0).ToString().PadLeft(2)}-{(hallEntity.LagerortId ?? 0).ToString().PadLeft(2)}{(countryEntity.Name)}";
		}
	}
}
