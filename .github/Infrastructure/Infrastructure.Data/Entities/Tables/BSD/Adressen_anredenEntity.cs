using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Adressen_anredenEntity
	{
		public string Anrede { get; set; }
		public int ID { get; set; }

		public Adressen_anredenEntity() { }

		public Adressen_anredenEntity(DataRow dataRow)
		{
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			ID = Convert.ToInt32(dataRow["ID"]);
		}
	}
}

