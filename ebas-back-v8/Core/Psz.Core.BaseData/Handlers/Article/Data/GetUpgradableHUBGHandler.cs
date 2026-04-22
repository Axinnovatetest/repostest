using System;

namespace Psz.Core.BaseData.Handlers.Article.Data
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetUpgradableHUBGHandler: IHandle<UserModel, ResponseModel<Models.Article.Data.UpgradableHUBGResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Data.UpgradableHUBGRequestModel _data { get; set; }
		public GetUpgradableHUBGHandler(UserModel user, Models.Article.Data.UpgradableHUBGRequestModel data)
		{
			this._user = user;
			_data = data;
		}
		public ResponseModel<Models.Article.Data.UpgradableHUBGResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// - 
				return ResponseModel<Models.Article.Data.UpgradableHUBGResponseModel>.SuccessResponse(
					new Models.Article.Data.UpgradableHUBGResponseModel
					{
						CurrentArticleId = this._data.CurrentArticleId,
						NewArticleId = this._data.NewArticleId,
						HBGItems = GetUpgradableHBGHandler.GetData(this._data.CurrentArticleId),
						UBGItems = GetUpgradableUBGHandler.GetData(this._data.CurrentArticleId)
					});
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Data.UpgradableHUBGResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Data.UpgradableHUBGResponseModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.CurrentArticleId);
			if(articleEntity == null)
				return ResponseModel<Models.Article.Data.UpgradableHUBGResponseModel>.FailureResponse("Article not found");
			//if (articleEntity.UBG != true)
			//    return ResponseModel<Models.Article.Data.UpgradableHUBGModel>.FailureResponse("Article is not UBG");

			return ResponseModel<Models.Article.Data.UpgradableHUBGResponseModel>.SuccessResponse();
		}

	}
}
