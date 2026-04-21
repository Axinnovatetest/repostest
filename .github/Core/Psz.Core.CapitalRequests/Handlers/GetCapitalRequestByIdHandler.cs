using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<CapitalRequestModel> ValidateGetRequestById(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<CapitalRequestModel>.AccessDeniedResponse();
			if(Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(id) == null)
				return ResponseModel<CapitalRequestModel>.FailureResponse("Request not found.");
			return ResponseModel<CapitalRequestModel>.SuccessResponse();
		}
		public ResponseModel<CapitalRequestModel> GetRequestById(UserModel user, int id)
		{
			var validationResponse = ValidateGetRequestById(user, id);
			if(!validationResponse.Success)
				return validationResponse;

			var response = new CapitalRequestModel
			{
				Header = new RequestHeaderModel(Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get(id)),
				Positions = Infrastructure.Data.Access.Tables.CPL.Capital_requests_positionsAccess.GetByHeaderId(id)?
				.Select(x => new RequestPositionModel(x))
			};
			return ResponseModel<CapitalRequestModel>.SuccessResponse(response);
		}
	}
}