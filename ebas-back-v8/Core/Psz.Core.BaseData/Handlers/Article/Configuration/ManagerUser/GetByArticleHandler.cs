using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ManagerUser
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetByArticleHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.ManagerUser.ManagerUserModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetByArticleHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Article.ManagerUser.ManagerUserModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// 
				return ResponseModel<Models.Article.ManagerUser.ManagerUserModel>.SuccessResponse(
					new Models.Article.ManagerUser.ManagerUserModel(
						Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByArtikelNr(this._data)));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Article.ManagerUser.ManagerUserModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ManagerUser.ManagerUserModel>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return new ResponseModel<Models.Article.ManagerUser.ManagerUserModel>()
				{
					Errors = new List<ResponseModel<Models.Article.ManagerUser.ManagerUserModel>.ResponseError>()
				   {
					   new ResponseModel<Models.Article.ManagerUser.ManagerUserModel>.ResponseError(){ Key = "", Value= "Article not found"}
				   }
				};
			}

			return ResponseModel<Models.Article.ManagerUser.ManagerUserModel>.SuccessResponse();
		}
	}
}
