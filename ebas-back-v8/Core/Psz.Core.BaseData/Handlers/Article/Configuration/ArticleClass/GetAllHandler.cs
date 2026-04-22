using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ArticleClass
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ArticleClass.ArticleClass>>>
	{
		private Identity.Models.UserModel _user { get; set; }


		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Article.ArticleClass.ArticleClass>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelstammKlassifizierungAccess.Get();

				if(articleEntities != null && articleEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.ArticleClass.ArticleClass>>.SuccessResponse(
							articleEntities.Select(x => new Models.Article.ArticleClass.ArticleClass(x)
								).Distinct().ToList()
							);
				}

				return ResponseModel<List<Models.Article.ArticleClass.ArticleClass>>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ArticleClass.ArticleClass>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ArticleClass.ArticleClass>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.ArticleClass.ArticleClass>>.SuccessResponse();
		}
	}
}
