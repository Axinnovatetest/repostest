using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class ExportReportModel
	{
		public ExportReportHeaderModel Header { get; set; }
		public List<ExportReportDetailsModel> Details { get; set; }

		public ExportReportModel()
		{

		}
	}
	public class ExportEntryModel: IDateRangeNullableModel
	{
		public string Artikel { get; set; }
		public int? Lager { get; set; }
		public bool? SortByArticle { get; set; }
		public bool? SortByDate { get; set; }
	}
	public class ExportReportHeaderModel
	{
		public string Von { get; set; }
		public string Bis { get; set; }
		public decimal SummGesamtpreis { get; set; }
		public string Date { get; set; } = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
		public string Title { get; set; }
	}
	public class ExportReportDetailsModel
	{
		public string Preis { get; set; }
		public Decimal Gesamtpreis { get; set; }
		public string Datum { get; set; }
		public string Artikelnummer { get; set; }
		public string Fertigungsnummer { get; set; }
		public string Uhrzeit { get; set; }
		public string Bezeichnung { get; set; }
		public string Auftragsbemerkung { get; set; }
		public string Anzahl_Auftrag { get; set; }
		public string Anzahl_aktuelle_Lieferung { get; set; }
		public string Anzahl_Kartons { get; set; }
		public string Mitarbeiter_Name { get; set; }
		public string Bemerkung { get; set; }
		public string Ausdr1 { get; set; }
		public string Ausdr2 { get; set; }
		public string Lagerort { get; set; }
		public string CS_Kontakt { get; set; }
		public string Kennzeichen { get; set; }
		public string Liefermenge_QRCode { get; set; }

		public ExportReportDetailsModel(Infrastructure.Data.Entities.Joins.CTS.ExportEntity entity)
		{
			Preis = string.Format("{0:n}", Math.Round(entity.Preis ?? 0));
			Gesamtpreis = entity.Gesamtpreis ?? 0;
			Datum = entity.Datum.HasValue ? entity.Datum.Value.ToString("dd.MM.yyyy") : "";
			Artikelnummer = entity.Artikelnummer;
			Fertigungsnummer = entity.Fertigungsnummer.HasValue ? entity.Fertigungsnummer.ToString() : "";
			Uhrzeit = entity.Uhrzeit.HasValue ? entity.Uhrzeit.Value.ToString("dd.MM.yyyy HH:mm:ss") : "";
			Bezeichnung = entity.Bezeichnung;
			Auftragsbemerkung = entity.Auftragsbemerkung;
			Anzahl_Auftrag = entity.Anzahl_Auftrag.HasValue ? entity.Anzahl_Auftrag.ToString() : "";
			Anzahl_aktuelle_Lieferung = entity.Anzahl_aktuelle_Lieferung.HasValue ? entity.Anzahl_aktuelle_Lieferung.ToString() : "";
			Anzahl_Kartons = entity.Anzahl_Kartons.HasValue ? entity.Anzahl_Kartons.ToString() : "";
			Mitarbeiter_Name = entity.Mitarbeiter_Name;
			Bemerkung = entity.Bemerkung;
			Ausdr1 = entity.Ausdr1.HasValue ? entity.Ausdr1.Value.ToString("dd.MM.yyyy") : "";
			Ausdr2 = entity.Ausdr2.HasValue ? entity.Ausdr2.Value.ToString("dd.MM.yyyy") : "";
			Lagerort = entity.Lagerort;
			CS_Kontakt = entity.CS_Kontakt;
			Kennzeichen = entity.Kennzeichen;
			Liefermenge_QRCode = entity.Liefermenge_QRCode.HasValue ? entity.Liefermenge_QRCode.ToString() : "";
		}
	}
}
