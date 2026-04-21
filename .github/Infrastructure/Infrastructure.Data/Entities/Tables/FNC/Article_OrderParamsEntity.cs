using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Article_OrderParamsEntity
	{

		//param Version Order
		public int? Max_VO { get; set; }
		public int? Nr_version_Order_param { get; set; }
		public int? Id_Level_param { get; set; }
		public int? Id_Status_param { get; set; }
		public int? Id_Dept_param { get; set; }
		public int? Id_Land_param { get; set; }
		public string Dept_name_param { get; set; }
		public string Land_name_param { get; set; }
		public int? Id_Currency_Order_param { get; set; }
		public int? Id_Supplier_VersionOrder_param { get; set; }
		public double? TotalCost_Order_param { get; set; }
		public string Step_Order_param { get; set; }
		public int? Id_Project_param { get; set; }
		public Article_OrderParamsEntity() { }

		public Article_OrderParamsEntity(DataRow dataRow)
		{
			//param Version Order
			Dept_name_param = (dataRow["Dept_name_param"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dept_name_param"]);
			Id_Currency_Order_param = (dataRow["Id_Currency_Order_param"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency_Order_param"]);
			Id_Dept_param = (dataRow["Id_Dept_param"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept_param"]);
			Id_Land_param = (dataRow["Id_Land_param"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Land_param"]);
			Id_Level_param = (dataRow["Id_Level_param"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Level_param"]);
			Id_Project_param = (dataRow["Id_Project_param"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Project_param"]);
			Id_Status_param = (dataRow["Id_Status_param"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Status_param"]);
			Id_Supplier_VersionOrder_param = (dataRow["Id_Supplier_VersionOrder_param"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Supplier_VersionOrder_param"]);
			Max_VO = (dataRow["Max_VO"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Max_VO"]);
			Land_name_param = (dataRow["Land_name_param"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name_param"]);
			Nr_version_Order_param = (dataRow["Nr_version_Order_param"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr_version_Order_param"]);
			Step_Order_param = (dataRow["Step_Order_param"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Step_Order_param"]);
			TotalCost_Order_param = (dataRow["TotalCost_Order_param"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["TotalCost_Order_param"]);
		}


	}
}

