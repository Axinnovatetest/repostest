using System;
using System.ComponentModel;
using System.Data;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class Bestellte_ArtikelEntity
	{
		public int Nr { get; set; }
		public int? Position { get; set; }
		public int? Bestellung_Nr { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Einheit { get; set; }
		public decimal? AnfangLagerBestand { get; set; }
		public decimal? Anzahl { get; set; }
		public decimal? Start_Anzahl { get; set; }
		public decimal? Erhalten { get; set; }
		public decimal? Aktuelle_Anzahl { get; set; }
		public decimal? EndeLagerBestand { get; set; }
		public Single? Umsatzsteuer { get; set; }
		public decimal? Einzelpreis { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public int? Preisgruppe { get; set; }
		public string Bestellnummer { get; set; }
		public Single? Rabatt { get; set; }
		public Single? Rabatt1 { get; set; }
		public string sortierung { get; set; }
		public string schriftart { get; set; }
		public decimal? Preiseinheit { get; set; }
		public DateTime? Liefertermin { get; set; }
		public bool? erledigt_pos { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public bool? Position_erledigt { get; set; }
		public string Bemerkung_Pos { get; set; }
		public bool? Bemerkung_Pos_ID { get; set; }
		public int? Produktionsort { get; set; }
		public int? BP_zu_RBposition { get; set; }
		public int? WE_Pos_zu_Bestellposition { get; set; }
		public string AB_Nr_Lieferant { get; set; }
		public decimal? RB_OriginalAnzahl { get; set; }
		public decimal? RB_Abgerufen { get; set; }
		public decimal? RB_Offen { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public bool? Loschen { get; set; }
		public bool? Kanban { get; set; }
		public DateTime? MhdDatumArtikel { get; set; }
		public bool? COC_bestatigung { get; set; }
		public string InfoRahmennummer { get; set; }
		public bool? EMPB_Bestatigung { get; set; }
		public decimal? CUPreis { get; set; }
		public int? RA_Pos_zu_Bestellposition { get; set; }
		public string CocVersion { get; set; }
		public int? LagerbewegungPositionId { get; set; }
		public bool? StandardSupplierViolation { get; set; }
		public Bestellte_ArtikelEntity() { }
		public Bestellte_ArtikelEntity(DataRow dataRow)
		{
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			AnfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Start_Anzahl = (dataRow["Start Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Start Anzahl"]);
			Erhalten = (dataRow["Erhalten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Erhalten"]);
			Aktuelle_Anzahl = (dataRow["Aktuelle Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aktuelle Anzahl"]);
			EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Umsatzsteuer"]);
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelpreis"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellnummer"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rabatt"]);
			Rabatt1 = (dataRow["Rabatt1"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Rabatt1"]);
			sortierung = (dataRow["sortierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["sortierung"]);
			schriftart = (dataRow["schriftart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["schriftart"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
			Position_erledigt = (dataRow["Position erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Position erledigt"]);
			Bemerkung_Pos = (dataRow["Bemerkung_Pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Pos"]);
			Bemerkung_Pos_ID = (dataRow["Bemerkung_Pos_ID"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bemerkung_Pos_ID"]);
			Produktionsort = (dataRow["Produktionsort"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionsort"]);
			BP_zu_RBposition = (dataRow["BP zu RBposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BP zu RBposition"]);
			WE_Pos_zu_Bestellposition = (dataRow["WE Pos zu Bestellposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WE Pos zu Bestellposition"]);
			AB_Nr_Lieferant = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
			RB_OriginalAnzahl = (dataRow["RB_OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_OriginalAnzahl"]);
			RB_Abgerufen = (dataRow["RB_Abgerufen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Abgerufen"]);
			RB_Offen = (dataRow["RB_Offen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Offen"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			Loschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			MhdDatumArtikel = (dataRow["MhdDatumArtikel"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MhdDatumArtikel"]);
			COC_bestatigung = (dataRow["COC_bestätigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COC_bestätigung"]);
			InfoRahmennummer = (dataRow["InfoRahmennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InfoRahmennummer"]);
			EMPB_Bestatigung = (dataRow["EMPB_Bestätigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Bestätigung"]);
			CUPreis = (dataRow["CUPreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CUPreis"]);
			RA_Pos_zu_Bestellposition = (dataRow["RA Pos zu Bestellposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RA Pos zu Bestellposition"]);
			CocVersion = (dataRow["CocVersion"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CocVersion"]);
			LagerbewegungPositionId = (dataRow["LagerbewegungPositionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerbewegungPositionId"]);
			StandardSupplierViolation = (dataRow["StandardSupplierViolation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["StandardSupplierViolation"]);
		}
		public Bestellte_ArtikelEntity ShallowClone()
		{
			return new Bestellte_ArtikelEntity
			{
				Nr = Nr,
				Position = Position,
				Bestellung_Nr = Bestellung_Nr,
				Artikel_Nr = Artikel_Nr,
				Bezeichnung_1 = Bezeichnung_1,
				Bezeichnung_2 = Bezeichnung_2,
				Einheit = Einheit,
				AnfangLagerBestand = AnfangLagerBestand,
				Anzahl = Anzahl,
				Start_Anzahl = Start_Anzahl,
				Erhalten = Erhalten,
				Aktuelle_Anzahl = Aktuelle_Anzahl,
				EndeLagerBestand = EndeLagerBestand,
				Umsatzsteuer = Umsatzsteuer,
				Einzelpreis = Einzelpreis,
				Gesamtpreis = Gesamtpreis,
				Preisgruppe = Preisgruppe,
				Bestellnummer = Bestellnummer,
				Rabatt = Rabatt,
				Rabatt1 = Rabatt1,
				sortierung = sortierung,
				schriftart = schriftart,
				Preiseinheit = Preiseinheit,
				Liefertermin = Liefertermin,
				erledigt_pos = erledigt_pos,
				Lagerort_id = Lagerort_id,
				Bestatigter_Termin = Bestatigter_Termin,
				Position_erledigt = Position_erledigt,
				Bemerkung_Pos = Bemerkung_Pos,
				Bemerkung_Pos_ID = Bemerkung_Pos_ID,
				Produktionsort = Produktionsort,
				BP_zu_RBposition = BP_zu_RBposition,
				WE_Pos_zu_Bestellposition = WE_Pos_zu_Bestellposition,
				AB_Nr_Lieferant = AB_Nr_Lieferant,
				RB_OriginalAnzahl = RB_OriginalAnzahl,
				RB_Abgerufen = RB_Abgerufen,
				RB_Offen = RB_Offen,
				In_Bearbeitung = In_Bearbeitung,
				Loschen = Loschen,
				Kanban = Kanban,
				MhdDatumArtikel = MhdDatumArtikel,
				COC_bestatigung = COC_bestatigung,
				InfoRahmennummer = InfoRahmennummer,
				EMPB_Bestatigung = EMPB_Bestatigung,
				CUPreis = CUPreis,
				RA_Pos_zu_Bestellposition = RA_Pos_zu_Bestellposition,
				CocVersion = CocVersion,
				LagerbewegungPositionId = LagerbewegungPositionId,
				StandardSupplierViolation = StandardSupplierViolation,
			};
		}
	}
}