namespace Psz.Core.MaterialManagement.CRP.Models.WorkLocation
{
	public class WorkLocationsModel
	{
		public List<Country> Countries { get; set; } = new List<Country>();

		public class Country
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public List<Hall> Halls { get; set; } = new List<Hall>();

			public class Hall
			{
				public int Id { get; set; }
				public string Name { get; set; }
				public string Area { get; set; }
			}
		}
	}
}
