using System.Collections.Generic;

namespace Psz.Core.Apps.Settings.Models.Users
{
	public class CreateWPLModel
	{
		public string Username { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public int AccessProfileId { get; set; }
		public List<int> Halls { get; set; } = new List<int>();
		public List<int> Countries { get; set; } = new List<int>();
		//- 
		public int CompanyId { get; set; }
		public int CountryId { get; set; }
		public int DepartmentId { get; set; }
	}
}
