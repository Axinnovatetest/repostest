using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_Article_VersionEntity
	{
		public string Action_Version_Article { get; set; }
		public string Currency_Version_Article { get; set; }
		public string Dept_name_VersionArticle { get; set; }
		public int Id_AOV { get; set; }
		public int Id_Article { get; set; }
		public int? Id_Currency_Version_Article { get; set; }
		public int? Id_Dept_VersionArticle { get; set; }
		public int? Id_Land_VersionArticle { get; set; }
		public int? Id_Level_VersionArticle { get; set; }
		public int Id_Order_Version { get; set; }
		public int? Id_Project_VersionArticle { get; set; }
		public int? Id_Status_VersionArticle { get; set; }
		public int? Id_Supplier_VersionArticle { get; set; }
		public int Id_User_VersionArticle { get; set; }
		public string Land_name_VersionArticle { get; set; }
		public decimal? Quantity_VersionArticle { get; set; }
		public double? TotalCost__VersionArticle { get; set; }
		public double? Unit_Price_VersionArticle { get; set; }
		public DateTime? Version_Article_date { get; set; }

		public Budget_Article_VersionEntity() { }

		public Budget_Article_VersionEntity(DataRow dataRow)
		{
			Action_Version_Article = (dataRow["Action_Version_Article"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Action_Version_Article"]);
			Currency_Version_Article = (dataRow["Currency_Version_Article"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Currency_Version_Article"]);
			Dept_name_VersionArticle = (dataRow["Dept_name_VersionArticle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dept_name_VersionArticle"]);
			Id_AOV = Convert.ToInt32(dataRow["Id_AOV"]);
			Id_Article = Convert.ToInt32(dataRow["Id_Article"]);
			Id_Currency_Version_Article = (dataRow["Id_Currency_Version_Article"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency_Version_Article"]);
			Id_Dept_VersionArticle = (dataRow["Id_Dept_VersionArticle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept_VersionArticle"]);
			Id_Land_VersionArticle = (dataRow["Id_Land_VersionArticle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Land_VersionArticle"]);
			Id_Level_VersionArticle = (dataRow["Id_Level_VersionArticle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Level_VersionArticle"]);
			Id_Order_Version = Convert.ToInt32(dataRow["Id_Order_Version"]);
			Id_Project_VersionArticle = (dataRow["Id_Project_VersionArticle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Project_VersionArticle"]);
			Id_Status_VersionArticle = (dataRow["Id_Status_VersionArticle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Status_VersionArticle"]);
			Id_Supplier_VersionArticle = (dataRow["Id_Supplier_VersionArticle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Supplier_VersionArticle"]);
			Id_User_VersionArticle = Convert.ToInt32(dataRow["Id_User_VersionArticle"]);
			Land_name_VersionArticle = (dataRow["Land_name_VersionArticle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name_VersionArticle"]);
			Quantity_VersionArticle = (dataRow["Quantity_VersionArticle"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity_VersionArticle"]);
			TotalCost__VersionArticle = (dataRow["TotalCost__VersionArticle"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["TotalCost__VersionArticle"]);
			Unit_Price_VersionArticle = (dataRow["Unit_Price_VersionArticle"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Unit_Price_VersionArticle"]);
			Version_Article_date = (dataRow["Version_Article_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Version_Article_date"]);
		}
	}
}

