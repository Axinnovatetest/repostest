using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class NachBerechnungTNEntity
	{
		public string Kostenart { get; set; }
		public bool? ab_buchen { get; set; }
		public DateTime? Datum { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public int? Originalanzahl { get; set; }
		public int? Anzahl { get; set; }
		public decimal? Betrag { get; set; }
		public decimal? Preis { get; set; }
		public decimal? Lohn_alt { get; set; }
		public decimal? Lohn_neu { get; set; }
		public decimal? Ausdr5 { get; set; }
		public decimal? Ausdr3 { get; set; }
		public string Bezfeld { get; set; }
		public NachBerechnungTNEntity(DataRow dataRow)
		{
			Kostenart = (dataRow["Kostenart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kostenart"]);
			ab_buchen = (dataRow["ab_buchen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ab_buchen"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Preis = (dataRow["Preis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preis"]);
			Lohn_alt = (dataRow["Lohn alt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Lohn alt"]);
			Lohn_neu = (dataRow["Lohn neu"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Lohn neu"]);
			Ausdr5 = (dataRow["Ausdr5"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ausdr5"]);
			Ausdr3 = (dataRow["Ausdr3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ausdr3"]);
			Bezfeld = (dataRow["Bezfeld"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezfeld"]);
		}
	}
}
