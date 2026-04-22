using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class RabatthauptGruppenEntity
	{
		public string Beschreibung { get; set; }
		public int ID { get; set; }
		public int Rabatthauptgruppe { get; set; }

		public RabatthauptGruppenEntity() { }
		public RabatthauptGruppenEntity(DataRow dataRow)
		{
			Beschreibung = (dataRow["Beschreibung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Beschreibung"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Rabatthauptgruppe = Convert.ToInt32(dataRow["Rabatthauptgruppe"]);
		}
	}
}

