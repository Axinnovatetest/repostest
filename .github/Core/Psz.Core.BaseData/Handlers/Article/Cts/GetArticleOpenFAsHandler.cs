using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Cts
{
	public class GetArticleOpenFAsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.Cts.ArticleOpenFA>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetArticleOpenFAsHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<List<Models.Article.Cts.ArticleOpenFA>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var openFas = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetNonStartedByArticle(this._data);
				return ResponseModel<List<Models.Article.Cts.ArticleOpenFA>>.SuccessResponse(
					openFas?.Select(x => new Models.Article.Cts.ArticleOpenFA(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Article.Cts.ArticleOpenFA>> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Cts.ArticleOpenFA>>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<List<Models.Article.Cts.ArticleOpenFA>>()
				{
					Errors = new List<ResponseModel<List<Models.Article.Cts.ArticleOpenFA>>.ResponseError>() {
						new ResponseModel<List<Models.Article.Cts.ArticleOpenFA>>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}
			//***
			return ResponseModel<List<Models.Article.Cts.ArticleOpenFA>>.SuccessResponse();
		}
	}
}
