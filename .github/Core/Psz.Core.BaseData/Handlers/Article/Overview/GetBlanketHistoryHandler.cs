using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class GetBlanketHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetBlanketHistoryHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var historyEntitites = Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.GetByArticle(this._data)
					?? new List<Infrastructure.Data.Entities.Tables.BSD.PSZ_ArtikelhistorieEntity>();

				// -
				return ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel>>.SuccessResponse(
					historyEntitites.Select(x => new Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel(x))
					?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel>> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel>>.FailureResponse("Article not found");

			// -
			return ResponseModel<List<Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel>>.SuccessResponse();
		}
	}
}
