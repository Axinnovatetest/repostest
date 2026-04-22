using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class KundenKurtzEntity
	{
		public string lVornameNameFirma { get; set; }
		public KundenKurtzEntity(DataRow dataRow)
		{
			lVornameNameFirma = (dataRow["LVornameNameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LVornameNameFirma"]);
		}
	}
}
