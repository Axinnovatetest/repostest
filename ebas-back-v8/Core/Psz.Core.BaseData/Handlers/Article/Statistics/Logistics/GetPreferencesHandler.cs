using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Logistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetPreferencesHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Logistics.PreferencesResponseModel>>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public GetPreferencesHandler(UserModel user, string nummer)
		{
			this._user = user;
			this._data = nummer;
		}
		public ResponseModel<List<Models.Article.Statistics.Logistics.PreferencesResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetPreferences(this._data);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Logistics.PreferencesResponseModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Logistics.PreferencesResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Logistics.PreferencesResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Logistics.PreferencesResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Logistics.PreferencesResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data) == null)
				return ResponseModel<List<Models.Article.Statistics.Logistics.PreferencesResponseModel>>.FailureResponse("Article not found.");

			return ResponseModel<List<Models.Article.Statistics.Logistics.PreferencesResponseModel>>.SuccessResponse();
		}
	}
}
