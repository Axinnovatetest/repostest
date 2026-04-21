using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Mahnwesen_ArchivEntity
	{
		public int? Adress_id { get; set; }
		public DateTime? Belegdatum { get; set; }
		public int? Belegnummer { get; set; }
		public DateTime? Datum { get; set; }
		public float? Haben_DM { get; set; }
		public int ID { get; set; }
		public int? Projekt_Nr { get; set; }
		public decimal? Soll_DM { get; set; }
		public DateTime? Zahlungsfrist { get; set; }

		public Mahnwesen_ArchivEntity() { }

		public Mahnwesen_ArchivEntity(DataRow dataRow)
		{
			Adress_id = (dataRow["Adress_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Adress_id"]);
			Belegdatum = (dataRow["Belegdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Belegdatum"]);
			Belegnummer = (dataRow["Belegnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegnummer"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Haben_DM = (dataRow["Haben_DM"] == System.DBNull.Value) ? (float?)null : Convert.ToSingle(dataRow["Haben_DM"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Projekt-Nr"]);
			Soll_DM = (dataRow["Soll_DM"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Soll_DM"]);
			Zahlungsfrist = (dataRow["Zahlungsfrist"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Zahlungsfrist"]);
		}
	}
}

