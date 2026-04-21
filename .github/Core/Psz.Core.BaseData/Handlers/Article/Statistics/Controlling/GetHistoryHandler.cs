using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Controlling
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHistoryHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Controlling.HistoryModel>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetHistoryHandler(UserModel user, int number)
		{
			this._user = user;
			this._data = number;
		}
		public ResponseModel<List<Models.Article.Statistics.Controlling.HistoryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Controlling.GetHistory(this._data);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Controlling.HistoryModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Controlling.HistoryModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Controlling.HistoryModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Controlling.HistoryModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Controlling.HistoryModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.Statistics.Controlling.HistoryModel>>.FailureResponse("Article not found.");

			return ResponseModel<List<Models.Article.Statistics.Controlling.HistoryModel>>.SuccessResponse();
		}
	}
}
