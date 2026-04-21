using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers.Administration.AccessProfiles
{
	public partial class CapitalRequestsAdminstrationService
	{
		public ResponseModel<List<Models.AccessProfileModel>> ValidateGetAllAccessProfiles(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<Models.AccessProfileModel>>.AccessDeniedResponse();
			return ResponseModel<List<Models.AccessProfileModel>>.SuccessResponse();
		}
		public ResponseModel<List<Models.AccessProfileModel>> GetAllAccessProfiles(UserModel user)
		{
			try
			{
				var validationResponse = ValidateGetAllAccessProfiles(user);
				if(!validationResponse.Success)
					return validationResponse;

				var profilesEntity = Infrastructure.Data.Access.Tables.CPL.AccessProfileAccess.Get();
				var response = new List<Models.AccessProfileModel>();


				if(profilesEntity != null)
				{
					foreach(var item in profilesEntity)
					{
						response.Add(new Models.AccessProfileModel(item));
					}

				}
				return ResponseModel<List<Models.AccessProfileModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}