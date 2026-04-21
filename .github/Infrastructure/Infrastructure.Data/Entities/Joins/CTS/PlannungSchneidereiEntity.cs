using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class PlannungSchneidereiEntity
	{
		public DateTime? FA_begonnen { get; set; }
		public bool? FA_Gestartet { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Freigabestatus { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public string Halle { get; set; }
		public string Kunde { get; set; }
		public Single? Order_Time { get; set; }
		public string PSZ_Artikelnummer { get; set; }
		public int? Quantity { get; set; }
		public DateTime? Termin_Schneiderei { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }

		public PlannungSchneidereiEntity() { }

		public PlannungSchneidereiEntity(DataRow dataRow)
		{
			FA_begonnen = (dataRow["FA_begonnen"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_begonnen"]);
			FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FA_Gestartet"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Gewerk_1 = Convert.ToString(dataRow["Gewerk 1"]);
			Gewerk_2 = Convert.ToString(dataRow["Gewerk 2"]);
			Gewerk_3 = Convert.ToString(dataRow["Gewerk 3"]);
			Halle = (dataRow["Halle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Halle"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			Order_Time = (dataRow["Order Time"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Order Time"]);
			PSZ_Artikelnummer = (dataRow["PSZ Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ Artikelnummer"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Quantity"]);
			Termin_Schneiderei = (dataRow["Termin Schneiderei"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin Schneiderei"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
		}
	}
}
