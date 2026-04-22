using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{

		public ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>> GetAllAccessProfiles(Identity.Models.UserModel user)
		{
			try
			{
				var validationResponse = this.ValidateGetAllAccessProfiles(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var profilesEntity = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get();
				var response = new List<Models.Administration.AccessProfiles.AccessProfileModel>();


				if(profilesEntity != null)
				{
					foreach(var item in profilesEntity)
					{
						response.Add(new Models.Administration.AccessProfiles.AccessProfileModel(item));
					}

				}
				return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>> ValidateGetAllAccessProfiles(Identity.Models.UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Administration.AccessProfiles.AccessProfileModel>>.SuccessResponse();
		}
	}
}
