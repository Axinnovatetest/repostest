using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class PSZ_Historique_Import_Excel_FAEntity
	{
		public DateTime? Änderungsdatum { get; set; }
		public string bermerkung { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int ID { get; set; }
		public string Mitarbeiter { get; set; }
		public string PC { get; set; }
		public DateTime? Termin_Wunsch { get; set; }

		public PSZ_Historique_Import_Excel_FAEntity() { }

		public PSZ_Historique_Import_Excel_FAEntity(DataRow dataRow)
		{
			Änderungsdatum = (dataRow["Änderungsdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Änderungsdatum"]);
			bermerkung = (dataRow["bermerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["bermerkung"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mitarbeiter"]);
			PC = (dataRow["PC"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PC"]);
			Termin_Wunsch = (dataRow["Termin_Wunsch"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Wunsch"]);
		}
	}
}

