using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class NachBerechnungTNReportModel
	{
		public List<NachBerechnungTNHeaderReportModel> Header { get; set; }
		public List<NachBerechnungTNReportDetailsModel> Details { get; set; }
	}

	public class NachBerechnungTNHeaderReportModel
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Rechnungnummer { get; set; }
		public string RechnungDatum { get; set; }
	}

	public class NachBerechnungTNReportDetailsModel
	{
		public string Kostenart { get; set; }
		public bool ab_buchen { get; set; }
		public string Datum { get; set; }
		public int Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public int Originalanzahl { get; set; }
		public int Anzahl { get; set; }
		public decimal Betrag { get; set; }
		public decimal Preis { get; set; }
		public decimal Lohn_alt { get; set; }
		public decimal Lohn_neu { get; set; }
		public decimal Ausdr5 { get; set; }
		public decimal Ausdr3 { get; set; }
		public string Bezfeld { get; set; }

		public NachBerechnungTNReportDetailsModel(Infrastructure.Data.Entities.Joins.CTS.NachBerechnungTNEntity entity)
		{
			Kostenart = entity.Kostenart;
			ab_buchen = entity.ab_buchen ?? false;
			Datum = entity.Datum.HasValue ? entity.Datum.Value.ToString("dd.MM.yyyy") : "";
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			Artikelnummer = entity.Artikelnummer;
			Originalanzahl = entity.Originalanzahl ?? 0;
			Anzahl = entity.Anzahl ?? 0;
			Betrag = entity.Betrag ?? 0;
			Preis = entity.Preis ?? 0;
			Lohn_alt = entity.Lohn_alt ?? 0;
			Lohn_neu = entity.Lohn_neu ?? 0;
			Ausdr5 = entity.Ausdr5 ?? 0;
			Ausdr3 = entity.Ausdr3 ?? 0;
			Bezfeld = entity.Bezfeld;
		}
	}
}
