using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class ExcessRohmaterialEntity
	{
		public ExcessRohmaterialEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			SummevonBestand = (dataRow["SummevonBestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SummevonBestand"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Kosten = (dataRow["Kosten"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Kosten"]);
			totalRows = (dataRow["totalRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalRows"]);

		}
		public int ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal SummevonBestand { get; set; }
		public decimal Einkaufspreis { get; set; }
		public decimal Kosten { get; set; }
		public int totalRows { get; set; }
	}
}
