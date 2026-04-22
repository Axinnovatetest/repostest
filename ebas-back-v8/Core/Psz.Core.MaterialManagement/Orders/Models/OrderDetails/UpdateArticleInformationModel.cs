namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class UpdateArticleInformationRequestModel
	{
		public int Bestellung_Nr { get; set; } // [bestellte Artikel].[Bestellung-Nr] = Formulare!Bestellwesen!Nr // Current Bestellung/Order
		public string Bestell_Nr { get; set; } //  [bestellte Artikel].Bestellnummer = Bestellnummern![Bestell-Nr],
		public string ArticleDiscription1 { get; set; } // [bestellte Artikel].[Bezeichnung 1] = Bestellnummern!Artikelbezeichnung, 
		public string ArticleDiscription2 { get; set; } //  [bestellte Artikel].[Bezeichnung 2] = Bestellnummern!Artikelbezeichnung2,
		public string Unit { get; set; } //  [bestellte Artikel].Einheit = Artikel!Einheit,
		public double VAT { get; set; } // [bestellte Artikel].Umsatzsteuer = Bestellnummern!Umsatzsteuer
		public decimal? Price { get; set; } //[bestellte Artikel].Einzelpreis = Bestellnummern!Einkaufspreis
		public double Discount { get; set; } // [bestellte Artikel].Rabatt = Bestellnummern!Rabatt
		public decimal UnitPrice { get; set; } // [bestellte Artikel].Preiseinheit = Bestellnummern!Preiseinheit, 
		public DateTime ReplinshementDate { get; set; } // [bestellte Artikel].Liefertermin = Date()+Bestellnummern!Wiederbeschaffungszeitraum, 
		public decimal Quantity { get; set; } //bestellte Artikel].Anzahl = Bestellnummern!Mindestbestellmenge, 
		public decimal TotalPrice { get; set; } //[bestellte Artikel].Gesamtpreis = Bestellnummern!Mindestbestellmenge*Bestellnummern!Einkaufspreis/Bestellnummern!Preiseinheit*(1-Bestellnummern!Rabatt)
		public int Artikel_Nr { get; set; } // Selected Article // Current Article
		public int Lagerort_id { get; set; } // Selected Depot
		public string AB_Nr_Lieferant { get; set; } // ---
		public bool Kanban { get; set; } // [bestellte Artikel].Kanban = Artikel!Kanban
		public int? InfoRahmennummer { get; set; } // 
		public decimal CUPreis { get; set; }
		public DateTime Bestatigter_Termin { get; set; }
		public Boolean IsEdit { get; set; }
		public string Bemerkung { get; set; }
		public string ABNr { get; set; }
		public int Nr { get; set; }
		public int Position { get; set; }
		public UpdateArticleInformationRequestModel()
		{
		}


		public Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity GetEntity(Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity bestellte_ArtikelEntity = null)
		{
			if(bestellte_ArtikelEntity == null)
			{
				bestellte_ArtikelEntity = new();
				// Add new Position start anzahl should have the quantity.
				bestellte_ArtikelEntity.Start_Anzahl = Quantity;

			}
			var entity = bestellte_ArtikelEntity.ShallowClone();

			entity.Lagerort_id = Lagerort_id;
			entity.Kanban = Kanban;
			entity.CUPreis = CUPreis;
			entity.Artikel_Nr = Artikel_Nr;
			entity.Bestellung_Nr = Bestellung_Nr;
			entity.Bezeichnung_1 = ArticleDiscription1;
			entity.Bezeichnung_2 = ArticleDiscription2;
			entity.Einheit = Unit;
			entity.Umsatzsteuer = (float)VAT;
			entity.Einzelpreis = Price;
			entity.Bestellnummer = Bestell_Nr;
			entity.Rabatt = (float)Discount;
			entity.Preiseinheit = UnitPrice;
			entity.Liefertermin = ReplinshementDate.Date;
			entity.Anzahl = Quantity;
			entity.Gesamtpreis = TotalPrice;
			entity.RA_Pos_zu_Bestellposition = InfoRahmennummer;
			entity.Bestatigter_Termin = Bestatigter_Termin.Date;
			// Edit
			entity.Start_Anzahl = bestellte_ArtikelEntity.Start_Anzahl.HasValue
				? bestellte_ArtikelEntity.Start_Anzahl.Value != Quantity
				? Quantity : bestellte_ArtikelEntity.Start_Anzahl : 0;
			entity.Position = bestellte_ArtikelEntity.Position.HasValue ? bestellte_ArtikelEntity.Position : 0;
			entity.AnfangLagerBestand = bestellte_ArtikelEntity.AnfangLagerBestand.HasValue ? bestellte_ArtikelEntity.AnfangLagerBestand : 0;
			entity.Erhalten = bestellte_ArtikelEntity.Erhalten.HasValue ? bestellte_ArtikelEntity.Erhalten : 0;
			entity.Aktuelle_Anzahl = bestellte_ArtikelEntity.Aktuelle_Anzahl.HasValue ? bestellte_ArtikelEntity.Aktuelle_Anzahl : 0;
			entity.EndeLagerBestand = bestellte_ArtikelEntity.EndeLagerBestand.HasValue ? bestellte_ArtikelEntity.EndeLagerBestand : 0;
			entity.Rabatt1 = bestellte_ArtikelEntity.Rabatt1 ?? 0;
			entity.Rabatt1 = bestellte_ArtikelEntity.Rabatt1 ?? 0;
			entity.Produktionsort = bestellte_ArtikelEntity.Produktionsort ?? 0;
			entity.BP_zu_RBposition = bestellte_ArtikelEntity.BP_zu_RBposition ?? 0;
			entity.WE_Pos_zu_Bestellposition = bestellte_ArtikelEntity.WE_Pos_zu_Bestellposition ?? 0;
			entity.RB_OriginalAnzahl = bestellte_ArtikelEntity.RB_OriginalAnzahl ?? 1;
			entity.RB_Abgerufen = bestellte_ArtikelEntity.RB_Abgerufen ?? 1;
			entity.RB_Offen = bestellte_ArtikelEntity.RB_Offen ?? 1;
			entity.erledigt_pos = bestellte_ArtikelEntity.erledigt_pos ?? false;
			entity.Position_erledigt = bestellte_ArtikelEntity.Position_erledigt ?? false;
			entity.Bemerkung_Pos_ID = bestellte_ArtikelEntity.Bemerkung_Pos_ID ?? false;
			entity.In_Bearbeitung = bestellte_ArtikelEntity.In_Bearbeitung ?? false;
			entity.Loschen = bestellte_ArtikelEntity.Loschen ?? false;

			entity.Bemerkung_Pos = Bemerkung;
			entity.AB_Nr_Lieferant = ABNr;
			entity.Nr = Nr;
			entity.Position = Position;

			return entity;
		}
	}
	public class UpdateArticleInformationResponseModel
	{

	}
	public class UpdateArticleConfirmationDateRequestModel
	{
		public int OrderId { get; set; }
		public int PositionId { get; set; }
		public DateTime ConfirmationDate { get; set; }
	}
}
