using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class CheckUserBdgEntity
	{
		public double? DeptBudget { get; set; }
		public double? SOMME_USERS { get; set; }
		public string USR { get; set; }
		public CheckUserBdgEntity() { }

		public CheckUserBdgEntity(DataRow dataRow)
		{
			DeptBudget = (dataRow["DeptBudget"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["DeptBudget"]);
			SOMME_USERS = (dataRow["SOMME_USERS"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["SOMME_USERS"]);
		}
		public CheckUserBdgEntity(double? landbudget, double? somme_dept, string dpt)
		{
			this.DeptBudget = landbudget;
			this.SOMME_USERS = somme_dept;
			this.USR = dpt;
		}
	}
}
