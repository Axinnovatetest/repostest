using Psz.Core.MaterialManagement.Helpers;

namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class QuickPORequestModel
	{
		public int ArticleNr { get; set; }
		public decimal Quantity { get; set; }
		public int LagerortId { get; set; }

		public Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity GetEntity(
			decimal cuPrice,
			int bestellungNr,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.MTM.BestellnummernEntity bestellnummernEntity,
			Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity bestellte_ArtikelEntity = null)
		{
			if(bestellte_ArtikelEntity == null)
				bestellte_ArtikelEntity = new();

			var minimumQuantity = bestellnummernEntity.Mindestbestellmenge.HasValue && (decimal)bestellnummernEntity.Mindestbestellmenge.Value > Quantity ? (decimal)bestellnummernEntity.Mindestbestellmenge.Value : Quantity;
			var lager = SpecialHelper.GetHauptLager(LagerortId);
			bestellte_ArtikelEntity.Bestellung_Nr = bestellungNr;
			bestellte_ArtikelEntity.Lagerort_id = lager;
			bestellte_ArtikelEntity.AB_Nr_Lieferant = "";
			bestellte_ArtikelEntity.Kanban = false;
			bestellte_ArtikelEntity.CUPreis = cuPrice;
			bestellte_ArtikelEntity.Artikel_Nr = artikelEntity.ArtikelNr;
			bestellte_ArtikelEntity.Bestellung_Nr = bestellungNr;
			bestellte_ArtikelEntity.Bezeichnung_1 = bestellnummernEntity.Artikelbezeichnung;
			bestellte_ArtikelEntity.Bezeichnung_2 = bestellnummernEntity.Artikelbezeichnung2;
			bestellte_ArtikelEntity.Einheit = artikelEntity.Einheit;
			bestellte_ArtikelEntity.Umsatzsteuer = bestellnummernEntity.Umsatzsteuer.HasValue ? (float)bestellnummernEntity.Umsatzsteuer.Value : 0;
			bestellte_ArtikelEntity.Einzelpreis = bestellnummernEntity.Einkaufspreis ?? 0;
			bestellte_ArtikelEntity.Bestellnummer = bestellnummernEntity.Bestell_Nr;
			bestellte_ArtikelEntity.Rabatt = (float)bestellnummernEntity.Rabatt;
			bestellte_ArtikelEntity.Preiseinheit = bestellnummernEntity.Preiseinheit ?? 1;
			bestellte_ArtikelEntity.Liefertermin = DateTime.Now.AddDays(bestellnummernEntity.Wiederbeschaffungszeitraum ?? 30).Date;
			bestellte_ArtikelEntity.Anzahl = minimumQuantity;
			bestellte_ArtikelEntity.Gesamtpreis = bestellte_ArtikelEntity.Einzelpreis * minimumQuantity;
			bestellte_ArtikelEntity.Bestatigter_Termin = new DateTime(2999, 12, 31).Date;
			bestellte_ArtikelEntity.Position = 10;
			bestellte_ArtikelEntity.InfoRahmennummer = null; // 2023-08-30 - !string.IsNullOrEmpty(artikelEntity.RahmenNr) ? artikelEntity.RahmenNr : artikelEntity.RahmenNr2;
			bestellte_ArtikelEntity.AnfangLagerBestand = 0;
			bestellte_ArtikelEntity.Start_Anzahl = minimumQuantity;
			bestellte_ArtikelEntity.Erhalten = 0;
			bestellte_ArtikelEntity.Aktuelle_Anzahl = 0;
			bestellte_ArtikelEntity.EndeLagerBestand = 0;
			bestellte_ArtikelEntity.Rabatt1 = 0;
			bestellte_ArtikelEntity.Rabatt1 = 0;
			bestellte_ArtikelEntity.Produktionsort = 0;
			bestellte_ArtikelEntity.BP_zu_RBposition = 0;
			bestellte_ArtikelEntity.WE_Pos_zu_Bestellposition = 0;
			bestellte_ArtikelEntity.RB_OriginalAnzahl = 1;
			bestellte_ArtikelEntity.RB_Abgerufen = 1;
			bestellte_ArtikelEntity.RB_Offen = 1;
			bestellte_ArtikelEntity.erledigt_pos = false;
			bestellte_ArtikelEntity.Position_erledigt = false;
			bestellte_ArtikelEntity.Bemerkung_Pos = "Quick PO";
			bestellte_ArtikelEntity.Bemerkung_Pos_ID = false;
			bestellte_ArtikelEntity.In_Bearbeitung = false;
			bestellte_ArtikelEntity.Loschen = false;

			return bestellte_ArtikelEntity;
		}
	}
	public class QuickPOResponseModel
	{
		public int Id { get; set; }
	}
}
