namespace Psz.Core.Logistics.Models.Lagebewegung.PDFReports
{
	public class HeaderGroupEntnahmeWertReportModel
	{
		public string datum { get; set; }
		public int totalLigne { get; set; }
		public decimal gesmtPreis { get; set; }
		public decimal percentPreis { get; set; }
		public HeaderGroupEntnahmeWertReportModel()
		{

		}
		public HeaderGroupEntnahmeWertReportModel(string datum, int totalLigne, decimal gesmtPreis, decimal percentPreis)
		{
			this.datum = datum;
			this.totalLigne = totalLigne;
			this.gesmtPreis = gesmtPreis;
			this.percentPreis = percentPreis;


		}
	}
}

