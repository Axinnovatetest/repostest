using Psz.Core.CapitalRequests.Models;
using Psz.Core.CapitalRequests.Services;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService: ICapitalRequestsService
	{
		public ResponseModel<IEnumerable<RequestHeaderModel>> ValidateGetRequests(UserModel user)
		{
			if(user == null)
				return ResponseModel<IEnumerable<RequestHeaderModel>>.AccessDeniedResponse();
			return ResponseModel<IEnumerable<RequestHeaderModel>>.SuccessResponse();
		}
		public ResponseModel<IEnumerable<RequestHeaderModel>> GetRequests(UserModel user)
		{
			var validationResponse = ValidateGetRequests(user);
			if(!validationResponse.Success)
				return validationResponse;

			var entities = Infrastructure.Data.Access.Tables.CPL.Capital_requests_headerAccess.Get();
			var response = new List<RequestHeaderModel>();
			if(user.Access.CapitalRequests.RequestCapital || user.Access.CapitalRequests.RequestAdmin)
				response = entities?.Select(x => new RequestHeaderModel(x)).ToList();
			else
			{
				if(user.Access.CapitalRequests.RequestCreation)
					response.AddRange(entities?.Where(a => a.UserId == user.Id)?.
						Select(x => new RequestHeaderModel(x)).ToList());
				if(user.Access.CapitalRequests.RequestEngeneering)
					response.AddRange(entities?.Where(a => a.PlantId == user.CompanyId)
						?.Select(x => new RequestHeaderModel(x)).ToList());
			}

			return ResponseModel<IEnumerable<RequestHeaderModel>>.SuccessResponse(response.DistinctBy(x => x.Id).OrderBy(x => x.Id).ToList());
		}
	}
}