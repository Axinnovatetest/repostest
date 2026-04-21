using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<List<NumberKreisResponseModel>> ValidateGetNumberKreis(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<NumberKreisResponseModel>>.AccessDeniedResponse();
			return ResponseModel<List<NumberKreisResponseModel>>.SuccessResponse();
		}
		public ResponseModel<List<NumberKreisResponseModel>> GetNumberKreis(UserModel user)
		{
			try
			{
				var validationResponse = ValidateGetNumberKreis(user);
				if(!validationResponse.Success)
					return validationResponse;

				// -
				return ResponseModel<List<NumberKreisResponseModel>>.SuccessResponse(
					  Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get(true)
					?.Select(x => new NumberKreisResponseModel(x))?.ToList()
					?? new List<NumberKreisResponseModel>());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
