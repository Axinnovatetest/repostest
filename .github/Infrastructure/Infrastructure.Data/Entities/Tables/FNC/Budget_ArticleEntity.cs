using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Budget_ArticleEntity
	{
		public string Article_code { get; set; }
		public string Article_designation1 { get; set; }
		public string Article_designation2 { get; set; }
		public int Article_number { get; set; }
		public int Article_supplier { get; set; }
		public int? Creator_Bind { get; set; }
		public string Description { get; set; }
		public int? Editor_Bind { get; set; }
		public int Id_Currency { get; set; }
		public double Unit_Price { get; set; }

		public Budget_ArticleEntity() { }

		public Budget_ArticleEntity(DataRow dataRow)
		{
			Article_code = Convert.ToString(dataRow["Article_code"]);
			Article_designation1 = Convert.ToString(dataRow["Article_designation1"]);
			Article_designation2 = (dataRow["Article_designation2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_designation2"]);
			Article_number = Convert.ToInt32(dataRow["Article_number"]);
			Article_supplier = Convert.ToInt32(dataRow["Article_supplier"]);
			Creator_Bind = (dataRow["Creator_Bind"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Creator_Bind"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Editor_Bind = (dataRow["Editor_Bind"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Editor_Bind"]);
			Id_Currency = Convert.ToInt32(dataRow["Id_Currency"]);
			Unit_Price = Convert.ToDouble(dataRow["Unit_Price"]);
		}
	}
}

