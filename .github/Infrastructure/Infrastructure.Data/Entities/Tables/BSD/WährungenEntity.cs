using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class WahrungenEntity
	{
		public double? Betrag_Fremdwahrung { get; set; }
		public int? Dezimalstellen { get; set; }
		public decimal? Entspricht_DM { get; set; }
		public bool? EU { get; set; }
		public string Land { get; set; }
		public int Nr { get; set; }
		public DateTime? Stand { get; set; }
		public string Symbol { get; set; }
		public string Wahrung { get; set; }

		public WahrungenEntity() { }
		public WahrungenEntity(DataRow dataRow)
		{
			Betrag_Fremdwahrung = (dataRow["Betrag Fremdwährung"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Betrag Fremdwährung"]);
			Dezimalstellen = (dataRow["Dezimalstellen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Dezimalstellen"]);
			Entspricht_DM = (dataRow["entspricht DM"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["entspricht DM"]);
			EU = (dataRow["EU"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EU"]);
			Land = (dataRow["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Stand = (dataRow["Stand"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Stand"]);
			Symbol = (dataRow["Symbol"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Symbol"]);
			Wahrung = (dataRow["Währung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Währung"]);
		}
	}
}

