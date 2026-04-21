using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class LagerArtikelEntity
	{
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public int lagerID { get; set; }
		public decimal bestand { get; set; }
		public LagerArtikelEntity()
		{

		}
		public LagerArtikelEntity(DataRow dataRow)
		{
			artikelNr = dataRow["ArtikelNr"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["ArtikelNr"]);
			artikelnummer = dataRow["Artikelnummer"] == DBNull.Value ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			bestand = (dataRow["bestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["bestand"]);
			lagerID = dataRow["LagerID"] == DBNull.Value ? 0 : Convert.ToInt32(dataRow["LagerID"]);
		}
	}
}
