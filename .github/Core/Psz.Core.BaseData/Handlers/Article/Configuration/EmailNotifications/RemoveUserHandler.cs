using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.EmailNotifications
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class RemoveUserHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.EmailNotifications.UpdateUserModel _data { get; set; }
		public RemoveUserHandler(UserModel user, Models.Article.Configuration.EmailNotifications.UpdateUserModel data)
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

				var userEntity = Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetByUsers(new List<long> { this._data.UserId })?[0];
				if(userEntity == null)
					return ResponseModel<int>.SuccessResponse();

				switch(this._data.NotificationOption)
				{
					case Enums.ArticleEnums.NotificationOptions.Sales:
						userEntity.ArticleSales = false;
						break;
					case Enums.ArticleEnums.NotificationOptions.Purchase:
						userEntity.ArticlePurchase = false;
						break;
					case Enums.ArticleEnums.NotificationOptions.BomCpControlEngineering:
						userEntity.ArticleBomCpControl_Engineering = false;
						break;
					case Enums.ArticleEnums.NotificationOptions.BomCpControlQuality:
						userEntity.ArticleBomCpControl_Quality = false;
						break;
					default:
						break;
				}

				if(userEntity.ArticleSales == false && userEntity.ArticlePurchase == false && userEntity.ArticleBomCpControl_Engineering == false && userEntity.ArticleBomCpControl_Quality == false)
					return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.Delete(userEntity.Id));
				else
					return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.Update(userEntity));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
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

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get((int)this._data.UserId) == null)
				return ResponseModel<int>.FailureResponse("User not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
