using Psz.Core.CapitalRequests.Services;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService: ICapitalRequestsAdminstrationService
	{
		public ResponseModel<int> ValidateAdd(UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
		public ResponseModel<int> Add(UserModel user, Models.AccessProfileModel data)
		{
			try
			{
				var validationResponse = ValidateAdd(user);
				if(!validationResponse.Success)
					return validationResponse;

				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
				var accessProfileEntity = data.ToEntity();
				accessProfileEntity.CreationTime = DateTime.Now;
				accessProfileEntity.CreationUserId = user.Id;
				accessProfileEntity.ModuleActivated = true;
				Infrastructure.Data.Access.Tables.CPL.AccessProfileAccess.Insert(accessProfileEntity);

				return ResponseModel<int>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}