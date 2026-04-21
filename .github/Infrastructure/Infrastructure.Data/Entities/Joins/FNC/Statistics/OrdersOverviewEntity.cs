using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class OrdersOverviewEntity
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public string SupplierName { get; set; }
		public decimal Amount { get; set; }
		public string PoPaymentTypeName { get; set; }
		public string OrderType { get; set; }
		public int Status { get; set; }
		public OrdersOverviewEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Id"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			SupplierName = (dataRow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierName"]);
			Amount = (dataRow["Amount"] == System.DBNull.Value) ? 0m : Convert.ToDecimal(dataRow["Amount"]);
			PoPaymentTypeName = (dataRow["PoPaymentTypeName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PoPaymentTypeName"]);
			OrderType = (dataRow["OrderType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderType"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Status"]);
		}
	}
}