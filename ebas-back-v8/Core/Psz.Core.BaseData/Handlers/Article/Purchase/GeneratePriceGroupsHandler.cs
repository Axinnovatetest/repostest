using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Purchase
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GeneratePriceGroupsHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		public Models.Article.Purchase.GeneratePriceGroupRequestModel _data { get; set; }
		public GeneratePriceGroupsHandler(UserModel user, Models.Article.Purchase.GeneratePriceGroupRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				int responseBody = InitPricingGroup(this._data.ArticleId, this._user, botransaction);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// -
					return ResponseModel<int>.SuccessResponse(responseBody);
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

		public static int InitPricingGroup(int articleId, UserModel user, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			// - get Preisgruppe
			var preiseGruppeEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(articleId, botransaction.connection, botransaction.transaction);
			var salesPriceEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndType(articleId, (int)Common.Enums.ArticleEnums.SalesItemType.Serie, botransaction.connection, botransaction.transaction);
			var preiseGruppenVorgaben = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.GetWithTransaction(botransaction.connection, botransaction.transaction)
				?? new List<Infrastructure.Data.Entities.Tables.BSD.Preisgruppen_VorgabenEntity>();
			var newPurchasePrice = (decimal?)(Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticle(articleId, true, botransaction.connection, botransaction.transaction)
				?? new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>())
				.FirstOrDefault()?.Basispreis ?? 0;

			// - no Purchase Prices - Add PreiseGruppe
			if(salesPriceEntity == null
				|| isSalesPrice(salesPriceEntity, preiseGruppeEntity))
			{
				if(preiseGruppeEntity == null)
				{
					// - Insert
					var newPreiseGruppen = preiseGruppenVorgaben.Select(x =>
					new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity
					{
						Artikel_Nr = articleId,
						Preisgruppe = x.Preisgruppe,
						Aufschlag = (decimal?)x.Aufschlag,
						Aufschlagsatz = (decimal?)x.Aufschlagsatz,
						Einkaufspreis = newPurchasePrice,
						// - set null values to 0
						Verkaufspreis = 0,
						brutto = false,
						Preisminderung_1____ = 0,
						bis_Anzahl_Mengeneinheiten_1 = 0,
						Preisminderung_2____ = 0,
						bis_Anzahl_Mengeneinheiten_2 = 0,
						Preisminderung_3____ = 0,
						bis_Anzahl_Mengeneinheiten_3 = 0,
						Preisminderung_4____ = 0,
						bis_Anzahl_Mengeneinheiten_4 = 0,
						Preisminderung_5____ = 0,
						bis_Anzahl_Mengeneinheiten_5 = 0,
						Preisminderung_6____ = 0,
						bis_Anzahl_Mengeneinheiten_6 = 0,
						Preisminderung_7____ = 0,
						bis_Anzahl_Mengeneinheiten_7 = 0,
						Preisminderung_8____ = 0,
						bis_Anzahl_Mengeneinheiten_8 = 0,
						Preisminderung_9____ = 0,
						bis_Anzahl_Mengeneinheiten_9 = 0,
						Preisminderung_10____ = 0,
						bis_Anzahl_Mengeneinheiten_10 = 0,
						letzte_Aktualisierung = DateTime.Now
					})?.ToList();
					Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.InsertWithTransaction(newPreiseGruppen, botransaction.connection, botransaction.transaction);
					// -- Article level Logging
					var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					logs.AddRange(newPreiseGruppen?.Select(x =>
					   ObjectLogHelper.getLog(user, articleId,
					   $"Purchase Price (Preisgruppen > Einkaufspreis)",
					   $"",
					   $"{x.Einkaufspreis}",
					   Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					   Enums.ObjectLogEnums.LogType.Add))?.ToList());
					logs.AddRange(newPreiseGruppen?.Select(x =>
					   ObjectLogHelper.getLog(user, articleId,
					   $"Purchase Price (Preisgruppen > Aufschlag)",
					   $"",
					   $"{x.Aufschlag}",
					   Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					   Enums.ObjectLogEnums.LogType.Add))?.ToList());
					logs.AddRange(newPreiseGruppen?.Select(x =>
					   ObjectLogHelper.getLog(user, articleId,
					   $"Purchase Price (Preisgruppen > Aufschlagsatz)",
					   $"",
					   $"{x.Aufschlagsatz}",
					   Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					   Enums.ObjectLogEnums.LogType.Add))?.ToList());
					// - save log data
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
					// -
					preiseGruppeEntity = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(articleId, botransaction.connection, botransaction.transaction);
				}
			}

			// - Update
			if(newPurchasePrice != preiseGruppeEntity.Einkaufspreis)
			{
				// -- Article level Logging
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(
					ObjectLogHelper.getLog(user, articleId,
					$"Purchase Price (Einkaufspreis)",
					$"{preiseGruppeEntity.Einkaufspreis}",
					$"{newPurchasePrice}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.Edit),
					botransaction.connection, botransaction.transaction);
			}
			int responseBody = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.UpdatePurchasePrice(newPurchasePrice, articleId, botransaction.connection, botransaction.transaction);
			return responseBody;
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<int>.FailureResponse("Article not found");

			//var basisPreise = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticle(this._data.ArticleId, true);
			//if (basisPreise == null || basisPreise.Count <= 0)
			//    return ResponseModel<int>.FailureResponse("Article Basis not found");

			return ResponseModel<int>.SuccessResponse();
		}

		private static bool isSalesPrice(Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity salesPriceEntity,
			Infrastructure.Data.Entities.Tables.PRS.PreisgruppenEntity preiseGruppeEntities)
		{
			if(salesPriceEntity == null || preiseGruppeEntities == null
				|| (!preiseGruppeEntities.Einkaufspreis.HasValue || preiseGruppeEntities.Einkaufspreis.Value <= 0))
				return true;


			return salesPriceEntity.Aufschlag == preiseGruppeEntities.Aufschlag
				&& salesPriceEntity.ArticleNr == preiseGruppeEntities.Artikel_Nr
				&& salesPriceEntity.Aufschlagsatz == (double?)preiseGruppeEntities.Aufschlagsatz
				&& salesPriceEntity.brutto == preiseGruppeEntities.brutto
				&& salesPriceEntity.Preisgruppe == preiseGruppeEntities.Preisgruppe
				&& salesPriceEntity.Verkaufspreis == preiseGruppeEntities.Verkaufspreis;
		}

	}

}
