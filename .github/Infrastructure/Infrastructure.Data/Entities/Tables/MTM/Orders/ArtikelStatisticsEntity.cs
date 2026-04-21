using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM.Orders
{
	public class ArtikelStatisticsEntity
	{
		public int ArtikelNr { get; set; }
		public string Artiklenummer { get; set; }
		public ArtikelStatisticsEntity(DataRow dataRow)
		{
			Artiklenummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Artikel-Nr"]);
		}
	}
}
