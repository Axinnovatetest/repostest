using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Statistics.MGO
{
	public class SalesInjectionEntity
	{
		public DateTime? Datum { get; set; }
		public decimal? VK { get; set; }
		public decimal? Preis { get; set; }
		public decimal? Produktionskosten { get; set; }

		public string FertigungsNummer { get; set; }
		public string Produktionsbereich { get; set; }

		public int? Menge { get; set; }
		public string Bezeichnung { get; set; }
		public string Ausdr { get; set; }

		public int? Originalanzahl { get; set; }
		public int? AnzahlErledigt { get; set; }

		public string ArticleNummer { get; set; }

		public SalesInjectionEntity() { }
		public SalesInjectionEntity(DataRow dataRow)
		{
			Produktionskosten = (dataRow["Produktionskosten"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Produktionskosten"]);
			Preis = (dataRow["Preis"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			VK = (dataRow["VK"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK"]);
			Datum = (dataRow["Datum"] == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			FertigungsNummer = (dataRow["Fertigungsnummer"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Fertigungsnummer"]);
			Bezeichnung = (dataRow["Bezeichnung"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Bezeichnung"]);
			Menge = (dataRow["Menge"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge"]);
			Originalanzahl = (dataRow["Originalanzahl"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			AnzahlErledigt = (dataRow["AnzahlErledigt"] == DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AnzahlErledigt"]);

			ArticleNummer = (dataRow["Artikelnummer"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Artikelnummer"]);

			Ausdr = (dataRow["Ausdr"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Ausdr"]);
			Produktionsbereich = (dataRow["Produktionsbereich"] == DBNull.Value) ? string.Empty : Convert.ToString(dataRow["Produktionsbereich"]);

		}
	}
}