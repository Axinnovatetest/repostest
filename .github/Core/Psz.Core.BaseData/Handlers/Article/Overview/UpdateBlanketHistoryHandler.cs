using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class UpdateBlanketHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel _data { get; set; }
		public UpdateBlanketHistoryHandler(Identity.Models.UserModel user, Models.Article.ArticleOverviewModel.BlanketHistoryResponseModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				this._data.Date = DateTime.Now;
				this._data.User = this._user.Username;

				// - 2022-03-30
				CreateHandler.generateFileDAT(this._data.ArticleId);

				// -
				return ResponseModel<int>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.PSZ_ArtikelhistorieAccess.Update(
						this._data.ToEntity()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null /*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// -
			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
