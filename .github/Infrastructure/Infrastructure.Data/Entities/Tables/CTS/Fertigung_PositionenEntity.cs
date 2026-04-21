using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Fertigung_PositionenEntity
	{
		public decimal? Anzahl { get; set; }
		public string Arbeitsanweisung { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Bemerkungen { get; set; }
		public bool? buchen { get; set; }
		public string Fertiger { get; set; }
		public DateTime? Fertigstellung_Ist { get; set; }
		public int ID { get; set; }
		public int? ID_Fertigung { get; set; }
		public int? ID_Fertigung_HL { get; set; }
		public int? Lagerort_ID { get; set; }
		public bool? Löschen { get; set; }
		public bool? ME_gebucht { get; set; }
		public DateTime? Termin_Soll { get; set; }
		public int? Vorgang_Nr { get; set; }

		public Fertigung_PositionenEntity() { }

		public Fertigung_PositionenEntity(DataRow dataRow)
		{
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Arbeitsanweisung = (dataRow["Arbeitsanweisung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Arbeitsanweisung"]);
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			buchen = (dataRow["buchen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["buchen"]);
			Fertiger = (dataRow["Fertiger"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fertiger"]);
			Fertigstellung_Ist = (dataRow["Fertigstellung_Ist"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Fertigstellung_Ist"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Fertigung = (dataRow["ID_Fertigung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Fertigung"]);
			ID_Fertigung_HL = (dataRow["ID_Fertigung_HL"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Fertigung_HL"]);
			Lagerort_ID = (dataRow["Lagerort_ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_ID"]);
			Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			ME_gebucht = (dataRow["ME gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ME gebucht"]);
			Termin_Soll = (dataRow["Termin_Soll"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Soll"]);
			Vorgang_Nr = (dataRow["Vorgang_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Vorgang_Nr"]);
		}
	}
}

