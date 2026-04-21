namespace Psz.Core.Logistics.Models.Lagebewegung.PDFReports
{
	public class HeaderEntnahmeWertReportModel
	{
		public string lagerort { get; set; }
		public string dateDebut { get; set; }
		public string dateFin { get; set; }

		public HeaderEntnahmeWertReportModel()
		{

		}
		public HeaderEntnahmeWertReportModel(string lagerort, string dateDebut, string DateFin)
		{
			this.lagerort = lagerort;
			this.dateDebut = dateDebut;
			this.dateFin = DateFin;

		}
	}
}
