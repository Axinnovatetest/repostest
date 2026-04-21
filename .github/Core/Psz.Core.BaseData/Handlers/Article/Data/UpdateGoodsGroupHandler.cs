using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateGoodsGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.UpdateGoodsGroupRequestModel _data { get; set; }

		public UpdateGoodsGroupHandler(Identity.Models.UserModel user, Models.Article.UpdateGoodsGroupRequestModel data)
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

				var originalArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.OriginalArticleId);
				var newIndexSequence = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNextIndexSequence(originalArticleEntity.CustomerNumber ?? -1, originalArticleEntity.ArtikelNummer?.Substring(0, originalArticleEntity.ArtikelNummer?.IndexOf('-') ?? 0), originalArticleEntity.CustomerItemNumber);


				originalArticleEntity.ArticleNumber = this._data.ArticleNumber;
				originalArticleEntity.ArtikelNummer = this._data.ArticleNumber;
				if(this._data.GoodsGroupName?.Trim()?.ToLower() == "ef")
				{
					originalArticleEntity.CustomerIndex = this._data.CustomerItemIndex;
					originalArticleEntity.CustomerIndexSequence = newIndexSequence; // - next IndexSeq
					originalArticleEntity.Index_Kunde = this._data.CustomerItemIndex;
					originalArticleEntity.Index_Kunde_Datum = this._data.CustomerIndexDate;

					var countryEntity = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(this._data.ProductionCountryId);
					originalArticleEntity.ProductionCountryCode = countryEntity.Designation;
					originalArticleEntity.ProductionCountryName = countryEntity.Name;
					originalArticleEntity.ProductionCountrySequence = countryEntity.MtdArticleSequence;
					// -
					var siteEntity = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(this._data.ProductionSiteId);
					originalArticleEntity.ProductionSiteCode = $"{(siteEntity.LagerortId ?? 0).ToString("D2")}";
					originalArticleEntity.ProductionSiteName = siteEntity.Name;
					originalArticleEntity.ProductionSiteSequence = siteEntity.LagerortId;
				}
				else
				{
					originalArticleEntity.Warengruppe = this._data.GoodsGroupName;
				}

				// - save new data
				var insertedNr = this._data.OriginalArticleId;
				Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.UpdateWithTransaction(
				originalArticleEntity, botransaction.connection, botransaction.transaction);


				#region // - BOM - //
				var bomExtension = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(insertedNr);
				if(bomExtension == null)
				{
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
						{
							ArticleId = insertedNr,
							ArticleDesignation = originalArticleEntity.Bezeichnung1,
							ArticleNumber = this._data.ArticleNumber,
							BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(),
							BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation,
							BomVersion = 0,
							BomValidFrom = null
						}, botransaction.connection, botransaction.transaction);
				}
				else
				{
					bomExtension.ArticleNumber = this._data.ArticleNumber;
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.UpdateWithTransaction(bomExtension, botransaction.connection, botransaction.transaction);
				}
				#endregion BOM

				#region // - Prod - //
				var prodExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(insertedNr);
				if(prodExtension == null)
				{
					Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.InsertWithTransaction(
							new Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity
							{
								ArticleId = insertedNr,
								CreateTime = DateTime.Now,
								CreateUserId = this._user.Id,
								ProductionPlace1_Id = int.TryParse(originalArticleEntity.ProductionSiteCode, out var v) ? v : 0,
								ProductionPlace1_Name = originalArticleEntity.ProductionSiteName,
							}, botransaction.connection, botransaction.transaction);
				}
				else
				{
					prodExtension.ProductionPlace1_Id = int.TryParse(originalArticleEntity.ProductionSiteCode, out var v) ? v : 0;
					prodExtension.ProductionPlace1_Name = originalArticleEntity.ProductionSiteName;
					Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.InsertWithTransaction(prodExtension, botransaction.connection, botransaction.transaction);
				}
				#endregion Prod

				#region // - Quality - // 
				var qualityExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(insertedNr);
				if(qualityExtension == null)
				{
					Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.BSD.ArtikelQualityExtensionEntity
						{
							ArticleId = insertedNr,
							CreateTime = DateTime.Now,
							CreateUserId = this._user.Id,
						}, botransaction.connection, botransaction.transaction);
				}
				#endregion Quality

				#region // - Logistics - // 
				var logisticsExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.GetByArticleId(insertedNr);
				if(logisticsExtension == null)
				{
					Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.InsertWithTransaction(
						new Infrastructure.Data.Entities.Tables.BSD.ArtikelLogisticsExtensionEntity
						{
							ArticleId = insertedNr,
							CreateTime = DateTime.Now,
							CreateUserId = this._user.Id,
						}, botransaction.connection, botransaction.transaction);
				}
				#endregion Logistics

				// - logs
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(this._user, insertedNr, "Warengruppe", originalArticleEntity.Warengruppe, this._data.GoodsGroupName, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit),
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
			var originalArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.OriginalArticleId);
			if(originalArticleEntity == null)
			{
				return ResponseModel<int>.FailureResponse($"Original Article not found.");
			}
			if(Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get(this._data.GoodsGroupId) == null)
			{
				return ResponseModel<int>.FailureResponse($"Goods Group (Warengruppe) [{this._data.GoodsGroupName}] not found.");
			}
			if(this._data.GoodsGroupName?.Trim()?.ToLower() == originalArticleEntity.Warengruppe?.Trim()?.ToLower())
			{
				return ResponseModel<int>.FailureResponse($"Goods Group (Warengruppe) [{this._data.GoodsGroupName}] not changed.");
			}
			if(!this._data.OverwriteArticleNumber)
			{
				if(this._data.ArticleNumber != originalArticleEntity.ArtikelNummer)
				{
					return ResponseModel<int>.FailureResponse($"Article number [{this._data.ArticleNumber}] is different from original [{originalArticleEntity.ArtikelNummer}].");
				}
			}
			else
			{
				var newArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber);
				if(this._data.ArticleNumber != originalArticleEntity.ArtikelNummer && newArticle?.ArtikelNr != originalArticleEntity.ArtikelNr)
				{
					return ResponseModel<int>.FailureResponse($"New article number [{this._data.ArticleNumber}] exists.");
				}
			}
			#endregion - all Articles


			var itemParts = originalArticleEntity.ArtikelNummer?.Trim().Split('-')?.ToList();
			// -
			var kreis = itemParts[0]?.Substring(0, 3);
			var customers = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(kreis);

			// - Group : from EF ->> nonEF
			if(this._data.GoodsGroupName?.Trim()?.ToLower() != "ef")
			{
				#region ROH
				var rohGoodsGroupEntity = (Infrastructure.Data.Access.Tables.PRS.WarengruppenAccess.Get()
					?? new List<Infrastructure.Data.Entities.Tables.PRS.WarengruppenEntity>()).Find(x => x.Warengruppe.Trim().ToLower() == "roh");
				if(this._data.GoodsGroupName.Trim().ToLower() == rohGoodsGroupEntity?.Warengruppe?.Trim().ToLower()
					&& !this._data.GoodsTypeId.HasValue)
					return ResponseModel<int>.FailureResponse("Article ROH must have a [Waren Type].");
				#endregion - ROH

				// - should not start with a Customer kreis
				if(customers != null && customers.Count > 0)
				{
					return ResponseModel<int>.FailureResponse(new List<string> { $"Kreis [{kreis}] exists for customers [{string.Join(", ", customers.Take(5).Select(x => $"{x.Kundennummer} | {x.Kunde}"))}].", $"Change article to EF or use another Nummerkreis." });
				}
			}
			else
			{
				// - Group : from NonEF ->> EF
				kreis = this._data.CustomerNumber.ToString(); //- "new" customer
				if(kreis != this._data.CustomerNumber.ToString())
				{
					return ResponseModel<int>.FailureResponse($"Selected customer [{this._data.CustomerNumber}] does not match article kreis [{kreis}].");
				}

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

				// - Customer ItemNumber
				var newIndexSequence = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerIndexSequence(this._data.CustomerNumber, itemParts[0], this._data.CustomerItemNumber, this._data.CustomerItemIndex);
				if(newIndexSequence >= 0)
				{
					return ResponseModel<int>.FailureResponse($"Index [{this._data.CustomerItemIndex}] exists for customer [{this._data.CustomerNumber}] and number (BZ1) [{this._data.CustomerItemNumber}].");
				}

				// - Customer ItemNumber
				var sameCustomerItems = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(originalArticleEntity.CustomerNumber ?? -1, itemParts[0], this._data.CustomerItemNumber);
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


			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
