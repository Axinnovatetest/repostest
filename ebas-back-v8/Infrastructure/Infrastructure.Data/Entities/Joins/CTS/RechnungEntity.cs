using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class RechnungEntity
	{
		public string Ausdr1 { get; set; }
		public Decimal? Ausdr3 { get; set; }
		public string Ausdr2 { get; set; }
		public string Ausdr4 { get; set; }
		public DateTime? Datum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Originalanzahl { get; set; }
		public int? Anzahl_erledigt { get; set; }
		public int? Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public Decimal? Betrag { get; set; }
		public Decimal? Preis { get; set; }
		public string Bemerkung { get; set; }
		public string Bezfeld { get; set; }
		public bool? Erstmuster { get; set; }
		public string Zolltarif_nr { get; set; }
		public Decimal? Material1 { get; set; }
		public Decimal? Größe { get; set; }
		public Decimal? Gesamtgewicht { get; set; }
		public string Ausdr5 { get; set; }
		public Decimal? Stundensatz { get; set; }
		public Decimal? MinutenKosten { get; set; }
		public Decimal? Zusatzkosten_FA_Basis_30_Min { get; set; }
		public Decimal? PREIS_Mit_Zusatzkosten_Pro_Stück { get; set; }
		public Decimal? Zusatzkosten_Produktion { get; set; }
		public Decimal? Preis_Total_Mit_Zusatzkosten { get; set; }
		public RechnungEntity(DataRow dataRow)
		{
			Ausdr1 = (dataRow["Ausdr1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ausdr1"]);
			Ausdr3 = (dataRow["Ausdr3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ausdr3"]);
			Ausdr2 = (dataRow["Ausdr2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ausdr2"]);
			Ausdr4 = (dataRow["Ausdr4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ausdr4"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			Anzahl_erledigt = (dataRow["Anzahl_erledigt"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_erledigt"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bezfeld = (dataRow["Bezfeld"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezfeld"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			Material1 = (dataRow["Material1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Material1"]);
			Größe = (dataRow["Größe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Größe"]);
			Gesamtgewicht = (dataRow["Gesamtgewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtgewicht"]);
			Ausdr5 = (dataRow["Ausdr5"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ausdr5"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
			MinutenKosten = (dataRow["MinutenKosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["MinutenKosten"]);
			Zusatzkosten_FA_Basis_30_Min = (dataRow["Zusatzkosten_FA(Basis 30 Min)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zusatzkosten_FA(Basis 30 Min)"]);
			PREIS_Mit_Zusatzkosten_Pro_Stück = (dataRow["PREIS_Mit_Zusatzkosten_Pro_Stück"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PREIS_Mit_Zusatzkosten_Pro_Stück"]);
			Zusatzkosten_Produktion = (dataRow["Zusatzkosten_Produktion"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Zusatzkosten_Produktion"]);
			Preis_Total_Mit_Zusatzkosten = (dataRow["Preis_Total_Mit_Zusatzkosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis_Total_Mit_Zusatzkosten"]);
		}
	}

	public class RgSpritzgussEntity
	{
		public Decimal? Ausdr3 { get; set; }
		public DateTime? Datum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Originalanzahl { get; set; }
		public int? Anzahl_erledigt { get; set; }
		public int? Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public Decimal? Betrag { get; set; }
		public Decimal? Preis { get; set; }
		public string Bemerkung { get; set; }
		public string Bezfeld { get; set; }
		public bool? Erstmuster { get; set; }
		public string Zolltarif_nr { get; set; }
		public Decimal? Material1 { get; set; }
		public Decimal? Größe { get; set; }
		public Decimal? Gesamtgewicht { get; set; }

		public RgSpritzgussEntity(DataRow dataRow)
		{

			Ausdr3 = (dataRow["Ausdr3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ausdr3"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			Anzahl_erledigt = (dataRow["Anzahl_erledigt"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_erledigt"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bezfeld = (dataRow["Bezfeld"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezfeld"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			Material1 = (dataRow["Material"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Material"]);
			Größe = (dataRow["Größe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Größe"]);
			Gesamtgewicht = (dataRow["Gesamtgewicht"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtgewicht"]);
		}
	}
}
