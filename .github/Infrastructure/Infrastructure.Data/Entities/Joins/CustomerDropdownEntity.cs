using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins
{
	public class CustomerDropdownEntity
	{
		public int Nr { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public int Kundennummer { get; set; }
		public string Ort { get; set; }
		public string AdressType { get; set; }

		public CustomerDropdownEntity(DataRow dataRow)
		{
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Kundennummer = Convert.ToInt32(dataRow["Kundennummer"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			AdressType = (dataRow["Adreßtyp"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Adreßtyp"]);
		}
	}
}
