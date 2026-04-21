using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Kunden_Project_BudgetEntity
	{
		public string Customer_Name { get; set; }
		public int? Kundennummer { get; set; }
		public string Ort { get; set; }
		public int Nr { get; set; }

		public Kunden_Project_BudgetEntity() { }

		public Kunden_Project_BudgetEntity(DataRow dataRow)
		{
			Customer_Name = (dataRow["Customer_Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Customer_Name"]);
			Kundennummer = (dataRow["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundennummer"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);


		}
	}
}

