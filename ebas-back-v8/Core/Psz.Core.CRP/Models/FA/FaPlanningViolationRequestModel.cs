using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Models.FA
{
	public class FaPlanningViolationRequestModel:  IPaginatedRequestModel
	{
		public string FaStatus { get; set; }
		public int? Lagerort { get; set; }
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
	}
}
