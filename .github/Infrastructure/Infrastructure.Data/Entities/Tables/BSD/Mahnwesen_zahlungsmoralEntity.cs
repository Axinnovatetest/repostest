using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Mahnwesen_zahlungsmoralEntity
	{
		public string Bezeichnung { get; set; }
		public int ID { get; set; }

		public Mahnwesen_zahlungsmoralEntity() { }

		public Mahnwesen_zahlungsmoralEntity(DataRow dataRow)
		{
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}
	}
}

