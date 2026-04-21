using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class InfoRahmennummerEntity
	{
		public int Nr { get; set; }
		public int AngeboteNr { get; set; }
		public int PositionNr { get; set; }
		public int Position { get; set; }
		public decimal Anzahl { get; set; }
		public string Bezug { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public DateTime? GultigAb { get; set; }
		public DateTime? GultigBis { get; set; }
		public InfoRahmennummerEntity() { }
		public InfoRahmennummerEntity(DataRow dataRow)
		{
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Nr"]);
			AngeboteNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Angebot-Nr"]);
			PositionNr = (dataRow["PositionNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["PositionNr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Position"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(dataRow["Anzahl"]);
			ExtensionDate = (dataRow["ExtensionDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ExtensionDate"]);
			GultigAb = (dataRow["GultigAb"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigAb"]);
			GultigBis = (dataRow["GultigBis"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["GultigBis"]);
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
		}
	}
}
