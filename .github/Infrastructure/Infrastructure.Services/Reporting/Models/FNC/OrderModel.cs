using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Services.Reporting.Models.FNC
{
	public class OrderModel
	{
		public int Id { get; set; }
		public byte[] ImportLogoImage { get; set; }
		public byte[] CompanyLogoImage { get; set; }
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
		public string SupplierSalutations { get; set; }

		public string CustomerNumber { get; set; }
		public string SupplierNumber { get; set; }
		public string OrderDate { get; set; } // 16
		public string TradingTerm { get; set; } // 17
		public string Payment { get; set; }
		public string PaymentTerm { get; set; }

		public string OrderDescription { get; set; } // 20 ------

		public string ShippingAddress { get; set; }
		public string ShippingCompanyName { get; set; }
		public string ShippingDepartmentName { get; set; }
		public string ShippingPostalCode { get; set; }
		public string ShippingCity { get; set; }
		public string ShippingCountry { get; set; }
		public string ShippingTelephone { get; set; }
		public string ShippingFax { get; set; }
		public string ShippingEmail { get; set; }
		public string ShippingStorageLocation { get; set; }

		// -
		public List<OrderItemModel> OrderItems { get; set; }

		public string Subtotal { get; set; }
		public string TotalDiscount { get; set; }
		public string TotalNet { get; set; }
		public string TotalTax { get; set; }
		public string Total { get; set; }

		public string ArticleTotalExTax { get; set; }
		public string ArticleTotalTax { get; set; }
		public string ArticleTotalInTax { get; set; }

		//-
		public string FormNumber { get; set; }
		public string Footer2 { get; set; }
		public string Footer3 { get; set; }
		public string Footer4 { get; set; }
		public string Footer5 { get; set; } // - link

		//- Company data
		public string CompanyName { get; set; }
		public string CompanyAddress { get; set; }
		public string CompanyPostalCode { get; set; }
		public string CompanyCity { get; set; }
		public string CompanyCountry { get; set; }
		public string CompanyTelephone { get; set; }
		public string CompanyFax { get; set; }
		public string CompanyEmail { get; set; }

		public string BillingContactName { get; set; }
		public string BillingAddress { get; set; }
		public string BillingCompanyName { get; set; }
		public string BillingDepartmentName { get; set; }
		public string BillingTelephone { get; set; }
		public string BillingFax { get; set; }


		public string DeliveryTelephone { get; set; }
		public string DeliveryFax { get; set; }

		public string IssuerName { get; set; }
		public string IssuerTelephone { get; set; }
		public string IssuerEmail { get; set; }
		public string IssuerFax { get; set; }


		// - Footer
		public string FooterAddress { get; set; }
		public string FooterCity { get; set; }
		public string FooterPostalCode { get; set; }
		public string FooterVAT_Title { get; set; }
		public string FooterVAT { get; set; }
		public string FooterFax { get; set; }
		public string FooterPhone { get; set; }
		public string FooterCity_Title { get; set; }
		public string FooterFax_Title { get; set; }
		public string FooterManager_Title { get; set; }
		public string FooterManager1 { get; set; }
		public string FooterManager2 { get; set; }
		public string FooterTaxNumber_Tilte { get; set; }
		public string FooterTaxNumber { get; set; }
		public string FooterHRB_Title { get; set; }
		public string FooterHRB { get; set; }
		public string FooterEmail { get; set; }
		public string FooterEmail_Title { get; set; }
		public string FooterCustomsNumber { get; set; }
		public string FooterCustomsNumber_Title { get; set; }

		public string FooterBankDetails_Title { get; set; }
		public string FooterBankDetails1 { get; set; }
		public string FooterBankDetails2 { get; set; }
		public string FooterBankDetails3 { get; set; }
		public string FooterAccount_Title { get; set; }
		public string FooterAccount1 { get; set; }
		public string FooterAccount2 { get; set; }
		public string FooterAccount3 { get; set; }
		public string FooterBLZ_Title { get; set; }
		public string FooterBLZ1 { get; set; }
		public string FooterBLZ2 { get; set; }
		public string FooterBLZ3 { get; set; }
		public string FooterIBAN_Title { get; set; }
		public string FooterIBAN1 { get; set; }
		public string FooterIBAN2 { get; set; }
		public string FooterIBAN3 { get; set; }
		public string FooterSWIFT_Title { get; set; }
		public string FooterSWIFT1 { get; set; }
		public string FooterSWIFT2 { get; set; }
		public string FooterSWIFT3 { get; set; }
		public string FooterSite { get; set; }
		public int BillingSite { get; set; } // - control field 1=PSZ Gmbh, 2=PSZ CZ, 3=PSZ TN, 4=PSZ AL, 5=WS

		public string PoPaymentType { get; set; }

		public OrderModel(
			Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
			Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity companyExtensionEntity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntityShipping,
			Infrastructure.Data.Entities.Tables.STG.Textbausteine_AB_LS_RG_GU_BEntity textbausteine_AB_LS_RG_GU_BEntity,
			Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity project_BudgetEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity bestellungEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> article_OrderEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> bestellteArticleEntities,
			Infrastructure.Data.Entities.Tables.FNC.LieferantenEntity lieferantenEntity,
			Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> artikelEntities,
			Infrastructure.Data.Entities.Tables.FNC.KonditionsZuordnungsTabelleEntity konditionsZuordnungsTabelleEntity
			)
		{
			// - escape delivery address
			companyEntityShipping = companyEntityShipping ?? companyEntity;

			// - 
			ImportLogoImage = null;
			CompanyLogoImage = companyEntity.Logo;

			Header1 = orderEntity?.OrderId.ToString("D5");
			Header2 = orderEntity?.OrderNumber;
			Header3 = "";
			SupplierLegalName = bestellungEntity?.Vorname_NameFirma;
			SupplierName = bestellungEntity?.Name2;
			SupplierType = bestellungEntity?.Anrede;
			ContactName = bestellungEntity?.Ansprechpartner;
			ContactTelephone = "";
			ContactFax = "";
			ContactEmail = "";
			var supllierAdd = bestellungEntity?.Straße_Postfach?.Split('|');
			var supplierCity = bestellungEntity?.Land_PLZ_Ort?.Split('|');
			SupplierStreet = (supllierAdd?[0]?.Trim().Trim(',').Trim('|') + ", " + (supllierAdd?.Count() > 1 ? supllierAdd[1] : "")?.Trim().Trim(',').Trim('|')).Trim().Trim(',');
			SupplierCountry = (supplierCity?[0]?.Trim().Trim(',').Trim('|') + ", " + (supplierCity?.Count() > 1 ? supplierCity[1] : "")?.Trim().Trim(',').Trim('|')).Trim().Trim(',');
			SupplierTelephone = "";
			SupplierFax = "";
			SupplierSalutations = bestellungEntity?.Briefanrede;

			CustomerNumber = "";
			SupplierNumber = orderEntity?.SupplierNumber?.ToString();
			OrderDate = orderEntity?.CreationDate?.ToString("dd.MM.yyyy"); // 16
			TradingTerm = orderEntity?.SupplierTradingTerm; // 17
			Payment = orderEntity?.SupplierPaymentMethod;
			PaymentTerm = orderEntity?.SupplierPaymentTerm; // lieferantenEntity?.Konditionszuordnungs_Nr;

			OrderDescription = ""; // - textbausteine_AB_LS_RG_GU_BEntity?.Bestellung; // 20 >>>>>>>>>>>

			ShippingAddress = $"{orderEntity.DeliveryAddress}, {companyEntityShipping?.City}, {companyEntityShipping?.PostalCode}, {companyEntityShipping?.Country}"?.Trim(',').Trim();
			ShippingCompanyName = !string.IsNullOrEmpty(orderEntity?.DeliveryDepartmentName)
				? ""
				: orderEntity?.DeliveryCompanyName;
			ShippingDepartmentName = orderEntity?.DeliveryDepartmentName;
			ShippingPostalCode = companyEntityShipping?.PostalCode;
			ShippingCity = companyEntityShipping?.City;
			ShippingCountry = companyEntityShipping?.Country;
			ShippingTelephone = !string.IsNullOrWhiteSpace(orderEntity?.DeliveryTelephone)
				? orderEntity?.DeliveryTelephone
				: $"{companyEntityShipping?.Telephone} / {companyEntityShipping?.Telephone2}".Trim().Trim('/');
			ShippingFax = !string.IsNullOrWhiteSpace(orderEntity?.DeliveryFax)
				? orderEntity?.DeliveryFax
				: companyEntityShipping?.Fax;
			ShippingEmail = companyEntityShipping?.Email;
			ShippingStorageLocation = orderEntity.StorageLocationName;

			BillingContactName = "";
			//!string.IsNullOrWhiteSpace(orderEntity?.BillingAddress)
			//? $"{orderEntity?.BillingContactName ?? ""}, "
			//: orderEntity?.BillingContactName ?? "";
			BillingAddress = $"{orderEntity?.BillingAddress}, {companyEntity?.City}, {companyEntity?.PostalCode}, {companyEntity?.Country}"?.Trim(',').Trim();
			BillingCompanyName = !string.IsNullOrWhiteSpace(orderEntity?.BillingDepartmentName)
				? ""
				: orderEntity?.BillingCompanyName;
			BillingDepartmentName = orderEntity?.BillingDepartmentName;
			BillingTelephone = orderEntity?.BillingTelephone;
			BillingFax = orderEntity?.BillingFax;

			decimal totextax = 0m, tottax = 0m, totintax = 0m;
			decimal subTot = 0m, totDiscount = 0m, totNet = 0m, totTax = 0m, tot = 0m;

			OrderItems = new List<OrderItemModel>();
			if(article_OrderEntities != null && article_OrderEntities.Count > 0)
			{
				foreach(var item in article_OrderEntities)
				{
					var bestellteArticle = bestellteArticleEntities?.Find(x => x.Nr == item.BestellteArtikelNr && (item.ArticleId <= 0 || (item.ArticleId > 0 && item.ArticleId == x.Artikel_Nr.Value)));
					var article = artikelEntities?.Find(x => x.Artikel_Nr == item.ArticleId);
					OrderItems.Add(new OrderItemModel(project_BudgetEntity, item, bestellteArticle, article));
					totextax += (item?.UnitPrice ?? 0) * (item.Quantity ?? 0);
					tottax += (item?.UnitPrice ?? 0) * (item.Quantity ?? 0) * (item.VAT ?? 0);
					totintax += ((item?.UnitPrice ?? 0) * (item.Quantity ?? 0) * (1 + (item.VAT ?? 0)));

					// - 
					subTot += (item?.UnitPrice ?? 0) * (item.Quantity ?? 0);
					totTax += ((item?.UnitPrice ?? 0) * (item.Quantity ?? 0) * (item.VAT ?? 0) * (1 - ((orderEntity?.Discount ?? 0) / 100)));
				}
			}
			totDiscount = subTot * ((orderEntity?.Discount ?? 0) / 100);
			totNet = subTot - totDiscount;
			tot = totNet + totTax;

			ArticleTotalExTax = $"{Math.Round(totextax, 2).ToString("0.00")} {orderEntity?.CurrencyName}";
			ArticleTotalTax = $"{Math.Round(tottax, 2).ToString("0.00")} {orderEntity?.CurrencyName}";
			ArticleTotalInTax = $"{Math.Round(totintax, 2).ToString("0.00")} {orderEntity?.CurrencyName}";
			// -
			Subtotal = $"{Math.Round(subTot, 2).ToString("0.00")} {orderEntity?.CurrencyName}";
			TotalDiscount = $"{Math.Round(totDiscount, 2).ToString("0.00")} {orderEntity?.CurrencyName}";
			TotalNet = $"{Math.Round(totNet, 2).ToString("0.00")} {orderEntity?.CurrencyName}";
			TotalTax = $"{Math.Round(totTax, 2).ToString("0.00")} {orderEntity?.CurrencyName}";
			Total = $"{Math.Round(tot, 2).ToString("0.00")} {orderEntity?.CurrencyName}";

			//- Company data
			CompanyName = companyEntity?.LagalName;
			CompanyAddress = companyEntity?.Address;
			CompanyPostalCode = companyEntity?.PostalCode;
			CompanyCity = companyEntity?.City;
			CompanyCountry = companyEntity?.Country;
			CompanyTelephone = companyEntity?.Telephone;
			CompanyFax = companyEntity?.Fax;
			CompanyEmail = companyEntity?.Email;

			IssuerName = userEntity?.Name;
			IssuerTelephone = $"{userEntity?.TelephoneMobile}, {userEntity?.TelephoneHome}, {userEntity?.TelephoneIP}".Trim().Trim(',');
			IssuerEmail = userEntity?.Email;
			IssuerFax = userEntity?.Fax;


			//-
			FormNumber = "F-XX-01";
			// - Footer
			FooterAddress = companyExtensionEntity?.Address;
			FooterCity = companyExtensionEntity?.City;
			FooterPostalCode = companyExtensionEntity?.PostalCode;
			FooterVAT = companyExtensionEntity?.VATNumberID;
			FooterSite = companyExtensionEntity?.Site;
			FooterFax = companyExtensionEntity?.Fax;
			FooterPhone = companyExtensionEntity?.Phone;
			FooterManager1 = companyExtensionEntity?.Manager1;
			FooterManager2 = companyExtensionEntity?.Manager2;
			FooterTaxNumber = companyExtensionEntity?.TaxNumberID;
			FooterHRB = companyExtensionEntity?.HRB;
			FooterEmail = companyExtensionEntity?.Email;
			FooterCustomsNumber = companyExtensionEntity?.CustomsNumber;

			FooterBankDetails1 = companyExtensionEntity?.BankDetails1;
			FooterBankDetails2 = companyExtensionEntity?.BankDetails2;
			FooterBankDetails3 = companyExtensionEntity?.BankDetails3;
			FooterAccount1 = companyExtensionEntity?.Account1;
			FooterAccount2 = companyExtensionEntity?.Account2;
			FooterAccount3 = companyExtensionEntity?.Account3;
			FooterBLZ1 = companyExtensionEntity?.BLZ1;
			FooterBLZ2 = companyExtensionEntity?.BLZ2;
			FooterBLZ3 = companyExtensionEntity?.BLZ3;
			FooterIBAN1 = companyExtensionEntity?.IBAN1;
			FooterIBAN2 = companyExtensionEntity?.IBAN2;
			FooterIBAN3 = companyExtensionEntity?.IBAN3;
			FooterSWIFT1 = companyExtensionEntity?.SWIFT_BIC1;
			FooterSWIFT2 = companyExtensionEntity?.SWIFT_BIC2;
			FooterSWIFT3 = companyExtensionEntity?.SWIFT_BIC3;

			var paymentType = "";
			switch(companyEntity?.Country?.ToLower())
			{
				case "de":
				case "deutschland":
					{
						FooterCity_Title = "Sitz:";
						FooterFax_Title = "Fax:";
						FooterManager_Title = "Geschäftsführer:";
						FooterVAT_Title = "USt.-IdNr.:";
						FooterTaxNumber_Tilte = "Steuernummer:";
						FooterHRB_Title = "HRB:";
						FooterEmail_Title = "Email:";
						FooterCustomsNumber_Title = "Zollnummer:";
						FooterBankDetails_Title = "Bankverbindung:";
						FooterAccount_Title = "Konto:";
						FooterBLZ_Title = "BLZ:";
						FooterIBAN_Title = "IBAN:";
						FooterSWIFT_Title = "SWIFT-BIC:";
						BillingSite = (int)BillingSites.PSZ_DE;
						paymentType = "Achtung: Abwicklung erfolgt über Leasing!";
						break;
					}
				case "tn":
				case "tunisie":
				case "tunisia":
					{
						FooterCity_Title = "Site:";
						FooterFax_Title = "Telefax:";
						FooterManager_Title = "Management:";
						FooterVAT_Title = "";
						FooterTaxNumber_Tilte = "Tax Number";
						FooterHRB_Title = "";
						FooterEmail_Title = "E-mail:";
						FooterCustomsNumber_Title = "";
						FooterBankDetails_Title = "Bank Details:";
						FooterAccount_Title = "Account:";
						FooterBLZ_Title = "RIB:";
						FooterIBAN_Title = "IBAN:";
						FooterSWIFT_Title = "SWIFT-BIC:";
						BillingSite = (int)BillingSites.PSZ_TN;
						paymentType = "Attention: processing takes place via leasing!";
						break;
					}
				case "cz":
				case "czech":
				case "czechia":
					{
						FooterCity_Title = "ĎIC:";
						FooterFax_Title = "Fax:";
						FooterManager_Title = "Jednatel:";
						FooterVAT_Title = "IČ:";
						FooterTaxNumber_Tilte = "";
						FooterHRB_Title = "";
						FooterEmail_Title = "";
						FooterCustomsNumber_Title = "";
						FooterBankDetails_Title = "Bankovni spojeni:";
						FooterAccount_Title = "Číslo účtu:";
						FooterBLZ_Title = "BLZ:";
						FooterIBAN_Title = "IBAN:";
						FooterSWIFT_Title = "SWIFT-BIC:";
						BillingSite = (int)BillingSites.PSZ_CZ;
						paymentType = "Attention: processing takes place via leasing!";
						break;
					}
				case "al":
				case "albania":
				case "albanie":
					{
						FooterCity_Title = "";
						FooterFax_Title = "Fax:";
						FooterManager_Title = "Administratorja:";
						FooterVAT_Title = "NIPT:";
						FooterTaxNumber_Tilte = "";
						FooterHRB_Title = "";
						FooterEmail_Title = "Email:";
						FooterCustomsNumber_Title = "";
						FooterBankDetails_Title = "Bank:";
						FooterAccount_Title = "Nr Llogarise:";
						FooterBLZ_Title = "";
						FooterIBAN_Title = "IBAN:";
						FooterSWIFT_Title = "SWIFT-BIC:";
						BillingSite = (int)BillingSites.PSZ_AL;
						paymentType = "Caution: processing takes place via leasing!";
						break;
					}
				default:
					{
						FooterCity_Title = "Site:";
						FooterFax_Title = "Fax:";
						FooterManager_Title = "Manager:";
						FooterVAT_Title = "VAT No.:";
						FooterTaxNumber_Tilte = "Tax No.:";
						FooterHRB_Title = "HRB:";
						FooterEmail_Title = "Email:";
						FooterCustomsNumber_Title = "Customs No.";
						FooterBankDetails_Title = "Bank Details:";
						FooterAccount_Title = "Account:";
						FooterBLZ_Title = "BLZ:";
						FooterIBAN_Title = "IBAN:";
						FooterSWIFT_Title = "SWIFT-BIC:";
						BillingSite = (int)BillingSites.PSZ_DE;
						paymentType = "Attention: Processing is carried out via leasing!";
					}
					break;
			}


			PoPaymentType = orderEntity?.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
				? ""
				: paymentType;
		}

		public class OrderItemModel
		{
			public int Id { get; set; }
			public string ArticleNumber { get; set; }
			public string ArticleDesignation { get; set; }
			public string ArticleQuqntity { get; set; }
			public string ArticleUnitPrice { get; set; }
			public string ArticleUnitOfMeasure { get; set; }
			public string ArticleUOMPrice { get; set; }
			public string ArticleTotal { get; set; }
			public string ArticleRef { get; set; }
			public string ArticleDeliveryDate { get; set; }


			public string VAT { get; set; }
			public string TotalVAT { get; set; }
			public string ArticleVATTotal { get; set; }

			public string DueDate { get; set; }
			public string Lager { get; set; }
			public string Project { get; set; }
			public string ContactName { get; set; }
			public string AccountNumber { get; set; }
			public OrderItemModel(
				Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity project_BudgetEntity,
				Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity bestellteArtikelExtensionEntity,
				Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity bestellteArticleEntity,
				Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity artikelEntity)
			{
				Id = bestellteArtikelExtensionEntity.Id;
				ArticleNumber = artikelEntity?.Artikelnummer;
				ArticleDesignation = bestellteArticleEntity?.Bezeichnung_1;
				ArticleQuqntity = Math.Round(bestellteArtikelExtensionEntity?.Quantity ?? 0, 2).ToString("0.00");
				ArticleUnitPrice = Math.Round(bestellteArtikelExtensionEntity?.UnitPrice ?? 0, 2).ToString("0.00");
				ArticleUnitOfMeasure = artikelEntity?.Einheit;
				ArticleUOMPrice = artikelEntity?.Preiseinheit.ToString("0.00");
				ArticleTotal = Math.Round(bestellteArtikelExtensionEntity?.TotalCost ?? 0, 2).ToString("0.00");

				VAT = $"{Math.Round(100 * (bestellteArtikelExtensionEntity?.VAT ?? 0), 2).ToString("0.00")} %";
				TotalVAT = Math.Round(bestellteArtikelExtensionEntity?.VAT * (decimal)(bestellteArtikelExtensionEntity.UnitPrice ?? 0) * bestellteArtikelExtensionEntity.Quantity ?? 0, 2).ToString("0.00");
				ArticleVATTotal = Math.Round((1 + bestellteArtikelExtensionEntity?.VAT) * (decimal)(bestellteArtikelExtensionEntity.UnitPrice ?? 0) * bestellteArtikelExtensionEntity.Quantity ?? 0, 2).ToString("0.00");

				DueDate = bestellteArtikelExtensionEntity?.DeliveryDate?.ToString("dd.MM.yyyy");
				Lager = bestellteArtikelExtensionEntity?.LocationName;
				Project = project_BudgetEntity?.ProjectName;
				ContactName = bestellteArtikelExtensionEntity?.InternalContact;
				AccountNumber = bestellteArtikelExtensionEntity?.AccountName;
				ArticleRef = bestellteArticleEntity?.Bestellnummer;
				ArticleDeliveryDate = bestellteArtikelExtensionEntity?.DeliveryDate?.ToString("dd.MM.yyyy");
			}
		}
	}
	enum BillingSites
	{
		PSZ_DE = 1,
		PSZ_CZ = 2,
		PSZ_TN = 3,
		PSZ_AL = 4,
		WS = 5
	}
}
