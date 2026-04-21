using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class OrdersSlowestBookingEntity
	{
		public int? Id { get; set; }
		public string OrderNumber { get; set; }
		public DateTime? MaxDate { get; set; }
		public DateTime? MinDate { get; set; }
		public int? Diff { get; set; }
		public OrdersSlowestBookingEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			MaxDate = (dataRow["MaxDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MaxDate"]);
			MinDate = (dataRow["MinDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MinDate"]);
			Diff = (dataRow["Diff"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Diff"]);
		}
	}
}