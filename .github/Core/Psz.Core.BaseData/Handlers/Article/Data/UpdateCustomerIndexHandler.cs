using System;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateCustomerIndexHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.UpdateCustomerIndexRequestModel _data { get; set; }

		public UpdateCustomerIndexHandler(Identity.Models.UserModel user, Models.Article.UpdateCustomerIndexRequestModel data)
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
				var oldIndex = originalArticleEntity.Index_Kunde;

				originalArticleEntity.CustomerIndex = this._data.NewCustomerIndex;
				originalArticleEntity.CustomerIndexSequence = newIndexSequence; // - next IndexSeq
				originalArticleEntity.Index_Kunde = this._data.NewCustomerIndex;
				originalArticleEntity.Index_Kunde_Datum = this._data.NewCustomerIndexDate;
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
				if(this._data.IsNewProduction)
				{
					var countryEntity = Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(this._data.ProductionCountryId);
					originalArticleEntity.ProductionCountryCode = countryEntity.Designation;
					originalArticleEntity.ProductionCountryName = countryEntity.Name;
					originalArticleEntity.ProductionCountrySequence = countryEntity.MtdArticleSequence;
					// -
					var siteEntity = Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(this._data.ProductionSiteId);
					originalArticleEntity.ProductionSiteCode = $"{(siteEntity.LagerortId ?? 0).ToString("D2")}";
					originalArticleEntity.ProductionSiteName = siteEntity.Name;
					originalArticleEntity.ProductionSiteSequence = siteEntity.LagerortId;

					// - 2023-08-20 - Original country linked to production country for EF
					if(originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef")
					{
						originalArticleEntity.Ursprungsland = originalArticleEntity.ProductionCountryCode;
					}
				}

				var newArticleNumber = "";

				// - if, CUstomer Item Number is not as Seq, then copy it as is!
				var itemParts = originalArticleEntity.ArtikelNummer?.Trim().Split('-')?.ToList();
				if(int.TryParse(itemParts[1], out var _itemNumber) != true || _itemNumber != (originalArticleEntity.CustomerItemNumberSequence ?? -1))
				{
					newArticleNumber = getNewArticleNumber(originalArticleEntity.CustomerPrefix, itemParts[1],
					newIndexSequence, originalArticleEntity.ProductionSiteCode, originalArticleEntity.ProductionCountryCode);
				}
				else
				{
					newArticleNumber = getNewArticleNumber(originalArticleEntity.CustomerPrefix, originalArticleEntity.CustomerItemNumberSequence ?? -1,
					newIndexSequence, originalArticleEntity.ProductionSiteCode, originalArticleEntity.ProductionCountryCode);
				}

				// - 2023-01-22 - remove old EdiDefault if any
				if(originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef" && this._data.IsEdiDefault == true)
				{
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.ResetCustomerEdiDefaultWithTransaction(originalArticleEntity.CustomerNumber ?? -1, originalArticleEntity.CustomerItemNumber, botransaction.connection, botransaction.transaction);
				}

				originalArticleEntity.ArtikelNummer = newArticleNumber;
				originalArticleEntity.ArticleNumber = newArticleNumber;

				// - 2023-01-11 - Ridha
				originalArticleEntity.Blokiert_Status = false;
				originalArticleEntity.Kupferbasis = 150;// - 2023-01-18 - Khelil
				originalArticleEntity.EdiDefault = originalArticleEntity.Warengruppe?.Trim()?.ToLower() == "ef" ? this._data.IsEdiDefault : false; // - 2023-01-22 - Sax & Schremmer

				// - save new data
				var insertedNr = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.InsertWithTransaction(
					originalArticleEntity, botransaction.connection, botransaction.transaction);

				var artikelExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(originalArticleEntity.ArtikelNr);
				var insertedExtension = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity
					{
						ArtikelNr = insertedNr,
						CreatorID = this._user.Id,
						DateCreation = DateTime.Now,
						ProjectTypeId = artikelExtensionEntity?.ProjectTypeId ?? -1
					}, botransaction.connection, botransaction.transaction);


				#region // - BOM - //
				Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.InsertWithTransaction(
					new Infrastructure.Data.Entities.Tables.BSD.StucklistenArticleExtensionEntity
					{
						ArticleId = insertedNr,
						ArticleDesignation = originalArticleEntity.Bezeichnung1,
						ArticleNumber = originalArticleEntity.ArticleNumber,
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
					ObjectLogHelper.getLog(this._user, insertedNr, "Kundenindex", oldIndex, this._data.NewCustomerIndex, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit),
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
				return ResponseModel<int>.FailureResponse($"Article not found.");
			}
			if(string.IsNullOrWhiteSpace(this._data.NewCustomerIndex))
			{
				return ResponseModel<int>.FailureResponse($"Customer Index [{this._data.NewCustomerIndex}] invalid.");
			}
			if(this._data.NewCustomerIndex == originalArticleEntity.CustomerIndex)
			{
				return ResponseModel<int>.FailureResponse($"Customer Index [{this._data.NewCustomerIndex}] should be different from old [{originalArticleEntity.CustomerIndex}].");
			}
			if(this._data.IsNewProduction)
			{
				if(Infrastructure.Data.Access.Tables.WPL.CountryAccess.Get(this._data.ProductionCountryId) == null)
				{
					return ResponseModel<int>.FailureResponse($"Country [{this._data.ProductionCountryName}] not found");
				}
				if(Infrastructure.Data.Access.Tables.WPL.HallAccess.Get(this._data.ProductionSiteId) == null)
				{
					return ResponseModel<int>.FailureResponse($"Hall [{this._data.ProductionSiteName}] not found");
				}
			}
			#endregion - all Articles

			// - Customer ItemNumber
			var newIndexSequence = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerIndexSequence(originalArticleEntity.CustomerNumber ?? -1, originalArticleEntity.ArtikelNummer?.Substring(0, originalArticleEntity.ArtikelNummer?.IndexOf('-') ?? 0), originalArticleEntity.CustomerItemNumber, this._data.NewCustomerIndex, this._data.ProductionCountryName, this._data.ProductionSiteCode);
			if(newIndexSequence >= 0)
			{
				return ResponseModel<int>.FailureResponse($"Index [{this._data.NewCustomerIndex}] exists for customer [{originalArticleEntity.CustomerNumber}] and number [{originalArticleEntity.CustomerItemNumber}].");
			}

			newIndexSequence = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetCustomerNextIndexSequence(originalArticleEntity.CustomerNumber ?? -1, originalArticleEntity.ArtikelNummer?.Substring(0, originalArticleEntity.ArtikelNummer?.IndexOf('-') ?? 0), originalArticleEntity.CustomerItemNumber);
			//var newArticleNumber = getNewArticleNumber(originalArticleEntity.CustomerPrefix, originalArticleEntity.CustomerItemNumberSequence ?? -1,
			//	newIndexSequence, originalArticleEntity.ProductionSiteCode, originalArticleEntity.ProductionCountryCode);
			var newArticleNumber = "";
			var itemParts = originalArticleEntity.ArtikelNummer?.Trim().Split('-')?.ToList();
			if(int.TryParse(itemParts[1], out var _itemNumber) != true || _itemNumber != (originalArticleEntity.CustomerItemNumberSequence ?? -1))
			{
				newArticleNumber = getNewArticleNumber(originalArticleEntity.CustomerPrefix, itemParts[1],
				newIndexSequence, originalArticleEntity.ProductionSiteCode, originalArticleEntity.ProductionCountryCode);
			}
			else
			{
				newArticleNumber = getNewArticleNumber(originalArticleEntity.CustomerPrefix, originalArticleEntity.CustomerItemNumberSequence ?? -1,
				newIndexSequence, originalArticleEntity.ProductionSiteCode, originalArticleEntity.ProductionCountryCode);
			}
			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(newArticleNumber) != null)
			{
				return ResponseModel<int>.FailureResponse($"New article number [{newArticleNumber}] exists.");
			}
			// -
			return ResponseModel<int>.SuccessResponse();
		}

		public static string getNewArticleNumber(string kreis, int itemSequence, int indexSequence, string siteCode, string countryCode)
		{
			var result = $"{kreis}-{itemSequence.ToString("D3")}-{indexSequence.ToString("D2")}{siteCode}{countryCode}";
			// - 

			return result;
		}
		public static string getNewArticleNumber(string kreis, string itemValue, int indexSequence, string siteCode, string countryCode)
		{
			var result = $"{kreis}-{itemValue}-{indexSequence.ToString("D2")}{siteCode}{countryCode}";
			// - 

			return result;
		}

		/// <summary>
		///  // - 2022-09-21 - // on editCustomerIndex - check for new CustomerIndex in BOM UBG
		/// </summary>
		/// <returns></returns>

	}
}
