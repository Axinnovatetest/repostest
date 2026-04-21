using System;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleMinimalModel
	{
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }

		public string Abladestelle { get; set; }
		public Nullable<bool> aktiv { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public string Artikelfamilie_Kunde_Detail1 { get; set; }
		public string Artikelfamilie_Kunde_Detail2 { get; set; }
		public string Artikelkurztext { get; set; }
		public Nullable<bool> Barverkauf { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Bezeichnung2 { get; set; }
		public string Bezeichnung3 { get; set; }
		public string BezeichnungAL { get; set; }
		public string Zeichnungsnummer { get; set; }
		public string Freigabestatus { get; set; }
		public string Warengruppe { get; set; }
		public string Index_Kunde { get; set; }
		public DateTime? Index_Kunde_Datum { get; set; }


		// - 2022-08-13 - New Nomm.
		public int CustomerNumber { get; set; }
		public string CustomerPrefix { get; set; }
		public string CustomerItemNumber { get; set; }
		public int? CustomerItemNumberSequence { get; set; }
		public string CustomerItemIndex { get; set; }
		public int? CustomerItemIndexSequence { get; set; }
		public string ProductionCountryCode { get; set; }
		public string ProductionCountryName { get; set; }
		public string ProductionSiteCode { get; set; }
		public string ProductionSiteName { get; set; }
		public string ArticleNumber { get; set; }
		// - 2022-09-05
		public bool UBG { get; set; }

		// - 2022-09-29 - CTS infos
		public decimal FaQuantity { get; set; }
		public decimal AbQuantity { get; set; }
		public decimal HbgFaQuantity { get; set; }
		// - 2023-01-20
		public bool EdiDefault { get; set; }
		// - 2023-02-13 - totalStock
		public decimal TotalStock { get; set; } = 0;
		public bool IsEDrawing { get; set; }
		public int CountRows { get; set; } = 0;
		public string OrderNumber { get; set; }

		public decimal ArticleCurrentQuantity { get; set; }
		public ArticleMinimalModel() { }
		public ArticleMinimalModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, string orderNumber = "")
		{
			if(artikelEntity == null)
				return;

			this.ArtikelNr = artikelEntity.ArtikelNr;
			this.ArtikelNummer = artikelEntity.ArtikelNummer;

			this.Abladestelle = artikelEntity.Abladestelle;
			this.aktiv = artikelEntity.aktiv;
			this.Artikelfamilie_Kunde = artikelEntity.Artikelfamilie_Kunde;
			this.Artikelfamilie_Kunde_Detail1 = artikelEntity.Artikelfamilie_Kunde_Detail1;
			this.Artikelfamilie_Kunde_Detail2 = artikelEntity.Artikelfamilie_Kunde_Detail2;
			this.Artikelkurztext = artikelEntity.Artikelkurztext;
			this.Barverkauf = artikelEntity.Barverkauf;
			this.Bezeichnung1 = artikelEntity.Bezeichnung1;
			this.Bezeichnung2 = artikelEntity.Bezeichnung2;
			this.Bezeichnung3 = artikelEntity.Bezeichnung3;
			this.BezeichnungAL = artikelEntity.BezeichnungAL;
			this.Zeichnungsnummer = artikelEntity.Zeichnungsnummer;
			this.Freigabestatus = artikelEntity.Freigabestatus;
			this.Index_Kunde = artikelEntity.Index_Kunde;
			this.Index_Kunde_Datum = artikelEntity.Index_Kunde_Datum;
			this.Warengruppe = artikelEntity.Warengruppe;

			// - 2022-08-13 - New Nomm.
			CustomerNumber = artikelEntity.CustomerNumber ?? -1;
			CustomerPrefix = artikelEntity.CustomerPrefix;
			CustomerItemNumber = artikelEntity.CustomerItemNumber;
			CustomerItemNumberSequence = artikelEntity.CustomerItemNumberSequence;
			CustomerItemIndex = artikelEntity.CustomerIndex;
			CustomerItemIndexSequence = artikelEntity.CustomerIndexSequence;
			ProductionCountryCode = artikelEntity.ProductionCountryCode;
			ProductionCountryName = artikelEntity.ProductionCountryName;
			ProductionSiteCode = artikelEntity.ProductionSiteCode;
			ProductionSiteName = artikelEntity.ProductionSiteName;
			ArticleNumber = artikelEntity.ArticleNumber;

			// - 2022-09-05
			UBG = artikelEntity.UBG;
			// - 2023-01-20
			EdiDefault = artikelEntity.EdiDefault ?? false;
			IsEDrawing = artikelEntity.IsEDrawing ?? false;
			CountRows = artikelEntity.CountRows;
			OrderNumber = orderNumber;
		}
	}
}
