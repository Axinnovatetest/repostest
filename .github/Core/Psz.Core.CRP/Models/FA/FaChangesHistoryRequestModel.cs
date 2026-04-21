using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Models.FA
{
	public class FaChangesHistoryRequestModel: IPaginatedRequestModel
	{
		public string SearchValue { get; set; }
		public bool SendOutOfH1 { get; set; }
		public bool BroughtIntoH1 { get; set; }
		public bool IncludeDelayBacklog { get; set; }
		public int? LagerId { get; set; }
		public string? FaStatus { get; set; }
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }

	}
}
