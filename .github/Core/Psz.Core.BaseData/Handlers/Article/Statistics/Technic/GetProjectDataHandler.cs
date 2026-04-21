using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Technic
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetProjectDataHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Technic.ProjectDataResponseModel>>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public GetProjectDataHandler(UserModel user, string articleNumber)
		{
			this._user = user;
			this._data = articleNumber;
		}
		public ResponseModel<List<Models.Article.Statistics.Technic.ProjectDataResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Technic.GetProjectData(this._data);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Technic.ProjectDataResponseModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Technic.ProjectDataResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Technic.ProjectDataResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Technic.ProjectDataResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Technic.ProjectDataResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Technic.ProjectDataResponseModel>>.SuccessResponse();
		}
	}
}
