using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_usersEntityXXXXX
	{

		public string departement_user { get; set; }
		public string username { get; set; }
		public int ID { get; set; }
		public string land { get; set; }
		public int? U_year { get; set; }
		public decimal? TotalSpent { get; set; }
		public int? LandId { get; set; }
		public int? DepartmentId { get; set; }
		public int? UserId { get; set; }
		public double? budget_month { get; set; }
		public double? budget_order { get; set; }
		public double? budget_year { get; set; }



		public Budget_usersEntityXXXXX() { }

		public Budget_usersEntityXXXXX(DataRow dataRow)
		{

			departement_user = (dataRow["departement_user"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["departement_user"]);
			username = (dataRow["username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["username"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			land = (dataRow["land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["land"]);
			U_year = (dataRow["U_year"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["U_year"]);
			LandId = (dataRow["LandId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LandId"]);
			DepartmentId = (dataRow["DepartmentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DepartmentId"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			TotalSpent = ((dataRow["TotalSpent"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalSpent"])) ?? 0;
			budget_month = (dataRow["budget_month"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["budget_month"]);
			budget_order = (dataRow["budget_order"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["budget_order"]);
			budget_year = (dataRow["budget_year"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["budget_year"]);
		}
	}
}

