using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CTS.INSOverview
{
	public class INSOverviewVK_Summe_ungebuchte_ABEntity
	{
		public int? AngebotNr { get; set; }
		public int? Nr { get; set; }
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public string Kunde { get; set; }
		public int? Produktionslager { get; set; }
		public string Mitarbeiter { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public INSOverviewVK_Summe_ungebuchte_ABEntity()
		{

		}
		public INSOverviewVK_Summe_ungebuchte_ABEntity(DataRow dataRow)
		{
			AngebotNr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Artikelnummer"]);
			Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Kunde"]);
			Produktionslager = (dataRow["Produktionslager"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionslager"]);
			Mitarbeiter = (dataRow["Mitarbeiter"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Mitarbeiter"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
		}
	}
}