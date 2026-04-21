namespace Psz.Core.CustomerService.Models.Statistics
{
	public class StatRequestModel
	{
		public string Typ { get; set; }
		public bool CurentYear { get; set; }
		public bool CurentMonth { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public bool AngeboteNr { get; set; }

	}
}
