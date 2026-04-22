using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetEFSiblingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ArticleMinimalModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetEFSiblingsHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Article.ArticleMinimalModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetKreisSiblings(this._data)
					 ?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();

				// -
				return ResponseModel<List<Models.Article.ArticleMinimalModel>>.SuccessResponse(
					articleEntities.Select(x => new Models.Article.ArticleMinimalModel(x)).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ArticleMinimalModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ArticleMinimalModel>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return ResponseModel<List<Models.Article.ArticleMinimalModel>>.FailureResponse($"Article not found.");
			}
			//if (articleEntity.Warengruppe?.Trim()?.ToLower() != "ef")
			//{
			//    return ResponseModel<List<Models.Article.ArticleMinimalModel>>.FailureResponse($"Article not EF.");
			//}

			return ResponseModel<List<Models.Article.ArticleMinimalModel>>.SuccessResponse();
		}
	}

}
