using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class LSDruckerEntity
	{
		public decimal gesamtgewicht { get; set; }
		public long skontotage { get; set; }
		public decimal skonto { get; set; }
		public long nettotage { get; set; }
		public string Artikelnummer { get; set; }
		public string fax { get; set; }
		public string ursprungsland { get; set; }
		public string textAuftragsbestätigung { get; set; }
		public string textLieferschein { get; set; }
		public string textRechnung { get; set; }
		public string textGutschrift { get; set; }
		public bool delFixiert { get; set; }
		public decimal grosse { get; set; }
		public string ZolltarifNr { get; set; }
		public string index_Kunde { get; set; }
		public DateTime? index_Kunde_Datum { get; set; }
		public long aB_Pos_zu_RA_Pos { get; set; }
		public long rA_OriginalAnzahl { get; set; }
		public long rA_Abgerufen { get; set; }
		public long rA_Offen { get; set; }
		public bool erledigt_pos { get; set; }
		public bool Versandstatus { get; set; }
		public bool gebucht { get; set; }
		public bool lS_von_Versand_gedruckt { get; set; }
		public string typ { get; set; }
		public long angeboteNr { get; set; }
		public int artikelNr { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung2 { get; set; }
		public string bezeichnung3 { get; set; }
		public string einheit { get; set; }
		public decimal anfangLagerBestand { get; set; }
		public decimal anzahl { get; set; }
		public decimal originalAnzahl { get; set; }
		public decimal geliefert { get; set; }
		public decimal aktuelleAnzahl { get; set; }

		public LSDruckerEntity(DataRow dataRow)
		{
			gesamtgewicht = (dataRow["Gesamtgewicht"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gesamtgewicht"]);
			skontotage = (dataRow["Skontotage"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Skontotage"]);
			skonto = (dataRow["Skonto"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Skonto"]);
			nettotage = (dataRow["Nettotage"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Nettotage"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"]);
			textAuftragsbestätigung = (dataRow["TextAuftragsbestätigung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TextAuftragsbestätigung"]);
			textLieferschein = (dataRow["TextLieferschein"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TextLieferschein"]);
			textRechnung = (dataRow["TextRechnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TextRechnung"]);
			textGutschrift = (dataRow["TextGutschrift"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TextGutschrift"]);
			delFixiert = (dataRow["DEL fixiert"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["DEL fixiert"]);
			grosse = (dataRow["Größe"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Größe"]);
			ZolltarifNr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"]);
			index_Kunde = (dataRow["Index_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Index_Kunde"]);
			index_Kunde_Datum = (dataRow["Index_Kunde_Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
			aB_Pos_zu_RA_Pos = (dataRow["AB Pos zu RA Pos"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["AB Pos zu RA Pos"]);
			rA_OriginalAnzahl = (dataRow["RA_OriginalAnzahl"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["RA_OriginalAnzahl"]);
			rA_Abgerufen = (dataRow["RA_Abgerufen"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["RA_Abgerufen"]);
			rA_Offen = (dataRow["RA_Offen"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["RA_Offen"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Versandstatus = (dataRow["Versandstatus"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Versandstatus"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["gebucht"]);
			lS_von_Versand_gedruckt = (dataRow["LS_von_Versand_gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["LS_von_Versand_gedruckt"]);
			typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			angeboteNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Angebot-Nr"]);
			artikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Artikel-Nr"]);
			bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			bezeichnung2 = (dataRow["Bezeichnung2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung2"]);
			bezeichnung3 = (dataRow["Bezeichnung3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung3"]);
			einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			anfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
			anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Anzahl"]);
			originalAnzahl = (dataRow["OriginalAnzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["OriginalAnzahl"]);
			geliefert = (dataRow["Geliefert"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Geliefert"]);
			aktuelleAnzahl = (dataRow["Aktuelle Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Aktuelle Anzahl"]);
		}
	}
}
