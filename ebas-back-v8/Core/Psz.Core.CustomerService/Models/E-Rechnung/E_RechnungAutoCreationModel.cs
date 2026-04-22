namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class E_RechnungAutoCreationModel
	{
		public int Nr { get; set; }
		public int ProjectNr { get; set; }
		public int ForfallNr { get; set; }
		public string Type { get; set; }
		public int LSNr { get; set; }
		public int LsAngebotNr { get; set; }
		public string Customer { get; set; }
		public int CustomerNr { get; set; }
		public bool? Validated { get; set; }
		public bool? Sent { get; set; }
		public int CustomerNumber { get; set; }

	}
}
