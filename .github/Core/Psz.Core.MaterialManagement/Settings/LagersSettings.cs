namespace Psz.Core.MaterialManagement.Settings
{
	public class LagersSettings
	{
		public Country[] Countries { get; set; }
	}

	public class Unit
	{
		public int UnitId { get; set; }
		public Lagers[] LagersId { get; set; }

	}
	public class Country
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public Unit[] CountryUnits { get; set; }
	}
	public class Lagers
	{
		public int LagerId { get; set; }
	}
}
