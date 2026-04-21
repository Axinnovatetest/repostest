using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Logistics
{
	public class GetArticleLogisticsHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.Logistics.ArticleLogisticsModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetArticleLogisticsHandler(Identity.Models.UserModel user, int ArticleID)
		{
			this._user = user;
			this._data = ArticleID;
		}
		public ResponseModel<Models.Article.Logistics.ArticleLogisticsModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
				var ArticleLogisticsEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelLogisticsExtensionAccess.GetByArticleId(this._data);
				var response = new Models.Article.Logistics.ArticleLogisticsModel(ArticleLogisticsEntity, artikelEntity);
				response.ArticleID = this._data;
				return ResponseModel<Models.Article.Logistics.ArticleLogisticsModel>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Models.Article.Logistics.ArticleLogisticsModel> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Logistics.ArticleLogisticsModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.Logistics.ArticleLogisticsModel>()
				{
					Errors = new List<ResponseModel<Models.Article.Logistics.ArticleLogisticsModel>.ResponseError>() {
						new ResponseModel<Models.Article.Logistics.ArticleLogisticsModel>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}
			//***
			return ResponseModel<Models.Article.Logistics.ArticleLogisticsModel>.SuccessResponse();
		}
	}
}
