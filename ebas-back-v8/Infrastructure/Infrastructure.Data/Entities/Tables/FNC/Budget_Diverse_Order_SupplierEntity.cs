using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_Diverse_Order_SupplierEntity
	{
		public int Id_Diverse_Supplier_Order { get; set; }
		public int? Id_Order_Diverse { get; set; }
		public int? Id_Supplier_Order_Diverse { get; set; }
		public int? Lieferantennummer_Order_Diverse { get; set; }
		public string Ort_Order_Supplier_Diverse { get; set; }
		public string Supplier_Contact_Description_Order_Diverse { get; set; }
		public string Supplier_Contact_Order_Diverse { get; set; }
		public string Supplier_Name_Order_Diverse { get; set; }

		public Budget_Diverse_Order_SupplierEntity() { }

		public Budget_Diverse_Order_SupplierEntity(DataRow dataRow)
		{
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

