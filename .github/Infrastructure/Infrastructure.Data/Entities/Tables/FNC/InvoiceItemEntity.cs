using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class InvoiceItemEntity
	{
		public string AB_Nr_Lieferant { get; set; }
		public int? AccountId { get; set; }
		public string AccountName { get; set; }
		public decimal? Aktuelle_Anzahl { get; set; }
		public decimal? AnfangLagerBestand { get; set; }
		public decimal? Anzahl { get; set; }
		public int ArticleId { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Bemerkung_Pos { get; set; }
		public bool? Bemerkung_Pos_ID { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public string Bestellnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public int? BP_zu_RBposition { get; set; }
		public bool? COC_bestätigung { get; set; }
		public DateTime? ConfirmationDate { get; set; }
		public decimal? CUPreis { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public int? DefaultCurrencyDecimals { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public decimal? DefaultCurrencyRate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public string Description { get; set; }
		public decimal? Discount { get; set; }
		public string Einheit { get; set; }
		public decimal? Einzelpreis { get; set; }
		public bool? EMPB_Bestätigung { get; set; }
		public decimal? EndeLagerBestand { get; set; }
		public decimal? Erhalten { get; set; }
		public bool? erledigt_pos { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public int Id { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public string InfoRahmennummer { get; set; }
		public string InternalContact { get; set; }
		public int? InvoiceId { get; set; }
		public bool? Kanban { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Liefertermin { get; set; }
		public int? LocationId { get; set; }
		public string LocationName { get; set; }
		public bool? Löschen { get; set; }
		public DateTime? MhdDatumArtikel { get; set; }
		public int OrderId { get; set; }
		public int? Position { get; set; }
		public bool? Position_erledigt { get; set; }
		public decimal? Preiseinheit { get; set; }
		public int? Preisgruppe { get; set; }
		public int? Produktionsort { get; set; }
		public decimal? Quantity { get; set; }
		public double? Rabatt { get; set; }
		public double? Rabatt1 { get; set; }
		public decimal? RB_Abgerufen { get; set; }
		public decimal? RB_Offen { get; set; }
		public decimal? RB_OriginalAnzahl { get; set; }
		public string schriftart { get; set; }
		public string sortierung { get; set; }
		public decimal? Start_Anzahl { get; set; }
		public DateTime? SupplierDeliveryDate { get; set; }
		public string SupplierOrderNumber { get; set; }
		public decimal? TotalCost { get; set; }
		public decimal? TotalCostDefaultCurrency { get; set; }
		public double? Umsatzsteuer { get; set; }
		public decimal? UnitPrice { get; set; }
		public decimal? UnitPriceDefaultCurrency { get; set; }
		public decimal? VAT { get; set; }
		public int? WE_Pos_zu_Bestellposition { get; set; }

		public InvoiceItemEntity() { }

		public InvoiceItemEntity(DataRow dataRow)
		{
			AB_Nr_Lieferant = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
			AccountId = (dataRow["AccountId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AccountId"]);
			AccountName = (dataRow["AccountName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccountName"]);
			Aktuelle_Anzahl = (dataRow["Aktuelle Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aktuelle Anzahl"]);
			AnfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			ArticleId = Convert.ToInt32(dataRow["ArticleId"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bemerkung_Pos = (dataRow["Bemerkung_Pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Pos"]);
			Bemerkung_Pos_ID = (dataRow["Bemerkung_Pos_ID"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bemerkung_Pos_ID"]);
			Bestatigter_Termin = (dataRow["Bestatigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestatigter_Termin"]);
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			BP_zu_RBposition = (dataRow["BP zu RBposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BP zu RBposition"]);
			COC_bestätigung = (dataRow["COC_bestätigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COC_bestätigung"]);
			ConfirmationDate = (dataRow["ConfirmationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ConfirmationDate"]);
			CUPreis = (dataRow["CUPreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CUPreis"]);
			CurrencyId = (dataRow["CurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CurrencyId"]);
			CurrencyName = (dataRow["CurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CurrencyName"]);
			DefaultCurrencyDecimals = (dataRow["DefaultCurrencyDecimals"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyDecimals"]);
			DefaultCurrencyId = (dataRow["DefaultCurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyId"]);
			DefaultCurrencyName = (dataRow["DefaultCurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DefaultCurrencyName"]);
			DefaultCurrencyRate = (dataRow["DefaultCurrencyRate"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DefaultCurrencyRate"]);
			DeliveryDate = (dataRow["DeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeliveryDate"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Discount = (dataRow["Discount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Discount"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelpreis"]);
			EMPB_Bestätigung = (dataRow["EMPB_Bestätigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Bestätigung"]);
			EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
			Erhalten = (dataRow["Erhalten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Erhalten"]);
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
			InfoRahmennummer = (dataRow["InfoRahmennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InfoRahmennummer"]);
			InternalContact = (dataRow["InternalContact"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InternalContact"]);
			InvoiceId = (dataRow["InvoiceId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["InvoiceId"]);
			Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			LocationId = (dataRow["LocationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LocationId"]);
			LocationName = (dataRow["LocationName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LocationName"]);
			Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			MhdDatumArtikel = (dataRow["MhdDatumArtikel"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MhdDatumArtikel"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Position_erledigt = (dataRow["Position erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Position erledigt"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			Produktionsort = (dataRow["Produktionsort"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionsort"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]);
			Rabatt = (dataRow["Rabatt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt"]);
			Rabatt1 = (dataRow["Rabatt1"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Rabatt1"]);
			RB_Abgerufen = (dataRow["RB_Abgerufen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Abgerufen"]);
			RB_Offen = (dataRow["RB_Offen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Offen"]);
			RB_OriginalAnzahl = (dataRow["RB_OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_OriginalAnzahl"]);
			schriftart = (dataRow["schriftart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["schriftart"]);
			sortierung = (dataRow["sortierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["sortierung"]);
			Start_Anzahl = (dataRow["Start Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Start Anzahl"]);
			SupplierDeliveryDate = (dataRow["SupplierDeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SupplierDeliveryDate"]);
			SupplierOrderNumber = (dataRow["SupplierOrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierOrderNumber"]);
			TotalCost = (dataRow["TotalCost"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalCost"]);
			TotalCostDefaultCurrency = (dataRow["TotalCostDefaultCurrency"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalCostDefaultCurrency"]);
			Umsatzsteuer = (dataRow["Umsatzsteuer"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Umsatzsteuer"]);
			UnitPrice = (dataRow["UnitPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["UnitPrice"]);
			UnitPriceDefaultCurrency = (dataRow["UnitPriceDefaultCurrency"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["UnitPriceDefaultCurrency"]);
			VAT = (dataRow["VAT"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VAT"]);
			WE_Pos_zu_Bestellposition = (dataRow["WE Pos zu Bestellposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WE Pos zu Bestellposition"]);
		}
	}
}

