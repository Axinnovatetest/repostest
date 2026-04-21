using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetMaterialStockProdHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Basics.MaterialStockProdResponseMode>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.MaterialStockProdRequestModel _data { get; set; }
		public GetMaterialStockProdHandler(UserModel user, Models.Article.Statistics.Basics.MaterialStockProdRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.Basics.MaterialStockProdResponseMode>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetMaterialBestandProd(this._data.MaterialNumber, this._data.LagerId);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Basics.MaterialStockProdResponseMode>>.SuccessResponse(
						statisticsEntities.Select(x => new Models.Article.Statistics.Basics.MaterialStockProdResponseMode(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Basics.MaterialStockProdResponseMode>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Basics.MaterialStockProdResponseMode>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Basics.MaterialStockProdResponseMode>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Basics.MaterialStockProdResponseMode>>.SuccessResponse();
		}
	}
}
