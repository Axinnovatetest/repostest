using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_Diverse_Project_CustomerEntity
	{
		public string Customer_Contact_Description_Project_Diverse { get; set; }
		public string Customer_Contact_Project_Diverse { get; set; }
		public string Custommer_Name_Project_Diverse { get; set; }
		public int? Id_Customer_Project_Diverse { get; set; }
		public int Id_Diverse_Customer_Project { get; set; }
		public int? Id_Project_Diverse { get; set; }
		public int? kumdennummer_Project_Diverse { get; set; }
		public string Nr_Customer_Project_Diverse { get; set; }
		public string Ort_Project_Diverse { get; set; }

		public Budget_Diverse_Project_CustomerEntity() { }

		public Budget_Diverse_Project_CustomerEntity(DataRow dataRow)
		{
			Customer_Contact_Description_Project_Diverse = (dataRow["Customer_Contact_Description_Project_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer_Contact_Description_Project_Diverse"]);
			Customer_Contact_Project_Diverse = (dataRow["Customer_Contact_Project_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer_Contact_Project_Diverse"]);
			Custommer_Name_Project_Diverse = (dataRow["Custommer_Name_Project_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Custommer_Name_Project_Diverse"]);
			Id_Customer_Project_Diverse = (dataRow["Id_Customer_Project_Diverse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Customer_Project_Diverse"]);
			Id_Diverse_Customer_Project = Convert.ToInt32(dataRow["Id_Diverse_Customer_Project"]);
			Id_Project_Diverse = (dataRow["Id_Project_Diverse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Project_Diverse"]);
			kumdennummer_Project_Diverse = (dataRow["kumdennummer_Project_Diverse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["kumdennummer_Project_Diverse"]);
			Nr_Customer_Project_Diverse = (dataRow["Nr_Customer_Project_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nr_Customer_Project_Diverse"]);
			Ort_Project_Diverse = (dataRow["Ort_Project_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort_Project_Diverse"]);
		}
	}
}

