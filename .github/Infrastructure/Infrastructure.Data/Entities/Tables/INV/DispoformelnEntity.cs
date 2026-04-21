using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.INV
{
	public class DispoformelnEntity
	{
		public string Beschreibung { get; set; }
		public int? Dispo_ID { get; set; }
		public int ID { get; set; }

		public DispoformelnEntity() { }

		public DispoformelnEntity(DataRow dataRow)
		{
			Beschreibung = (dataRow["Beschreibung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Beschreibung"]);
			Dispo_ID = (dataRow["Dispo-ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Dispo-ID"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}
	}
}

