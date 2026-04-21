using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Purchase
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetByArticleHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Purchase.GetModel>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetByArticleHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}
		public ResponseModel<List<Models.Article.Purchase.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var purchaseEntities = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticle(this._data) ?? new List<Infrastructure.Data.Entities.Tables.BSD.BestellnummernEntity>();
				return ResponseModel<List<Models.Article.Purchase.GetModel>>.SuccessResponse(purchaseEntities.Select(x => new Models.Article.Purchase.GetModel(x)).ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Purchase.GetModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Purchase.GetModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.Purchase.GetModel>>.FailureResponse("Article not found");

			return ResponseModel<List<Models.Article.Purchase.GetModel>>.SuccessResponse();
		}

	}

}
