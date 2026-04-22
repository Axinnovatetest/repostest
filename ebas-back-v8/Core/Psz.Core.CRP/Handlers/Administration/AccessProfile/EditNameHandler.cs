using Newtonsoft.Json;
using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<int> EditName(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				var validationResponse = this.ValidateEditName(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
				var accessProfileEntity = data.ToEntity();

				Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.UpdateName(accessProfileEntity);

				return ResponseModel<int>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateEditName(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(data.Id) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{data.AccessProfileName.Trim()}] not found.");

			var accessProfileEntity = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.GetByName(data.AccessProfileName);
			if(accessProfileEntity != null && accessProfileEntity.Id != data.Id)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{data.AccessProfileName.Trim()}] exists.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}