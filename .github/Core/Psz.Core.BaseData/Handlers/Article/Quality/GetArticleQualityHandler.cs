using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Quality
{
	public class GetArticleQualityHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.Quality.ArticleQualityModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetArticleQualityHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Quality.ArticleQualityModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var articleQualityEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelQualityExtensionAccess.GetByArticleId(this._data);
				var response = new Models.Article.Quality.ArticleQualityModel(articleEntity, articleQualityEntity);
				return ResponseModel<Models.Article.Quality.ArticleQualityModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Article.Quality.ArticleQualityModel> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Quality.ArticleQualityModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.Quality.ArticleQualityModel>()
				{
					Errors = new List<ResponseModel<Models.Article.Quality.ArticleQualityModel>.ResponseError>() {
						new ResponseModel<Models.Article.Quality.ArticleQualityModel>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}
			//***
			return ResponseModel<Models.Article.Quality.ArticleQualityModel>.SuccessResponse();
		}
	}
}
