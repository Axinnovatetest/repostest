using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class PSZ_Projektdaten_DetailsEntity
	{
		public DateTime? AB_Datum { get; set; }
		public int? Arbeitszeit_Serien_Pro_Kabesatz { get; set; }
		public string Artikelnummer { get; set; }
		public string Bemerkungen { get; set; }
		public int? EAU { get; set; }
		public string EMPB { get; set; }
		public DateTime? Erstanlage { get; set; }
		public DateTime? FA_Datum { get; set; }
		public int ID { get; set; }
		public string Kontakt_AV_PSZ { get; set; }
		public string Kontakt_CS_PSZ { get; set; }
		public string Kontakt_Technik_Kunde { get; set; }
		public string Kontakt_Technik_PSZ { get; set; }
		public int? Kosten { get; set; }
		public string Krimp_WKZ { get; set; }
		public string Material_Eskalation_AV { get; set; }
		public string Material_Eskalation_Termin { get; set; }
		public string Material_Komplett { get; set; }
		public int? Menge { get; set; }
		public int? MOQ { get; set; }
		public string Projekt_betreung { get; set; }
		public string Projekt_Start { get; set; }
		public bool? Projektmeldung { get; set; }
		public string Projekt_Nr { get; set; }
		public string Rapid_Prototyp { get; set; }
		public string Serie_PSZ { get; set; }
		public string SG_WKZ { get; set; }
		public string Standort_Muster { get; set; }
		public string Standort_Serie { get; set; }
		public string Summe_Arbeitszeit { get; set; }
		public string Termin_mit_Technik_abgesprochen { get; set; }
		public string TSP_Kunden { get; set; }
		public string Typ { get; set; }
		public string UL_Verpackung { get; set; }
		public DateTime? Wunschtermin_Kunde { get; set; }
		public string Zuschlag { get; set; }

		public PSZ_Projektdaten_DetailsEntity() { }

		public PSZ_Projektdaten_DetailsEntity(DataRow dataRow)
		{
			AB_Datum = (dataRow["AB_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AB_Datum"]);
			Arbeitszeit_Serien_Pro_Kabesatz = (dataRow["Arbeitszeit Serien Pro Kabesatz"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Arbeitszeit Serien Pro Kabesatz"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			EAU = (dataRow["EAU"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EAU"]);
			EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EMPB"]);
			Erstanlage = (dataRow["Erstanlage"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Erstanlage"]);
			FA_Datum = (dataRow["FA_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Datum"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kontakt_AV_PSZ = (dataRow["Kontakt_AV_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_AV_PSZ"]);
			Kontakt_CS_PSZ = (dataRow["Kontakt_CS_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_CS_PSZ"]);
			Kontakt_Technik_Kunde = (dataRow["Kontakt_Technik_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_Technik_Kunde"]);
			Kontakt_Technik_PSZ = (dataRow["Kontakt_Technik_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontakt_Technik_PSZ"]);
			Kosten = (dataRow["Kosten"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kosten"]);
			Krimp_WKZ = (dataRow["Krimp_WKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Krimp_WKZ"]);
			Material_Eskalation_AV = (dataRow["Material_Eskalation_AV"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Eskalation_AV"]);
			Material_Eskalation_Termin = (dataRow["Material_Eskalation_Termin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Eskalation_Termin"]);
			Material_Komplett = (dataRow["Material_Komplett"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material_Komplett"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
			MOQ = (dataRow["MOQ"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["MOQ"]);
			Projekt_betreung = (dataRow["Projekt betreung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt betreung"]);
			Projekt_Start = (dataRow["Projekt_Start"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt_Start"]);
			Projektmeldung = (dataRow["Projektmeldung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Projektmeldung"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			Rapid_Prototyp = (dataRow["Rapid Prototyp"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rapid Prototyp"]);
			Serie_PSZ = (dataRow["Serie_PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Serie_PSZ"]);
			SG_WKZ = (dataRow["SG_WKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SG_WKZ"]);
			Standort_Muster = (dataRow["Standort_Muster"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standort_Muster"]);
			Standort_Serie = (dataRow["Standort_Serie"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standort_Serie"]);
			Summe_Arbeitszeit = (dataRow["Summe Arbeitszeit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Summe Arbeitszeit"]);
			Termin_mit_Technik_abgesprochen = (dataRow["Termin mit Technik abgesprochen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin mit Technik abgesprochen"]);
			TSP_Kunden = (dataRow["TSP Kunden"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TSP Kunden"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			UL_Verpackung = (dataRow["UL_Verpackung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UL_Verpackung"]);
			Wunschtermin_Kunde = (dataRow["Wunschtermin_Kunde"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Wunschtermin_Kunde"]);
			Zuschlag = (dataRow["Zuschlag"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zuschlag"]);
		}
	}
}

