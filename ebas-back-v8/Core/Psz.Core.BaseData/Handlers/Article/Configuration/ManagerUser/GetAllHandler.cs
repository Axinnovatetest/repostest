using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ManagerUser
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ManagerUser.ManagerUserModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Article.ManagerUser.ManagerUserModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<Models.Article.ManagerUser.ManagerUserModel> results = null;
				// 
				var userEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetUniqueUsers();
				if(userEntities != null && userEntities.Count > 0)
				{
					results = new List<Models.Article.ManagerUser.ManagerUserModel>();
					foreach(var userEntity in userEntities)
					{
						results.Add(new Models.Article.ManagerUser.ManagerUserModel(userEntity));
					}
					results = results.Distinct().ToList();
				}
				return ResponseModel<List<Models.Article.ManagerUser.ManagerUserModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ManagerUser.ManagerUserModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ManagerUser.ManagerUserModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.ManagerUser.ManagerUserModel>>.SuccessResponse();
		}
	}
}
