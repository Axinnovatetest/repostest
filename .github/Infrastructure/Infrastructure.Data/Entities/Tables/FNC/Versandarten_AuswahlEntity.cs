using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class Versandarten_AuswahlEntity
	{
		public int ID { get; set; }
		public string Versandarten { get; set; }

		public Versandarten_AuswahlEntity() { }

		public Versandarten_AuswahlEntity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Versandarten = (dataRow["Versandarten"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandarten"]);
		}
	}
}

