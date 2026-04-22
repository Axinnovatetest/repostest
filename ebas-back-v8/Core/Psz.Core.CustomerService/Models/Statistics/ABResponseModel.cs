namespace Psz.Core.CustomerService.Models.Statistics
{
	public class ABResponseModel
	{
		public int? TotalAB { get; set; }
		public int? ZahlungsweiseRechnung { get; set; }
		public int? ZahlungsweiseVorkasse { get; set; }
		public int? ZahlungsweiseVorauskasse { get; set; }
		public int? ZahlungsweiseGutschrift { get; set; }
		public int? ZahlungsweiseLastschrift { get; set; }
		public int? ZahlungsweiseUberweisung { get; set; }
		public int? Edi { get; set; }
		public int? NotEdi { get; set; }
	}

}
