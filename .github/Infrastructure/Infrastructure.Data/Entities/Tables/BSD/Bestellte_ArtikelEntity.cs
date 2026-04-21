using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Bestellte_ArtikelEntity
	{
		public string AB_Nr_Lieferant { get; set; }
		public decimal? Aktuelle_Anzahl { get; set; }
		public decimal? AnfangLagerBestand { get; set; }
		public decimal? Anzahl { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Bemerkung_Pos { get; set; }
		public bool? Bemerkung_Pos_ID { get; set; }
		public DateTime? Bestätigter_Termin { get; set; }
		public string Bestellnummer { get; set; }
		public int? Bestellung_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public int? BP_zu_RBposition { get; set; }
		public bool? COC_bestätigung { get; set; }
		public decimal? CUPreis { get; set; }
		public string Einheit { get; set; }
		public decimal? Einzelpreis { get; set; }
		public bool? EMPB_Bestätigung { get; set; }
		public decimal? EndeLagerBestand { get; set; }
		public decimal? Erhalten { get; set; }
		public bool? erledigt_pos { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public string InfoRahmennummer { get; set; }
		public bool? Kanban { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Liefertermin { get; set; }
		public bool? Löschen { get; set; }
		public DateTime? MhdDatumArtikel { get; set; }
		public int Nr { get; set; }
		public int? Position { get; set; }
		public bool? Position_erledigt { get; set; }
		public decimal? Preiseinheit { get; set; }
		public int? Preisgruppe { get; set; }
		public int? Produktionsort { get; set; }
		public Single? Rabatt { get; set; }
		public Single? Rabatt1 { get; set; }
		public decimal? RB_Abgerufen { get; set; }
		public decimal? RB_Offen { get; set; }
		public decimal? RB_OriginalAnzahl { get; set; }
		public string schriftart { get; set; }
		public string sortierung { get; set; }
		public decimal? Start_Anzahl { get; set; }
		public Single? Umsatzsteuer { get; set; }
		public int? WE_Pos_zu_Bestellposition { get; set; }

		public Bestellte_ArtikelEntity() { }

		public Bestellte_ArtikelEntity(DataRow dataRow)
		{
			AB_Nr_Lieferant = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
			Aktuelle_Anzahl = (dataRow["Aktuelle Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aktuelle Anzahl"]);
			AnfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bemerkung_Pos = (dataRow["Bemerkung_Pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Pos"]);
			Bemerkung_Pos_ID = (dataRow["Bemerkung_Pos_ID"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bemerkung_Pos_ID"]);
			Bestätigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellnummer"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			BP_zu_RBposition = (dataRow["BP zu RBposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BP zu RBposition"]);
			COC_bestätigung = (dataRow["COC_bestätigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COC_bestätigung"]);
			CUPreis = (dataRow["CUPreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CUPreis"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelpreis"]);
			EMPB_Bestätigung = (dataRow["EMPB_Bestätigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Bestätigung"]);
			EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
			Erhalten = (dataRow["Erhalten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Erhalten"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			InfoRahmennummer = (dataRow["InfoRahmennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InfoRahmennummer"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			MhdDatumArtikel = (dataRow["MhdDatumArtikel"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MhdDatumArtikel"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Position_erledigt = (dataRow["Position erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Position erledigt"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			Produktionsort = (dataRow["Produktionsort"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionsort"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rabatt"]);
			Rabatt1 = (dataRow["Rabatt1"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rabatt1"]);
			RB_Abgerufen = (dataRow["RB_Abgerufen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Abgerufen"]);
			RB_Offen = (dataRow["RB_Offen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Offen"]);
			RB_OriginalAnzahl = (dataRow["RB_OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_OriginalAnzahl"]);
			schriftart = (dataRow["schriftart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["schriftart"]);
			sortierung = (dataRow["sortierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["sortierung"]);
			Start_Anzahl = (dataRow["Start Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Start Anzahl"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Umsatzsteuer"]);
			WE_Pos_zu_Bestellposition = (dataRow["WE Pos zu Bestellposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WE Pos zu Bestellposition"]);
		}
	}
}

