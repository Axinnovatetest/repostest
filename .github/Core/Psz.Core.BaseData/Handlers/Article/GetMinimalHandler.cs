using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetMinimalHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ArticleMinimalModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetMinimalHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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

				var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get();

				var response = new List<Models.Article.ArticleMinimalModel>();

				foreach(var artikelEntity in artikelEntities)
				{
					response.Add(new Models.Article.ArticleMinimalModel(artikelEntity));
				}

				return ResponseModel<List<Models.Article.ArticleMinimalModel>>.SuccessResponse(response);
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

			return ResponseModel<List<Models.Article.ArticleMinimalModel>>.SuccessResponse();
		}
	}
}
