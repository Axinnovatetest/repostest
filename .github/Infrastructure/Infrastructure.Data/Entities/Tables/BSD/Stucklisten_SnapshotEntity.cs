using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Stucklisten_SnapshotEntity
	{
		public double? Anzahl { get; set; }
		public int? Artikel_Nr { get; set; }
		public int? Artikel_Nr_des_Bauteils { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_des_Bauteils { get; set; }
		public int? BomVersion { get; set; }
		public int? DocumentId { get; set; }
		public string KundenIndex { get; set; }
		public DateTime? KundenIndexDate { get; set; }
		public int Nr { get; set; }
		public bool? Overwritten { get; set; }
		public DateTime? OverwrittenTime { get; set; }
		public int? OverwrittenUserId { get; set; }
		public string Position { get; set; }
		public DateTime SnapshotTime { get; set; }
		public int SnapshotUserId { get; set; }
		public string Variante { get; set; }
		public int? Vorgang_Nr { get; set; }

		public Stucklisten_SnapshotEntity() { }

		public Stucklisten_SnapshotEntity(DataRow dataRow)
		{
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Anzahl"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikel_Nr_des_Bauteils = (dataRow["Artikel-Nr des Bauteils"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr des Bauteils"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
			BomVersion = (dataRow["BomVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BomVersion"]);
			DocumentId = (dataRow["DocumentId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DocumentId"]);
			KundenIndex = (dataRow["KundenIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["KundenIndex"]);
			KundenIndexDate = (dataRow["KundenIndexDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["KundenIndexDate"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Overwritten = (dataRow["Overwritten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Overwritten"]);
			OverwrittenTime = (dataRow["OverwrittenTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OverwrittenTime"]);
			OverwrittenUserId = (dataRow["OverwrittenUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OverwrittenUserId"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
			SnapshotTime = Convert.ToDateTime(dataRow["SnapshotTime"]);
			SnapshotUserId = Convert.ToInt32(dataRow["SnapshotUserId"]);
			Variante = (dataRow["Variante"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Variante"]);
			Vorgang_Nr = (dataRow["Vorgang_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Vorgang_Nr"]);
		}
		public Stucklisten_SnapshotEntity ShallowClone()
		{
			return new Stucklisten_SnapshotEntity
			{
				Anzahl = Anzahl,
				Artikel_Nr = Artikel_Nr,
				Artikel_Nr_des_Bauteils = Artikel_Nr_des_Bauteils,
				Artikelnummer = Artikelnummer,
				Bezeichnung_des_Bauteils = Bezeichnung_des_Bauteils,
				BomVersion = BomVersion,
				DocumentId = DocumentId,
				KundenIndex = KundenIndex,
				KundenIndexDate = KundenIndexDate,
				Nr = Nr,
				Overwritten = Overwritten,
				OverwrittenTime = OverwrittenTime,
				OverwrittenUserId = OverwrittenUserId,
				Position = Position,
				SnapshotTime = SnapshotTime,
				SnapshotUserId = SnapshotUserId,
				Variante = Variante,
				Vorgang_Nr = Vorgang_Nr
			};
		}
	}
	public class Stucklisten_KundenIndex
	{
		public string KundenIndex { get; set; }
		public DateTime? KundenIndexDate { get; set; }
		public DateTime SnapshotTime { get; set; }
		public Stucklisten_KundenIndex()
		{
		}
		public Stucklisten_KundenIndex(DataRow dataRow)
		{
			KundenIndex = (dataRow["KundenIndex"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["KundenIndex"]);
			KundenIndexDate = (dataRow["KundenIndexDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["KundenIndexDate"]);
			SnapshotTime = Convert.ToDateTime(dataRow["SnapshotTime"]);
		}
	}
}

