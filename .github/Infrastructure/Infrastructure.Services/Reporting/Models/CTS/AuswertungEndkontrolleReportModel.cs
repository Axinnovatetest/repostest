using System.Collections;

namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class AuswertungEndkontrolleReportModel
	{
		public string Artikelnummer { get; set; }
		public string GesamtMenge { get; set; }
		public string MengeOffen { get; set; }
		public int Fertigungsnummer { get; set; }
		public int Lagerort_id { get; set; }
		public string Datum { get; set; }
		public string Urs_Artikelnummer { get; set; }
		public int Urs_Fa { get; set; }
		public bool Endkontrolle { get; set; }
		public string Kennzeichen { get; set; }
		public string Termin_Bestätigt1 { get; set; }
		public AuswertungEndkontrolleReportModel(Infrastructure.Data.Entities.Joins.FAPlannung.AuswertungEndkontrolleEntity entity)
		{
			Artikelnummer = entity.Artikelnummer;
			GesamtMenge = entity.GesamtMenge.HasValue ? entity.GesamtMenge.ToString() : "";
			MengeOffen = entity.MengeOffen.HasValue ? entity.MengeOffen.ToString() : "";
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			Lagerort_id = entity.Lagerort_id ?? 0;
			Datum = entity.Datum.HasValue ? entity.Datum.Value.ToString("dd/MM/yyyy HH:mm:ss") : "";
			Urs_Artikelnummer = entity.Urs_Artikelnummer;
			Urs_Fa = entity.Urs_Fa ?? 0;
			Endkontrolle = entity.Endkontrolle ?? false;
			Kennzeichen = entity.Kennzeichen;
			Termin_Bestätigt1 = entity.Termin_Bestätigt1.HasValue ? entity.Termin_Bestätigt1.Value.ToString("dd/MM/yyyy") : "";
		}
	}
}