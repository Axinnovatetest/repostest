using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<int> ValidateDelete(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> Delete(UserModel user, int data)
		{
			try
			{
				var validationResponse = ValidateDelete(user);
				if(!validationResponse.Success)
					return validationResponse;

				Infrastructure.Data.Access.Tables.CPL.AccessProfileAccess.Delete(data);
				Infrastructure.Data.Access.Tables.CPL.AccessProfileUsersAccess.DeleteByAccessProfileID(data);

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}