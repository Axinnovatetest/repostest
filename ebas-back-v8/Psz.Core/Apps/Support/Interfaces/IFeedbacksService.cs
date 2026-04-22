using Psz.Core.Apps.Support.Models.Feedback;
using Psz.Core.Common.Models;
using System.Collections.Generic;
namespace Psz.Core.Apps.Support.Interfaces
{
	public interface IFeedbacksService
	{
		ResponseModel<int> CreateFeedback(Identity.Models.UserModel user, CreateFeedbackRequestModel data);
		ResponseModel<GetFeedbackByModuleResponseModel> GetFeedbackByModule(Identity.Models.UserModel user, GetFeedbacksRequestModel data);
		ResponseModel<List<KeyValuePair<string, int>>> GetModulesFeedbackCount(Identity.Models.UserModel user);
		ResponseModel<int> UpdateFeedbackTreated(Identity.Models.UserModel user, int Id);
		ResponseModel<List<KeyValuePair<string, string>>> GetFeedbacksModules(Identity.Models.UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetFeedbacksSubmodules(Identity.Models.UserModel user, string module);
		ResponseModel<int> UpdatePriority(Identity.Models.UserModel user, UpdatePriorityRequestModel request);
		ResponseModel<GetFeedbacksResponseModel> GetFeedbackById(Identity.Models.UserModel user, int Id);

		ResponseModel<GetFeedbackByUrlResponseModel> GetFeedbacksByPageUrl(Identity.Models.UserModel user, GetFeedbackByUrlRequestModel request);

		ResponseModel<int> MarkAllTreated(Identity.Models.UserModel user, List<int> ids);

	}
}
