using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class checkDeptBdgEntity
	{
		public double? LandBudget { get; set; }
		public double? SOMME_DEPT { get; set; }
		public string DPT { get; set; }
		public int? Id_DPT { get; set; }

		public double? LandBudgetSupplement { get; set; }
		public checkDeptBdgEntity() { }

		public checkDeptBdgEntity(DataRow dataRow)
		{
			LandBudget = (dataRow["LandBudget"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["LandBudget"]);
			SOMME_DEPT = (dataRow["SOMME_DEPT"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["SOMME_DEPT"]);
			LandBudgetSupplement = (dataRow["LandBudgetSupplement"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["LandBudgetSupplement"]);
			Id_DPT = (dataRow["Id_DPT"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_DPT"]);

		}
		public checkDeptBdgEntity(double? landbudget, float? somme_dept, string dpt, double? supplementLand)
		{
			this.LandBudget = landbudget;
			this.SOMME_DEPT = somme_dept;
			this.DPT = dpt;
			this.LandBudgetSupplement = supplementLand;
		}

		public checkDeptBdgEntity(double? landbudget, double? somme_dept, int? dpt, double? supplementLand)
		{
			this.LandBudget = landbudget;
			this.SOMME_DEPT = somme_dept;
			this.Id_DPT = dpt;
			this.LandBudgetSupplement = supplementLand;
		}
	}
}
