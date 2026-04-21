using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Handlers.User
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class SearchHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Users.GetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }


		public SearchHandler(Identity.Models.UserModel user, string userName)
		{
			this._user = user;
			this._data = userName;
		}

		public ResponseModel<List<Models.Users.GetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<Models.Users.GetModel> results = null;
				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.GetLikeName(this._data);
				if(userEntities != null && userEntities.Count > 0)
				{
					results = new List<Models.Users.GetModel>();
					foreach(var userEntity in userEntities)
					{
						results.Add(new Models.Users.GetModel(userEntity));
					}
				}
				return ResponseModel<List<Models.Users.GetModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Users.GetModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Users.GetModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Users.GetModel>>.SuccessResponse();
		}
	}
}
