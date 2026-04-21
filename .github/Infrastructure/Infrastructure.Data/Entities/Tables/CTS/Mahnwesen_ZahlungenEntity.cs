using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Mahnwesen_ZahlungenEntity
	{
		public DateTime? Datum { get; set; }
		public bool? gebucht { get; set; }
		public decimal? Haben_DM { get; set; }
		public decimal? Haben_FW { get; set; }
		public int ID { get; set; }
		public int? Mahn_ID { get; set; }
		public decimal? Soll_DM { get; set; }
		public decimal? Soll_FW { get; set; }
		public string Text { get; set; }

		public Mahnwesen_ZahlungenEntity() { }

		public Mahnwesen_ZahlungenEntity(DataRow dataRow)
		{
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			Haben_DM = (dataRow["Haben_DM"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Haben_DM"]);
			Haben_FW = (dataRow["Haben_FW"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Haben_FW"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Mahn_ID = (dataRow["Mahn_ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Mahn_ID"]);
			Soll_DM = (dataRow["Soll_DM"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Soll_DM"]);
			Soll_FW = (dataRow["Soll_FW"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Soll_FW"]);
			Text = (dataRow["Text"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text"]);
		}
	}
}

