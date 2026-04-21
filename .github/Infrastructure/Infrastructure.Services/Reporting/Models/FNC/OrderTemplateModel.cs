using System.ComponentModel;

namespace Infrastructure.Services.Reporting.Models.FNC
{
	public enum ReportLanguage
	{
		[Description("English")]
		English = 0,
		[Description("Deutsch")]
		German = 1,
		[Description("Français")]
		French = 2,
		[Description("Albanian")]
		Albanian = 3,
		[Description("Czech")]
		Czech = 4
	}
	public class OrderTemplateModel
	{
		public int Id { get; set; }
		public string Header1 { get; set; }
		public string Header2 { get; set; }
		public string Header3 { get; set; }
		public string SupplierLegalName { get; set; }
		public string SupplierName { get; set; }
		public string SupplierType { get; set; }
		public string ContactName { get; set; }
		public string ContactTelephone { get; set; }
		public string ContactFax { get; set; }
		public string ContactEmail { get; set; }
		public string SupplierStreet { get; set; }
		public string SupplierCountry { get; set; }
		public string SupplierTelephone { get; set; }
		public string SupplierFax { get; set; }

		public string CustomerNumber { get; set; }
		public string SupplierNumber { get; set; }
		public string OrderDate { get; set; } // 16
		public string TradingTerm { get; set; } // 17
		public string Payment { get; set; }
		public string PaymentTerm { get; set; }

		public string OrderDescription { get; set; } // 20 ------

		public string ShippingAddress { get; set; }
		public string ShippingName { get; set; }
		public string ShippingStreet { get; set; }
		public string ShippingStorageLocation { get; set; }
		public string BillingAddress { get; set; }

		// -
		public string ArticleNumber { get; set; }
		public string ArticleRef { get; set; }
		public string ArticleDeliveryDate { get; set; }
		public string ArticleDesignation { get; set; }
		public string ArticleQuqntity { get; set; }
		public string ArticleUnitPrice { get; set; }
		public string ArticleUnitOfMeasure { get; set; }
		public string ArticleUOMPrice { get; set; }
		public string ArticleTotal { get; set; }
		public string VAT { get; set; }
		public string TotalVAT { get; set; }
		public string ArticleVATTotal { get; set; }


		public string ArticleTotalExTax { get; set; }
		public string ArticleTotalTax { get; set; }
		public string ArticleTotalInTax { get; set; }
		public string ArticleDiscount { get; set; }
		public string ArticleSubtotal { get; set; }

		//-
		public string Footer1 { get; set; }
		public string Footer2 { get; set; }
		public string Footer3 { get; set; }
		public string Footer4 { get; set; }
		public string Footer5 { get; set; }
		public string Footer6 { get; set; }
		public string Footer7 { get; set; } // - link
											// - 
		public string PoContact { get; set; }
		public string PoTelephone { get; set; }
		public string PoFax { get; set; }
		public string PoEmail { get; set; }
		public string StorageLocation { get; set; }
		public string Items { get; set; }


		public OrderTemplateModel(ReportLanguage language = ReportLanguage.English)
		{
			switch(language)
			{
				case ReportLanguage.English:
					{
						Header1 = "Purchase Order";
						Header2 = "PO #:";
						Header3 = "Date:";
						SupplierLegalName = "";
						SupplierName = "";
						SupplierType = "";
						SupplierStreet = "";
						SupplierCountry = "";
						SupplierTelephone = "Tel.:";
						SupplierFax = "Fax:";

						ContactName = "Contact";
						ContactTelephone = "Telephone";
						ContactFax = "Fax";
						ContactEmail = "Email";

						CustomerNumber = "Customer Nr:";
						SupplierNumber = "Supplier Nr:";
						OrderDate = "Date"; // 16
						TradingTerm = "Trading Term:"; // 17
						Payment = "Payment:";
						PaymentTerm = "Payment Term:";

						PoContact = "Contact:";
						PoTelephone = "Tel.:";
						PoFax = "Fax:";
						PoEmail = "Email:";

						OrderDescription = ""; // 20 ------

						ShippingAddress = "Delivery Address";
						ShippingName = "";
						ShippingStreet = "";
						ShippingStorageLocation = "";
						BillingAddress = "Billing Address";
						StorageLocation = "Storage L.:";

						// -
						Items = "Items";
						ArticleNumber = "Article Nr";
						ArticleRef = "Article Ref.";
						ArticleDeliveryDate = "Delivery date";
						ArticleDesignation = "Designation";
						ArticleQuqntity = "Quantity";
						ArticleUnitPrice = "Unit Price";
						ArticleUnitOfMeasure = "UoM";
						ArticleUOMPrice = "UoM Price";
						VAT = "VAT";
						TotalVAT = "Total VAT";
						ArticleVATTotal = "Total";

						ArticleTotal = "Total";


						ArticleTotalExTax = "Total Net:";
						ArticleTotalTax = "Tax:";
						ArticleTotalInTax = "Total Amount:";
						ArticleDiscount = "Discount:";
						ArticleSubtotal = "Subtotal:";

						//-
						Footer1 = "- Dieses Dokument wurde elektronisch erstellt und ist daher auch ohne Unterschrift gültig";
						Footer2 = "- This document was issued electronically and is therefore valid without signature";
						Footer3 = "- Für unsere Bestellungen gelten ausschließlich unsere allgemeinen Einkaufsbedingungen";
						Footer4 = "- Einsicht möglich  unter www.psz-electronic.com";

						Footer5 = "- Wir sind gemäß Art.13 DSGVO verpflichtet, Sie als Geschäftskunde über unsere Datenschutzmaßnahmen zu informieren.";
						Footer6 = "- Unsere Datenschutzerklärung finden Sie auf unserer Website unter folgenden Link:";

						Footer7 = " https://www.psz-electronic.com/wp-content/uploads/2019/02/19-02-04-PSZ_Informationspflicht-Datenschutz-Art.13_Gesch%C3%A4ftspartnerBewerber.pdf"; // - link
					}
					break;
				case ReportLanguage.German:
				case ReportLanguage.Czech:
				case ReportLanguage.Albanian:
				case ReportLanguage.French:
				default:
					{
						Header1 = "Commande d'achat";
						Header2 = "N° :";
						Header3 = "Date :";
						SupplierLegalName = "";
						SupplierName = "";
						SupplierType = "";
						SupplierStreet = "";
						SupplierCountry = "";
						SupplierTelephone = "Tél. :";
						SupplierFax = "Fax :";

						ContactName = "Contact";
						ContactTelephone = "Téléphone";
						ContactFax = "Fax";
						ContactEmail = "E-mail";

						CustomerNumber = "N° client :";
						SupplierNumber = "N° fournisseur :";
						OrderDate = "Date :";
						TradingTerm = "Condition commerciale :";
						Payment = "Paiement :";
						PaymentTerm = "Condition de paiement :";

						PoContact = "Contact :";
						PoTelephone = "Tél. :";
						PoFax = "Fax :";
						PoEmail = "E-mail :";

						OrderDescription = "";
						ShippingAddress = "Adresse de livraison";
						ShippingName = "";
						ShippingStreet = "";
						ShippingStorageLocation = "";
						BillingAddress = "Adresse de facturation";
						StorageLocation = "Emplacement :";

						Items = "Articles";
						ArticleNumber = "N° d'article";
						ArticleRef = "Réf. article";
						ArticleDeliveryDate = "Date livraison";
						ArticleDesignation = "Désignation";
						ArticleQuqntity = "Quantité";
						ArticleUnitPrice = "Prix unitaire";
						ArticleUnitOfMeasure = "Unité";
						ArticleUOMPrice = "Prix/UdM";
						VAT = "TVA";
						TotalVAT = "Total TVA";
						ArticleVATTotal = "Total";
						ArticleTotal = "Total";

						ArticleTotalExTax = "Total net :";
						ArticleTotalTax = "Taxe :";
						ArticleTotalInTax = "Montant total :";
						ArticleDiscount = "Remise :";
						ArticleSubtotal = "Sous-total :";

						Footer1 = "- Ce document a été créé électroniquement et est donc valide sans signature";
						Footer2 = "- Ce document a été émis électroniquement et est donc valable sans signature";
						Footer3 = "- Seules nos conditions générales d'achat s'appliquent à nos commandes";
						Footer4 = "- Consultable sur www.psz-electronic.com";
						Footer5 = "- Conformément à l'article 13 du RGPD, nous sommes tenus d'informer nos clients professionnels de nos mesures de protection des données.";
						Footer6 = "- Vous trouverez notre politique de confidentialité sur notre site Internet à l'adresse suivante :";

						Footer7 = " https://www.psz-electronic.com/wp-content/uploads/2019/02/19-02-04-PSZ_Informationspflicht-Datenschutz-Art.13_Gesch%C3%A4ftspartnerBewerber.pdf"; // - link
					}
					break;
			}
		}
	}
}
