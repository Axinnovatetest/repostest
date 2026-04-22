using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CTS.INSOverview
{
	public class INSOverviewMindesbestand_AuswertungEntity
	{
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public int? Produktionslager { get; set; }
		public string Mitarbeiter { get; set; }
		public decimal? Mindestbestand { get; set; }
		public decimal? Bestand { get; set; }
		public decimal? Differenz { get; set; }
		public decimal? Differenzwert { get; set; }
		public INSOverviewMindesbestand_AuswertungEntity()
		{

		}
		public INSOverviewMindesbestand_AuswertungEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Artikelnummer"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Kunde"]);
			Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
			Produktionslager = (dataRow["Produktionslager"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionslager"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Mitarbeiter"]);
			Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestand"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Bestand"]);
			Differenz = (dataRow["Differenz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Differenz"]);
			Differenzwert = (dataRow["Differenzwert"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Differenzwert"]);

		}
	}
}