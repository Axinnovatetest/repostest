using System;
using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.FNC
{
	public class InvoiceTemplateModel
	{
		public int Id { get; set; }

		public string CustomerCompanyName { get; set; }
		public string CustomerCompanyStreet { get; set; }
		public string CustomerCompanyCity { get; set; }
		public string CustomerCompanyPhone { get; set; }

		public string PONumber { get; set; }
		public string Number { get; set; }
		public string DocumentType { get; set; }
		public string Date { get; set; }

		public string SummarySubtotal { get; set; }
		public string SummaryDiscount { get; set; }
		public string SummaryTotal { get; set; }
		public string SummaryTaxes { get; set; }
		public string SummaryTotalwTaxes { get; set; }

		// -
		public string LastPageText1 { get; set; }
		public string LastPageText2 { get; set; }
		public string LastPageText3 { get; set; }
		public string LastPageText4 { get; set; }
		public string LastPageText5 { get; set; }
		public string LastPageText6 { get; set; }
		public string LastPageText7 { get; set; }
		public string LastPageText8 { get; set; }
		public string LastPageText9 { get; set; }
		public string LastPageText10 { get; set; }
		public InvoiceTemplateModel(
			Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity invoiceEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.InvoiceItemEntity> invoiceItemEntities,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyEntity,
			ReportLanguage language = ReportLanguage.English)
		{
			if(invoiceEntity == null)
				return;

			// -
			PONumber = invoiceEntity.OrderNumber;
			Number = invoiceEntity.Reference;
			Date = invoiceEntity.CreationDate?.ToString("dd-MM-yyyy")?.Trim();
			DocumentType = "";

			if(companyEntity != null)
			{
				CustomerCompanyName = companyEntity.Name;
				CustomerCompanyStreet = $"{companyEntity.Address}, {companyEntity.PostalCode}".Trim().Trim(',');
				CustomerCompanyCity = $"{companyEntity.City}, {companyEntity.Country}".Trim().Trim(',');
				CustomerCompanyPhone = $"{(string.IsNullOrWhiteSpace(companyEntity.Telephone) ? "" : $"Tel.: {companyEntity.Telephone}")}, {(string.IsNullOrWhiteSpace(companyEntity.Fax) ? "" : $"Fax: {companyEntity.Fax}")}".Trim().Trim(',');
			}

			DocumentType = "";

			decimal totextax = 0m, tottax = 0m, totintax = 0m;
			decimal subTot = 0m, totDiscount = 0m, totNet = 0m, totTax = 0m, tot = 0m, hFees = 0m;

			if(invoiceItemEntities != null && invoiceItemEntities.Count > 0)
			{
				foreach(var item in invoiceItemEntities)
				{
					totextax += (item?.Einzelpreis * item.Anzahl) ?? 0;
					tottax += (item?.Einzelpreis * item.Anzahl * (decimal?)item.Umsatzsteuer) ?? 0;
					totintax += (item?.Einzelpreis * item.Anzahl * (1 + (decimal?)item.Umsatzsteuer)) ?? 0;
					hFees = invoiceEntity.IgnoreHandlingFees == true ? 0m : 0.05m * ((item?.Einzelpreis * item.Anzahl) ?? 0);
					// - 
					subTot += ((item?.Einzelpreis * item.Anzahl) ?? 0) + (hFees > 150 ? 150 : hFees);
					totTax += (item?.Einzelpreis * item.Anzahl * (decimal?)item.Umsatzsteuer * (1 - ((invoiceEntity?.Discount ?? 0) / 100))) ?? 0;
				}
			}
			totDiscount = subTot * ((invoiceEntity?.Discount ?? 0) / 100);
			totNet = subTot - totDiscount;
			tot = totNet; //  + totTax; // no taxes

			// -
			SummarySubtotal = $"{Math.Round(subTot, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			SummaryDiscount = $"{Math.Round(totDiscount, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			SummaryTotal = $"{Math.Round(totNet, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			SummaryTaxes = $"{Math.Round(totTax, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";
			SummaryTotalwTaxes = $"{Math.Round(tot, 2).ToString("0.00")} {invoiceEntity?.CurrencyName}";


			switch(language)
			{
				case ReportLanguage.German:
					{
						LastPageText1 = "Artikel Nr";
						LastPageText2 = "Artikelref.";

						//-
						LastPageText3 = "- Dieses Dokument wurde elektronisch erstellt und ist daher auch ohne Unterschrift gültig";
						LastPageText4 = "- This document was issued electronically and is therefore valid without signature";
						LastPageText5 = "- Für unsere Bestellungen gelten ausschließlich unsere allgemeinen Einkaufsbedingungen";
						LastPageText6 = "- Einsicht möglich  unter www.psz-electronic.com";

						LastPageText7 = "- Wir sind gemäß Art.13 DSGVO verpflichtet, Sie als Geschäftskunde über unsere Datenschutzmaßnahmen zu informieren.";
						LastPageText8 = "- Unsere Datenschutzerklärung finden Sie auf unserer Website unter folgenden Link:";

						LastPageText9 = " https://www.psz-electronic.com/wp-content/uploads/2019/02/19-02-04-PSZ_Informationspflicht-Datenschutz-Art.13_Gesch%C3%A4ftspartnerBewerber.pdf"; // - link
						LastPageText10 = "- Diese Lieferung ist gem. §4 UstG steuerfrei";
					}
					break;
				case ReportLanguage.Czech:
				case ReportLanguage.Albanian:
				case ReportLanguage.French:
				case ReportLanguage.English:
				default:
					{
						LastPageText1 = "Article Nr";
						LastPageText2 = "Article Ref.";

						//-
						LastPageText3 = "- Dieses Dokument wurde elektronisch erstellt und ist daher auch ohne Unterschrift gültig";
						LastPageText4 = "- This document was issued electronically and is therefore valid without signature";
						LastPageText5 = "- Für unsere Bestellungen gelten ausschließlich unsere allgemeinen Einkaufsbedingungen";
						LastPageText6 = "- Einsicht möglich  unter www.psz-electronic.com";

						LastPageText7 = "- Wir sind gemäß Art.13 DSGVO verpflichtet, Sie als Geschäftskunde über unsere Datenschutzmaßnahmen zu informieren.";
						LastPageText8 = "- Unsere Datenschutzerklärung finden Sie auf unserer Website unter folgenden Link:";

						LastPageText9 = " https://www.psz-electronic.com/wp-content/uploads/2019/02/19-02-04-PSZ_Informationspflicht-Datenschutz-Art.13_Gesch%C3%A4ftspartnerBewerber.pdf"; // - link
						LastPageText10 = "- Diese Lieferung ist gem. §4 UstG steuerfrei";
					}
					break;
			}
		}
	}
}
