namespace Psz.Core.Models
{
	public class SldSettings
	{
		public WPLSettings WPL { get; set; }
		public class WPLSettings
		{
			public int CzechCountryId { get; set; }
			public string CzechHallName { get; set; }
			public string CzechHallAddress { get; set; }
		}
	}
}
