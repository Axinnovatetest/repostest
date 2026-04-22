using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class OrderLeasingFeesHistoryEntity
	{
		public int? DefaultCurrencyDecimals { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public decimal? DefaultCurrencyRate { get; set; }
		public int Id { get; set; }
		public DateTime InsertTime { get; set; }
		public int OrderArticleCount { get; set; }
		public int OrderId { get; set; }
		public int OrderIssuerId { get; set; }
		public decimal OrderLeasingMonthAmount { get; set; }
		public decimal OrderLeasingMonthAmountDefaultCurrency { get; set; }
		public int? OrderLeasingYear { get; set; }
		public decimal? OrderLeasingYearTotalAmount { get; set; }
		public decimal? OrderLeasingYearTotalAmountDefaultCurrency { get; set; }
		public int? OrderLeasingYearTotalMonths { get; set; }
		public int OrderProjectId { get; set; }
		public decimal OrderTotalAmount { get; set; }
		public decimal OrderTotalAmountDefaultCurrency { get; set; }
		public string OrderType { get; set; }
		public int UserId { get; set; }

		public OrderLeasingFeesHistoryEntity() { }

		public OrderLeasingFeesHistoryEntity(DataRow dataRow)
		{
			DefaultCurrencyDecimals = (dataRow["DefaultCurrencyDecimals"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyDecimals"]);
			DefaultCurrencyId = (dataRow["DefaultCurrencyId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DefaultCurrencyId"]);
			DefaultCurrencyName = (dataRow["DefaultCurrencyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DefaultCurrencyName"]);
			DefaultCurrencyRate = (dataRow["DefaultCurrencyRate"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DefaultCurrencyRate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			InsertTime = Convert.ToDateTime(dataRow["InsertTime"]);
			OrderArticleCount = Convert.ToInt32(dataRow["OrderArticleCount"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderIssuerId = Convert.ToInt32(dataRow["OrderIssuerId"]);
			OrderLeasingMonthAmount = Convert.ToDecimal(dataRow["OrderLeasingMonthAmount"]);
			OrderLeasingMonthAmountDefaultCurrency = Convert.ToDecimal(dataRow["OrderLeasingMonthAmountDefaultCurrency"]);
			OrderLeasingYear = (dataRow["OrderLeasingYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderLeasingYear"]);
			OrderLeasingYearTotalAmount = (dataRow["OrderLeasingYearTotalAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OrderLeasingYearTotalAmount"]);
			OrderLeasingYearTotalAmountDefaultCurrency = (dataRow["OrderLeasingYearTotalAmountDefaultCurrency"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OrderLeasingYearTotalAmountDefaultCurrency"]);
			OrderLeasingYearTotalMonths = (dataRow["OrderLeasingYearTotalMonths"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderLeasingYearTotalMonths"]);
			OrderProjectId = Convert.ToInt32(dataRow["OrderProjectId"]);
			OrderTotalAmount = Convert.ToDecimal(dataRow["OrderTotalAmount"]);
			OrderTotalAmountDefaultCurrency = Convert.ToDecimal(dataRow["OrderTotalAmountDefaultCurrency"]);
			OrderType = Convert.ToString(dataRow["OrderType"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
		}
	}
}

