using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class OrdersHighestDelayEntity
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public string Supplier { get; set; }
		public DateTime? DeliveryWishDate { get; set; }
		public DateTime? DeliveryActualDate { get; set; }
		public int Diff { get; set; }
		public OrdersHighestDelayEntity()
		{

		}
		public OrdersHighestDelayEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Id"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			Supplier = (dataRow["Supplier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier"]);
			DeliveryWishDate = (dataRow["DeliveryWishDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeliveryWishDate"]);
			DeliveryActualDate = (dataRow["DeliveryActualDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeliveryActualDate"]);
			Diff = (dataRow["Diff"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Diff"]);
		}
	}
}