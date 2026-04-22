using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AllDataAssign_budget_landEntity
	{
		public int? B_year { get; set; }
		public double? budget { get; set; }
		public int ID { get; set; }
		public string Land_name { get; set; }
		public int? LandId { get; set; }
		public decimal? TotalSpent { get; set; }
		public double? SommeSupplement { get; set; }
		public double? SommebudgetSupplement { get; set; }
		public double? SommebudgetDept { get; set; }
		public double? NotAssignedBudgetDept { get; set; }

		public AllDataAssign_budget_landEntity() { }

		public AllDataAssign_budget_landEntity(DataRow dataRow)
		{
			B_year = (dataRow["B_year"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["B_year"]);
			budget = (dataRow["budget"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["budget"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			LandId = (dataRow["LandId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LandId"]);
			TotalSpent = (dataRow["TotalSpent"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalSpent"]);
			SommeSupplement = (dataRow["SommeSupplement"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["SommeSupplement"]);
			SommebudgetSupplement = (dataRow["SommebudgetSupplement"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["SommebudgetSupplement"]);
			SommebudgetDept = (dataRow["SommebudgetDept"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["SommebudgetDept"]);
			NotAssignedBudgetDept = (dataRow["NotAssignedBudgetDept"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["NotAssignedBudgetDept"]);

		}
	}
}

