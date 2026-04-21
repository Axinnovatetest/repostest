using Psz.Core.Apps.Support.Models.FeedbackLogs;
using Psz.Core.Apps.Support.Models.Logs;
using Psz.Core.Common.Models;

namespace Psz.Core.Apps.Support.Interfaces
{
	public interface ILogService
	{
		ResponseModel<FeedbackLogResponseModel> GetLogs(Identity.Models.UserModel user, FeedbackLogRequestModel request);
		ResponseModel<int> UpdateLogTreated(FeedbackLogUpdateModel model, Identity.Models.UserModel user);

	}

}
