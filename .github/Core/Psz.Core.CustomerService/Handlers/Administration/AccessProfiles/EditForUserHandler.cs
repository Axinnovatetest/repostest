using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Administration.AccessProfiles
{
	public class EditForUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AddToUserModel _data { get; set; }

		public EditForUserHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddToUserModel data)
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
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.UserId);
				int insertedUsers = 0;

				if(this._data.ProfileIds != null && this._data.ProfileIds.Count > 0)
				{
					Infrastructure.Data.Access.Tables.CTS.AccessProfileUsersAccess.DeleteForUsers(new List<int> { this._data.UserId });
					insertedUsers = Infrastructure.Data.Access.Tables.CTS.AccessProfileUsersAccess.Insert(
						this._data.ProfileIds.Select(x => new Infrastructure.Data.Entities.Tables.CTS.AccessProfileUsersEntity
						{
							AccessProfileId = x.Key,
							AccessProfileName = x.Value,
							CreationTime = DateTime.Now,
							CreationUserId = this._user.Id,
							Id = -1,
							UserEmail = userEntity.Email,
							UserId = userEntity.Id,
							UserName = userEntity.Username
						})?.ToList());
				}

				return ResponseModel<int>.SuccessResponse(insertedUsers);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}


		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.UserId) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"User not found.");

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
