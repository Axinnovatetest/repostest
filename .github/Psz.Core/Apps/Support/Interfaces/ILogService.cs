using Infrastructure.Data.Entities.Joins.NLog;
using Psz.Core.Apps.Support.Models.FeedbackLogs;
using Psz.Core.Apps.Support.Models.Logs;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;

namespace Psz.Core.Apps.Support.Interfaces
{
	public interface ILogService
	{
		ResponseModel<NLogsSummary> GetLogsSummary(UserModel user);
		ResponseModel<ApiCallCountResponseModel> GetApisCount(Identity.Models.UserModel user,ApiCallRequestModel _data);
		ResponseModel<ApiCallResponseModel> GetApiLastCall(Identity.Models.UserModel user, ApiCallRequestModel data);
		ResponseModel<FeedbackLogResponseModel> GetLogs(Identity.Models.UserModel user, FeedbackLogRequestModel request);
		ResponseModel<int> UpdateLogTreated(FeedbackLogUpdateModel model, Identity.Models.UserModel user);

	}

}
