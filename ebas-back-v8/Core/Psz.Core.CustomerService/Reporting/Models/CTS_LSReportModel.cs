using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class CTS_LSReportModel
	{
		public string Logo { get; set; }
		public string Top100Logo { get; set; }
		public string Top100Description { get; set; }
		public string Top100_2026Logo { get; set; }
		//Delivery adress
		public string LAnrede { get; set; }
		public string LVorname { get; set; }
		public string LName2 { get; set; }
		public string LName3 { get; set; }
		public string Lansprechpartner { get; set; }
		public string Labteilung { get; set; }
		public string LStrabe { get; set; }
		public string LLand { get; set; }
		public string LBriefanrede { get; set; }
		//Adress
		public string Address1 { get; set; }
		public string Name2 { get; set; }
		public string Address2 { get; set; }
		public string Unser_Zeichen1 { get; set; }
		public string Address3 { get; set; }
		//Others
		public string Textbausteine_LS { get; set; }
		public string DocumentType { get; set; }
		public string OrderNumberTitle { get; set; }
		public string OrderNumberValue { get; set; }
		public string OrderNumberPOTitle { get; set; }
		public string OrderNumberPOValue { get; set; }
		public string Barcode { get; set; }
		public string BarcodeDocumentNumber { get; set; }
		public string ClientNumberTitle { get; set; }
		public string ClientNumberValue { get; set; }
		public string OrderDateValue { get; set; }
		public string OrderDateTitle { get; set; }
		public string ShippingMethodTitle { get; set; }
		public string ShippingMethodValue { get; set; }
		public string Freitext { get; set; }
		public string ItemsHeaderTitle { get; set; }
		public string ItemsHeaderValue { get; set; }
		public string InvoiceHeader { get; set; }
		public string AbladestelleValue { get; set; }
		public string Text { get; set; }
		public string LastPageText4 { get; set; }
		public string LastPageText5 { get; set; }
		public string LastPageText10 { get; set; }
		//Columns
		public string PositionTitle { get; set; }
		public string ArticleTitle { get; set; }
		public string CustomerDateTitle { get; set; }
		public string Designation1Title { get; set; }
		public string CustomerNumberTitle { get; set; }
		public string ArtikelCountryTitle { get; set; }
		public string Designation2Title { get; set; }
		public string Footer24Title { get; set; }
		public string ArtikelPriceTitle { get; set; }
		public string ArtikelStockTitle { get; set; }
		public string ArtikelWeightTitle { get; set; }
		public string ArtikelQuantityTitle { get; set; }
		//Summaries
		public string SummarySumTitle { get; set; }
		public string SummaryUSTTitle { get; set; }
		public string SummaryTotalTitle { get; set; }
		public string SummaryWeightValue { get; set; }
		public List<CTC_LSReportItemsModel> Items { get; set; }
		// - 2024-02-08 - QrCodes for LS-Nummer, LS-Datum & PO-Nummer
		public string QrCodeDocLsNumberDatum { get; set; }
		public CTS_LSReportModel()
		{

		}
		public CTS_LSReportModel(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity reportEntity, Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity orderEntity)
		{
			var TextbausteineEntity = Infrastructure.Data.Access.Tables.STG.Textbausteine_AB_LS_RG_GU_BAccess.Get();
			LAnrede = orderEntity?.LAnrede;
			LVorname = orderEntity?.LVorname_NameFirma;
			LName2 = orderEntity?.LName2;
			LName3 = orderEntity?.LName3;
			Lansprechpartner = orderEntity?.LAnsprechpartner;
			Labteilung = orderEntity?.LAbteilung;
			LStrabe = orderEntity?.LStraße_Postfach;
			LLand = orderEntity?.LLand_PLZ_Ort;
			LBriefanrede = orderEntity?.LBriefanrede;
			//
			Address1 = orderEntity?.Anrede?.Trim();
			Address2 = orderEntity?.Vorname_NameFirma?.Trim();
			Name2 = orderEntity?.Name2?.Trim();
			//Unser_Zeichen1=
			Textbausteine_LS = TextbausteineEntity[0]?.Lieferschein;
			DocumentType = orderEntity?.Typ;
			OrderNumberTitle = reportEntity.OrderNumber;
			OrderNumberValue = orderEntity?.Angebot_Nr?.ToString();
			//Barcode=
			ClientNumberTitle = reportEntity?.ClientNumber;
			ClientNumberValue = orderEntity?.Ihr_Zeichen;
			OrderDateTitle = reportEntity?.OrderDate;
			OrderDateValue = orderEntity?.Datum?.ToString("dd.MM.yyyy");
			ShippingMethodTitle = reportEntity?.ShippingMethod;
			ShippingMethodValue = orderEntity?.Versandart;
			Freitext = orderEntity?.Freitext;
			ItemsHeaderTitle = reportEntity?.ItemsHeader;
			ItemsHeaderValue = "";
			InvoiceHeader = reportEntity?.Header;
			//AbladestelleValue=
			//
			Text = (orderEntity.USt_Berechnen.HasValue && !orderEntity.USt_Berechnen.Value) ? "DIESE LIEFERUNG IST GEM. §4 USTG STEUERFREI" : "";
			LastPageText4 = reportEntity?.LastPageText4;
			LastPageText5 = reportEntity?.LastPageText5;
			LastPageText10 = reportEntity?.LastPageText10;
			//
			PositionTitle = reportEntity?.Position;
			ArticleTitle = reportEntity?.Article;
			CustomerDateTitle = reportEntity?.CustomerDate;
			Designation1Title = reportEntity?.Designation1;
			CustomerNumberTitle = reportEntity?.CustomerNumber;
			ArtikelCountryTitle = reportEntity?.ArtikelCountry;
			Designation2Title = reportEntity?.Designation2;
			Footer24Title = reportEntity?.Footer24;
			ArtikelPriceTitle = reportEntity?.ArtikelPrice;
			ArtikelStockTitle = reportEntity?.ArtikelStock;
			ArtikelWeightTitle = reportEntity?.ArtikelWeight;
			ArtikelQuantityTitle = reportEntity?.ArtikelQuantity;
			OrderNumberPOTitle = reportEntity?.OrderNumberPO;
			OrderNumberPOValue = orderEntity?.Bezug.ToString();
			//
			SummarySumTitle = reportEntity.SummarySum;
			SummaryUSTTitle = reportEntity.SummaryUST;
			SummaryTotalTitle = reportEntity.SummaryTotal;
		}
	}
	public class CTC_LSReportItemsModel
	{
		public string Postext { get; set; }
		public string PositionValue { get; set; }
		public string ArticleValue { get; set; }
		public string CustomerDateValue { get; set; }
		public string Designation1Value { get; set; }
		public string CustomerNumberValue { get; set; }
		public string ArtikelCountryValue { get; set; }
		public string Designation2Value { get; set; }
		public string ZolltarifnummerValue { get; set; }
		public string ArtikelPriceValue { get; set; }
		public string ArtikelStockValue { get; set; }
		public string ArtikelWeightValue { get; set; }
		public string ArtikelQuantityValue { get; set; }
		public string DeliveryNoteCustomerComments { get; set; }
		public CTC_LSReportItemsModel()
		{

		}
		public CTC_LSReportItemsModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity itemEntity)
		{
			var _articleItem = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(itemEntity.ArtikelNr ?? -1);

			Postext = itemEntity?.POSTEXT;
			PositionValue = itemEntity?.Position?.ToString();
			ArticleValue = cleanArticleSuffix(_articleItem?.ArtikelNummer);
			CustomerDateValue = $"{_articleItem?.Index_Kunde_Datum?.ToString("dd.MM.yyyy")}";
			Designation1Value = itemEntity?.Bezeichnung1?.ToString()?.Trim();
			CustomerNumberValue = _articleItem?.Index_Kunde?.ToString();
			ArtikelCountryValue = _articleItem?.Ursprungsland?.ToString()?.Trim();
			Designation2Value = itemEntity?.Bezeichnung2?.ToString()?.Trim();
			ZolltarifnummerValue = _articleItem?.Zolltarif_nr;
			ArtikelPriceValue = itemEntity?.Einheit?.ToString();
			ArtikelStockValue = Math.Round(((_articleItem.Größe ?? 0) / 1000), 2).ToString("0.00");
			ArtikelWeightValue = Math.Round(((itemEntity.Anzahl ?? 0) * (_articleItem.Größe ?? 0) / 1000), 2).ToString();
			ArtikelQuantityValue = $"{itemEntity?.Anzahl}";
			DeliveryNoteCustomerComments = _articleItem?.DeliveryNoteCustomerComments;
		}
		private static string cleanArticleSuffix(string articlenumber)
		{
			articlenumber = articlenumber.Trim();
			if(string.IsNullOrWhiteSpace(articlenumber) || articlenumber.Length < 2)
			{
				return articlenumber;
			}
			if(articlenumber.ToLower().EndsWith("al") || articlenumber.ToLower().EndsWith("tn") || articlenumber.ToLower().EndsWith("de"))
			{
				return articlenumber.Substring(0, articlenumber.Length - 2);
			}
			return articlenumber;
		}
	}
}