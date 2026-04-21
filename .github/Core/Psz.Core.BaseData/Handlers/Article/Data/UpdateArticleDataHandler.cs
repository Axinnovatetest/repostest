using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	public class UpdateArticleDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ArticleDataModel _data { get; set; }
		public UpdateArticleDataHandler(Identity.Models.UserModel user, Models.Article.ArticleDataModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				lock(Locks.ArticleEditLock.GetOrAdd(this._data.ArtikelNr, new object()))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}


					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
					if(this._data.Index_Kunde?.Trim()?.ToLower() != articleEntity.Index_Kunde?.Trim()?.ToLower()
						|| this._data.Index_Kunde_Datum != articleEntity.Index_Kunde_Datum)
					{
						// - BOM Update
						var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArtikelNr);
						if(articleExtEntity != null)
						{
							if(articleExtEntity.BomStatusId == (int)Enums.ArticleEnums.BomStatus.Approved)
							{
								articleExtEntity.LastUpdateTime = DateTime.Now;
								articleExtEntity.LastUpdateUserId = this._user.Id;
								articleExtEntity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation;
								articleExtEntity.BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription();
								articleExtEntity.BomVersion = articleExtEntity.BomVersion + 1;
								Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.Update(articleExtEntity);

								// -- Article level Logging
								Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
									new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
                                    // - Status change log
                                    ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Article BOM Status from", $"{Enums.ArticleEnums.BomStatus.Approved.GetDescription()}",
									$"{Enums.ArticleEnums.BomStatus.InPreparation.GetDescription()}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit),
                                    // - Version change log
                                    ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Article BOM Version from", $"{articleExtEntity.BomVersion - 1}",
									$"{articleExtEntity.BomVersion}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit)
									});

								// -- BOM level logging
								Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(
									new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> {
                                    // - Status change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									Enums.ArticleEnums.BomStatus.Approved.GetDescription(), Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(), Enums.ObjectLogEnums.BOMLogType.StatusChange),
                                    // - Version change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									((articleExtEntity.BomVersion?? 0) -1).ToString(), articleExtEntity.BomVersion?.ToString(), Enums.ObjectLogEnums.BOMLogType.Version)
									});
							}
						}
						else
						{
							Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.Insert(
								new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
								{
									ArticleId = this._data.ArtikelNr,
									ArticleDesignation = this._data.Bezeichnung1,
									ArticleNumber = this._data.ArtikelNummer,
									BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(),
									BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation,
									//BomValidFrom,
									BomVersion = 0,
									LastUpdateTime = DateTime.Now,
									LastUpdateUserId = this._user.Id
								});
						}

						// - save KundenIndex History
						Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.Insert(
							new Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity
							{
								Artikel_Nr = this._data.ArtikelNr,
								Datum_Anderung = DateTime.Now,
								ID = -1, // not needed coz AutoIncrement
								Anderungsbereich = $"Index Kunden changed from [{articleEntity.Index_Kunde}] to [{this._data.Index_Kunde}]",
								Anderungsbeschreibung = this._data.Index_Kunde_ChangeReason,
								Anderung_von = this._user.Username
							});
					}

					var logs = LogChanges();
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);

					// - 
					var articleExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data.ArtikelNr);
					var updateId = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditData(this._data.ToEntity());
					if(articleExtensionEntity != null)
					{
						Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.Update(this._data.ToExtensionEntity());
					}
					else
					{
						Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.Insert(this._data.ToExtensionEntity());
					}

					// - 2024-02-13
					var responseBody = new ResponseModel<int>();
					// - allow only St for Warentyp 1:Stuckware - and prevent the opposite
					if(this._data.Einheit?.ToLower()?.Trim() == "st" && this._data.Warentyp != 1)
					{
						responseBody.Warnings.Add($"[Einheit]: value [{this._data.Einheit}] is valid ony for Warentyp [Stückware].");
					}
					if(this._data.Einheit?.ToLower()?.Trim() != "st" && this._data.Warentyp == 1)
					{
						responseBody.Warnings.Add($"[Einheit]: invalid value [{this._data.Einheit}], [Stückware] accepts only [St].");
					}

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArtikelNr);

					// - 
					return ResponseModel<int>.SuccessResponse(updateId);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(string.IsNullOrWhiteSpace(this._data.ArtikelNummer))
				return ResponseModel<int>.FailureResponse($"[Artikelnummer] should not be empty");

			if(string.IsNullOrWhiteSpace(this._data.Einheit))
				return ResponseModel<int>.FailureResponse($"[Einheit] invalid value '{this._data.Einheit}'");
			//if (string.IsNullOrWhiteSpace(this._data.Bezeichnung1))
			//    return ResponseModel<int>.FailureResponse("[Herstellernummer/Manufacturer Number] should not be empty");

			var artikelSameNummer = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArtikelNummer);
			if(artikelSameNummer != null && artikelSameNummer.ArtikelNr != this._data.ArtikelNr)
				return ResponseModel<int>.FailureResponse("[Artikelnummer] exists");

			//var artikelSameBezeichnung1 = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByKundeBezeichnung(this._data.ArtikelNummer.Trim(), this._data.Bezeichnung1.Trim());
			//if (artikelSameBezeichnung1 != null && artikelSameBezeichnung1.Count>0 && artikelSameBezeichnung1.Exists(x=> x.ArtikelNr != this._data.ArtikelNr) == true)
			//    return ResponseModel<int>.FailureResponse($"[Herstellernummer/Manufacturer Number] exists in [{string.Join(", ",artikelSameBezeichnung1.Take(5).Select(x=> x.ArtikelNummer).ToList())}{(artikelSameBezeichnung1.Count > 5?" ...":"")}]");

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse("2", "Article not found");

			if(articleEntity.aktiv.HasValue && !articleEntity.aktiv.Value)
				return ResponseModel<int>.FailureResponse("3", "Article is not Active");

			if(this._data.ArtikelNummer?.Trim()?.ToLower() != articleEntity.ArtikelNummer?.Trim()?.ToLower())
			{
				var parentBIds = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetParentIds(this._data.ArtikelNr);
				if(parentBIds != null && parentBIds.Count > 0)
				{
					var parentBOMEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(parentBIds?.Distinct()?.ToList())?.Take(5).ToList();
					return ResponseModel<int>.FailureResponse($"Article is used in BOM for [{string.Join(", ", parentBOMEntities.Select(x => x.ArtikelNummer))}]");
				}
			}

			if(this._data.Index_Kunde?.Trim()?.ToLower() != articleEntity.Index_Kunde?.Trim()?.ToLower()
				&& string.IsNullOrWhiteSpace(this._data.Index_Kunde_ChangeReason))
				return ResponseModel<int>.FailureResponse("4", "[Kunden Index] changed, please add a reason.");
			if(this._data.Index_Kunde_Datum != articleEntity.Index_Kunde_Datum
				&& string.IsNullOrWhiteSpace(this._data.Index_Kunde_ChangeReason))
				return ResponseModel<int>.FailureResponse("4", "[Kunden Index Datum] changed, please add a reason.");

			// - 2022-06-08 - changing Warengruppe to/from EF
			if(this._data.Warengruppe?.Trim()?.ToLower() != articleEntity.Warengruppe?.Trim()?.ToLower()
				&& (this._data.Warengruppe?.Trim()?.ToLower() == "ef" || articleEntity.Warengruppe?.Trim()?.ToLower() == "ef"))
			{
				//if(this._data.Warengruppe?.Trim()?.ToLower() == "ef")
				//{
				//    var kreis = this._data.ArtikelNummer?.Trim().Substring(0, 3);
				//    var customers = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(kreis);
				//    if (customers==null || customers.Count <=0)
				//    {
				//        return ResponseModel<int>.FailureResponse($"EF Article Nummerschlüssel [{kreis}] does not exist.");
				//    }
				//    
				//}
				//if (articleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
				//{
				//}

				// - 2022-08-12 - 
				return ResponseModel<int>.FailureResponse($"Warengruppe change is not allowed.");
			}


			if(articleEntity.Warengruppe?.Trim()?.ToLower() == "ef" && this._data.ProjectTypeId.HasValue == false)
			{
				return ResponseModel<int>.FailureResponse("Article EF must have a [Project Type (Projektart FG)].");
			}
			if(articleEntity.Warengruppe?.Trim()?.ToLower() == "ef" && string.IsNullOrWhiteSpace(this._data.artikelklassifizierung))
			{
				return ResponseModel<int>.FailureResponse("Article EF must have a [Project Classification (Projektklassifizierung)].");
			}
			// - 2023-01-03 - allow nummer change
			if(this._data.ArtikelNummer != articleEntity.ArtikelNummer)
			{
				// - 
				if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArtikelNummer?.Trim()) != null)
				{
					return ResponseModel<int>.FailureResponse($"[{this._data.ArtikelNummer}] exists.");
				}


				if(articleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
				{
					var itemParts = this._data.ArtikelNummer?.Trim().Split('-')?.ToList();
					if(itemParts == null || itemParts.Count < 3)
					{
						return ResponseModel<int>.FailureResponse($"Article Number structure invalid.");
					}
					var kreis = itemParts[0];
					if(kreis != articleEntity.CustomerPrefix)
					{
						return ResponseModel<int>.FailureResponse($"Article Kreis should not change.");
					}
					var itemNumberSeq = itemParts[1];
					var itemIndexSeq = itemParts[2]?.Substring(0, 2);
					var warehouseSeq = itemParts[2]?.Substring(2, 2);
					var countrySeq = itemParts[2]?.Substring(4);
					if((int.TryParse(warehouseSeq, out var _seq) ? _seq : -1) != articleEntity.ProductionSiteSequence)
					{
						return ResponseModel<int>.FailureResponse($"Article Site should not change.");
					}
					if(countrySeq?.Trim()?.ToLower() != articleEntity.ProductionCountryCode?.Trim()?.ToLower())
					{
						return ResponseModel<int>.FailureResponse($"Article Land should not change.");
					}
				}

				// - 1 article not in STL
				var stk = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticleUBG(articleEntity.ArtikelNr);
				if(stk != null && stk.Count > 0)
				{
					var hbg = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(stk.Select(x => x.Artikel_Nr ?? -1).Distinct().Take(5).ToList());
					return ResponseModel<int>.FailureResponse($"[{articleEntity.ArtikelNummer}] is used in STL [{string.Join(",", hbg.Select(x => x.ArtikelNummer))}].");
				}
				// - 2 article not in AB/LS/RA/...
				var pos = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyArticleIds(new List<int> { articleEntity.ArtikelNr });
				if(pos != null && pos.Count > 0)
				{
					var docs = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(pos.Select(x => x.AngebotNr ?? -1).Distinct().Take(5).ToList());
					return ResponseModel<int>.FailureResponse($"[{articleEntity.ArtikelNummer}] is used in Projects [{string.Join(",", docs.Select(x => x.Projekt_Nr))}].");
				}
				// - 3 - article not in Prod
				var fas = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByArticles(new List<int> { articleEntity.ArtikelNr });
				if(fas != null && fas.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"[{articleEntity.ArtikelNummer}] is used in FAs [{string.Join(",", fas.Select(x => x.Fertigungsnummer))}].");
				}
				// - 4 - article not in FaPos
				var posFa = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByArticles(new List<int> { articleEntity.ArtikelNr });
				if(posFa != null && posFa.Count > 0)
				{
					var pfas = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Get(posFa.Select(x => x.ID_Fertigung ?? -1).Distinct().Take(5).ToList());
					return ResponseModel<int>.FailureResponse($"[{articleEntity.ArtikelNummer}] is used in FAs [{string.Join(",", pfas.Select(x => x.Fertigungsnummer))}].");
				}
				// - 5 - article not in BestPos
				var bArts = Infrastructure.Data.Access.Tables.BSD.Bestellte_ArtikelAccess.GetByArticles(new List<int> { articleEntity.ArtikelNr });
				if(bArts != null && bArts.Count > 0)
				{
					var best = Infrastructure.Data.Access.Tables.BSD.BestellungenAccess.Get(bArts.Select(x => x.Bestellung_Nr ?? -1).Distinct().Take(5).ToList());
					return ResponseModel<int>.FailureResponse($"[{articleEntity.ArtikelNummer}] is used in Bestellung [{string.Join(",", best.Select(x => x.Projekt_Nr))}].");
				}
			}

			if(this._data.Warentyp != articleEntity.Warentyp && !this._data.Warentyp.HasValue)
			{
				var rohGoodsGroupEntity = (Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get()
				   ?? new List<Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity>()).Find(x => x.Warengruppe.Trim().ToLower() == "roh");
				if(this._data.Warengruppe?.Trim()?.ToLower() == rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower())
				{
					return ResponseModel<int>.FailureResponse($"Article ROH must have a [Waren Type]");
				}
			}

			// - 2024-01-25 - Einheit as dropdown
			var units = Infrastructure.Data.Access.Tables.BSD.UnitOfMeasureAccess.GetBySymbol(this._data.Einheit);
			if(units?.Count <= 0)
			{
				return ResponseModel<int>.FailureResponse($"[Einheit]: invalid value [{this._data.Einheit}].");
			}

			// - 2024-02-13 - They change their mind, and want to mix-and-match :-)
			////// - allow only St for Warentyp 1:Stuckware - and prevent the opposite
			////if(this._data.Einheit?.ToLower()?.Trim()=="st" && this._data.Warentyp != 1)
			////{
			////	return ResponseModel<int>.FailureResponse($"[Einheit]: value [{this._data.Einheit}] is valid ony for Warentyp [Stückware].");
			////}
			////if(this._data.Einheit?.ToLower()?.Trim() != "st" && this._data.Warentyp == 1)
			////{
			////	return ResponseModel<int>.FailureResponse($"[Einheit]: invalid value [{this._data.Einheit}], [Stückware] accepts only [St].");
			////}
			return ResponseModel<int>.SuccessResponse();
		}
		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			var articleExtension = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data.ArtikelNr);
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Edit;

			if(articleEntity.ArtikelNummer != this._data.ArtikelNummer)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "ArtikelNummer", articleEntity.ArtikelNummer, this._data.ArtikelNummer, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleEntity.Bezeichnung1 != this._data.Bezeichnung1)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Bezeichnung1", articleEntity.Bezeichnung1, this._data.Bezeichnung1, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			if(articleEntity.Bezeichnung2 != this._data.Bezeichnung2)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Bezeichnung2", articleEntity.Bezeichnung2, this._data.Bezeichnung2, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			// Kunde Index 
			if(articleEntity.Index_Kunde != this._data.Index_Kunde)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "KundeIndex", articleEntity.Index_Kunde, this._data.Index_Kunde, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Index_Kunde_Datum != this._data.Index_Kunde_Datum)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "IndexKunde Datum", articleEntity.Index_Kunde_Datum?.ToString("dd/MM/yyyy"), this._data.Index_Kunde_Datum?.ToString("dd/MM/yyyy"), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}

			// Copper attributes
			if(articleEntity.CuGewicht != this._data.CuGewicht)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CuGewicht", articleEntity.CuGewicht?.ToString(), this._data.CuGewicht?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.DEL != this._data.DEL)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "DEL", articleEntity.DEL?.ToString(), this._data.DEL?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.VKFestpreis != this._data.VKFestpreis)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "VKFestpreis", articleEntity.VKFestpreis?.ToString(), this._data.VKFestpreis?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.Warengruppe != this._data.Warengruppe)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Warengruppe", articleEntity.Warengruppe, this._data.Warengruppe, Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));

			}
			if(articleEntity.ULzertifiziert != this._data.ULzertifiziert)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "UL Zertifiziert", articleEntity.ULzertifiziert?.ToString(), this._data.ULzertifiziert?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
			}
			if(articleEntity.ROHSEEEConfirmity != this._data.ROHS_EEE_Confirmity)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "ROHS EEE Confirmity", articleEntity.ULzertifiziert?.ToString(), this._data.ROHS_EEE_Confirmity?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));

			}
			if(articleEntity.Warentyp != this._data.Warentyp)
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Warentyp", articleEntity.Warentyp?.ToString(), this._data.Warentyp?.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));

			}

			if(articleExtension != null)
			{
				if(articleExtension.CopperCostBasis != this._data.CopperCostBasis)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis", articleExtension.CopperCostBasis.ToString(), this._data.CopperCostBasis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
				if(articleExtension.CopperCostBasis150 != this._data.CopperCostBasis150)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis150", articleExtension.CopperCostBasis150.ToString(), this._data.CopperCostBasis150.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
			}
			else
			{
				if(this._data.CopperCostBasis.HasValue)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis", "", this._data.CopperCostBasis.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
				if(this._data.CopperCostBasis150.HasValue)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "CopperCostBasis150", "", this._data.CopperCostBasis150.ToString(), Enums.ObjectLogEnums.Objects.Article.GetDescription(), logTypeEdit));
				}
			}
			//
			return logs;
		}
	}
}
