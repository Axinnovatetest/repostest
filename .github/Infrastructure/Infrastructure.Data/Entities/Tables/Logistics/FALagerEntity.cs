using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class FALagerEntity
	{
		public long id { get; set; }
		public long fertigungsnummer { get; set; }
		public string artikelnummer { get; set; }
		public string externStatus { get; set; }
		public int anzahl { get; set; }
		public FALagerEntity()
		{


		}
		public FALagerEntity(DataRow dataRow)
		{
			id = (dataRow["id"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["id"]);
			fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Fertigungsnummer"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			externStatus = (dataRow["ExternStatut"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ExternStatut"]);
			anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Anzahl"]);

		}
	}
}
