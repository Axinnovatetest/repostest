using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
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
		public DateTime? Bestatigter_Termin { get; set; }
		public string Bestellnummer { get; set; }
		public int? Bestellung_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public int? BP_zu_RBposition { get; set; }
		public bool? COC_bestatigung { get; set; }
		public decimal? CUPreis { get; set; }
		public string Einheit { get; set; }
		public decimal? Einzelpreis { get; set; }
		public bool? EMPB_Bestatigung { get; set; }
		public decimal? EndeLagerBestand { get; set; }
		public decimal? Erhalten { get; set; }
		public bool? erledigt_pos { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public string InfoRahmennummer { get; set; }
		public bool? Kanban { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Liefertermin { get; set; }
		public bool? Loschen { get; set; }
		public DateTime? MhdDatumArtikel { get; set; }
		public int Nr { get; set; }
		public int? Position { get; set; }
		public bool? Position_erledigt { get; set; }
		public decimal? Preiseinheit { get; set; }
		public int? Preisgruppe { get; set; }
		public int? Produktionsort { get; set; }
		public double? Rabatt { get; set; }
		public double? Rabatt1 { get; set; }
		public decimal? RB_Abgerufen { get; set; }
		public decimal? RB_Offen { get; set; }
		public decimal? RB_OriginalAnzahl { get; set; }
		public string schriftart { get; set; }
		public string sortierung { get; set; }
		public decimal? Start_Anzahl { get; set; }
		public decimal? Umsatzsteuer { get; set; }
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
			Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellnummer"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			BP_zu_RBposition = (dataRow["BP zu RBposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BP zu RBposition"]);
			COC_bestatigung = (dataRow["COC_bestätigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COC_bestätigung"]);
			CUPreis = (dataRow["CUPreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CUPreis"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelpreis"]);
			EMPB_Bestatigung = (dataRow["EMPB_Bestätigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Bestätigung"]);
			EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
			Erhalten = (dataRow["Erhalten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Erhalten"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			InfoRahmennummer = (dataRow["InfoRahmennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InfoRahmennummer"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Loschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			MhdDatumArtikel = (dataRow["MhdDatumArtikel"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MhdDatumArtikel"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Position_erledigt = (dataRow["Position erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Position erledigt"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			Produktionsort = (dataRow["Produktionsort"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionsort"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt"]);
			Rabatt1 = (dataRow["Rabatt1"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt1"]);
			RB_Abgerufen = (dataRow["RB_Abgerufen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Abgerufen"]);
			RB_Offen = (dataRow["RB_Offen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Offen"]);
			RB_OriginalAnzahl = (dataRow["RB_OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_OriginalAnzahl"]);
			schriftart = (dataRow["schriftart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["schriftart"]);
			sortierung = (dataRow["sortierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["sortierung"]);
			Start_Anzahl = (dataRow["Start Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Start Anzahl"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Umsatzsteuer"]);
			WE_Pos_zu_Bestellposition = (dataRow["WE Pos zu Bestellposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WE Pos zu Bestellposition"]);
		}
		public Bestellte_ArtikelEntity SelfCopy()
		{
			return new Bestellte_ArtikelEntity
			{
				AB_Nr_Lieferant = AB_Nr_Lieferant,
				Aktuelle_Anzahl = Aktuelle_Anzahl,
				AnfangLagerBestand = AnfangLagerBestand,
				Anzahl = Anzahl,
				Artikel_Nr = Artikel_Nr,
				Bemerkung_Pos = Bemerkung_Pos,
				Bemerkung_Pos_ID = Bemerkung_Pos_ID,
				Bestatigter_Termin = Bestatigter_Termin,
				Bestellnummer = Bestellnummer,
				Bestellung_Nr = Bestellung_Nr,
				Bezeichnung_1 = Bezeichnung_1,
				Bezeichnung_2 = Bezeichnung_2,
				BP_zu_RBposition = BP_zu_RBposition,
				COC_bestatigung = COC_bestatigung,
				CUPreis = CUPreis,
				Einheit = Einheit,
				Einzelpreis = Einzelpreis,
				EMPB_Bestatigung = EMPB_Bestatigung,
				EndeLagerBestand = EndeLagerBestand,
				Erhalten = Erhalten,
				erledigt_pos = erledigt_pos,
				Gesamtpreis = Gesamtpreis,
				In_Bearbeitung = In_Bearbeitung,
				InfoRahmennummer = InfoRahmennummer,
				Kanban = Kanban,
				Lagerort_id = Lagerort_id,
				Liefertermin = Liefertermin,
				Loschen = Loschen,
				MhdDatumArtikel = MhdDatumArtikel,
				Nr = Nr,
				Position = Position,
				Position_erledigt = Position_erledigt,
				Preiseinheit = Preiseinheit,
				Preisgruppe = Preisgruppe,
				Produktionsort = Produktionsort,
				Rabatt = Rabatt,
				Rabatt1 = Rabatt1,
				RB_Abgerufen = RB_Abgerufen,
				RB_Offen = RB_Offen,
				RB_OriginalAnzahl = RB_OriginalAnzahl,
				schriftart = schriftart,
				sortierung = sortierung,
				Start_Anzahl = Start_Anzahl,
				Umsatzsteuer = Umsatzsteuer,
				WE_Pos_zu_Bestellposition = WE_Pos_zu_Bestellposition,
			};
		}
	}
}

