using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Tbl_Historie_VK_UpdateEntity
	{
		public decimal? Alte_Preis { get; set; }
		public string Artikelnummer { get; set; }
		public DateTime? Datum { get; set; }
		public int id { get; set; }
		public decimal? Neue_Preis { get; set; }
		public string User { get; set; }

		public Tbl_Historie_VK_UpdateEntity() { }

		public Tbl_Historie_VK_UpdateEntity(DataRow dataRow)
		{
			Alte_Preis = (dataRow["Alte Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Alte Preis"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			id = Convert.ToInt32(dataRow["id"]);
			Neue_Preis = (dataRow["Neue Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Neue Preis"]);
			User = (dataRow["User"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["User"]);
		}
	}
}

