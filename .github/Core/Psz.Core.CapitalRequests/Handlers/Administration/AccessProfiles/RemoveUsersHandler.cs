using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<int> ValidateRemoveUsers(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> RemoveUsers(UserModel user, Models.AddUsersModel data)
		{
			try
			{
				var validationResponse = ValidateRemoveUsers(user);
				if(!validationResponse.Success)
					return validationResponse;

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.DeleteUsers(data.ProfileId, data.UserIds));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}