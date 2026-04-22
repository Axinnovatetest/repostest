using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAUpdate
{
	public class OpenFANotVersionningEntity
	{
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Kennzeichen { get; set; }
		public bool? UpdateS { get; set; }
		public int? Artikel_Nr { get; set; }
		public int? ID_Fer { get; set; }
		public int? Type_Update { get; set; }
		public bool? gedruckt { get; set; }
		public DateTime? FA_Druckdatum { get; set; }
		public int? Lagerort_id { get; set; }

		public OpenFANotVersionningEntity(DataRow dataRow)
		{
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			UpdateS = (dataRow["UpdateS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UpdateS"]);
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			ID_Fer = (dataRow["ID_Fer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Fer"]);
			Type_Update = (dataRow["Type_Update"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Type_Update"]);
			gedruckt = (dataRow["gedruckt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gedruckt"]);
			FA_Druckdatum = (dataRow["FA_Druckdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_Druckdatum"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
		}
	}
}
