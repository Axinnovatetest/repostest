using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_Order_VersionEntity
	{
		public string Dept_name { get; set; }
		public int? Id_Currency_Order { get; set; }
		public int? Id_Dept { get; set; }
		public int? Id_Land { get; set; }
		public int? Id_Level { get; set; }
		public int Id_Order { get; set; }
		public int? Id_Project { get; set; }
		public int? Id_Status { get; set; }
		public int? Id_Supplier_VersionOrder { get; set; }
		public int Id_User { get; set; }
		public int Id_VO { get; set; }
		public string Land_name { get; set; }
		public int? Nr_version_Order { get; set; }
		public string Step_Order { get; set; }
		public double? TotalCost_Order { get; set; }
		public DateTime? Version_Order_date { get; set; }

		public Budget_Order_VersionEntity() { }

		public Budget_Order_VersionEntity(DataRow dataRow)
		{
			Dept_name = (dataRow["Dept_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dept_name"]);
			Id_Currency_Order = (dataRow["Id_Currency_Order"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency_Order"]);
			Id_Dept = (dataRow["Id_Dept"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept"]);
			Id_Land = (dataRow["Id_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Land"]);
			Id_Level = (dataRow["Id_Level"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Level"]);
			Id_Order = Convert.ToInt32(dataRow["Id_Order"]);
			Id_Project = (dataRow["Id_Project"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Project"]);
			Id_Status = (dataRow["Id_Status"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Status"]);
			Id_Supplier_VersionOrder = (dataRow["Id_Supplier_VersionOrder"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Supplier_VersionOrder"]);
			Id_User = Convert.ToInt32(dataRow["Id_User"]);
			Id_VO = Convert.ToInt32(dataRow["Id_VO"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			Nr_version_Order = (dataRow["Nr_version_Order"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr_version_Order"]);
			Step_Order = (dataRow["Step_Order"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Step_Order"]);
			TotalCost_Order = (dataRow["TotalCost_Order"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["TotalCost_Order"]);
			Version_Order_date = (dataRow["Version_Order_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Version_Order_date"]);
		}
	}
}

