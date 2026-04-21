namespace Psz.Core.FinanceControl.Models.Budget.Reception.Article
{
	public class UpdateModel
	{
		//public string AB_Nr_Lieferant { get; set; }
		public decimal? Aktuelle_Anzahl { get; set; }
		//public decimal? AnfangLagerBestand { get; set; }
		public decimal? Anzahl { get; set; }
		//public int? Artikel_Nr { get; set; }
		//public string Bemerkung_Pos { get; set; }
		//public bool? Bemerkung_Pos_ID { get; set; }
		//public DateTime? Bestätigter_Termin { get; set; }
		//public string Bestellnummer { get; set; }
		//public int? Bestellung_Nr { get; set; }
		//public string Bezeichnung_1 { get; set; }
		//public string Bezeichnung_2 { get; set; }
		//public int? BP_zu_RBposition { get; set; }
		//public bool? COC_bestätigung { get; set; }
		//public decimal? CUPreis { get; set; }
		//public string Einheit { get; set; }
		public decimal? Einzelpreis { get; set; }
		//public bool? EMPB_Bestätigung { get; set; }
		//public decimal? EndeLagerBestand { get; set; }
		//public decimal? Erhalten { get; set; }
		//public bool? erledigt_pos { get; set; }
		//public decimal? Gesamtpreis { get; set; }
		//public bool? In_Bearbeitung { get; set; }
		//public string InfoRahmennummer { get; set; }
		//public bool? Kanban { get; set; }
		public int? Lagerort_id { get; set; }
		//public DateTime? Liefertermin { get; set; }
		//public bool? Löschen { get; set; }
		//public DateTime? MhdDatumArtikel { get; set; }
		public int Nr { get; set; }
		//public int? Position { get; set; }
		//public bool? Position_erledigt { get; set; }
		public decimal? Preiseinheit { get; set; }
		//public int? Preisgruppe { get; set; }
		//public int? Produktionsort { get; set; }
		//public double? Rabatt { get; set; }
		//public double? Rabatt1 { get; set; }
		//public decimal? RB_Abgerufen { get; set; }
		//public decimal? RB_Offen { get; set; }
		//public decimal? RB_OriginalAnzahl { get; set; }
		//public string schriftart { get; set; }
		//public string sortierung { get; set; }
		//public decimal? Start_Anzahl { get; set; }
		//public double? Umsatzsteuer { get; set; }
		//public int? WE_Pos_zu_Bestellposition { get; set; }

		public decimal? ReceivingQuantiy { get; set; }
		public UpdateModel()
		{

		}
		public UpdateModel(Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity entity)
		{
			if(entity == null)
				return;

			// -
			Aktuelle_Anzahl = entity.Aktuelle_Anzahl;
			Anzahl = entity.Anzahl;
			Einzelpreis = entity.Einzelpreis;
			Lagerort_id = entity.Lagerort_id;
			Nr = entity.Nr;
			Preiseinheit = entity.Preiseinheit;
			ReceivingQuantiy = entity.Anzahl; // -
		}
	}
}
