namespace Psz.Core.Logistics.Models.Lagebewegung.PDFReports
{
	public class DetailEntnahmeWertReportModel
	{
		public string datum { get; set; }
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public decimal anzahl { get; set; }
		public long zuFA { get; set; }
		public int grund { get; set; }
		public decimal kosten { get; set; }
		public string bemerkung { get; set; }
		public DetailEntnahmeWertReportModel()
		{

		}
		public DetailEntnahmeWertReportModel(Infrastructure.Data.Entities.Joins.Logistics.EntnahmeWertEntity entnahmeWertEntity)
		{
			if(entnahmeWertEntity == null)
				return;
			datum = entnahmeWertEntity.datum == null ? "" : entnahmeWertEntity.datum.Value.ToString("dd-MM-yyyy");
			artikelNr = entnahmeWertEntity.artikelNr;
			artikelnummer = entnahmeWertEntity.artikelnummer;
			bezeichnung1 = entnahmeWertEntity.bezeichnung1;
			anzahl = entnahmeWertEntity.anzahl;
			zuFA = entnahmeWertEntity.zuFA;
			grund = entnahmeWertEntity.grund;
			kosten = entnahmeWertEntity.kosten;
			bemerkung = entnahmeWertEntity.bemerkung;

		}
	}
}
