using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetMinimumStockAnalyseHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetMinimumStockAnalyseHandler(UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_MaterialBestandAnalyse>();
				try
				{
					statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetMinimumStockAnalyse(this._data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
				}
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel>>.SuccessResponse(
						statisticsEntities.Select(x => new Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Basics.MinimumStockAnalyseResponseModel>>.SuccessResponse();
		}
	}
}
