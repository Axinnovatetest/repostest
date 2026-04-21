using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class TBL_KUPFEREntity
	{
		public decimal? Aktueller_Kupfer_Preis_in_Gramm { get; set; }
		public DateTime? Datum { get; set; }
		public int ID { get; set; }

		public TBL_KUPFEREntity() { }

		public TBL_KUPFEREntity(DataRow dataRow)
		{
			Aktueller_Kupfer_Preis_in_Gramm = (dataRow["Aktueller_Kupfer_Preis_in_Gramm"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aktueller_Kupfer_Preis_in_Gramm"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}

		public TBL_KUPFEREntity ShallowClone()
		{
			return new TBL_KUPFEREntity
			{
				Aktueller_Kupfer_Preis_in_Gramm = Aktueller_Kupfer_Preis_in_Gramm,
				Datum = Datum,
				ID = ID
			};
		}
	}
}

