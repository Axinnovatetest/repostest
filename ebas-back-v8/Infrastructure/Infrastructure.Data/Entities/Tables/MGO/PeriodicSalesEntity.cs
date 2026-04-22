using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Statistics.MGO
{
	public class PeriodicSalesEntity
	{
		public int Year { get; set; }
		public int KW { get; set; }
		public decimal? InvoiceAmount { get; set; }
		public decimal? StockFGWoUBGAmount { get; set; }
		public decimal? StockFGUBGAmount { get; set; }
		public decimal? StockROHAmount { get; set; }
		public decimal? OrderAmount { get; set; }
		public decimal? ProductionOrderFinishedAmount { get; set; }
		public decimal? ProductionOrderFinishedHours { get; set; }
		public decimal? ProductionOrderPlannedAmount { get; set; }
		public decimal? ProductionOrderPlannedHours { get; set; }

		public PeriodicSalesEntity() { }
		public PeriodicSalesEntity(DataRow dataRow)
		{
			Year = Convert.ToInt32(dataRow["Year"]);
			KW = Convert.ToInt32(dataRow["KW"]);
			InvoiceAmount = (dataRow["InvoiceAmount"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["InvoiceAmount"]);
			StockFGWoUBGAmount = (dataRow["StockFGWoUBGAmount"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["StockFGWoUBGAmount"]);
			StockFGUBGAmount = (dataRow["StockFGUBGAmount"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["StockFGUBGAmount"]);
			StockROHAmount = (dataRow["StockROHAmount"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["StockROHAmount"]);
			OrderAmount = (dataRow["OrderAmount"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OrderAmount"]);
			ProductionOrderFinishedAmount = (dataRow["ProductionOrderFinishedAmount"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionOrderFinishedAmount"]);
			ProductionOrderFinishedHours = (dataRow["ProductionOrderFinishedHours"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionOrderFinishedHours"]);
			ProductionOrderPlannedAmount = (dataRow["ProductionOrderPlannedAmount"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionOrderPlannedAmount"]);
			ProductionOrderPlannedHours = (dataRow["ProductionOrderPlannedHours"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionOrderPlannedHours"]);
		}
	}
}