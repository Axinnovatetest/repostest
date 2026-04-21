using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using MoreLinq;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateSalesPriceWCopperOrdersHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.SalesPriceCopperOrdersRequestModel _data { get; set; }
		public UpdateSalesPriceWCopperOrdersHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.SalesPriceCopperOrdersRequestModel data)
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


				// - RA
				updateRahmenPrices();
				var updatedRaPos = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.UpdateSalesPriceWCopperOrders_wRa(this._data.ArticleNumber, this._data.HM);

				var results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.UpdateSalesPriceWCopperOrders(this._data.ArticleNumber, this._data.HM);

				// -- Article level Logging
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumberPreffix(this._data.ArticleNumber)
					?.Where(x => x.DELFixiert == true)?.ToList();
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					articleEntities?.Select(x => ObjectLogHelper.getLog(this._user, x.ArtikelNr,
					$"Article - UpdateSalesPriceWCopper Open Orders",
					$"HM: {x.Hubmastleitungen}",
					$"HM: {this._data.HM}",
					Enums.ObjectLogEnums.Objects.Article.GetDescription(),
					Enums.ObjectLogEnums.LogType.BulkUpdate))?.ToList());

				return ResponseModel<int>.SuccessResponse(/*results*/0);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
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

			//var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumber(this._data.ArticleNumber);
			//if (articleEntities == null || articleEntities.Count <= 0)
			//    return ResponseModel<int>.FailureResponse("Article not found");

			return ResponseModel<int>.SuccessResponse();
		}

		/// <summary>
		/// Update all validate and open RA on selectd Article where price date is after today
		/// </summary>
		void updateRahmenPrices()
		{
			var raPosEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetOpenRAPositions_wCopper(this._data.ArticleNumber, this._data.HM);
			if(raPosEntities == null || raPosEntities.Count <= 0)
			{
				return;
			}

			// - 
			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(raPosEntities.Select(x => x.ArtikelNr ?? -1)?.ToList());
			var raPosNrs = raPosEntities.Select(x => x.Nr)?.ToList();
			var raPosHistories = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByPositions(raPosNrs);
			var raPosExtensionEntities = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenPositionNr(raPosNrs);
			var currencyEntities = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.Get(raPosExtensionEntities?.Select(x => x.WahrungId ?? -1)?.ToList());
			foreach(var raPos in raPosEntities)
			{
				// - lastPrice b4 today for current raPos
				var lastPriceB4Today = MoreEnumerable.MaxBy(raPosHistories.Where(x => x.PositionNr == raPos.Nr && (x.ValidFrom <= DateTime.Today == true)), x => x.ValidFrom);
				if(lastPriceB4Today != null && lastPriceB4Today.Count() > 0)
				{
					foreach(var priceItem in lastPriceB4Today)
					{
						// - update Price - 3 steps
						var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == raPos.ArtikelNr);
						var raPosExt = raPosExtensionEntities.FirstOrDefault(x => x.AngeboteArtikelNr == raPos.Nr);
						var currency = currencyEntities.FirstOrDefault(x => x.Nr == raPosExt.WahrungId);

						Common.Helpers.CTS.BlanketHelpers.ComputePositionPrice(raPos, raPosExt, article, currency, raPosExt.Zielmenge ?? 0, priceItem.BasePrice ?? 0);
						// - 1 + 2 - AngeboteneArtikel
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(raPos);
						Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.Update(raPosExt);

						//- 3 
						priceItem.Price = raPos.Einzelpreis;
						priceItem.PriceDefault = raPosExt.PreisDefault;
						Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.Update(priceItem);


						//updating price
						Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(raPos.AngebotNr ?? -1);
					}
				}

				// - all prices after Today
				var pricesAfterToday = raPosHistories.Where(x => x.PositionNr == raPos.Nr && (x.ValidFrom > DateTime.Today == true));
				if(pricesAfterToday != null && pricesAfterToday.Count() > 0)
				{
					foreach(var priceItem in pricesAfterToday)
					{
						// - update Price - 3 steps
						var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == raPos.ArtikelNr);
						var raPosExt = raPosExtensionEntities.FirstOrDefault(x => x.AngeboteArtikelNr == raPos.Nr);
						var currency = currencyEntities.FirstOrDefault(x => x.Nr == raPosExt.WahrungId);

						Common.Helpers.CTS.BlanketHelpers.ComputePositionPrice(raPos, raPosExt, article, currency, raPosExt.Zielmenge ?? 0, priceItem.BasePrice ?? 0);
						// - 1 + 2 - AngeboteneArtikel - no need
						//- 3 
						priceItem.Price = raPos.Einzelpreis;
						priceItem.PriceDefault = raPosExt.PreisDefault;
						Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.Update(priceItem);
					}
				}
			}
		}
	}
}
