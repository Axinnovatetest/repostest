using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetUBGParentsHandler: IHandle<UserModel, ResponseModel<List<Models.Article.BillOfMaterial.UBGParentModel>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetUBGParentsHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.UBGParentModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Article.BillOfMaterial.UBGParentModel>();
				var parentIds = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetParentIds(this._data);
				if(parentIds != null && parentIds.Count > 0)
				{
					var bomExteionEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticles(parentIds);
					var parentEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(parentIds);
					responseBody.AddRange(parentEntities.Select(x => new Models.Article.BillOfMaterial.UBGParentModel(x,
						bomExteionEntities?.FirstOrDefault(y => y.ArticleId == x.ArtikelNr))));
				}

				return ResponseModel<List<Models.Article.BillOfMaterial.UBGParentModel>>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.BillOfMaterial.UBGParentModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.UBGParentModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.UBGParentModel>>.FailureResponse(key: "1", value: "Article item not found");

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data) == null
				&& Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data) == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.UBGParentModel>>.FailureResponse(key: "1", value: "BOM item not found");


			return ResponseModel<List<Models.Article.BillOfMaterial.UBGParentModel>>.SuccessResponse();
		}

	}

}
