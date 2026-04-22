using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHistoryHandler: IHandle<UserModel, ResponseModel<List<Models.Article.BillOfMaterial.BomHistoryResponseModel>>>
	{
		private UserModel _user { get; set; }
		public int _data { get; set; }
		public GetHistoryHandler(UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.BomHistoryResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Article.BillOfMaterial.BomHistoryResponseModel>();
				var uniqueBoms = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetBOMVersionByArticle(this._data);
				if(uniqueBoms != null && uniqueBoms.Count > 0)
				{
					var bomCounts = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetPositionsCount(uniqueBoms.Select(x => new Tuple<int, int>(this._data, x)).ToList());
					var boms = Infrastructure.Data.Access.Tables.BSD.Stucklisten_SnapshotAccess.GetFirstPositions(this._data);
					var users = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(boms.Select(x => x.SnapshotUserId).ToList());
					var headers = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtension_SnapshotAccess.GetByArticleId(this._data);
					foreach(var item in boms)
					{
						var h = headers?.FirstOrDefault(x => x.ArticleId == item.Artikel_Nr && x.BomVersion == item.BomVersion);
						var u = users?.FirstOrDefault(x => x.Id == item.SnapshotUserId);
						var c = bomCounts.FirstOrDefault(x => x.Item1 == item.Artikel_Nr && x.Item2 == item.BomVersion);
						responseBody.Add(new Models.Article.BillOfMaterial.BomHistoryResponseModel(h, item, u, c?.Item3 ?? 0));
					}
				}
				var bomEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data);

				return ResponseModel<List<Models.Article.BillOfMaterial.BomHistoryResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.BillOfMaterial.BomHistoryResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.BomHistoryResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.BomHistoryResponseModel>>.FailureResponse(key: "1", value: "Article item not found");

			if(Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data) == null
				&& Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data) == null)
				return ResponseModel<List<Models.Article.BillOfMaterial.BomHistoryResponseModel>>.FailureResponse(key: "1", value: "BOM item not found");


			return ResponseModel<List<Models.Article.BillOfMaterial.BomHistoryResponseModel>>.SuccessResponse();
		}

	}

}
