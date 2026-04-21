using Psz.Core.CapitalRequests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		//public ResponseModel<List<CapitalRequestsLogsModel>> ValidateGetRequestsLogs(UserModel user)
		//{
		//	if(user == null)
		//		return ResponseModel<List<CapitalRequestsLogsModel>>.AccessDeniedResponse();
		//	return ResponseModel<List<CapitalRequestsLogsModel>>.SuccessResponse();
		//}
		//public ResponseModel<List<CapitalRequestsLogsModel>> GetRequestsLogs(UserModel user)
		//{
		//	try
		//	{
		//		var validationResponse = ValidateGetRequestsLogs(user);
		//		if(!validationResponse.Success)
		//			return validationResponse;

		//		var logEntities = Infrastructure.Data.Access.Tables.CPL.Capital_requests_logAccess.Get();
		//		var response = logEntities?.Select(x => new CapitalRequestsLogsModel(x)).OrderByDescending(y => y.Date).ToList();

		//		return ResponseModel<List<CapitalRequestsLogsModel>>.SuccessResponse(response);
		//	} catch(Exception e)
		//	{
		//		Infrastructure.Services.Logging.Logger.Log(e);
		//		throw;
		//	}
		//}
	}
}
