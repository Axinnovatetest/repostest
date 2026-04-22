using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAExcelUpdate
{
	public class FAWunshUpdateEntity
	{
		public string Termin_Bestätigt1 { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public string Termin { get; set; }
		public int FertigungId { get; set; }
		public FAWunshUpdateEntity()
		{

		}
		public FAWunshUpdateEntity(DataRow dataRow)
		{
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin_Bestätigt1"]);
			Fertigungsnummer = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Termin = (dataRow["Termin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Termin"]);
			FertigungId = Convert.ToInt32(dataRow["ID"]);
		}
	}
}
