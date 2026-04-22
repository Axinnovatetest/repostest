using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class PSZ_KundengruppenEntity
	{
		public int ID { get; set; }
		public string Kundengruppe { get; set; }

		public PSZ_KundengruppenEntity() { }

		public PSZ_KundengruppenEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Kundengruppe = (dataRow["Kundengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundengruppe"]);
		}
	}
}

