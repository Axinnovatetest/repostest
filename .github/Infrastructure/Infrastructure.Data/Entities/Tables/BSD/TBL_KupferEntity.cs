using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class TBL_KupferEntity
	{
		public decimal? Aktueller_Kupfer_Preis_in_Gramm { get; set; }
		public DateTime? Datum { get; set; }
		public int ID { get; set; }

		public TBL_KupferEntity() { }

		public TBL_KupferEntity(DataRow dataRow)
		{
			Aktueller_Kupfer_Preis_in_Gramm = (dataRow["Aktueller_Kupfer_Preis_in_Gramm"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aktueller_Kupfer_Preis_in_Gramm"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}
	}
}

