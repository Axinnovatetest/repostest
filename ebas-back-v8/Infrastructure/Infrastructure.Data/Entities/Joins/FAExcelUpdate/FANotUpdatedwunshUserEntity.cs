using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAExcelUpdate
{
	public class FANotUpdatedwunshUserEntity
	{
		public DateTime? Termin_Bestätigt1 { get; set; }
		public DateTime? Termin { get; set; }
		public string Kennzeichen { get; set; }
		public bool? Geschnitten { get; set; }
		public bool? Begonnen { get; set; }
		public bool? GT2 { get; set; }
		public bool? GT3 { get; set; }
		public bool? GT1 { get; set; }
		public bool? G1 { get; set; }
		public bool? G2 { get; set; }
		public bool? G3 { get; set; }
		public bool? gedruckt { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public bool? FA_Gestartet { get; set; }
		public int FertigungId { get; set; }

		public FANotUpdatedwunshUserEntity()
		{

		}
		public FANotUpdatedwunshUserEntity(DataRow dataRow)
		{
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Termin = (dataRow["Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Geschnitten = (dataRow["Geschnitten"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Geschnitten"]);
			Begonnen = (dataRow["Begonnen"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Begonnen"]);
			GT2 = (dataRow["GT2"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["GT2"]);
			GT3 = (dataRow["GT3"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["GT3"]);
			GT1 = (dataRow["GT1"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["GT1"]);
			G1 = (dataRow["G1"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["G1"]);
			G2 = (dataRow["G2"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["G2"]);
			G3 = (dataRow["G3"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["G3"]);
			gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["gedruckt"]);
			FA_Druckdatum = (dataRow["FA_Druckdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Druckdatum"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["FA_Gestartet"]);
			FertigungId = Convert.ToInt32(dataRow["ID"]);
		}
	}
}
