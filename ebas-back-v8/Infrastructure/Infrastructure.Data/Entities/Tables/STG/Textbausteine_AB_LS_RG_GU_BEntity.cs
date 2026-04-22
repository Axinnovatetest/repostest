using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.STG
{
	public class Textbausteine_AB_LS_RG_GU_BEntity
	{
		public string Auftragsbestätigung { get; set; }
		public string Bestellung { get; set; }
		public string Gutschrift { get; set; }
		public int ID { get; set; }
		public string Lieferschein { get; set; }
		public string Rechnung { get; set; }

		public Textbausteine_AB_LS_RG_GU_BEntity() { }

		public Textbausteine_AB_LS_RG_GU_BEntity(DataRow dataRow)
		{
			Auftragsbestätigung = (dataRow["Auftragsbestätigung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Auftragsbestätigung"]);
			Bestellung = (dataRow["Bestellung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestellung"]);
			Gutschrift = (dataRow["Gutschrift"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gutschrift"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Lieferschein = (dataRow["Lieferschein"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferschein"]);
			Rechnung = (dataRow["Rechnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Rechnung"]);
		}
	}
}

