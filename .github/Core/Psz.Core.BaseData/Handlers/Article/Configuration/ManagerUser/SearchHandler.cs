using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.ManagerUser
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class SearchHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.ManagerUser.ManagerUserSearchModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }


		public SearchHandler(Identity.Models.UserModel user, string userName)
		{
			this._user = user;
			this._data = userName;
		}

		public ResponseModel<List<Models.Article.ManagerUser.ManagerUserSearchModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<Models.Article.ManagerUser.ManagerUserSearchModel> results = null;
				// 
				var userEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetLikeName(this._data);
				if(userEntities != null && userEntities.Count > 0)
				{
					results = new List<Models.Article.ManagerUser.ManagerUserSearchModel>();
					foreach(var userEntity in userEntities)
					{
						results.Add(new Models.Article.ManagerUser.ManagerUserSearchModel()
						{
							UserId = userEntity.UserId,
							UserName = userEntity.UserName,
							UserFullName = userEntity.UserFullName
						});
					}
				}
				return ResponseModel<List<Models.Article.ManagerUser.ManagerUserSearchModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.ManagerUser.ManagerUserSearchModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.ManagerUser.ManagerUserSearchModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.ManagerUser.ManagerUserSearchModel>>.SuccessResponse();
		}
	}
}
