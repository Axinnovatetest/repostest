using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetProductivityDetailsHandelr: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Basics.ProductivityDetailsResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.ProductivityRequestModel _data { get; set; }
		public GetProductivityDetailsHandelr(UserModel user, Models.Article.Statistics.Basics.ProductivityRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.Basics.ProductivityDetailsResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProductivityDetails> results = null;
				switch(this._data.Site?.ToLower())
				{
					case "tn":
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivityDetails_TN(this._data.ArticleNumber);
						break;
					case "al":
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivityDetails_AL(this._data.ArticleNumber);
						break;
					case "ws":
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivityDetails_WS(this._data.ArticleNumber);
						break;
					case "gz":
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivityDetails_GZ(this._data.ArticleNumber);
						break;
					default:
						results = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivityDetails_CZ(this._data.ArticleNumber);
						break;
				}

				return ResponseModel<List<Models.Article.Statistics.Basics.ProductivityDetailsResponseModel>>.SuccessResponse(
				   results?.Select(x => new Models.Article.Statistics.Basics.ProductivityDetailsResponseModel(x))?.ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Basics.ProductivityDetailsResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Basics.ProductivityDetailsResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber) == null)
				return ResponseModel<List<Models.Article.Statistics.Basics.ProductivityDetailsResponseModel>>.FailureResponse("Article not found");

			return ResponseModel<List<Models.Article.Statistics.Basics.ProductivityDetailsResponseModel>>.SuccessResponse();
		}
	}
}
