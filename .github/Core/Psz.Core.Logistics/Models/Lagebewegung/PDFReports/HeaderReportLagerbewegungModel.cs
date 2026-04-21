namespace Psz.Core.Logistics.Models.Lagebewegung.PDFReports
{
	public class HeaderReportLagerbewegungModel
	{
		public long id { get; set; }
		public string typ { get; set; }
		public string datum { get; set; }
		public string gebuchtVon { get; set; }
		public HeaderReportLagerbewegungModel()
		{

		}
		public HeaderReportLagerbewegungModel(long id, string typ, string datum, string gebuchtVon)
		{
			this.id = id;
			this.typ = typ;
			this.datum = datum;
			this.gebuchtVon = gebuchtVon;
		}
	}
}
