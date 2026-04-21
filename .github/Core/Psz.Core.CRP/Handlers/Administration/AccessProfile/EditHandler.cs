using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<int> Edit(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			try
			{
				var validationResponse = this.ValidateEdit(user, data);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
				var accessProfileEntity = data.ToEntity();

				var response = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Update(accessProfileEntity);


				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> ValidateEdit(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
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