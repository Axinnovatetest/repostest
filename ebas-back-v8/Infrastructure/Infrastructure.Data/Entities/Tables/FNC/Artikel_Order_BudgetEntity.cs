using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Artikel_Order_BudgetEntity
	{
		public string bindName { get; set; }
		public string Artikelnummer { get; set; }
		public string Article_Name1 { get; set; }
		public string Article_Name2 { get; set; }
		public int Artikel_Nr { get; set; }

		public Artikel_Order_BudgetEntity() { }

		public Artikel_Order_BudgetEntity(DataRow dataRow)
		{
			bindName = (dataRow["bindName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["bindName"]);
			Artikel_Nr = Convert.ToInt32(dataRow["Artikel_Nr"]);
			Artikelnummer = Convert.ToString(dataRow["Artikelnummer"]);
			Article_Name1 = (dataRow["Article_Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_Name1"]);
			Article_Name2 = (dataRow["Article_Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article_Name2"]);


		}
	}
}

