using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS.FA_NPEX_REP
{
	public class Query1Entity
	{
		public string Artikelnummer { get; set; }
		public string Freigabestatus { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public DateTime? Termin_Fertigstellung { get; set; }
		public int? Anzahl { get; set; }
		public string Bemerkung { get; set; }
		public Decimal? Preis { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Kunde { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }
		public bool? Erstmuster { get; set; }

		public Query1Entity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Termin_Fertigstellung = (dataRow["Termin_Fertigstellung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Fertigstellung"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
		}

	}

	public class Query2Entity
	{
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public int? Anzahl { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int? Artikel_Nr_des_Bauteils { get; set; }
		public string Artikelnummer_ROH { get; set; }
		public string Bezeichnung_des_Bauteils { get; set; }
		public Decimal? Anzahl_ROH { get; set; }
		public Decimal? Bedarf { get; set; }
		public Decimal? Bestand { get; set; }

		public Query2Entity(DataRow dataRow)
		{
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Artikel_Nr_des_Bauteils = (dataRow["Artikel-Nr des Bauteils"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr des Bauteils"]);
			Artikelnummer_ROH = (dataRow["Artikelnummer ROH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer ROH"]);
			Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
			Anzahl_ROH = (dataRow["Anzahl ROH"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl ROH"]);
			Bedarf = (dataRow["Bedarf"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bedarf"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
		}
	}
}
