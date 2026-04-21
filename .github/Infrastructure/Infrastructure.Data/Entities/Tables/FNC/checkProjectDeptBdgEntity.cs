using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class checkProjectDeptBdgEntity
	{

		public double? Budget_DEPT { get; set; }
		public string DPT { get; set; }
		public checkProjectDeptBdgEntity() { }

		public checkProjectDeptBdgEntity(DataRow dataRow)
		{
			Budget_DEPT = (dataRow["Budget_DEPT"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["LandBudget"]);
		}
		public checkProjectDeptBdgEntity(float? deptbudget, string dpt)
		{
			this.Budget_DEPT = deptbudget;
			this.DPT = dpt;
		}
	}
}
