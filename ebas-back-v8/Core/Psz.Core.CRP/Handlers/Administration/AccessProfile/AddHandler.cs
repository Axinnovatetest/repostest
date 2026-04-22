using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService: ICrpAdministrationService
	{
		public ResponseModel<int> Add(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel model)
		{
			try
			{
				var validationResponse = this.ValidateAdd(user, model);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
				var accessProfileEntity = model.ToEntity();
				accessProfileEntity.CreationTime = DateTime.Now;
				accessProfileEntity.CreationUserId = user.Id;
				accessProfileEntity.ModuleActivated = true;
				Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Insert(accessProfileEntity);

				return ResponseModel<int>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> ValidateAdd(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AccessProfileModel model)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			if(Infrastructure.Data.Access.Tables.CTS.AccessProfileAccess.GetByName(model.AccessProfileName) != null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Access profile [{model.AccessProfileName}] exists.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}