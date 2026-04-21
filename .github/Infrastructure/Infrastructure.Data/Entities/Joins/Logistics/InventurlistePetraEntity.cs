using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class InventurlistePetraEntity
	{
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public int TotalRows { get; set; }
		public InventurlistePetraEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? null : Convert.ToDecimal(dataRow["Bestand"]);
			Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
			TotalRows = (dataRow["totalRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalRows"]);

		}
	}
}
