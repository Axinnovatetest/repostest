using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Create
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetIndexSiblingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ArticleMinimalModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.IndexSiblingsRequestModel _data { get; set; }

		public GetIndexSiblingsHandler(Identity.Models.UserModel user, Models.Article.IndexSiblingsRequestModel data)
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
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetIndexSiblings(this._data.CustomerNumber, this._data.CustomerItemNumber, this._data.CustomerItemIndex)
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

			return ResponseModel<List<Models.Article.ArticleMinimalModel>>.SuccessResponse();
		}
	}

}
