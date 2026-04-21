
//namespace Psz.Core.Apps.Settings.Handlers.AccessProfiles
//{
//public class AddUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
//{
//	private Identity.Models.UserModel _user { get; set; }
//	private Models.AccessProfiles.AddUsersModel _data { get; set; }

//	public AddUsersHandler(Identity.Models.UserModel user, Models.AccessProfiles.AddUsersModel data)
//	{
//		this._user = user;
//		this._data = data;
//	}

//	public ResponseModel<int> Handle()
//	{
//		try
//		{
//			var validationResponse = this.Validate();
//			if(!validationResponse.Success)
//			{
//				return validationResponse;
//			}

//			/// 
//			var profileEntity = Infrastructure.Data.Access.Tables.AccessProfileAccess.Get(this._data.ProfileId);
//			var profileUsers = Infrastructure.Data.Access.Tables.AccessProfileUsersAccess.GetByAccessProfileIds(new List<int> { this._data.ProfileId });
//			var newUsers = this._data.UserIds.FindAll(x => profileUsers.FindIndex(y => y.UserId == x) < 0);
//			int insertedUsers = 0;

//			if(newUsers != null && newUsers.Count > 0)
//			{
//				var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(newUsers);
//				if(userEntities != null && userEntities.Count > 0)
//				{
//					insertedUsers = Infrastructure.Data.Access.Tables.AccessProfileUsersAccess.Insert(
//						userEntities.Select(x => new Infrastructure.Data.Entities.Tables.BSD.AccessProfileUsersEntity
//						{
//							AccessProfileId = profileEntity.Id,
//							AccessProfileName = profileEntity.AccessProfileName,
//							CreationTime = DateTime.Now,
//							CreationUserId = this._user.Id,
//							Id = -1,
//							UserEmail = x.Email,
//							UserId = x.Id,
//							UserName = x.Username
//						})?.ToList());
//				}
//			}

//			return ResponseModel<int>.SuccessResponse(insertedUsers);
//		} catch(Exception e)
//		{
//			Infrastructure.Services.Logging.Logger.Log(e);
//			throw;
//		}
//	}


//	public ResponseModel<int> Validate()
//	{
//		if(this._user == null)
//		{
//			return ResponseModel<int>.AccessDeniedResponse();
//		}

//		// - 
//		var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
//		if(userEntity == null)
//			return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

//		if(Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(this._data.ProfileId) == null)
//			return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

//		return ResponseModel<int>.SuccessResponse();
//	}
//}
//}
/*
 
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Administration.AccessProfiles
{
	public class AddUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AddUsersModel _data { get; set; }

		public AddUsersHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersModel data)
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

				/// 
				var profileEntity = Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(this._data.ProfileId);
				var profileUsers = Infrastructure.Data.Access.Tables.AccessProfileUsersAccess.GetByAccessProfileIds(new List<int> { this._data.ProfileId });
				var newUsers = this._data.UserIds.FindAll(x => profileUsers.FindIndex(y => y.UserId == x) < 0);
				int insertedUsers = 0;

				if(newUsers != null && newUsers.Count > 0)
				{
					var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(newUsers);
					if(userEntities != null && userEntities.Count > 0)
					{
						insertedUsers = Infrastructure.Data.Access.Tables.AccessProfileUsersAccess.Insert(
							userEntities.Select(x => new Infrastructure.Data.Entities.Tables.BSD.AccessProfileUsersEntity
							{
								AccessProfileId = profileEntity.Id,
								AccessProfileName = profileEntity.AccessProfileName,
								CreationTime = DateTime.Now,
								CreationUserId = this._user.Id,
								Id = -1,
								UserEmail = x.Email,
								UserId = x.Id,
								UserName = x.Username
							})?.ToList());
					}
				}

				return ResponseModel<int>.SuccessResponse(insertedUsers);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____* /)
			{
	return ResponseModel<int>.AccessDeniedResponse();
}

// - 
var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
if(userEntity == null)
	return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

if(Infrastructure.Data.Access.Tables.BSD.AccessProfileAccess.Get(this._data.ProfileId) == null)
	return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

return ResponseModel<int>.SuccessResponse();
		}

	}
}


 **/