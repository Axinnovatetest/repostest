using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<int> ValidateAddUsers(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> AddUsers(UserModel user, Models.AddUsersModel data)
		{
			try
			{
				var validationResponse = ValidateAddUsers(user);
				if(!validationResponse.Success)
					return validationResponse;

				var profileEntity = Infrastructure.Data.Access.Tables.CPL.AccessProfileAccess.Get(data.ProfileId);
				var profileUsers = Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.GetByAccessProfileIds(new List<int> { data.ProfileId });
				var newUsers = data.UserIds.FindAll(x => profileUsers.FindIndex(y => y.UserId == x) < 0);
				int insertedUsers = 0;

				if(newUsers != null && newUsers.Count > 0)
				{
					var userEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(newUsers);
					if(userEntities != null && userEntities.Count > 0)
					{
						insertedUsers = Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.Insert(
							userEntities.Select(x => new Infrastructure.Data.Entities.Tables.CPL.AccessProfileUsersEntity
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
				throw;
			}
		}
	}
}