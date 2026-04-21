using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<IEnumerable<KeyValuePair<int, string>>> ValidateGetRequestCategories(UserModel user)
		{
			if(user == null)
				return ResponseModel<IEnumerable<KeyValuePair<int, string>>>.AccessDeniedResponse();
			return ResponseModel<IEnumerable<KeyValuePair<int, string>>>.SuccessResponse();
		}
		public ResponseModel<IEnumerable<KeyValuePair<int, string>>> GetRequestCategories(UserModel user)
		{
			var validationResponse = ValidateGetRequestCategories(user);
			if(!validationResponse.Success)
				return validationResponse;

			var response = Enum.GetValues(typeof(Enums.RequestEnums.RequestCategories)).Cast<Enums.RequestEnums.RequestCategories>()
									?.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription()));
			return ResponseModel<IEnumerable<KeyValuePair<int, string>>>.SuccessResponse(response);
		}
	}
}
