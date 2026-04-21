using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class BookingCountForOneOrderEntity
	{
		public int? Id { get; set; }
		public string OrderNumber { get; set; }
		public string Supplier { get; set; }
		public int? BookingCount { get; set; }
		public BookingCountForOneOrderEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			Supplier = (dataRow["Supplier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier"]);
			BookingCount = (dataRow["BookingCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BookingCount"]);
		}
	}
}