using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_OrderInsertedEntity
	{
		public string order_index => Id_Order + "_" + Type_Order;
		public string Dept_name { get; set; }
		public int? Id_Currency_Order { get; set; }
		public int? Id_Dept { get; set; }
		public int? Id_Land { get; set; }
		public int Id_Order { get; set; }
		public int? Id_Project { get; set; }
		public int? Id_Supplier { get; set; }


		public int Id_User { get; set; }
		public string Land_name { get; set; }
		public DateTime? Order_date { get; set; }
		public string Order_Number { get; set; }
		public string Type_Order { get; set; }


		public int? Id_Level { get; set; }

		public int? Id_Status { get; set; }
		public int? Id_Supplier_VersionOrder { get; set; }

		public int Id_VO { get; set; }

		public int? Nr_version_Order { get; set; }
		public string Step_Order { get; set; }
		public double? TotalCost_Order { get; set; }
		public DateTime? Version_Order_date { get; set; }

		public int Id_Diverse_Supplier_Order { get; set; }
		public int? Id_Order_Diverse { get; set; }
		public int? Id_Supplier_Order_Diverse { get; set; }
		public int? Lieferantennummer_Order_Diverse { get; set; }
		public string Ort_Order_Supplier_Diverse { get; set; }
		public string Supplier_Contact_Description_Order_Diverse { get; set; }
		public string Supplier_Contact_Order_Diverse { get; set; }
		public string Supplier_Name_Order_Diverse { get; set; }

		public Budget_OrderInsertedEntity() { }

		public Budget_OrderInsertedEntity(DataRow dataRow)
		{
			Dept_name = (dataRow["Dept_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dept_name"]);
			Id_Currency_Order = (dataRow["Id_Currency_Order"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency_Order"]);
			Id_Dept = (dataRow["Id_Dept"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept"]);
			Id_Land = (dataRow["Id_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Land"]);
			Id_Order = Convert.ToInt32(dataRow["Id_Order"]);
			Id_Project = (dataRow["Id_Project"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Project"]);
			Id_Supplier = (dataRow["Id_Supplier"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Supplier"]);
			Id_User = Convert.ToInt32(dataRow["Id_User"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			Order_date = (dataRow["Order_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Order_date"]);
			Order_Number = (dataRow["Order_Number"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Order_Number"]);
			//order_index = (dataRow["order_index"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["order_index"]);
			Type_Order = (dataRow["Type_Order"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type_Order"]);

			Id_Level = (dataRow["Id_Level"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Level"]);
			Id_Status = (dataRow["Id_Status"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Status"]);
			Id_Supplier_VersionOrder = (dataRow["Id_Supplier_VersionOrder"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Supplier_VersionOrder"]);
			Id_VO = Convert.ToInt32(dataRow["Id_VO"]);
			Nr_version_Order = (dataRow["Nr_version_Order"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr_version_Order"]);
			Step_Order = (dataRow["Step_Order"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Step_Order"]);
			TotalCost_Order = (dataRow["TotalCost_Order"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["TotalCost_Order"]);
			Version_Order_date = (dataRow["Version_Order_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Version_Order_date"]);

			Id_Diverse_Supplier_Order = Convert.ToInt32(dataRow["Id_Diverse_Supplier_Order"]);
			Id_Order_Diverse = (dataRow["Id_Order_Diverse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Order_Diverse"]);
			Id_Supplier_Order_Diverse = (dataRow["Id_Supplier_Order_Diverse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Supplier_Order_Diverse"]);
			Lieferantennummer_Order_Diverse = (dataRow["Lieferantennummer_Order_Diverse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferantennummer_Order_Diverse"]);
			Ort_Order_Supplier_Diverse = (dataRow["Ort_Order_Supplier_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort_Order_Supplier_Diverse"]);
			Supplier_Contact_Description_Order_Diverse = (dataRow["Supplier_Contact_Description_Order_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier_Contact_Description_Order_Diverse"]);
			Supplier_Contact_Order_Diverse = (dataRow["Supplier_Contact_Order_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier_Contact_Order_Diverse"]);
			Supplier_Name_Order_Diverse = (dataRow["Supplier_Name_Order_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier_Name_Order_Diverse"]);
		}
	}
}

