using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<int> ValidateEditForUser(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> EditForUser(UserModel user, Models.AddToUserModel data)
		{
			try
			{
				var validationResponse = ValidateEditForUser(user);
				if(!validationResponse.Success)
					return validationResponse;

				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.UserId);
				int insertedUsers = 0;

				if(data.ProfileIds != null && data.ProfileIds.Count > 0)
				{
					Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.DeleteForUsers(new List<int> { data.UserId });
					insertedUsers = Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.Insert(
						data.ProfileIds.Select(x => new Infrastructure.Data.Entities.Tables.CPL.AccessProfileUsersEntity
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