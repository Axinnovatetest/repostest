using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.EmailNotifications
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetUserListHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Configuration.EmailNotifications.GetUserModel>>>
	{
		private UserModel _user { get; set; }
		public GetUserListHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Configuration.EmailNotifications.GetUserModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var productionPlaceEntites = Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.Get();
				if(productionPlaceEntites != null && productionPlaceEntites.Count > 0)
				{
					return ResponseModel<List<Models.Article.Configuration.EmailNotifications.GetUserModel>>.SuccessResponse(productionPlaceEntites
							.Select(x => new Models.Article.Configuration.EmailNotifications.GetUserModel(x)).Distinct().ToList());
				}

				return ResponseModel<List<Models.Article.Configuration.EmailNotifications.GetUserModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Configuration.EmailNotifications.GetUserModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Configuration.EmailNotifications.GetUserModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Configuration.EmailNotifications.GetUserModel>>.SuccessResponse();
		}
	}
}
