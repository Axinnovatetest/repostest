using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class FehlermaterialFAEntity
	{
		public int? Artikel_nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichung1 { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? BedarfGesamt { get; set; }
		public decimal? Verfugbar { get; set; }
		public string Name1 { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public string Lagerort { get; set; }
		public FehlermaterialFAEntity()
		{

		}
		public FehlermaterialFAEntity(DataRow dataRow)
		{
			Artikel_nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Artikel-Nr"].ToString());
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"].ToString());
			Bezeichung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"].ToString());
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"].ToString());
			BedarfGesamt = (dataRow["Bedarf Gesamt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bedarf Gesamt"].ToString());
			Verfugbar = (dataRow["Verfügbar"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verfügbar"].ToString());
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"].ToString());
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"].ToString());
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"].ToString());
		}
	}
}
