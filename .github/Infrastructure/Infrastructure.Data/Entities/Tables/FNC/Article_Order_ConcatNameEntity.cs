using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Article_Order_ConcatNameEntity
	{
		public string Action_Article { get; set; }
		public DateTime? Article_date { get; set; }
		public string Currency_Article { get; set; }
		public string Dept_name { get; set; }
		public int Id_AO { get; set; }
		public int Id_Article { get; set; }
		public int? Id_Currency_Article { get; set; }
		public int? Id_Dept { get; set; }
		public int? Id_Land { get; set; }
		public int Id_Order { get; set; }
		public int? Id_Project { get; set; }
		public int Id_User { get; set; }
		public string Land_name { get; set; }
		public int? Quantity { get; set; }
		public double? TotalCost_Article { get; set; }
		public double? Unit_Price { get; set; }
		public string Article_Name { get; set; }
		public string Article_Name_Order_Diverse { get; set; }
		public double? Unit_Price_Diverse { get; set; }

		public Article_Order_ConcatNameEntity() { }

		public Article_Order_ConcatNameEntity(DataRow dataRow)
		{
			Action_Article = (dataRow["Action_Article"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Action_Article"]);
			Article_date = (dataRow["Article_date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Article_date"]);
			Currency_Article = (dataRow["Currency_Article"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Currency_Article"]);
			Dept_name = (dataRow["Dept_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Dept_name"]);
			Id_AO = Convert.ToInt32(dataRow["Id_AO"]);
			Id_Article = Convert.ToInt32(dataRow["Id_Article"]);
			Id_Currency_Article = (dataRow["Id_Currency_Article"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Currency_Article"]);
			Id_Dept = (dataRow["Id_Dept"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Dept"]);
			Id_Land = (dataRow["Id_Land"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Land"]);
			Id_Order = Convert.ToInt32(dataRow["Id_Order"]);
			Id_Project = (dataRow["Id_Project"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_Project"]);
			Id_User = Convert.ToInt32(dataRow["Id_User"]);
			Land_name = (dataRow["Land_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land_name"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Quantity"]);
			TotalCost_Article = (dataRow["TotalCost_Article"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["TotalCost_Article"]);
			Unit_Price = (dataRow["Unit_Price"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Unit_Price"]);
			Article_Name = (dataRow["Article_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_Name"]);
			Article_Name_Order_Diverse = (dataRow["Article_Name_Order_Diverse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_Name_Order_Diverse"]);
			Unit_Price_Diverse = (dataRow["Unit_Price_Diverse"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Unit_Price_Diverse"]);

		}
	}
}

