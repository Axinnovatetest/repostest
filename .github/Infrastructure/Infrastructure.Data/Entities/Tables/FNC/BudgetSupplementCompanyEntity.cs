using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class BudgetSupplementCompanyEntity
	{
		public decimal AmountInitial { get; set; }
		public string ComapnyName { get; set; }
		public int CompanyId { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int Year { get; set; }

		public BudgetSupplementCompanyEntity() { }

		public BudgetSupplementCompanyEntity(DataRow dataRow)
		{
			AmountInitial = Convert.ToDecimal(dataRow["AmountInitial"]);
			ComapnyName = (dataRow["ComapnyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ComapnyName"]);
			CompanyId = Convert.ToInt32(dataRow["CompanyId"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			Year = Convert.ToInt32(dataRow["Year"]);
		}
	}
}

