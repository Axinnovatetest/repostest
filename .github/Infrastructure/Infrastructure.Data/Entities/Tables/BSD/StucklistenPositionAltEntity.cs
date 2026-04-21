using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class StucklistenPositionAltEntity
	{
		public decimal? Anzahl { get; set; }
		public string ArtikelBezeichnung { get; set; }
		public int? ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public int? DocumentId { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public int Nr { get; set; }
		public int OriginalStucklistenNr { get; set; }
		public int ParentArtikelNr { get; set; }
		public string Position { get; set; }

		public StucklistenPositionAltEntity() { }

		public StucklistenPositionAltEntity(DataRow dataRow)
		{
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Anzahl"]);
			ArtikelBezeichnung = (dataRow["ArtikelBezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelBezeichnung"]);
			ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArtikelNr"]);
			ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNummer"]);
			DocumentId = (dataRow["DocumentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DocumentId"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			OriginalStucklistenNr = Convert.ToInt32(dataRow["OriginalStucklistenNr"]);
			ParentArtikelNr = Convert.ToInt32(dataRow["ParentArtikelNr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
		}
	}
}

