namespace Psz.Core.MaterialManagement.CRP.Models.Configuration
{
	public class DeleteRequestModel
	{
		public int CountryId { get; set; }
		public int HallId { get; set; }
		public decimal ProductionOrderThreshold { get; set; }
	}
}
