namespace Psz.Core.MaterialManagement.CRP.Models.Capacity
{
	public class UpdateCapacityModel
	{
		public int CountryId { get; set; }
		public int? HallId { get; set; }
		public int Year { get; set; }
		public int FirstWeekNumber { get; set; }
		public int LastWeekNumber { get; set; }
		public bool? IsFirstVersion { get; set; }
	}
}
