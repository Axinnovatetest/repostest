using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class AuswertungEndkontrolleEntity
	{
		public string Artikelnummer { get; set; }
		public Decimal? GesamtMenge { get; set; }
		public Decimal? MengeOffen { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Datum { get; set; }
		public string Urs_Artikelnummer { get; set; }
		public int? Urs_Fa { get; set; }
		public bool? Endkontrolle { get; set; }
		public string Kennzeichen { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }

		public AuswertungEndkontrolleEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			GesamtMenge = (dataRow["GesamtMenge"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["GesamtMenge"]);
			MengeOffen = (dataRow["MengeOffen"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["MengeOffen"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Urs_Artikelnummer = (dataRow["Urs-Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Urs-Artikelnummer"]);
			Urs_Fa = (dataRow["Urs-Fa"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Urs-Fa"]);
			Endkontrolle = (dataRow["Endkontrolle"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Endkontrolle"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
		}
	}
}
