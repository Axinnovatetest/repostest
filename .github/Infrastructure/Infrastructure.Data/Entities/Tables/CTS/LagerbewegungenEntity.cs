using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class LagerbewegungenEntity
	{
		public int? angebot_nr { get; set; }
		public DateTime? Datum { get; set; }
		public bool? gebucht { get; set; }
		public string Gebucht_von { get; set; }
		public int ID { get; set; }
		public int? Kunden_nr { get; set; }
		public bool? Löschen { get; set; }
		public string Ort { get; set; }
		public string Typ { get; set; }

		public LagerbewegungenEntity() { }

		public LagerbewegungenEntity(DataRow dataRow)
		{
			angebot_nr = (dataRow["angebot-nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["angebot-nr"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gebucht"]);
			Gebucht_von = (dataRow["Gebucht von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gebucht von"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kunden_nr = (dataRow["Kunden_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kunden_nr"]);
			Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
		}
	}
}

