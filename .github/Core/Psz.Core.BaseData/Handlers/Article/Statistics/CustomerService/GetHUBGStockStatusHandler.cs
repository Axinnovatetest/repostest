using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.CustomerService
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHUBGStockStatusHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.CustomerService.StockStatusSimpleModel>>>
	{
		private UserModel _user { get; set; }
		private int? _data { get; set; }
		public GetHUBGStockStatusHandler(UserModel user, int? data)
		{
			this._user = user;
			_data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.CustomerService.StockStatusSimpleModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.CustomerService.GetStockStatus(this._data)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus>();
				if(!this._data.HasValue)
				{
					var stockLagers = Module.AppSettings.ProductionLagerIds ?? new List<int>(); // - new List<int> { 6, 7, 15, 26, 42, 60 };
					stockLagers.AddRange(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetTransfertLagers()?.Select(x => x.Lagerort_id) ?? new List<int>());
					stockLagers.AddRange(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetPLLagers()?.Select(x => x.Lagerort_id) ?? new List<int>());
					stockLagers.AddRange(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetHauptLagers()?.Select(x => x.Lagerort_id) ?? new List<int>());
					var restLagers = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.CustomerService.GetStockStatusLagers(stockLagers);

					if(restLagers != null && restLagers.Count > 0)
					{
						restLagers = restLagers.Where(x => !statisticsEntities.Exists(y => y.Lagerort_id == x.Lagerort_id))?.ToList()
							?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.CS_StockStatus>();
						statisticsEntities.AddRange(restLagers);
					}
				}

				// -
				if(statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.CustomerService.StockStatusSimpleModel>>.SuccessResponse(
						statisticsEntities.Select(x => new Models.Article.Statistics.CustomerService.StockStatusSimpleModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.CustomerService.StockStatusSimpleModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.CustomerService.StockStatusSimpleModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.CustomerService.StockStatusSimpleModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.CustomerService.StockStatusSimpleModel>>.SuccessResponse();
		}
	}
}
