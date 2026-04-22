using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class AV_Abteilung_MitarbeiterEntity
	{
		public string AV_Mitarbeiter { get; set; }
		public string EMAIL { get; set; }
		public int ID { get; set; }

		public AV_Abteilung_MitarbeiterEntity() { }

		public AV_Abteilung_MitarbeiterEntity(DataRow dataRow)
		{
			AV_Mitarbeiter = (dataRow["AV_Mitarbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AV_Mitarbeiter"]);
			EMAIL = (dataRow["EMAIL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EMAIL"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}
	}
}

