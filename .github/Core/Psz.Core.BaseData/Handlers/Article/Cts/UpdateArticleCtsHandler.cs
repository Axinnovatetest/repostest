using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.Cts
{
	public class UpdateArticleCtsHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.Cts.ArticleCtsUpdateRequestModel _data { get; set; }


		public UpdateArticleCtsHandler(Identity.Models.UserModel user, Models.Article.Cts.ArticleCtsUpdateRequestModel data)
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
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditCTS(this._data.ToEntity()));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId) == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
