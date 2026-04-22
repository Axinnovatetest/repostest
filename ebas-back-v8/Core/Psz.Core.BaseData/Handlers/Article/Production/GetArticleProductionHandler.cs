using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Production
{
	public class GetArticleProductionHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.Production.ArticleProductionModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetArticleProductionHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Production.ArticleProductionModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var articleProductionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(this._data);
				var articleTeam = Infrastructure.Data.Access.Tables.BSD.TeamsAccess.GetByName(articleEntity?.Artikelkurztext ?? "");
				var response = new Models.Article.Production.ArticleProductionModel(articleProductionEntity, articleEntity, articleTeam);
				return ResponseModel<Models.Article.Production.ArticleProductionModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Article.Production.ArticleProductionModel> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Production.ArticleProductionModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.Production.ArticleProductionModel>()
				{
					Errors = new List<ResponseModel<Models.Article.Production.ArticleProductionModel>.ResponseError>() {
						new ResponseModel<Models.Article.Production.ArticleProductionModel>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}
			//***
			return ResponseModel<Models.Article.Production.ArticleProductionModel>.SuccessResponse();
		}
	}
}
