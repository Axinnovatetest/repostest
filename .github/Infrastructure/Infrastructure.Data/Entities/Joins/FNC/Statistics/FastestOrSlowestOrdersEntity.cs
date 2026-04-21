using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class FastestOrSlowestOrdersEntity
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public DateTime? ValidationRequestTime { get; set; }
		public DateTime? Termin { get; set; }
		public int? Diff { get; set; }
		public FastestOrSlowestOrdersEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Id"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			ValidationRequestTime = (dataRow["ValidationRequestTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidationRequestTime"]);
			Termin = (dataRow["Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin"]);
			Diff = (dataRow["Diff"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Diff"]);
		}
	}
}