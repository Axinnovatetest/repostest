using Psz.Core.Common.Models;

namespace Psz.Core.Apps.Support.Models.Feedback
{
	public class GetFeedbacksRequestModel: IPaginatedRequestModel
	{
		public string? Module { get; set; }
		public string? SubModule { get; set; }
		public string? FeedbackType { get; set; }
		public string? SearchValue { get; set; }
		public string? Priority { get; set; }

		public GetFeedbacksRequestModel()
		{

		}
		public GetFeedbacksRequestModel(string module, string subModule, string feedbackType)
		{
			Module = module;
			SubModule = subModule;
			FeedbackType = feedbackType;
		}
	}
}
