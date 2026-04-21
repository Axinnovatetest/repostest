using Newtonsoft.Json;
using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<int> EditForUser(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddToUserModel data)
		{
			try
			{
				var validationResponse = this.ValidateEditForUser(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.UserId);
				int insertedUsers = 0;

				if(data.ProfileIds != null && data.ProfileIds.Count > 0)
				{
					Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.DeleteForUsers(new List<int> { data.UserId });
					insertedUsers = Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.Insert(
						data.ProfileIds.Select(x => new Infrastructure.Data.Entities.Tables.CRP.AccessProfileUsersEntity
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
		public ResponseModel<int> ValidateEditForUser(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddToUserModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.UserId) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"User not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}