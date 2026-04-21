using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Controlling
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetLangtextHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Controlling.LangtextResponseModel>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetLangtextHandler(UserModel user, int number)
		{
			this._user = user;
			this._data = number;
		}
		public ResponseModel<Models.Article.Statistics.Controlling.LangtextResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntity = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Controlling.GetArticle(this._data);
				if(statisticsEntity != null)
				{
					return ResponseModel<Models.Article.Statistics.Controlling.LangtextResponseModel>.SuccessResponse(
							new Models.Article.Statistics.Controlling.LangtextResponseModel(statisticsEntity));
				}

				return ResponseModel<Models.Article.Statistics.Controlling.LangtextResponseModel>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Controlling.LangtextResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Controlling.LangtextResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<Models.Article.Statistics.Controlling.LangtextResponseModel>.FailureResponse("Article not found.");

			return ResponseModel<Models.Article.Statistics.Controlling.LangtextResponseModel>.SuccessResponse();
		}
	}
}
