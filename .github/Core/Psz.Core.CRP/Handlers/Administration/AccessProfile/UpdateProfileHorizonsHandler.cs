using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Administration.AccessProfiles;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.Administration.AccessProfiles
{
	public partial class CrpAdministrationService
	{
		public ResponseModel<int> UpdateProfileHorizons(Identity.Models.UserModel user, Horizonsmodel data)
		{
			try
			{
				var validationResponse = this.ValidateUpdateProfileHorizons(user);
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var profile = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Get(data.Id);

				profile.FACreateHorizon1 = data.FACreateHorizon1;
				profile.FACreateHorizon2 = data.FACreateHorizon2;
				profile.FACreateHorizon3 = data.FACreateHorizon3;

				profile.FAUpdateTerminHorizon1 = data.FAUpdateTerminHorizon1;
				profile.FAUpdateTerminHorizon2 = data.FAUpdateTerminHorizon2;
				profile.FAUpdateTerminHorizon3 = data.FAUpdateTerminHorizon3;

				profile.FACancelHorizon1 = data.FACancelHorizon1;
				profile.FACancelHorizon2 = data.FACancelHorizon2;
				profile.FACancelHorizon3 = data.FACancelHorizon3;

				profile.DLFPosHorizon1 = data.DLFPosHorizon1;
				profile.DLFPosHorizon2 = data.DLFPosHorizon2;
				profile.DLFPosHorizon3 = data.DLFPosHorizon3;

				var result = Infrastructure.Data.Access.Tables.CRP.AccessProfileAccess.Update(profile);

				return ResponseModel<int>.SuccessResponse(result);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> ValidateUpdateProfileHorizons(Identity.Models.UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}