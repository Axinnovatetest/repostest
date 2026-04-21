using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class KapazitatLangEntity
	{
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public bool? gebucht { get; set; }
		public string Kennzeichen { get; set; }
		public int? artikel_nr { get; set; }
		public string Kostenart { get; set; }
		public Double? Betrag { get; set; }
		public Double? Auftragszeit { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public string ProdTage { get; set; }
		public DateTime? Ausdr1 { get; set; }
		public DateTime? Ausdr2 { get; set; }
		public int? Anzahl { get; set; }
		public Double? LohnkostLohnkosten { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Bemerkung { get; set; }
		public string Freigabestatus { get; set; }
		public int? Lagerort_id { get; set; }
		public Double? Preis { get; set; }
		public string Lagerort { get; set; }
		public string Sysmonummer { get; set; }
		public bool? Technik { get; set; }
		public string Techniker { get; set; }
		public bool? Erstmuster { get; set; }
		public string Kunde { get; set; }

		public KapazitatLangEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			artikel_nr = (dataRow["artikel_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["artikel_nr"]);
			Kostenart = (dataRow["Kostenart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kostenart"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Betrag"]);
			Auftragszeit = (dataRow["Auftragszeit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Auftragszeit"]);
			Termin_Fertigstellung = (dataRow["Termin_Fertigstellung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Fertigstellung"]);
			ProdTage = (dataRow["ProdTage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProdTage"]);
			Ausdr1 = (dataRow["Ausdr1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ausdr1"]);
			Ausdr2 = (dataRow["Ausdr2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ausdr2"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			LohnkostLohnkosten = (dataRow["LohnkostLohnkosten"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["LohnkostLohnkosten"]);
			Termin_Bestatigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Preis"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Sysmonummer = (dataRow["Sysmonummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sysmonummer"]);
			Technik = (dataRow["Technik"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Technik"]);
			Techniker = (dataRow["Techniker"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Techniker"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
		}


	}
}
