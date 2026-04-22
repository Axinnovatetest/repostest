using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<int> RemoveUsers(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersModel data)
		{
			try
			{
				var validationResponse = this.ValidateRemoveUsers(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.DeleteUsers(data.ProfileId, data.UserIds));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateRemoveUsers(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddUsersModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(data.ProfileId) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}