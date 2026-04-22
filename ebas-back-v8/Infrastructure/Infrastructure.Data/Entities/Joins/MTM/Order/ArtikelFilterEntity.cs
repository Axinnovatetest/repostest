using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class ArtikelFilterEntity
	{
		public int Artikel_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Artikelnummer { get; set; }

		public ArtikelFilterEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung 1"].ToString();
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
		}
	}
}
