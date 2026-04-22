using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticleProjectTypeHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.ArticleProjectTypeRequestModel _data { get; set; }
		public GetArticleProjectTypeHandler(UserModel user, Models.Article.Statistics.Basics.ArticleProjectTypeRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetArticleProjectType(this._data.SearchTerms, this._data.RequestedPage, this._data.ItemsPerPage)
					?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProjektartArtikel>();
				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetArticleProjectType_count(this._data.SearchTerms);

				//- 
				Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel responseBody = new Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel();

				responseBody.AllCount = allCount;
				responseBody.AllPagesCount = (int)Math.Ceiling((decimal)allCount / this._data.ItemsPerPage);
				responseBody.ItemsPerPage = this._data.ItemsPerPage;
				responseBody.RequestedPage = this._data.RequestedPage;
				responseBody.Projectarts = statisticsEntities.Select(x => new Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel.Projecktart(x))?.ToList();

				return ResponseModel<Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.Statistics.Basics.ArticleProjectTypeResponseModel>.SuccessResponse();
		}
	}
}
