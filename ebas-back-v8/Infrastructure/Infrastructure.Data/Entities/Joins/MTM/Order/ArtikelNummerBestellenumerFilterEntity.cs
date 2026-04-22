using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class ArtikelNummerBestellenumerFilterEntity
	{
		public int Artikel_Nr { get; set; }
		public string Bestellnummer { get; set; }
		public string Artikelnummer { get; set; }

		public ArtikelNummerBestellenumerFilterEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bestellnummer = (dataRow["Bestellnummer"] == System.DBNull.Value) ? "" : dataRow["Bestellnummer"].ToString();
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
		}

	}
}
