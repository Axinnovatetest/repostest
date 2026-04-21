using System;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArticleDataModel
	{
		public int ArtikelNr { get; set; }//
		public string ArtikelNummer { get; set; }//
		public string Bezeichnung1 { get; set; }//
		public string Bezeichnung2 { get; set; }//
		public string Bezeichnung3 { get; set; }//
		public Nullable<decimal> CuGewicht { get; set; }//
		public Nullable<int> DEL { get; set; }//
		public string Index_Kunde { get; set; }//
		public Nullable<DateTime> Index_Kunde_Datum { get; set; }//
		public string Klassifizierung { get; set; }//
		public int? ID_Klassifizierung { get; set; }
		public Nullable<int> Kupferbasis { get; set; }//
		public Nullable<bool> ULEtikett { get; set; }//
		public Nullable<bool> ULzertifiziert { get; set; }//
		public Nullable<bool> VKFestpreis { get; set; }//
		public string Warengruppe { get; set; }//

		// >>>>> Data Extension
		public int DataExtensionId { get; set; }
		public int? ProjectTypeId { get; set; }//
		public string OrderNumber { get; set; }//
		public string CustomerInquiryNumber { get; set; }//
		public decimal? QuotationsBased12Months { get; set; }//
		public DateTime? SOPAppointmentCustomer { get; set; }//
		public string Consumption12Months { get; set; }//
		public string Sales12MonthsPerItem { get; set; }//
		public string CreatorOrder { get; set; }//
		public DateTime? OrderValidity { get; set; }//
		public int? CustomerContactPersonId { get; set; }//
		public decimal? CopperCostBasis { get; set; }//
		public decimal? CopperCostBasis150 { get; set; }//

		public string Index_Kunde_ChangeReason { get; set; } // - to save in article history
		public bool? ROHS_EEE_Confirmity { get; set; }

		// - Stage v2
		public bool? DELFixiert { get; set; }
		public bool? Lager { get; set; }
		public bool? Stuckliste { get; set; }
		public decimal? Zuschlag_VK { get; set; }
		public string Einheit { get; set; }
		public decimal? Gewicht { get; set; } // MGK
		public string Zeichnungsnummer { get; set; }
		public decimal? Grosse { get; set; } // - Ber. Gewicht
		public int? Warentyp { get; set; }
		public decimal? Stundensatz { get; set; }
		public int? Verpackungsmenge { get; set; }
		public bool? VDA_E { get; set; }
		public bool? VDA_P { get; set; }
		public string Praeferenz_Aktuelles_jahr { get; set; }
		public string Praeferenz_Folgejahr { get; set; }
		public string EAN { get; set; }
		public decimal? Kupferzahl { get; set; } // Cu-Zahl
		public decimal? Cu_Preis { get; set; }
		public string artikelklassifizierung { get; set; }
		public string CustomerName { get; set; }

		public decimal? Umsatzsteuer { get; set; }
		public decimal? Preiseinheit { get; set; }
		public string Werkzeug { get; set; }

		public string Verpackung { get; set; }
		public string Langtext { get; set; }
		public string Artikelfamilie_Kunde { get; set; }
		public string Lieferzeit { get; set; }
		public bool aktiv { get; set; }
		// - 2022-08-11 - new nomm.
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
		public bool UBG { get; set; }
		// - 2023-01-20
		public bool EdiDefault { get; set; }
		// - 2023-02-01 -
		public string Artikelfamilie_Kunde_Detail1 { get; set; }
		public string Artikelfamilie_Kunde_Detail2 { get; set; }


		public ArticleDataModel() { }
		public ArticleDataModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity artikelExtensionEntity)
		{
			if(artikelEntity == null)
				return;
			ArtikelNr = artikelEntity.ArtikelNr;
			ArtikelNummer = artikelEntity.ArtikelNummer;
			Bezeichnung1 = artikelEntity.Bezeichnung1;
			Bezeichnung2 = artikelEntity.Bezeichnung2;
			Bezeichnung3 = artikelEntity.Bezeichnung3;
			CuGewicht = artikelEntity.CuGewicht;
			DEL = artikelEntity.DEL;//
			Index_Kunde = artikelEntity.Index_Kunde;//
			Index_Kunde_Datum = artikelEntity.Index_Kunde_Datum;//
			Klassifizierung = artikelEntity.Klassifizierung;//
			ID_Klassifizierung = artikelEntity.ID_Klassifizierung;
			Kupferbasis = artikelEntity.Kupferbasis;//
			ULEtikett = artikelEntity.ULEtikett;//
			ULzertifiziert = artikelEntity.ULzertifiziert;//
			ROHS_EEE_Confirmity = artikelEntity.ROHSEEEConfirmity;//
			VKFestpreis = artikelEntity.VKFestpreis;//
			Warengruppe = artikelEntity.Warengruppe;//
													// - Stage v2
			DELFixiert = artikelEntity.DELFixiert;
			Lager = artikelEntity.Lagerartikel;
			Stuckliste = artikelEntity.Stuckliste;
			Zuschlag_VK = artikelEntity.Zuschlag_VK;
			Einheit = artikelEntity.Einheit;
			Gewicht = artikelEntity.Gewicht; // MGK
			Zeichnungsnummer = artikelEntity.Zeichnungsnummer;
			Grosse = artikelEntity.Größe; // - Ber. Gewicht
			Warentyp = artikelEntity.Warentyp;
			Stundensatz = artikelEntity.Stundensatz;
			Verpackungsmenge = artikelEntity.Verpackungsmenge;
			VDA_E = artikelEntity.VDA_1;
			VDA_P = artikelEntity.VDA_2;
			Praeferenz_Aktuelles_jahr = artikelEntity.Praeferenz_Aktuelles_jahr;
			Praeferenz_Folgejahr = artikelEntity.Praeferenz_Folgejahr;
			EAN = artikelEntity.EAN;
			Kupferzahl = artikelEntity.Kupferzahl; // Cu-Zahl
			artikelklassifizierung = artikelEntity.artikelklassifizierung;
			Cu_Preis = null; // Computations needed from Khelil

			Umsatzsteuer = (artikelEntity.Umsatzsteuer ?? 0);
			Preiseinheit = artikelEntity.Preiseinheit;
			Werkzeug = artikelEntity.Werkzeug;

			Langtext = artikelEntity.Langtext;
			Verpackung = artikelEntity.Verpackung;
			Artikelfamilie_Kunde = artikelEntity.Artikelfamilie_Kunde;
			Lieferzeit = artikelEntity.Lieferzeit;
			aktiv = artikelEntity.aktiv ?? false;

			// - 2022-08-11 - new nomm.
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

			EdiDefault = artikelEntity.EdiDefault ?? false;
			UBG = artikelEntity.UBG;

			Artikelfamilie_Kunde_Detail1 = artikelEntity.Artikelfamilie_Kunde_Detail1;
			Artikelfamilie_Kunde_Detail2 = artikelEntity.Artikelfamilie_Kunde_Detail2;

			// >>>>> Data Extension
			if(artikelExtensionEntity != null)
			{
				DataExtensionId = artikelExtensionEntity.Id;
				ProjectTypeId = artikelExtensionEntity.ProjectTypeId;//
				OrderNumber = artikelExtensionEntity.OrderNumber;//
				CustomerInquiryNumber = artikelExtensionEntity.CustomerInquiryNumber;//
				CustomerName = artikelExtensionEntity.CustomerName;//
				QuotationsBased12Months = artikelExtensionEntity.QuotationsBased12Months;//
				SOPAppointmentCustomer = artikelExtensionEntity.SOPAppointmentCustomer;//
				Consumption12Months = artikelExtensionEntity.Consumption12Months;//
				Sales12MonthsPerItem = artikelExtensionEntity.Sales12MonthsPerItem;//
				CreatorOrder = artikelExtensionEntity.CreatorOrder;//
				OrderValidity = artikelExtensionEntity.OrderValidity;//
				CustomerContactPersonId = artikelExtensionEntity.CustomerContactPersonId;//
				CopperCostBasis = artikelExtensionEntity.CopperCostBasis;//
				CopperCostBasis150 = artikelExtensionEntity.CopperCostBasis150;//
			}
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
			{
				ArtikelNr = this.ArtikelNr,
				ArtikelNummer = this.ArtikelNummer,
				Bezeichnung1 = this.Bezeichnung1,
				Bezeichnung2 = this.Bezeichnung2,
				Bezeichnung3 = this.Bezeichnung3,
				CuGewicht = this.CuGewicht,
				DEL = this.DEL,//
				Index_Kunde = this.Index_Kunde,//
				Index_Kunde_Datum = this.Index_Kunde_Datum,//
				Klassifizierung = this.Klassifizierung,//
				ID_Klassifizierung = this.ID_Klassifizierung,
				Kupferbasis = this.Kupferbasis,//
				ULEtikett = this.ULEtikett,//
				ROHSEEEConfirmity = this.ROHS_EEE_Confirmity,//
				ULzertifiziert = this.ULzertifiziert,//
				VKFestpreis = this.VKFestpreis,//
				Warengruppe = this.Warengruppe,//

				// - Stage v2
				DELFixiert = this.DELFixiert,
				Lagerartikel = this.Lager,
				Stuckliste = this.Stuckliste,
				Zuschlag_VK = this.Zuschlag_VK,
				Einheit = this.Einheit,
				Gewicht = this.Gewicht, // MGK
				Zeichnungsnummer = this.Zeichnungsnummer,
				Größe = this.Grosse, // - Ber. Gewicht
				Warentyp = this.Warentyp,
				Stundensatz = this.Stundensatz,
				Verpackungsmenge = this.Verpackungsmenge,
				VDA_1 = this.VDA_E,
				VDA_2 = this.VDA_P,
				Praeferenz_Aktuelles_jahr = this.Praeferenz_Aktuelles_jahr,
				Praeferenz_Folgejahr = this.Praeferenz_Folgejahr,
				EAN = this.EAN,
				Kupferzahl = this.Kupferzahl, // Cu-Zahl
				artikelklassifizierung = this.artikelklassifizierung,
				//Cu_Preis = null, // Computations needed from Khelil
				Umsatzsteuer = (Umsatzsteuer ?? 0),
				Preiseinheit = Preiseinheit,
				Werkzeug = Werkzeug,
				Verpackung = Verpackung,
				Langtext = Langtext,
				Artikelfamilie_Kunde = Artikelfamilie_Kunde,
				Lieferzeit = Lieferzeit,

				Artikelfamilie_Kunde_Detail1 = Artikelfamilie_Kunde_Detail1,
				Artikelfamilie_Kunde_Detail2 = Artikelfamilie_Kunde_Detail2
			};
		}
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity ToExtensionEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.ArtikelExtensionEntity
			{
				Id = this.DataExtensionId,
				ArtikelNr = this.ArtikelNr,
				ProjectTypeId = this.ProjectTypeId,//
				OrderNumber = this.OrderNumber,//
				CustomerInquiryNumber = this.CustomerInquiryNumber,//
				CustomerName = this.CustomerName,//
				QuotationsBased12Months = this.QuotationsBased12Months,//
				SOPAppointmentCustomer = this.SOPAppointmentCustomer,//
				Consumption12Months = this.Consumption12Months,//
				Sales12MonthsPerItem = this.Sales12MonthsPerItem,//
				CreatorOrder = this.CreatorOrder,//
				OrderValidity = this.OrderValidity,//
				CustomerContactPersonId = this.CustomerContactPersonId,//
				CopperCostBasis = this.CopperCostBasis,//
				CopperCostBasis150 = this.CopperCostBasis150,//

			};
		}
	}
}
