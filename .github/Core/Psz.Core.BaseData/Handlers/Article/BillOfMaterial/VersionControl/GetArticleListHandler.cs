using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.VersionControl
{
	public class GetArticleListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.VersionControl.GetArticleListRequestModel _data { get; set; }
		public GetArticleListHandler(UserModel user, Models.Article.BillOfMaterial.VersionControl.GetArticleListRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			return ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>.SuccessResponse(
						Infrastructure.Data.Access.Tables.BSD.StucklistenArticle_VersionValidationAccess.Get(this._data.Engineering, this._data.Quality, fullValidation: this._data.IncludeFullyValidated == true)
						?.Select(x => new Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel(x)
							).Distinct().ToList());
		}
		public ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.BillOfMaterial.VersionControl.GetArticleListResponseModel>>.SuccessResponse();
		}
	}
}
