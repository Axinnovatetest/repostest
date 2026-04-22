using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Fertigungs_Planung_Sicht_Gewerk_2_BETNEntity
	{
		public DateTime? Ack_Date { get; set; }
		public int? Anzahl { get; set; }
		public string Artikelnummer { get; set; }
		public DateTime? FA_begonnen { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Freigabestatus { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public string Halle { get; set; }
		public int ID { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public string Kennzeichen { get; set; }
		public string Klassifizierung { get; set; }
		public int? KW_Aktuel { get; set; }
		public int? KW_Produktion { get; set; }
		public int? Lagerort_id { get; set; }
		public Single? Order_Time { get; set; }
		public string PSZ_Artikelnummer { get; set; }

		public Fertigungs_Planung_Sicht_Gewerk_2_BETNEntity() { }

		public Fertigungs_Planung_Sicht_Gewerk_2_BETNEntity(DataRow dataRow)
		{
			Ack_Date = (dataRow["Ack Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ack Date"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			FA_begonnen = (dataRow["FA_begonnen"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_begonnen"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Gewerk_1 = Convert.ToString(dataRow["Gewerk 1"]);
			Gewerk_2 = Convert.ToString(dataRow["Gewerk 2"]);
			Gewerk_3 = Convert.ToString(dataRow["Gewerk 3"]);
			Halle = (dataRow["Halle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Halle"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Klassifizierung = (dataRow["Klassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Klassifizierung"]);
			KW_Aktuel = (dataRow["KW Aktuel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW Aktuel"]);
			KW_Produktion = (dataRow["KW_Produktion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW_Produktion"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Order_Time = (dataRow["Order Time"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Order Time"]);
			PSZ_Artikelnummer = (dataRow["PSZ Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ Artikelnummer"]);
		}
	}
}

