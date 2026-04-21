using System;
using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.Itext
{
	public class CTS_ABReportModel
	{
		//Header
		public string Logo { get; set; }
		public string Top100Logo { get; set; }
		public string Top100Description { get; set; }
		public string Top100_2026Logo { get; set; }
		public string InvoiceHeader { get; set; }
		public string InvoiceAdress1Value { get; set; }
		public string InvoiceAdress2Value { get; set; }
		public string InvoiceAdress3Value { get; set; }
		public string InvoiceAdress4Value { get; set; }
		public string InvoiceAdress5Value { get; set; }
		public string InvoiceName2Value { get; set; }
		public string InvoiceName3Value { get; set; }
		public string InvoiceAnsprechpartnerValue { get; set; }
		public string InvoiceAbteilungValue { get; set; }
		//Items header
		public string DocumentType { get; set; }
		public string OrderNumberPOTitle { get; set; }
		public string OrderNumberPOValue { get; set; }
		public string OrderDateValue { get; set; }
		public string OrderDateTitle { get; set; }
		public string OrderNumberTitle { get; set; }
		public string OrderNumberValue { get; set; }
		public string ClientNumberTitle { get; set; }
		public string ClientNumberValue { get; set; }
		public string InternalNumberTitle { get; set; }
		public string InternalNumberValue { get; set; }
		public string ShippingMethodTitle { get; set; }
		public string ShippingMethodValue { get; set; }
		public string PayementMethodTitle { get; set; }
		public string PayementMethodValue { get; set; }
		public string PayementTargetTitle { get; set; }
		public string PayementTargetValue { get; set; }
		public string UST_IDTitle { get; set; }
		public string UST_IDValue { get; set; }
		public string ItemsHeaderTitle { get; set; }
		public string ItemsHeaderValue { get; set; }
		//Items titles
		public string PositionTitle { get; set; }
		public string ArticleTitle { get; set; }
		public string DesignationTitle { get; set; }
		public string DescriptionTitle { get; set; }
		public string KundenIndexTitle { get; set; }
		public string UnitTitle { get; set; }
		public string BasisPrice150Title { get; set; }
		public string TotalPrice150Title { get; set; }
		public string DELTitle { get; set; }
		public string CU_SurchargeTitle { get; set; }
		public string CU_TotalTitle { get; set; }
		public string CU_GTitle { get; set; }
		public string UnitPriceTitle { get; set; }
		public string UnitTotalTitle { get; set; }
		public string AmonutTitle { get; set; }
		public string PETitle { get; set; }
		public string lieferterminTitle { get; set; }
		//Items values
		public List<CTS_ABReportItemsModel> Items { get; set; }
		//Summaries
		public string SummarySumTitle { get; set; }
		public string SummarySumValue { get; set; }
		public string SummaryUSTTitle { get; set; }
		public string SummaryUSTValue { get; set; }
		public string SummaryTotalTitle { get; set; }
		public string SummaryTotalValue { get; set; }
		//Footers
		public string LastPageText1 { get; set; }
		public string LastPageText2 { get; set; }
		public string LastPageText3 { get; set; }
		public string LastPageText4 { get; set; }
		public string LastPageText5 { get; set; }
		public string LastPageText10 { get; set; }
		//delivery Adress
		public string DeliveryAdressTitle { get; set; }
		public string LAnrede { get; set; }
		public string LVorname { get; set; }
		public string LName2 { get; set; }
		public string LName3 { get; set; }
		public string Lansprechpartner { get; set; }
		public string Labteilung { get; set; }
		public string LStrabe { get; set; }
		public string LLandPLZOrt { get; set; }
		public string lieferterminValue { get; set; }
		public string OrderedTitle { get; set; }
		public string DeliveredTitle { get; set; }
		public string OpenTitle { get; set; }
		public string Lieferadresse { get; set; }
		public CTS_ABReportModel
			(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity reportEntity, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity orderEntity,
			Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity buyerEntity, Infrastructure.Data.Entities.Tables.PRS.AdressenEntity adressEntity, decimal ust)
		{
			var trimChars = new char[] { ',', ' ' };
			InvoiceHeader = reportEntity.Header;
			InvoiceAdress1Value = orderEntity.Anrede?.Trim();
			InvoiceAdress2Value = orderEntity.Vorname_NameFirma?.Trim();
			InvoiceAdress3Value = orderEntity.Land_PLZ_Ort.Contains(orderEntity.Straße_Postfach?.Trim()) == true
						  ? buyerEntity?.Name2.Trim()
							: ((string.IsNullOrWhiteSpace(buyerEntity?.Name2) || orderEntity.Straße_Postfach?.Contains(buyerEntity?.Name2?.Trim()) == true)
								? orderEntity.Straße_Postfach?.Trim()
								: orderEntity.Straße_Postfach?.Trim() + " " + buyerEntity?.Name2)?.Trim();
			InvoiceName2Value = orderEntity.Name2.Trim();
			InvoiceName3Value = orderEntity.Name3.Trim();
			InvoiceAdress4Value = orderEntity.Land_PLZ_Ort?.Trim();
			InvoiceAdress5Value = $"Fax: {adressEntity.Fax?.Trim()}";
			InvoiceAnsprechpartnerValue = orderEntity.Ansprechpartner?.Trim();
			InvoiceAbteilungValue = orderEntity.Abteilung?.Trim();
			OrderNumberPOTitle = reportEntity.OrderNumberPO;
			OrderNumberPOValue = orderEntity.Bezug.ToString();
			//DocumentType
			OrderNumberTitle = reportEntity.OrderNumber;
			OrderNumberValue = orderEntity.Angebot_Nr?.ToString();
			OrderDateTitle = reportEntity.OrderDate;
			OrderDateValue = orderEntity.Datum?.ToString("dd.MM.yyyy");
			ClientNumberTitle = reportEntity.ClientNumber;
			ClientNumberValue = orderEntity.Ihr_Zeichen;
			InternalNumberTitle = reportEntity.InternalNumber;
			InternalNumberValue = orderEntity.Unser_Zeichen.ToString();
			ShippingMethodTitle = reportEntity.ShippingMethod;
			ShippingMethodValue = orderEntity.Versandart;
			PayementMethodTitle = reportEntity.PaymentMethod;
			PayementMethodValue = orderEntity.Zahlungsweise;
			PayementTargetTitle = reportEntity.PaymentTarget;
			PayementTargetValue = orderEntity.Konditionen;
			UST_IDTitle = reportEntity.UST_ID?.Trim();
			UST_IDValue = orderEntity.Freitext?.Trim();

			SummarySumTitle = reportEntity.SummarySum;
			SummaryUSTTitle = $"{reportEntity.SummaryUST} {(ust * 100).ToString("0.##")}%";
			SummaryTotalTitle = reportEntity.SummaryTotal;

			//SummarySumValue =;
			//SummaryUSTValue =;
			//SummaryTotalValue =;

			LastPageText1 = reportEntity.LastPageText1 ?? "";
			LastPageText2 = reportEntity.LastPageText2 ?? "";
			LastPageText3 = reportEntity.LastPageText3 ?? "";
			LastPageText4 = reportEntity.LastPageText4 ?? "";
			LastPageText5 = reportEntity.LastPageText5 ?? "";
			LastPageText10 = reportEntity.LastPageText10 ?? "";

			DeliveryAdressTitle = reportEntity.Lieferadresse;
			LAnrede = orderEntity.LAnrede;
			LVorname = orderEntity.LVorname_NameFirma;
			LName2 = orderEntity.LName2;
			LName3 = orderEntity.LName3;
			Lansprechpartner = orderEntity.LAnsprechpartner;
			Labteilung = orderEntity.LAbteilung;
			LStrabe = orderEntity.LStraße_Postfach;
			LLandPLZOrt = ((orderEntity.LLand_PLZ_Ort ?? "").StartsWith(orderEntity.LStraße_Postfach?.Trim() ?? "")
							? (orderEntity.LLand_PLZ_Ort ?? "").Trim().Substring(Math.Max((orderEntity.LStraße_Postfach ?? "").Length, 0))
							: orderEntity.LLand_PLZ_Ort ?? "").Trim(trimChars);

			ItemsHeaderTitle = reportEntity.ItemsHeader;
			ItemsHeaderValue = "";
			AmonutTitle = reportEntity.Amount;
			PETitle = reportEntity.PE;
			CU_SurchargeTitle = reportEntity.Cu_Surcharge;
			CU_TotalTitle = reportEntity.Cu_Total;
			CU_GTitle = reportEntity.Cu_G;
			PositionTitle = reportEntity.Position;
			DesignationTitle = reportEntity.Designation;
			DescriptionTitle = reportEntity.Description;
			KundenIndexTitle = "";// - reportEntity.Index_Kunde;
			UnitPriceTitle = reportEntity.UnitPrice;
			UnitTitle = reportEntity.Unit;
			UnitTotalTitle = reportEntity.UnitTotal;
			ArticleTitle = reportEntity.Article;
			BasisPrice150Title = reportEntity.BasisPrice150;
			TotalPrice150Title = reportEntity.TotalPrice150;
			lieferterminTitle = reportEntity.Liefertermin;

			OrderedTitle = reportEntity.Bestellt;
			DeliveredTitle = reportEntity.Geliefert;
			OpenTitle = reportEntity.Offen;

			DELTitle = reportEntity.DEL;
			Lieferadresse = reportEntity.Lieferadresse;
		}

	}
	public class CTS_ABReportItemsModel
	{
		public string PositionValue { get; set; }
		public string ArticleValue { get; set; }
		public string DesignationValue { get; set; }
		public string DescriptionValue { get; set; }
		public string KundenIndexValue { get; set; }
		public string OriginalAmountValue { get; set; }
		public string AmountValue { get; set; }
		public string RestAmountValue { get; set; }
		public string PEValue { get; set; }
		public string UnitValue { get; set; }
		public string BasisPrice150Value { get; set; }
		public string TotalPrice150Value { get; set; }
		public string Cu_GValue { get; set; }
		public string AbladestelleValue { get; set; }
		public string DELValue { get; set; }
		public string CU_SurchargeValue { get; set; }
		public string CU_TotalValue { get; set; }
		public string UnitPriceValue { get; set; }
		public string UnitTotalValue { get; set; }
		public string PostextValue { get; set; }
		public string lieferterminValue { get; set; }
		public string DELFixiertValue { get; set; }
		public CTS_ABReportItemsModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity itemEntity)
		{
			var _articleItem = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(itemEntity.ArtikelNr ?? -1);

			PositionValue = itemEntity.Position?.ToString();
			ArticleValue = cleanArticleSuffix(_articleItem?.ArtikelNummer);
			DesignationValue = itemEntity.Bezeichnung2;
			DescriptionValue = itemEntity.Bezeichnung1;
			KundenIndexValue = itemEntity.Index_Kunde?.Trim() ?? "";
			OriginalAmountValue = itemEntity.OriginalAnzahl?.ToString();
			RestAmountValue = itemEntity.Geliefert.ToString();
			AmountValue = itemEntity.Anzahl.ToString();
			PEValue = itemEntity.Preiseinheit.ToString();
			UnitValue = itemEntity.Einheit.ToString();
			BasisPrice150Value = $"{Convert.ToDecimal(itemEntity.VKEinzelpreis, System.Globalization.CultureInfo.InvariantCulture).FormatDecimal(2)} €";
			TotalPrice150Value = $"{Convert.ToDecimal(itemEntity.VKGesamtpreis, System.Globalization.CultureInfo.InvariantCulture).FormatDecimal(2)} €";
			Cu_GValue = $"{itemEntity.EinzelCuGewicht?.FormatDecimal(3)} Kg";
			CU_SurchargeValue = $"{Convert.ToDecimal(itemEntity.Einzelkupferzuschlag, System.Globalization.CultureInfo.InvariantCulture).FormatDecimal(2)} €";
			AbladestelleValue = !string.IsNullOrEmpty(itemEntity?.Abladestelle) && !string.IsNullOrWhiteSpace(itemEntity?.Abladestelle) ?
							$"Abladestelle:{itemEntity?.Abladestelle}" : "";
			DELValue = itemEntity.DEL?.ToString();
			CU_TotalValue = $"{Convert.ToDecimal(itemEntity.Gesamtkupferzuschlag, System.Globalization.CultureInfo.InvariantCulture).FormatDecimal(2)} €";
			UnitPriceValue = $"{Convert.ToDecimal(itemEntity.Einzelpreis, System.Globalization.CultureInfo.InvariantCulture).FormatDecimal(2)} €";
			UnitTotalValue = $"{Convert.ToDecimal(itemEntity.Gesamtpreis, System.Globalization.CultureInfo.InvariantCulture).FormatDecimal(2)} €";
			PostextValue = itemEntity.POSTEXT?.ToString()?.Trim();
			lieferterminValue = itemEntity?.Liefertermin.HasValue == true ? $"{itemEntity?.Liefertermin?.ToString("dd.MM.yyyy")}" : "";
			DELFixiertValue = itemEntity?.DELFixiert == true ? "DEL fixiert laut Angebot" : "";
		}
		private static string cleanArticleSuffix(string articlenumber)
		{
			articlenumber = articlenumber.Trim();
			if(string.IsNullOrWhiteSpace(articlenumber) || articlenumber.Length < 2)
			{
				return articlenumber;
			}


			// -
			if(articlenumber.ToLower().EndsWith("al") || articlenumber.ToLower().EndsWith("tn") || articlenumber.ToLower().EndsWith("de"))
			{
				return articlenumber.Substring(0, articlenumber.Length - 2);
			}

			// -
			return articlenumber;
		}
	}
}
