namespace Psz.Core.Apps.Settings.Models.Users
{
	public class CreateModel
	{
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int AccessProfileId { get; set; }
		//- 
		public int CompanyId { get; set; }
		public int CountryId { get; set; }
		public int DepartmentId { get; set; }
	}
}
