using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class AuswertungSchneidereiEntity
	{
		public int? KG { get; set; }
		public int? KNG { get; set; }
		public string Woche { get; set; }
		public int? KGesamt { get; set; }
		public string Datum_Bis { get; set; }
		public int? Lagerort_Id { get; set; }
		public AuswertungSchneidereiEntity(DataRow dataRow)
		{
			KG = (dataRow["KG"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KG"]);
			KNG = (dataRow["KNG"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KNG"]);
			Woche = (dataRow["Woche"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Woche"]);
			KGesamt = (dataRow["KGesamt"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KGesamt"]);
			Datum_Bis = (dataRow["Datum Bis"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Datum Bis"]);
			Lagerort_Id = (dataRow["Lagerort_Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_Id"]);
		}
	}
}
