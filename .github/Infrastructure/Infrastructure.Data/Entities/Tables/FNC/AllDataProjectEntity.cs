using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AllDataProjectEntity
	{
		public int Id_proj { get; set; }
		public string Name_proj { get; set; }
		public double Proj_Budget { get; set; }
		public int? Id_Customer { get; set; }
		public string Customer_Name { get; set; }
		public int? Kundennummer { get; set; }
		public int? Nr { get; set; }
		public string Ort { get; set; }
		public string Nr_Customer { get; set; }
		public int Id_Responsable { get; set; }
		public string Responsable_Name { get; set; }
		public int Id_State { get; set; }
		public string State { get; set; }
		public int? Id_Currency { get; set; }
		public string Symol { get; set; }
		public int? Id_Land { get; set; }
		public string Land_name { get; set; }
		public int? Id_Dept { get; set; }
		public string Departement_Name { get; set; }
		public int Id_Type { get; set; }
		public string Type_Project { get; set; }
		public string Description { get; set; }
		//public DateTime? Action_date { get; set; }
		public AllDataProjectEntity() { }

		public AllDataProjectEntity(DataRow dataRow)
		{

			Id_proj = Convert.ToInt32(dataRow["Id_proj"]);
			Name_proj = Convert.ToString(dataRow["Name_proj"]);
			Proj_Budget = float.Parse(dataRow["Proj_Budget"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
			Id_Customer = (dataRow["Id_Customer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Customer"]);
			Customer_Name = (dataRow["Customer_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer_Name"]);
			Kundennummer = (dataRow["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundennummer"]);
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			Nr_Customer = (dataRow["Nr_Customer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Nr_Customer"]);
			Id_Responsable = Convert.ToInt32(dataRow["Id_Responsable"]);
			Responsable_Name = Convert.ToString(dataRow["Responsable_Name"]);
			Id_State = Convert.ToInt32(dataRow["Id_State"]);
			State = Convert.ToString(dataRow["State"]);
			Id_Currency = (dataRow["Id_Currency"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency"]);
			Symol = Convert.ToString(dataRow["Symol"]);
			Id_Land = (dataRow["Id_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Land"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			Id_Dept = (dataRow["Id_Dept"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept"]);
			Departement_Name = (dataRow["Departement_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Departement_Name"]);
			Id_Type = Convert.ToInt32(dataRow["Id_Type"]);
			Type_Project = (dataRow["Type_Project"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type_Project"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			//Action_date = (dataRow["Action_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Action_date"]);
		}
	}
}

