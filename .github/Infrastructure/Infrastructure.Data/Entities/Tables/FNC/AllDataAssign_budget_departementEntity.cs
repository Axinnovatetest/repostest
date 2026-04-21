using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AllDataAssign_budget_departementEntity
	{
		public int? B_year { get; set; }
		public double? budget { get; set; }
		public int ID { get; set; }
		public string Land_name { get; set; }
		public string Departement_name { get; set; }
		public double? SommebudgetDept { get; set; }
		public double? SommebudgetUser { get; set; }
		public double? NotAssignedBudgetUser { get; set; }
		public int? LandId { get; set; }
		public int? DepartmentId { get; set; }
		public decimal? TotalSpent { get; set; }
		public AllDataAssign_budget_departementEntity() { }

		public AllDataAssign_budget_departementEntity(DataRow dataRow)
		{
			B_year = (dataRow["B_year"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["B_year"]);
			budget = (dataRow["budget"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["budget"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			Departement_name = (dataRow["Departement_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Departement_name"]);
			SommebudgetDept = (dataRow["SommebudgetDept"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["SommebudgetDept"]);
			SommebudgetUser = (dataRow["SommebudgetUser"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["SommebudgetUser"]);
			NotAssignedBudgetUser = (dataRow["NotAssignedBudgetUser"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["NotAssignedBudgetUser"]);
			DepartmentId = (dataRow["DepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DepartmentId"]);
			LandId = (dataRow["LandId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LandId"]);
			TotalSpent = ((dataRow["TotalSpent"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalSpent"])) ?? 0;


		}
	}
}

