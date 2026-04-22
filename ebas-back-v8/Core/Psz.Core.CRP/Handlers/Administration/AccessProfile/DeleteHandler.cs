using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{

		public ResponseModel<int> Delete(Identity.Models.UserModel user, int id)
		{
			try
			{
				var validationResponse = this.ValidateDelete(user, id);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Delete(id);
				Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.DeleteByAccessProfileID(id);

				return ResponseModel<int>.SuccessResponse(1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{id}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateDelete(Identity.Models.UserModel user, int id)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(id) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}