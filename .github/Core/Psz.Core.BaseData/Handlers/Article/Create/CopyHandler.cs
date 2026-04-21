using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class CopyHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.CopyRequestModel _data { get; set; }

		public CopyHandler(Identity.Models.UserModel user, Models.Article.CopyRequestModel data)
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

				var originalArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.CopyFromArticleId);
				var countryEntity = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(this._data.ProductionCountryId);
				var siteEntity = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(this._data.ProductionSiteId);
				var itemParts = this._data.ArticleNumber?.Trim().Split('-')?.ToList();
				int customerNumberSeq = 0;
				int customerIndexSeq = 0;
				var originalArticleNumber = originalArticleEntity.ArtikelNummer;

				#region >>> Data sequences <<<
				if(originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
				{
					var maxCustomerSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerMaxNumberSequence(originalArticleEntity.CustomerNumber ?? -1, itemParts[0]);
					if(maxCustomerSeq < 0)
					{
						// - no articles for Customer
					}
					else
					{
						// - current ItemNumber
						customerNumberSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNumberSequence(originalArticleEntity.CustomerNumber ?? -1, itemParts[0], this._data.CustomerItemNumber);
						if(customerNumberSeq < 0)
						{
							// - current ItemNumber does not exist
							customerNumberSeq = maxCustomerSeq + 1;
						}
						else
						{
							// - current Index
							customerIndexSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerIndexSequence(originalArticleEntity.CustomerNumber ?? -1, itemParts[0], this._data.CustomerItemNumber, this._data.CustomerItemIndex, originalArticleEntity.IsArticleNumberSpecial ?? false);
							if(customerIndexSeq < 0)
							{
								// - current Index does not exist
								customerIndexSeq = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNextIndexSequence(originalArticleEntity.CustomerNumber ?? -1, itemParts[0], this._data.CustomerItemNumber);
							}
						}
					}
				}
				#endregion data sequences

				if(!this._data.IsArticleNumberCustom && originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
				{
					this._data.ArticleNumber = Data.UpdateCustomerIndexHandler.getNewArticleNumber(originalArticleEntity.CustomerPrefix, customerNumberSeq,
						customerIndexSeq, this._data.ProductionSiteCode, this._data.ProductionCountryCode);
				}

				// - 2023-01-22 - remove old EdiDefault if any
				if(originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef" && this._data.IsEdiDefault == true)
				{
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.ResetCustomerEdiDefaultWithTransaction(originalArticleEntity.CustomerNumber ?? -1, this._data.CustomerItemNumber, botransaction.connection, botransaction.transaction);
				}
				// - 2023-08-20 - Original country linked to production country for EF
				if(originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
				{
					originalArticleEntity.Ursprungsland = _data.ProductionCountryCode;
				}

				originalArticleEntity.ArtikelNummer = this._data.ArticleNumber;
				originalArticleEntity.ArticleNumber = this._data.ArticleNumber;
				originalArticleEntity.Bezeichnung1 = this._data.Designation;
				originalArticleEntity.CustomerItemNumber = this._data.CustomerItemNumber;
				originalArticleEntity.CustomerItemNumberSequence = customerNumberSeq;
				originalArticleEntity.CustomerIndex = this._data.CustomerItemIndex;
				originalArticleEntity.Index_Kunde = this._data.CustomerItemIndex;
				originalArticleEntity.Index_Kunde_Datum = this._data.CustomerItemIndexDate;
				originalArticleEntity.CustomerIndexSequence = customerIndexSeq;
				originalArticleEntity.ProductionCountryCode = countryEntity?.Designation;
				originalArticleEntity.ProductionCountryName = countryEntity?.Name;
				originalArticleEntity.ProductionCountrySequence = countryEntity?.MtdArticleSequence ?? 0;
				originalArticleEntity.ProductionSiteCode = $"{(siteEntity?.LagerortId ?? 0).ToString("D2")}";
				originalArticleEntity.ProductionSiteName = siteEntity?.Name;
				originalArticleEntity.ProductionSiteSequence = siteEntity?.LagerortId ?? 0;
				// - remove rahmen data
				originalArticleEntity.Rahmen = null;
				originalArticleEntity.Rahmen2 = null;
				originalArticleEntity.Rahmenauslauf = null;
				originalArticleEntity.Rahmenauslauf2 = null;
				originalArticleEntity.Rahmenmenge = null;
				originalArticleEntity.Rahmenmenge2 = null;
				originalArticleEntity.RahmenNr = null;
				originalArticleEntity.RahmenNr2 = null;
				originalArticleEntity.Freigabestatus = "N";
				originalArticleEntity.PrufstatusTNWare = "T";
				originalArticleEntity.FreigabestatusTNIntern = "N";
				// - 2023-01-11 - Ridha
				originalArticleEntity.Blokiert_Status = false;
				originalArticleEntity.Kupferbasis = 150;// - 2023-01-18 - Khelil
				originalArticleEntity.EdiDefault = originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef" ? this._data.IsEdiDefault : false; // - 2023-01-22 - Sax & Schremmer
																																				   
				originalArticleEntity.COF_Pflichtig = this._data.CocActive;// - 2023-08-24
				originalArticleEntity.CocVersion = this._data.CocActive == true ? this._data.CocVersion : null;

				originalArticleEntity.IsEDrawing = this._data.IsEDrawing;


				var insertedNr = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.InsertWithTransaction(originalArticleEntity, botransaction.connection, botransaction.transaction);

				var insertedExtension = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity
					{
						ArtikelNr = insertedNr,
						CreatorID = this._user.Id,
						DateCreation = DateTime.Now
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
				if(this._data.WithBOM)
				{
					var originalArticleBOM = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(originalArticleEntity.ArtikelNr);
					if(originalArticleBOM != null && originalArticleBOM.Count > 0)
					{
						originalArticleBOM.ForEach(x => x.Artikel_Nr = insertedNr);
						Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.InsertWithTransaction(originalArticleBOM, botransaction.connection, botransaction.transaction);
					}
				}
				#endregion BOM

				#region // - Prod - //
				Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity
					{
						ArticleId = insertedNr,
						CreateTime = DateTime.Now,
						CreateUserId = this._user.Id,
						ProductionPlace1_Id = int.TryParse(originalArticleEntity.ProductionSiteCode, out var v) ? v : 0,
						ProductionPlace1_Name = originalArticleEntity.ProductionSiteName,
					}, botransaction.connection, botransaction.transaction);
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

				// - logs
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, insertedNr, $"Article", originalArticleNumber, this._data.ArticleNumber, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.AddFromCopy),
					botransaction.connection, botransaction.transaction);

				#endregion transaction-based logic

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2022-04-22 - async 
					System.Threading.Tasks.Task.Factory.StartNew(() => CreateHandler.addToLagers(insertedNr));

					// - 2022-03-30
					CreateHandler.generateFileDAT(insertedNr, isNew: true);

					// -
					return ResponseModel<int>.SuccessResponse(insertedNr);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
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

			var fromArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.CopyFromArticleId);
			if(fromArticleEntity == null)
			{
				return ResponseModel<int>.FailureResponse($"Original Article not found.");
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
			var itemParts = this._data.ArticleNumber?.Trim().Split('-')?.ToList();
			if(itemParts == null || itemParts.Count < 3)
			{
				return ResponseModel<int>.FailureResponse($"Article Number structure invalid.");
			}
			#endregion - all Articles

			// -
			var kreis = itemParts[0]; //?.Substring(0, 3);
			var customers = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(kreis);

			// - 2022-06-08
			if(fromArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
			{
				#region --- prems ---
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

				// - 1 - ItemNumber
				if(string.IsNullOrWhiteSpace(this._data.CustomerItemNumber))
				{
					return ResponseModel<int>.FailureResponse($"Customer Item Number invalid value [{this._data.CustomerItemNumber}].");
				}
				// - 2 - Index
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
				var sameCustomerItems = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(fromArticleEntity.CustomerNumber ?? -1, itemParts[0], this._data.CustomerItemNumber, fromArticleEntity.IsArticleNumberSpecial ?? false);
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
					return ResponseModel<int>.FailureResponse(new List<string> { $"Kreis [{kreis}] exists for customers [{string.Join(", ", customers.Take(5).Select(x => $"{x.Kundennummer} | {x.Kunde}"))}].", $"Change article to EF or use another Kreis." });
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

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
