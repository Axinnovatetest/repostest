using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetUsers(Identity.Models.UserModel user, List<int> data)
		{
			try
			{
				var validationResponse = this.ValidateGetUsers(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.CRP.AccessProfileUsersAccess.GetByAccessProfileIds(data)
						?.DistinctBy(x => x.UserId)
						?.Select(x => new KeyValuePair<int, string>(x.UserId ?? -1, x.UserName))
						?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> ValidateGetUsers(Identity.Models.UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}