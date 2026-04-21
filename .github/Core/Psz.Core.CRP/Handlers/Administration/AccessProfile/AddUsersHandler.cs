using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<int> AddUsers(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				var validationResponse = this.ValidateAddUsers(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var profileEntity = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(data.ProfileId);
				var profileUsers = Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.GetByAccessProfileIds(new List<int> { data.ProfileId });
				var newUsers = data.UserIds.FindAll(x => profileUsers.FindIndex(y => y.UserId == x) < 0);
				int insertedUsers = 0;

				if(newUsers != null && newUsers.Count > 0)
				{
					var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(newUsers);
					if(userEntities != null && userEntities.Count > 0)
					{
						insertedUsers = Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.Insert(
							userEntities.Select(x => new Infrastructure.Data.Entities.Tables.CRP.AccessProfileUsersEntity
							{
								AccessProfileId = profileEntity.Id,
								AccessProfileName = profileEntity.AccessProfileName,
								CreationTime = DateTime.Now,
								CreationUserId = user.Id,
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
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateAddUsers(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(data.ProfileId) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}