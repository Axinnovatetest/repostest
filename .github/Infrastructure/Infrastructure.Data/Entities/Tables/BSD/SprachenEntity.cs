using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class SprachenEntity
	{
		public int ID { get; set; }
		public string Sprache { get; set; }

		public SprachenEntity() { }

		public SprachenEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Sprache = (dataRow["Sprache"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sprache"]);
		}
	}
}

