namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlanValidation
{
	public class AddUserModel
	{
		public int CountryId { get; set; }
		public int Level { get; set; }
		public List<int> UserIds { get; set; }
	}
}
