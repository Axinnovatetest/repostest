using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> ValidateGetUsers(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
		public ResponseModel<List<KeyValuePair<int, string>>> GetUsers(UserModel user, List<int> data)
		{
			try
			{
				var validationResponse = ValidateGetUsers(user);
				if(!validationResponse.Success)
					return validationResponse;

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.GetByAccessProfileIds(data)
						?.DistinctBy(x => x.UserId)
						?.Select(x => new KeyValuePair<int, string>(x.UserId, x.UserName))
						?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
