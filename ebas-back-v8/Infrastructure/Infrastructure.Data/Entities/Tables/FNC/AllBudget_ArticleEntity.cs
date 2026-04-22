using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AllBudget_ArticleEntity
	{
		public string Article_code { get; set; }
		public string Article_designation1 { get; set; }
		public string Article_designation2 { get; set; }
		public int Article_number { get; set; }
		public int Article_supplier { get; set; }
		public string Article_supplier_name { get; set; }
		public int? Lieferantennummer { get; set; }
		public int? Nr { get; set; }
		public string Ort { get; set; }
		public int? Creator_Bind { get; set; }
		public string Article_creator_name { get; set; }
		public string Description { get; set; }
		public int? Editor_Bind { get; set; }
		public string Article_editor_name { get; set; }
		public int Id_Currency { get; set; }
		public string Symol { get; set; }
		public double Unit_Price { get; set; }


		public AllBudget_ArticleEntity() { }

		public AllBudget_ArticleEntity(DataRow dataRow)
		{
			Article_code = Convert.ToString(dataRow["Article_code"]);
			Article_designation1 = Convert.ToString(dataRow["Article_designation1"]);
			Article_designation2 = (dataRow["Article_designation2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_designation2"]);
			Article_number = Convert.ToInt32(dataRow["Article_number"]);
			Article_supplier = Convert.ToInt32(dataRow["Article_supplier"]);
			Article_supplier_name = (dataRow["Article_supplier_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_supplier_name"]);
			Lieferantennummer = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferantennummer"]);
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			Creator_Bind = (dataRow["Creator_Bind"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Creator_Bind"]);
			Article_creator_name = (dataRow["Article_creator_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_creator_name"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Editor_Bind = (dataRow["Editor_Bind"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Editor_Bind"]);
			Article_editor_name = (dataRow["Article_editor_name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_editor_name"]);
			Id_Currency = Convert.ToInt32(dataRow["Id_Currency"]);
			Symol = Convert.ToString(dataRow["Symol"]);
			Unit_Price = Convert.ToDouble(dataRow["Unit_Price"]);
		}
	}
}

