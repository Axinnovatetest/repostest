using System;

namespace Psz.Core.FinanceControl.Models.Budget.Reception.Article
{
	public class GetModel
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

		public decimal? ReceivingQuantiy { get; set; }

		// - 
		public string ArticleNumber { get; set; } // - Artikelnummer
												  // - 
		public DateTime? OrderDeliveryDate { get; set; } // - Liefertermin
		public string ProjectNumber { get; set; } // - Projekt-Nr
		public string OrderUser { get; set; } // - Benutzer
		public string Mandante { get; set; }
		public string IncomingDeliveryNoteNumber { get; set; } // - Eingangslieferscheinnr
		public string QrCode { get; set; }

		public GetModel(Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity bestellte_ArtikelEntity,
			Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity bestellungenEntity)
		{
			AB_Nr_Lieferant = bestellte_ArtikelEntity.AB_Nr_Lieferant;
			Aktuelle_Anzahl = bestellte_ArtikelEntity.Aktuelle_Anzahl;
			AnfangLagerBestand = bestellte_ArtikelEntity.AnfangLagerBestand;
			Anzahl = bestellte_ArtikelEntity.Anzahl;
			Artikel_Nr = bestellte_ArtikelEntity.Artikel_Nr;
			Bemerkung_Pos = bestellte_ArtikelEntity.Bemerkung_Pos;
			Bemerkung_Pos_ID = bestellte_ArtikelEntity.Bemerkung_Pos_ID;
			Bestatigter_Termin = bestellte_ArtikelEntity.Bestatigter_Termin;
			Bestellnummer = bestellte_ArtikelEntity.Bestellnummer;
			Bestellung_Nr = bestellte_ArtikelEntity.Bestellung_Nr;
			Bezeichnung_1 = bestellte_ArtikelEntity.Bezeichnung_1;
			Bezeichnung_2 = bestellte_ArtikelEntity.Bezeichnung_2;
			BP_zu_RBposition = bestellte_ArtikelEntity.BP_zu_RBposition;
			COC_bestatigung = bestellte_ArtikelEntity.COC_bestatigung;
			CUPreis = bestellte_ArtikelEntity.CUPreis;
			Einheit = bestellte_ArtikelEntity.Einheit;
			Einzelpreis = bestellte_ArtikelEntity.Einzelpreis;
			EMPB_Bestatigung = bestellte_ArtikelEntity.EMPB_Bestatigung;
			EndeLagerBestand = bestellte_ArtikelEntity.EndeLagerBestand;
			Erhalten = bestellte_ArtikelEntity.Erhalten;
			erledigt_pos = bestellte_ArtikelEntity.erledigt_pos;
			Gesamtpreis = bestellte_ArtikelEntity.Gesamtpreis;
			In_Bearbeitung = bestellte_ArtikelEntity.In_Bearbeitung;
			InfoRahmennummer = bestellte_ArtikelEntity.InfoRahmennummer;
			Kanban = bestellte_ArtikelEntity.Kanban;
			Lagerort_id = bestellte_ArtikelEntity.Lagerort_id;
			Liefertermin = bestellte_ArtikelEntity.Liefertermin;
			Loschen = bestellte_ArtikelEntity.Loschen;
			MhdDatumArtikel = bestellte_ArtikelEntity.MhdDatumArtikel;
			Nr = bestellte_ArtikelEntity.Nr;
			Position = bestellte_ArtikelEntity.Position;
			Position_erledigt = bestellte_ArtikelEntity.Position_erledigt;
			Preiseinheit = bestellte_ArtikelEntity.Preiseinheit;
			Preisgruppe = bestellte_ArtikelEntity.Preisgruppe;
			Produktionsort = bestellte_ArtikelEntity.Produktionsort;
			Rabatt = bestellte_ArtikelEntity.Rabatt;
			Rabatt1 = bestellte_ArtikelEntity.Rabatt1;
			RB_Abgerufen = bestellte_ArtikelEntity.RB_Abgerufen;
			RB_Offen = bestellte_ArtikelEntity.RB_Offen;
			RB_OriginalAnzahl = bestellte_ArtikelEntity.RB_OriginalAnzahl;
			schriftart = bestellte_ArtikelEntity.schriftart;
			sortierung = bestellte_ArtikelEntity.sortierung;
			Start_Anzahl = bestellte_ArtikelEntity.Start_Anzahl;
			Umsatzsteuer = bestellte_ArtikelEntity.Umsatzsteuer;
			WE_Pos_zu_Bestellposition = bestellte_ArtikelEntity.WE_Pos_zu_Bestellposition;

			ReceivingQuantiy = 0;

			ArticleNumber = artikelEntity?.Artikelnummer;
			OrderDeliveryDate = bestellungenEntity?.Liefertermin;
			ProjectNumber = bestellungenEntity?.Projekt_Nr;
			OrderUser = bestellungenEntity?.Benutzer;
			Mandante = bestellungenEntity?.Mandant;
			IncomingDeliveryNoteNumber = bestellungenEntity?.Eingangslieferscheinnr;

			QrCode = Infrastructure.Services.Utils.QRCodeUtils.GetQRCode($"{bestellte_ArtikelEntity.Nr}|{bestellte_ArtikelEntity.Position ?? -1}|{bestellte_ArtikelEntity.Bestellung_Nr ?? -1}|{bestellte_ArtikelEntity.Artikel_Nr ?? -1}|{bestellte_ArtikelEntity.Bezeichnung_1}|{bestellte_ArtikelEntity.Anzahl}|{bestellte_ArtikelEntity.Liefertermin ?? DateTime.MinValue}|{bestellte_ArtikelEntity.Lagerort_id ?? -1}|{bestellte_ArtikelEntity.Gesamtpreis ?? -1}|{bestellungenEntity?.Benutzer}"?.Trim());
		}
	}
}
