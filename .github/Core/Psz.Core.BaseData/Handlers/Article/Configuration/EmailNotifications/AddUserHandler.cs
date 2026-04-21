using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.EmailNotifications
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class AddUserHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.EmailNotifications.AddUserModel _data { get; set; }
		public AddUserHandler(UserModel user, Models.Article.Configuration.EmailNotifications.AddUserModel data)
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

				if(this._data.Items == null && this._data.Items.Count <= 0)
					return ResponseModel<int>.SuccessResponse();

				// -
				var errors = new List<string>();
				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.Items.Select(x => (int)x.UserId).ToList())
					?? new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();
				foreach(var dataItem in this._data.Items)
				{
					if(userEntities.FindIndex(x => x.Id == dataItem.UserId) < 0)
						errors.Add($"User [{dataItem.UserName}] not found");
				}


				// -
				var userNotifEntities = Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.GetByUsers(this._data.Items.Select(x => x.UserId).ToList())
					?? new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();

				var editUserNotifs = new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
				var newUserNotifs = new List<Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity>();
				foreach(var userEntity in userEntities)
				{
					var dataItem = this._data.Items.Find(x => x.UserId == userEntity.Id);
					var userNotifEntity = userNotifEntities.Find(x => x.UserId == userEntity.Id);
					if(userNotifEntity == null)
					{
						var newUserNotification = new Infrastructure.Data.Entities.Tables.BSD.EmailNotificationUsersEntity
						{
							Id = 1,
							UserId = userEntity.Id,
							UserEmail = userEntity.Email,
							UserName = userEntity.Name
						};

						switch(dataItem?.NotificationOption)
						{
							case Enums.ArticleEnums.NotificationOptions.Sales:
								newUserNotification.ArticleSales = true;
								break;
							case Enums.ArticleEnums.NotificationOptions.Purchase:
								newUserNotification.ArticlePurchase = true;
								break;
							case Enums.ArticleEnums.NotificationOptions.BomCpControlEngineering:
								newUserNotification.ArticleBomCpControl_Engineering = true;
								break;
							case Enums.ArticleEnums.NotificationOptions.BomCpControlQuality:
								newUserNotification.ArticleBomCpControl_Quality = true;
								break;
							default:
								break;
						}
						// -
						newUserNotifs.Add(newUserNotification);
					}
					else
					{
						switch(dataItem?.NotificationOption)
						{
							case Enums.ArticleEnums.NotificationOptions.Sales:
								userNotifEntity.ArticleSales = true;
								break;
							case Enums.ArticleEnums.NotificationOptions.Purchase:
								userNotifEntity.ArticlePurchase = true;
								break;
							case Enums.ArticleEnums.NotificationOptions.BomCpControlEngineering:
								userNotifEntity.ArticleBomCpControl_Engineering = true;
								break;
							case Enums.ArticleEnums.NotificationOptions.BomCpControlQuality:
								userNotifEntity.ArticleBomCpControl_Quality = true;
								break;
							default:
								break;
						}
						// -
						editUserNotifs.Add(userNotifEntity);
					}
				}

				// -
				if(newUserNotifs.Count > 0)
					Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.Insert(newUserNotifs);
				// -
				if(editUserNotifs.Count > 0)
					Infrastructure.Data.Access.Tables.BSD.EmailNotificationUsersAccess.Update(editUserNotifs);

				// -
				if(errors.Count > 0)
					return ResponseModel<int>.FailureResponse(errors);

				// -
				return ResponseModel<int>.SuccessResponse();
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

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
