using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class RechnungEndkontrolleReportModel
	{
		public RechnungEndkontrolleReportHeaderModel Header { get; set; }
		public List<RechnungEndkontrolleReportDetailsModel> Details { get; set; }
	}
	public class RechnungEndkontrolleReportEntryModel: IDateRangeModel
	{
		public string Rechnungnummer { get; set; }
		public DateTime RechnungDatum { get; set; }
	}

	public class RechnungEndkontrolleReportHeaderModel
	{
		public string From { get; set; }
		public string To { get; set; }
		public string Rechnungnummer { get; set; }
		public string RechnungDatum { get; set; }
		public string DatePlus10 { get; set; }
		public string Logo { get; set; }
	}

	public class RechnungEndkontrolleReportDetailsModel
	{
		public string Datum { get; set; }
		public int Fertigungsnummer { get; set; }
		public int Originalanzahl { get; set; }
		public int Anzahl_erledigt { get; set; }
		public int Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public decimal Betrag { get; set; }
		public decimal Ausdr3 { get; set; }
		public decimal Preis { get; set; }
		public string Bemerkung { get; set; }
		public string Bezfeld { get; set; }
		public bool Erstmuster { get; set; }

		public RechnungEndkontrolleReportDetailsModel(Infrastructure.Data.Entities.Joins.CTS.RechnungEndkontrolleEntity entity)
		{
			Datum = entity.Datum.HasValue ? entity.Datum.Value.ToString("dd.MM.yyyy") : "";
			Fertigungsnummer = entity.Fertigungsnummer ?? 0;
			Originalanzahl = entity.Originalanzahl ?? 0;
			Anzahl_erledigt = entity.Anzahl_erledigt ?? 0;
			Anzahl = entity.Anzahl ?? 0;
			Artikelnummer = entity.Artikelnummer;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Betrag = entity.Betrag ?? 0;
			Ausdr3 = entity.Ausdr3 ?? 0;
			Preis = entity.Preis ?? 0;
			Bemerkung = entity.Bemerkung;
			Bezfeld = entity.Bezfeld;
			Erstmuster = entity.Erstmuster ?? false;
		}
	}
}
