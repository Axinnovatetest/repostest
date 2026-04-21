using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class ArtikelWareneingangGewichtEntity
	{
		public decimal Materialgewicht { get; set; }
		public string Artikelnummer { get; set; }

		public ArtikelWareneingangGewichtEntity(DataRow dataRow)
		{
			Materialgewicht = (dataRow["Materialgewicht"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Materialgewicht"].ToString());
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
		}
	}
}
