using Psz.Core.Common.Models;

namespace Psz.Core.CRP.Models.FA
{
	public class FaHoursChangesRequestModel: IPaginatedRequestModel
	{
		public List<int> Weeks { get; set; }
		public int? Year { get; set; }
		public int Lagerort { get; set; }
		public int? FaPositionZone { get; set; }
	}
}
