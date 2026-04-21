using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class BestellteArtikelExtensionEntity
	{
		public int? AccountId { get; set; }
		public string AccountName { get; set; }
		public int ArticleId { get; set; }
		public int BestellteArtikelNr { get; set; }
		public DateTime? ConfirmationDate { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public int? DefaultCurrencyDecimals { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public decimal? DefaultCurrencyRate { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public string Description { get; set; }
		public decimal? Discount { get; set; }
		public int Id { get; set; }
		public string InternalContact { get; set; }
		public int? LocationId { get; set; }
		public string LocationName { get; set; }
		public int OrderId { get; set; }
		public decimal? Quantity { get; set; }
		public DateTime? SupplierDeliveryDate { get; set; }
		public string SupplierOrderNumber { get; set; }
		public decimal? TotalCost { get; set; }
		public decimal? TotalCostDefaultCurrency { get; set; }
		public decimal? UnitPrice { get; set; }
		public decimal? UnitPriceDefaultCurrency { get; set; }
		public decimal? VAT { get; set; }

		public BestellteArtikelExtensionEntity() { }

		public BestellteArtikelExtensionEntity(DataRow dataRow)
		{
			AccountId = (dataRow["AccountId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AccountId"]);
			AccountName = (dataRow["AccountName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccountName"]);
			ArticleId = Convert.ToInt32(dataRow["ArticleId"]);
			BestellteArtikelNr = Convert.ToInt32(dataRow["BestellteArtikelNr"]);
			ConfirmationDate = (dataRow["ConfirmationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ConfirmationDate"]);
			CurrencyId = (dataRow["CurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CurrencyId"]);
			CurrencyName = (dataRow["CurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CurrencyName"]);
			DefaultCurrencyDecimals = (dataRow["DefaultCurrencyDecimals"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyDecimals"]);
			DefaultCurrencyId = (dataRow["DefaultCurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyId"]);
			DefaultCurrencyName = (dataRow["DefaultCurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DefaultCurrencyName"]);
			DefaultCurrencyRate = (dataRow["DefaultCurrencyRate"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DefaultCurrencyRate"]);
			DeliveryDate = (dataRow["DeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeliveryDate"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Discount = (dataRow["Discount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Discount"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			InternalContact = (dataRow["InternalContact"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InternalContact"]);
			LocationId = (dataRow["LocationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LocationId"]);
			LocationName = (dataRow["LocationName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LocationName"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]);
			SupplierDeliveryDate = (dataRow["SupplierDeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SupplierDeliveryDate"]);
			SupplierOrderNumber = (dataRow["SupplierOrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierOrderNumber"]);
			TotalCost = (dataRow["TotalCost"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalCost"]);
			TotalCostDefaultCurrency = (dataRow["TotalCostDefaultCurrency"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalCostDefaultCurrency"]);
			UnitPrice = (dataRow["UnitPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["UnitPrice"]);
			UnitPriceDefaultCurrency = (dataRow["UnitPriceDefaultCurrency"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["UnitPriceDefaultCurrency"]);
			VAT = (dataRow["VAT"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VAT"]);
		}
	}
}

