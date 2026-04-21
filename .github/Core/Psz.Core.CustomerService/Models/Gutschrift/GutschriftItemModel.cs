using System;

namespace Psz.Core.CustomerService.Models.Gutshrift
{
	public class GutschriftItemModel
	{
		public int Nr { get; set; }
		public int AngebotNr { get; set; }
		public int? Position { get; set; }
		public int? ItemNumber { get; set; }
		public string ItemNummer { get; set; }
		public string KundenIndex { get; set; }
		public string Designation { get; set; }
		public string Designation2 { get; set; }
		public string Designation3 { get; set; }
		public decimal? OriginalOrderQuantity { get; set; }
		public DateTime? DesiredDate { get; set; }
		public decimal? DelivredQuantity { get; set; }
		public decimal? OpenQuantity { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public bool? Done { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? UnitPriceBasis { get; set; }
		public decimal? OpenQuantity_UnitPrice { get; set; }
		public decimal? OpenQuantity_TotalPrice { get; set; }
		public decimal? VAT { get; set; }
		public decimal? Discount { get; set; }
		public string InternComment { get; set; }
		public string Comment { get; set; }
		public bool FixedPrice { get; set; } = false;
		public decimal NewUnitPrice { get; set; } = 0;
		public decimal? UnitPrice { get; set; }
		public decimal? TotalPrice { get; set; }
		// - 2022-10-14 - Mario
		public bool WithoutCopper { get; set; } = false;
		public bool WithoutVAT { get; set; } = false;
		public string POSTEXT { get; set; }
		public string MeasureUnitQualifier { get; set; }
		public string Index_Kunde { get; set; }
		public string UnloadingPoint { get; set; }
		public decimal CopperWeight { get; set; }
		public decimal CopperSurcharge { get; set; }
		public decimal OpenQuantity_CopperWeight { get; set; }
		public decimal OpenQuantity_CopperSurcharge { get; set; }
		public int DelNote { get; set; }
		public bool DelFixed { get; set; }
		public int CopperBase { get; set; }

		public GutschriftItemModel()
		{

		}

		public GutschriftItemModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity entity)
		{
			if(entity == null)
				return;

			var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)entity.ArtikelNr);
			Nr = entity.Nr;
			Position = entity.Position;
			ItemNumber = entity.ArtikelNr;
			ItemNummer = article?.ArtikelNummer;
			KundenIndex = entity.Index_Kunde;
			Designation = entity.Bezeichnung1;
			OriginalOrderQuantity = entity.OriginalAnzahl;
			DesiredDate = entity.Wunschtermin;
			DelivredQuantity = entity.Geliefert;
			OpenQuantity = entity.Anzahl;
			DeliveryDate = entity.Liefertermin;
			Done = entity.erledigt_pos;
			UnitPriceBasis = entity.Preiseinheit;
			OpenQuantity_UnitPrice = entity.Einzelpreis;
			OpenQuantity_TotalPrice = entity.Gesamtpreis;
			VAT = Math.Round(Convert.ToDecimal(entity.USt ?? 0) * 100, 2);
			Discount = Math.Round(Convert.ToDecimal(entity.Rabatt ?? 0) * 100, 2);
			FixedPrice = entity.EKPreise_Fix ?? false;
			UnitPrice = entity.VKEinzelpreis;
			TotalPrice = entity.VKGesamtpreis;
			WithoutCopper = entity.GSWithoutCopper ?? false;
		}
		public GutschriftItemModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angeboteneArtikelEntity,
			string articleNumber, decimal availableQuantity)
		{
			if(angeboteneArtikelEntity == null)
			{
				return;
			}

			Nr = angeboteneArtikelEntity.Nr;
			AngebotNr = angeboteneArtikelEntity.AngebotNr ?? -1;
			Position = angeboteneArtikelEntity.Position;
			ItemNumber = angeboteneArtikelEntity.ArtikelNr;
			ItemNummer = articleNumber;
			KundenIndex = angeboteneArtikelEntity.Index_Kunde;
			Designation = angeboteneArtikelEntity.Bezeichnung1;
			Designation2 = angeboteneArtikelEntity.Bezeichnung2;
			Designation3 = angeboteneArtikelEntity.Bezeichnung3;
			OriginalOrderQuantity = angeboteneArtikelEntity.OriginalAnzahl;
			DesiredDate = angeboteneArtikelEntity.Wunschtermin;
			DelivredQuantity = angeboteneArtikelEntity.Geliefert;
			OpenQuantity = availableQuantity;
			DeliveryDate = angeboteneArtikelEntity.Liefertermin;
			Done = angeboteneArtikelEntity.erledigt_pos;
			UnitPriceBasis = angeboteneArtikelEntity.Preiseinheit;
			OpenQuantity_UnitPrice = angeboteneArtikelEntity.Einzelpreis;
			OpenQuantity_TotalPrice = angeboteneArtikelEntity.Gesamtpreis;
			VAT = angeboteneArtikelEntity.USt;
			Discount = angeboteneArtikelEntity.Rabatt;
			FixedPrice = angeboteneArtikelEntity.EKPreise_Fix ?? false;
			UnitPrice = angeboteneArtikelEntity.VKEinzelpreis;
			TotalPrice = angeboteneArtikelEntity.VKGesamtpreis;
			WithoutCopper = angeboteneArtikelEntity.GSWithoutCopper ?? false;
			Comment = angeboteneArtikelEntity.GSExternComment;
			InternComment = angeboteneArtikelEntity.GSInternComment;
			// -
			POSTEXT = angeboteneArtikelEntity.POSTEXT;
			MeasureUnitQualifier = angeboteneArtikelEntity.Einheit;
			Index_Kunde = angeboteneArtikelEntity.Index_Kunde;
			UnloadingPoint = angeboteneArtikelEntity.Abladestelle;
			CopperWeight = angeboteneArtikelEntity.EinzelCuGewicht ?? 0;
			CopperSurcharge = angeboteneArtikelEntity.Einzelkupferzuschlag ?? 0;
			OpenQuantity_CopperWeight = angeboteneArtikelEntity.GesamtCuGewicht ?? 0;
			OpenQuantity_CopperSurcharge = angeboteneArtikelEntity.Gesamtkupferzuschlag ?? 0;
			DelNote = angeboteneArtikelEntity.DEL ?? 0;
			CopperBase = angeboteneArtikelEntity.Kupferbasis ?? 0;
			DelFixed = angeboteneArtikelEntity.DELFixiert ?? false;
		}
	}
}
