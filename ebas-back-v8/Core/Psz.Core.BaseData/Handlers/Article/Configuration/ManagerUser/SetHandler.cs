using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ManagerUser
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class SetHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ManagerUser.SetManagerUserModel _data { get; set; }


		public SetHandler(Identity.Models.UserModel user, Models.Article.ManagerUser.SetManagerUserModel model)
		{
			this._user = user;
			this._data = model;
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

				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.UserIds);

				var managers = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity>();
				foreach(var user in userEntities)
				{
					managers.Add(new Infrastructure.Data.Entities.Tables.PRS.ArtikelManagerUserEntity
					{
						Id = -1,
						ArtikelNr = this._data.ArtikelNr,
						UserId = user.Id,
						UserName = user?.Username,
						UserFullName = user?.Name
					});
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.Insert(managers));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArtikelNr);
			if(articleEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>()
				   {
					   new ResponseModel<int>.ResponseError(){ Key = "", Value= "Article not found"}
				   }
				};
			}
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.UserIds);
			if(userEntity == null || userEntity.Count <= 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>()
				   {
					   new ResponseModel<int>.ResponseError(){ Key = "", Value= "no user found"}
				   }
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
