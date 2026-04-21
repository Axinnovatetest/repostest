using System;
using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.FNC
{
	public class InvoiceModel
	{
		public int Id { get; set; }
		public byte[] CompanyLogoImage { get; set; }
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

		public InvoiceModel(
			Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity,
			Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity companyExtensionEntity,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity billingCompanyEntity,
			Infrastructure.Data.Entities.Tables.FNC.CompanyExtensionEntity billingCompanyExtension,
			Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity invoiceEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> invoiceItemEntities
			)
		{
			// - 
			CompanyLogoImage = billingCompanyEntity?.Logo;

			decimal totextax = 0m, tottax = 0m, totintax = 0m;
			decimal subTot = 0m, totDiscount = 0m, totNet = 0m, totTax = 0m, tot = 0m;

			OrderItems = new List<OrderItemModel>();
			if(invoiceItemEntities != null && invoiceItemEntities.Count > 0)
			{
				foreach(var item in invoiceItemEntities)
				{
					//var bestellteArticle = bestellteArticleEntities?.Find(x => x.Nr == item.BestellteArtikelNr && (item.ArticleId <= 0 || (item.ArticleId > 0 && item.ArticleId == x.Artikel_Nr.Value)));
					OrderItems.Add(new OrderItemModel(item, invoiceEntity.IgnoreHandlingFees ?? false));
					totextax += (item?.Einzelpreis ?? 0) * (item.Anzahl ?? 0);
					tottax += (item?.Einzelpreis ?? 0) * (item.Anzahl ?? 0) * ((decimal?)item.Umsatzsteuer ?? 0);
					totintax += (item?.Einzelpreis ?? 0) * (item.Anzahl ?? 0) * (1 + ((decimal?)item.Umsatzsteuer ?? 0));

					// - 
					subTot += (item?.Einzelpreis ?? 0) * (item.Anzahl ?? 0);
					totTax += ((item?.Einzelpreis ?? 0) * (item.Anzahl ?? 0) * ((decimal?)item.Umsatzsteuer ?? 0) * (1 - ((invoiceEntity?.Discount ?? 0) / 100)));
				}
			}
			totDiscount = subTot * ((invoiceEntity?.Discount ?? 0) / 100);
			totNet = subTot - totDiscount;
			tot = totNet + totTax;

			ArticleTotalExTax = $"{Math.Round(totextax, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			ArticleTotalTax = $"{Math.Round(tottax, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			ArticleTotalInTax = $"{Math.Round(totintax, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			// -
			Subtotal = $"{Math.Round(subTot, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			TotalDiscount = $"{Math.Round(totDiscount, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			TotalNet = $"{Math.Round(totNet, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			TotalTax = $"{Math.Round(totTax, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			Total = $"{Math.Round(tot, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";

			////- Company data
			//CompanyName = companyEntity?.LagalName;
			//CompanyAddress = companyEntity?.Address;
			//CompanyPostalCode = companyEntity?.PostalCode;
			//CompanyCity = companyEntity?.City;
			//CompanyCountry = companyEntity?.Country;
			//CompanyTelephone = companyEntity?.Telephone;
			//CompanyFax = companyEntity?.Fax;
			//CompanyEmail = companyEntity?.Email;

			IssuerName = userEntity?.Name;
			IssuerTelephone = $"{userEntity?.TelephoneMobile}, {userEntity?.TelephoneHome}, {userEntity?.TelephoneIP}".Trim().Trim(',');
			IssuerEmail = userEntity?.Email;
			IssuerFax = userEntity?.Fax;


			//-
			FormNumber = "F-XX-01";
			// - Footer
			FooterAddress = billingCompanyExtension?.Address;
			FooterCity = billingCompanyExtension?.City;
			FooterPostalCode = billingCompanyExtension?.PostalCode;
			FooterVAT = billingCompanyExtension?.VATNumberID;
			FooterSite = billingCompanyExtension?.Site;
			FooterFax = billingCompanyExtension?.Fax;
			FooterPhone = billingCompanyExtension?.Phone;
			FooterManager1 = billingCompanyExtension?.Manager1;
			FooterManager2 = billingCompanyExtension?.Manager2;
			FooterTaxNumber = billingCompanyExtension?.TaxNumberID;
			FooterHRB = billingCompanyExtension?.HRB;
			FooterEmail = billingCompanyExtension?.Email;
			FooterCustomsNumber = billingCompanyExtension?.CustomsNumber;

			FooterBankDetails1 = billingCompanyExtension?.BankDetails1;
			FooterBankDetails2 = billingCompanyExtension?.BankDetails2;
			FooterBankDetails3 = billingCompanyExtension?.BankDetails3;
			FooterAccount1 = billingCompanyExtension?.Account1;
			FooterAccount2 = billingCompanyExtension?.Account2;
			FooterAccount3 = billingCompanyExtension?.Account3;
			FooterBLZ1 = billingCompanyExtension?.BLZ1;
			FooterBLZ2 = billingCompanyExtension?.BLZ2;
			FooterBLZ3 = billingCompanyExtension?.BLZ3;
			FooterIBAN1 = billingCompanyExtension?.IBAN1;
			FooterIBAN2 = billingCompanyExtension?.IBAN2;
			FooterIBAN3 = billingCompanyExtension?.IBAN3;
			FooterSWIFT1 = billingCompanyExtension?.SWIFT_BIC1;
			FooterSWIFT2 = billingCompanyExtension?.SWIFT_BIC2;
			FooterSWIFT3 = billingCompanyExtension?.SWIFT_BIC3;

			switch(billingCompanyEntity?.Country?.ToLower())
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
						break;
					}
				case "tn":
				case "tunisie":
				case "tunisia":
					{
						FooterCity_Title = "Site:";
						FooterFax_Title = "Telefax:";
						FooterManager_Title = "Geschäftsführung:";
						FooterVAT_Title = "";
						FooterTaxNumber_Tilte = "Steuernummer";
						FooterHRB_Title = "";
						FooterEmail_Title = "E-mail:";
						FooterCustomsNumber_Title = "";
						FooterBankDetails_Title = "Bankverbindung:";
						FooterAccount_Title = "Konto:";
						FooterBLZ_Title = "RIB/BLZ:";
						FooterIBAN_Title = "IBAN:";
						FooterSWIFT_Title = "SWIFT-BIC:";
						BillingSite = (int)BillingSites.PSZ_TN;
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
					}
					break;
			}
		}

		public class OrderItemModel
		{
			public int Id { get; set; }
			public string Position { get; set; }
			public string Designation { get; set; }
			public string Quantity { get; set; }
			public string UnitPrice { get; set; }
			public string VAT { get; set; }
			public string Amount { get; set; }
			public string HandlingFees { get; set; }
			public OrderItemModel(
				Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity invoiceItemEntity, bool ignoreHandlingFees)
			{
				Id = invoiceItemEntity.Id;
				Position = invoiceItemEntity?.Position?.ToString();
				Designation = invoiceItemEntity?.Bezeichnung_1;
				//Quantity = Math.Round(bestellteArtikelExtensionEntity?.Quantity ?? 0, 2).ToString("0.00");
				//UnitPrice = Math.Round(bestellteArtikelExtensionEntity?.UnitPrice ?? 0, 2).ToString("0.00");
				//Amount = Math.Round(bestellteArtikelExtensionEntity?.TotalCost ?? 0, 2).ToString("0.00");
				//VAT = $"{Math.Round(100 * (bestellteArtikelExtensionEntity?.VAT ?? 0), 2).ToString("0.00")} %";
				Quantity = Math.Round(invoiceItemEntity?.Anzahl ?? 0, 2).ToString("0.00");
				UnitPrice = Math.Round(invoiceItemEntity?.Einzelpreis ?? 0, 2).ToString("0.00");
				Amount = Math.Round(invoiceItemEntity?.Gesamtpreis ?? 0, 2).ToString("0.00");
				VAT = $"{Math.Round(100 * (invoiceItemEntity?.Umsatzsteuer ?? 0), 2).ToString("0.00")} %";
				// -
				var handlingFees = ignoreHandlingFees ? 0m : 0.05m * (invoiceItemEntity?.Gesamtpreis ?? 0);
				HandlingFees = $"{Math.Round(handlingFees > 150 ? 150 : handlingFees, 2).ToString("0.00")}";
			}
			public OrderItemModel(Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity bestellteArticleEntity)
			{
				Id = bestellteArticleEntity.Nr;
				Position = bestellteArticleEntity?.Position?.ToString();
				Designation = bestellteArticleEntity?.Bezeichnung_1;
				Quantity = Math.Round(bestellteArticleEntity?.Anzahl ?? 0, 2).ToString("0.00");
				UnitPrice = Math.Round(bestellteArticleEntity?.Einzelpreis ?? 0, 2).ToString("0.00");
				Amount = Math.Round(bestellteArticleEntity?.Gesamtpreis ?? 0, 2).ToString("0.00");
				VAT = $"{Math.Round(100 * (bestellteArticleEntity?.Umsatzsteuer ?? 0), 2).ToString("0.00")} %";
				// -
				var handlingFees = 0.05m * (bestellteArticleEntity?.Gesamtpreis ?? 0);
				HandlingFees = $"{Math.Round(handlingFees > 150 ? 150 : handlingFees, 2).ToString("0.00")}";
			}
		}
	}
}
