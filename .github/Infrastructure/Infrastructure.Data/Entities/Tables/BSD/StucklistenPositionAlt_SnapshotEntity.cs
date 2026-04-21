using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class StucklistenPositionAlt_SnapshotEntity
	{
		public decimal? Anzahl { get; set; }
		public string ArtikelBezeichnung { get; set; }
		public int? ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public int? BomVersion { get; set; }
		public int? DocumentId { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public int Nr { get; set; }
		public int OriginalStucklistenNr { get; set; }
		public bool? Overwritten { get; set; }
		public DateTime? OverwrittenTime { get; set; }
		public int? OverwrittenUserId { get; set; }
		public int ParentArtikelNr { get; set; }
		public string Position { get; set; }
		public DateTime SnapshotTime { get; set; }
		public int SnapshotUserId { get; set; }

		public StucklistenPositionAlt_SnapshotEntity() { }

		public StucklistenPositionAlt_SnapshotEntity(DataRow dataRow)
		{
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			ArtikelBezeichnung = (dataRow["ArtikelBezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelBezeichnung"]);
			ArtikelNr = (dataRow["ArtikelNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArtikelNr"]);
			ArtikelNummer = (dataRow["ArtikelNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelNummer"]);
			BomVersion = (dataRow["BomVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BomVersion"]);
			DocumentId = (dataRow["DocumentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DocumentId"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			OriginalStucklistenNr = Convert.ToInt32(dataRow["OriginalStucklistenNr"]);
			Overwritten = (dataRow["Overwritten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Overwritten"]);
			OverwrittenTime = (dataRow["OverwrittenTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OverwrittenTime"]);
			OverwrittenUserId = (dataRow["OverwrittenUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OverwrittenUserId"]);
			ParentArtikelNr = Convert.ToInt32(dataRow["ParentArtikelNr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
			SnapshotTime = Convert.ToDateTime(dataRow["SnapshotTime"]);
			SnapshotUserId = Convert.ToInt32(dataRow["SnapshotUserId"]);
		}

		public StucklistenPositionAlt_SnapshotEntity ShallowClone()
		{
			return new StucklistenPositionAlt_SnapshotEntity
			{
				Anzahl = Anzahl,
				ArtikelBezeichnung = ArtikelBezeichnung,
				ArtikelNr = ArtikelNr,
				ArtikelNummer = ArtikelNummer,
				BomVersion = BomVersion,
				DocumentId = DocumentId,
				LastUpdateTime = LastUpdateTime,
				LastUpdateUserId = LastUpdateUserId,
				Nr = Nr,
				OriginalStucklistenNr = OriginalStucklistenNr,
				Overwritten = Overwritten,
				OverwrittenTime = OverwrittenTime,
				OverwrittenUserId = OverwrittenUserId,
				ParentArtikelNr = ParentArtikelNr,
				Position = Position,
				SnapshotTime = SnapshotTime,
				SnapshotUserId = SnapshotUserId
			};
		}
	}
}

