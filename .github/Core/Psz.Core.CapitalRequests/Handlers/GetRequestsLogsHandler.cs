using Psz.Core.CapitalRequests.Models;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<List<CapitalRequestsLogsResponseModel>> ValidateGetRequestsLogs(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<CapitalRequestsLogsResponseModel>>.AccessDeniedResponse();
			return ResponseModel<List<CapitalRequestsLogsResponseModel>>.SuccessResponse();
		}
		public ResponseModel<List<CapitalRequestsLogsResponseModel>> GetRequestsLogs(UserModel user, CapitalRequestsLogsRequestModel data)
		{
			try
			{
				var validationResponse = ValidateGetRequestsLogs(user);
				if(!validationResponse.Success)
					return validationResponse;

				var logEntities = Infrastructure.Data.Access.Tables.CPL.Capital_requests_logAccess.Get(data.RequestId, data.PlantId, data.SearchTerms);
				var response = logEntities?.Select(x => new CapitalRequestsLogsResponseModel(x)).OrderByDescending(y => y.DateTime).ToList();

				return ResponseModel<List<CapitalRequestsLogsResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}