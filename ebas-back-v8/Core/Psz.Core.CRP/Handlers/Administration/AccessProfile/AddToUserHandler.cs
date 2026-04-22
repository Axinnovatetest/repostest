using Newtonsoft.Json;
using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<int> AddToUser(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				var validationResponse = this.ValidateAddToUser(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.UserId);
				var existingUserProfiles = Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.GetByUserId(new List<int> { data.UserId });
				var newUsers = data.ProfileIds.FindAll(x => existingUserProfiles.FindIndex(y => y.AccessProfileId == x.Key) < 0); // - not found in existing profiles
				int insertedUsers = 0;

				if(newUsers != null && newUsers.Count > 0)
				{
					insertedUsers = Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.Insert(
						newUsers.Select(x => new Infrastructure.Data.Entities.Tables.CRP.AccessProfileUsersEntity
						{
							AccessProfileId = x.Key,
							AccessProfileName = x.Value,
							CreationTime = DateTime.Now,
							CreationUserId = user.Id,
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
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateAddToUser(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddToUserModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.UserId) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"User not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}