using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.STG
{
	public class PSZ_MandantenEntity
	{
		public int ID { get; set; }
		public string Mandant { get; set; }

		public PSZ_MandantenEntity() { }

		public PSZ_MandantenEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
		}
	}
}

