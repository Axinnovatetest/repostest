using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	public class GetArticleDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.ArticleDataModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetArticleDataHandler(Identity.Models.UserModel user, int ArticleNr)
		{
			this._user = user;
			this._data = ArticleNr;
		}
		public ResponseModel<Models.Article.ArticleDataModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				//articleEntity.UL
				var artikelExtensionEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data);
				var response = new Models.Article.ArticleDataModel(articleEntity, artikelExtensionEntity);
				return ResponseModel<Models.Article.ArticleDataModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Article.ArticleDataModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ArticleDataModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.ArticleDataModel>()
				{
					Success = false,
					Errors = new List<ResponseModel<Models.Article.ArticleDataModel>.ResponseError>() {
						new ResponseModel<Models.Article.ArticleDataModel>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}

			return ResponseModel<Models.Article.ArticleDataModel>.SuccessResponse();
		}
	}
}
