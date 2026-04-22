using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class AllocationsVsOrdersAmountEntity
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public int? OrdersCount { get; set; }
		public decimal? ProjectBudget { get; set; }
		public decimal? OrdersAmount { get; set; }
		public AllocationsVsOrdersAmountEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Id"]);
			ProjectName = (dataRow["ProjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectName"]);
			OrdersCount = (dataRow["OrdersCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrdersCount"]);
			ProjectBudget = (dataRow["ProjectBudget"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProjectBudget"]);
			OrdersAmount = (dataRow["OrdersAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OrdersAmount"]);
		}
	}
}