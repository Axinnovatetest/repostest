using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<List<KeyValuePair<int, string>>> ValidateGetUnits(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
		public ResponseModel<List<KeyValuePair<int, string>>> GetUnits(UserModel user)
		{
			try
			{
				var validationResponse = ValidateGetUnits(user);
				if(!validationResponse.Success)
					return validationResponse;

				var units = Enum.GetValues(typeof(Enums.CRPEnums.FaPlannugUnits)).Cast<Enums.CRPEnums.FaPlannugUnits>().ToList();
				var response = units
							.Select(x => new KeyValuePair<int, string>((int)x, $"{x.GetDescription()}".Trim())).Distinct().ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}