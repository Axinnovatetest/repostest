namespace Psz.Core.MaterialManagement.CRP.Models.CapacityPlanValidation
{
	public class GetModel
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public List<ValidationUserModel> Level1 { get; set; }
		public List<ValidationUserModel> Level2 { get; set; }
	}
	public class ValidationUserModel
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string UserEmail { get; set; }
	}
}
