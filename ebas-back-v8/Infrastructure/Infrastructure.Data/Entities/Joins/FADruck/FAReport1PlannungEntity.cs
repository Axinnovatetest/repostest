using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FADruck
{
	public class FAReport1PlannungEntity
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public Decimal? Anzahl { get; set; }
		public string Lagerort { get; set; }
		public int? Fertigungsnummer { get; set; }
		public DateTime? Datum { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public string Kennzeichen { get; set; }
		public string Bemerkung { get; set; }
		public string EAN { get; set; }
		public Decimal? Betrag { get; set; }
		public string Freigabestatus { get; set; }
		public Decimal? Produktionszeit { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }
		public bool? Erstmuster { get; set; }
		public string Freigabestatus_TN_intern { get; set; }
		public string Index_Kunde { get; set; }
		public int? Lagerort_id_zubuchen { get; set; }
		public string Mandant { get; set; }
		public string Sysmonummer { get; set; }
		public bool? UL_Etikett { get; set; }
		public bool? Technik { get; set; }
		public string Techniker { get; set; }
		public bool? Kanban { get; set; }
		public string Verpackungsart { get; set; }
		public Decimal? Verpackungsmenge { get; set; }
		public Decimal? Losgroesse { get; set; }
		public bool? Quick_Area { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public string Artikelfamilie_Kunde_Detail1 { get; set; }
		public string Artikelfamilie_Kunde_Detail2 { get; set; }
		public string Klassifizierung { get; set; }
		public string Bezeichnung { get; set; }
		public string Nummernkreis { get; set; }
		public string Kupferzahl { get; set; }
		public int? ID { get; set; }
		public string Gewerk { get; set; }
		public FAReport1PlannungEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Termin_Fertigstellung = (dataRow["Termin_Fertigstellung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Fertigstellung"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			EAN = (dataRow["EAN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EAN"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Produktionszeit"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			Freigabestatus_TN_intern = (dataRow["Freigabestatus TN intern"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus TN intern"]);
			Index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
			Lagerort_id_zubuchen = (dataRow["Lagerort_id zubuchen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id zubuchen"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
			Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
			UL_Etikett = (dataRow["UL Etikett"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UL Etikett"]);
			Technik = (dataRow["Technik"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Technik"]);
			Techniker = (dataRow["Techniker"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Techniker"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
			Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Verpackungsmenge"]);
			Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Losgroesse"]);
			Quick_Area = (dataRow["Quick_Area"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Quick_Area"]);
			Artikelfamilie_Kunde = (dataRow["Artikelfamilie_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde"]);
			Artikelfamilie_Kunde_Detail1 = (dataRow["Artikelfamilie_Kunde_Detail1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail1"]);
			Artikelfamilie_Kunde_Detail2 = (dataRow["Artikelfamilie_Kunde_Detail2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde_Detail2"]);
			Klassifizierung = (dataRow["Klassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Klassifizierung"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			Nummernkreis = (dataRow["Nummernkreis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nummernkreis"]);
			Kupferzahl = (dataRow["Kupferzahl"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kupferzahl"]);
			ID = (dataRow["ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID"]);
			Gewerk = (dataRow["Gewerk"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk"]);
		}
	}
}
