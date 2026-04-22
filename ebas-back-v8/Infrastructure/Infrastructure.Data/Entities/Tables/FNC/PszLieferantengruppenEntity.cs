using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class PszLieferantengruppenEntity
	{
		public int ID { get; set; }
		public string Lieferantengruppe { get; set; }

		public PszLieferantengruppenEntity() { }
		public PszLieferantengruppenEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Lieferantengruppe = (dataRow["Lieferantengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantengruppe"]);
		}
	}
}

