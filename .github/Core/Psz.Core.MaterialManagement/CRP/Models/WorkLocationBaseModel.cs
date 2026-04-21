namespace Psz.Core.MaterialManagement.CRP.Models
{
	public class WorkLocationBaseModel
	{
		public int CurrentCountryId { get; set; }
		public int? CurrentHallId { get; set; } // > null = all Halls
	}
}
