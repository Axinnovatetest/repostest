using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Bestellnummern_StaffelpreiseEntity
	{
		public decimal? ab_Anzahl { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public int ID { get; set; }
		public int? nummer { get; set; }

		public Bestellnummern_StaffelpreiseEntity() { }

		public Bestellnummern_StaffelpreiseEntity(DataRow dataRow)
		{
			ab_Anzahl = (dataRow["ab_Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ab_Anzahl"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			nummer = (dataRow["nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nummer"]);
		}

		public Bestellnummern_StaffelpreiseEntity ShallowClone()
		{
			return new Bestellnummern_StaffelpreiseEntity
			{
				ab_Anzahl = ab_Anzahl,
				Einkaufspreis = Einkaufspreis,
				ID = ID,
				nummer = nummer
			};
		}
	}
}

