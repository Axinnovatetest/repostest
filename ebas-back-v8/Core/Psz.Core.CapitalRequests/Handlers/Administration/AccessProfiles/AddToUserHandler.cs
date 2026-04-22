using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<int> ValidateAddToUser(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> AddToUser(UserModel user, Models.AddToUserModel data)
		{
			try
			{
				var validationResponse = ValidateAddToUser(user);
				if(!validationResponse.Success)
					return validationResponse;

				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.UserId);
				var existingUserProfiles = Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.GetByUserId(new List<int> { data.UserId });
				var newUsers = data.ProfileIds.FindAll(x => existingUserProfiles.FindIndex(y => y.AccessProfileId == x.Key) < 0); // - not found in existing profiles
				int insertedUsers = 0;

				if(newUsers != null && newUsers.Count > 0)
				{
					insertedUsers = Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.Insert(
						newUsers.Select(x => new Infrastructure.Data.Entities.Tables.CPL.AccessProfileUsersEntity
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
				throw;
			}
		}
	}
}