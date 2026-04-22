using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetCandidateArticlesHandler: IHandle<UserModel, ResponseModel<List<Models.Article.BillOfMaterial.BomCandidateSearchResponseModel>>>
	{
		private UserModel _user { get; set; }
		public Models.Article.BillOfMaterial.BomCandidateSearchRequestModel _data { get; set; }
		public GetCandidateArticlesHandler(UserModel user, int articleId, string searchNummer, int? maxNumber)
		{
			this._user = user;
			this._data = new Models.Article.BillOfMaterial.BomCandidateSearchRequestModel
			{
				ArticleId = articleId,
				SearchNummer = searchNummer,
				MaxItemsNumber = maxNumber
			};
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.BomCandidateSearchResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				if(!this._data.MaxItemsNumber.HasValue)
					this._data.MaxItemsNumber = 20;

				var exceptIds = new List<int> { this._data.ArticleId };

				// get all articles that include current as (direct) BOM Position
				var parentArticleIds = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetParentIds(this._data.ArticleId);
				if(parentArticleIds != null && parentArticleIds.Count > 0)
					exceptIds.AddRange(parentArticleIds);

				// get all positions of current article
				var positionArticleIds = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetChildrenIds(this._data.ArticleId);
				if(positionArticleIds != null && positionArticleIds.Count > 0)
					exceptIds.AddRange(positionArticleIds);

				return ResponseModel<List<Models.Article.BillOfMaterial.BomCandidateSearchResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNotNumbers(exceptIds, this._data.SearchNummer, this._data.MaxItemsNumber)?
						.Select(x => new Models.Article.BillOfMaterial.BomCandidateSearchResponseModel
						{
							ArticleId = x.ArtikelNr,
							ArticleNumber = x.ArtikelNummer,
							ArticleDesignation = x.Bezeichnung1
						})?.ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.BillOfMaterial.BomCandidateSearchResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.BomCandidateSearchResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.BomCandidateSearchResponseModel>>.FailureResponse(key: "1", value: "Article not found");


			return ResponseModel<List<Models.Article.BillOfMaterial.BomCandidateSearchResponseModel>>.SuccessResponse();
		}
	}
}
