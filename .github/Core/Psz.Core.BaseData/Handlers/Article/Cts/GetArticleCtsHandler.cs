using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Cts
{
	public class GetArticleCtsHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.Cts.ArticleCtsModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetArticleCtsHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<Models.Article.Cts.ArticleCtsModel> Handle()
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
				var prodPlaces = Configuration.Production.GetListProductionPlaceHandler.getProductionPlaces();
				var articleExtEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelExtensionAccess.GetByArticleNr(this._data);
				var response = new Models.Article.Cts.ArticleCtsModel(articleProductionEntity, articleEntity, prodPlaces, articleExtEntity);

				return ResponseModel<Models.Article.Cts.ArticleCtsModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Article.Cts.ArticleCtsModel> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Cts.ArticleCtsModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.Cts.ArticleCtsModel>()
				{
					Errors = new List<ResponseModel<Models.Article.Cts.ArticleCtsModel>.ResponseError>() {
						new ResponseModel<Models.Article.Cts.ArticleCtsModel>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}
			//***
			return ResponseModel<Models.Article.Cts.ArticleCtsModel>.SuccessResponse();
		}
	}
}
