using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<int> ValidateEdit(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> Edit(UserModel user, Models.AccessProfileModel data)
		{
			try
			{
				var validationResponse = ValidateEdit(user);
				if(!validationResponse.Success)
					return validationResponse;

				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
				var accessProfileEntity = data.ToEntity();
				var response = Infrastructure.Data.Access.Tables.CPL.AccessProfileAccess.Update(accessProfileEntity);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}