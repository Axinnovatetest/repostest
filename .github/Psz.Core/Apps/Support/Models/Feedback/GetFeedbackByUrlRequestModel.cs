using Psz.Core.Common.Models;

namespace Psz.Core.Apps.Support.Models.Feedback
{
	public class GetFeedbackByUrlRequestModel: IPaginatedRequestModel
	{
		public string PageUrl { get; set; }
	}
}
