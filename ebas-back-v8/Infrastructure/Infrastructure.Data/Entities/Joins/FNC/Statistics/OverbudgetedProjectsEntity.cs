using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Statistics
{
	public class OverbudgetedProjectsEntity
	{
		public int Id { get; set; }
		public string ProjectName { get; set; }
		public decimal? ProjectBudget { get; set; }
		public decimal? OrdersAmount { get; set; }
		public decimal? Diffrence { get; set; }
		public OverbudgetedProjectsEntity(DataRow dataRow)
		{
			Id = (dataRow["Id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Id"]);
			ProjectName = (dataRow["ProjectName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProjectName"]);
			ProjectBudget = (dataRow["ProjectBudget"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProjectBudget"]);
			OrdersAmount = (dataRow["OrdersAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["OrdersAmount"]);
			Diffrence = (dataRow["Diffrence"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Diffrence"]);
		}
	}
}
