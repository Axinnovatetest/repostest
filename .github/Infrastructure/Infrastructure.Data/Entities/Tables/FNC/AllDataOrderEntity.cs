using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AllDataOrderEntity
	{

		public int Id_Order { get; set; }
		public string Order_Number { get; set; }
		public string Type_Order { get; set; }
		public int? Id_Project { get; set; }
		//public int? Id_proj { get; set; }
		public int Id_Type { get; set; }
		public string Type_Project { get; set; }
		public int? Id_Currency_Order { get; set; }
		public string Symol { get; set; }
		public string Name_proj { get; set; }
		public int? Id_Dept { get; set; }
		public string Dept_name { get; set; }
		public int? Id_Land { get; set; }
		public string Land_name { get; set; }
		public string Description { get; set; }
		public double Proj_Budget { get; set; }
		//public double Proj_Rest_Budget { get; set; }
		public int? Id_Customer { get; set; }
		public string Customer_Name { get; set; }
		public int? Kundennummer { get; set; }
		public string Ort { get; set; }
		public string Nr { get; set; }
		public string Nr_Customer { get; set; }
		//public string Customer_Contact { get; set; }
		//public string Customer_Contact_Description { get; set; }
		public int Id_Responsable { get; set; }
		public string Responsable_Name { get; set; }
		public int Id_State { get; set; }
		public string State { get; set; }
		public int Nr_version_Order { get; set; }
		public int? Id_Level { get; set; }
		public string Level_Order { get; set; }
		public int? Id_Status_Order { get; set; }
		public string Status_Order { get; set; }
		public int? Id_Supplier { get; set; }
		public string Order_supplier_name { get; set; }
		public string Order_supplier_adress { get; set; }

		public int? Lieferantennummer { get; set; }
		public string Lieferantennummer_Order_Diverse { get; set; }
		public string Ort_Order_Supplier_Diverse { get; set; }
		public string Supplier_Contact_Description_Order_Diverse { get; set; }

		public string Supplier_Contact_Order_Diverse { get; set; }
		public string Supplier_Name_Order_Diverse { get; set; }
		public int Id_User { get; set; }
		public string Order_User_Name { get; set; }
		public double User_Budget { get; set; }
		//public double User_Rest_Budget { get; set; }
		//public int Order_year { get; set; }
		public int? Id_Dept_Responsable { get; set; }
		public string Dept_Responsable_Name { get; set; }

		public DateTime? Order_date { get; set; }
		public DateTime? Version_Order_date { get; set; }
		public double TotalCost_Order { get; set; }



		public AllDataOrderEntity() { }

		public AllDataOrderEntity(DataRow dataRow)
		{

			Id_Order = Convert.ToInt32(dataRow["Id_Order"]);
			Order_Number = (dataRow["Order_Number"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Order_Number"]);
			Nr_version_Order = Convert.ToInt32(dataRow["Nr_version_Order"]);
			Type_Order = (dataRow["Type_Order"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type_Order"]);
			Id_Project = (dataRow["Id_Project"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Project"]);
			//Id_proj = (dataRow["Id_proj"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_proj"]);
			Id_Type = Convert.ToInt32(dataRow["Id_Type"]);
			Type_Project = (dataRow["Type_Project"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type_Project"]);
			Name_proj = Convert.ToString(dataRow["Name_proj"]);
			Id_Currency_Order = (dataRow["Id_Currency_Order"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency_Order"]);
			Symol = Convert.ToString(dataRow["Symol"]);
			Id_Dept = (dataRow["Id_Dept"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept"]);
			Dept_name = (dataRow["Dept_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dept_name"]);
			Id_Land = (dataRow["Id_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Land"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Proj_Budget = float.Parse(dataRow["Proj_Budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			//Proj_Rest_Budget = float.Parse(dataRow["Proj_Rest_Budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			Id_Customer = (dataRow["Id_Customer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Customer"]);
			Customer_Name = (dataRow["Customer_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer_Name"]);
			Kundennummer = (dataRow["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundennummer"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nr"]);
			Nr_Customer = (dataRow["Nr_Customer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nr_Customer"]);
			//Customer_Contact = (dataRow["Customer_Contact"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer_Contact"]);
			Id_Supplier = (dataRow["Id_Supplier"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Supplier"]);
			//Customer_Contact_Description = (dataRow["Customer_Contact_Description "] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer_Contact_Description "]);
			Id_Responsable = Convert.ToInt32(dataRow["Id_Responsable"]);
			Responsable_Name = Convert.ToString(dataRow["Responsable_Name"]);
			Id_State = Convert.ToInt32(dataRow["Id_State"]);
			State = Convert.ToString(dataRow["State"]);
			Id_Supplier = Convert.ToInt32(dataRow["Id_Supplier"]);
			Order_supplier_name = (dataRow["Order_supplier_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Order_supplier_name"]);
			Order_supplier_adress = (dataRow["Order_supplier_adress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Order_supplier_adress"]);
			Lieferantennummer = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferantennummer"]);
			Lieferantennummer_Order_Diverse = (dataRow["Lieferantennummer_Order_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantennummer_Order_Diverse"]);
			Ort_Order_Supplier_Diverse = (dataRow["Ort_Order_Supplier_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort_Order_Supplier_Diverse"]);
			Supplier_Contact_Description_Order_Diverse = (dataRow["Supplier_Contact_Description_Order_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier_Contact_Description_Order_Diverse"]);
			Supplier_Contact_Order_Diverse = (dataRow["Supplier_Contact_Order_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier_Contact_Order_Diverse"]);
			Supplier_Name_Order_Diverse = (dataRow["Supplier_Name_Order_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Supplier_Name_Order_Diverse"]);
			Id_User = Convert.ToInt32(dataRow["Id_User"]);
			Order_User_Name = (dataRow["Order_User_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Order_User_Name"]);
			User_Budget = float.Parse(dataRow["User_Budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			//User_Rest_Budget = float.Parse(dataRow["User_Rest_Budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			//Order_year = Convert.ToInt32(dataRow["Order_year"]);
			Id_Dept_Responsable = (dataRow["Id_Dept_Responsable"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept_Responsable"]);
			Dept_Responsable_Name = (dataRow["Dept_Responsable_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dept_Responsable_Name"]);
			Order_date = (dataRow["Order_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Order_date"]);
			Version_Order_date = (dataRow["Version_Order_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Version_Order_date"]);
			TotalCost_Order = float.Parse(dataRow["TotalCost_Order"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			Id_Level = (dataRow["Id_Level"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Level"]);
			Level_Order = (dataRow["Level_Order"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Level_Order"]);
			Id_Status_Order = (dataRow["Id_Status_Order"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Status_Order"]);
			Status_Order = (dataRow["Status_Order"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status_Order"]);

		}
	}
}

