using Psz.Core.Common.Models;


namespace Psz.Core.Apps.Support.Models.FeedbackLogs
{
	public class ApiCallRequestModel : IPaginatedRequestModel
	{
		public string ApiUrl { get; set; }
	}
}
