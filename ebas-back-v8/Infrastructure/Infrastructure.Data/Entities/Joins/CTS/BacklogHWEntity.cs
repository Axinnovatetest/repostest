using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class BacklogHWEntity
	{
		public string Name1 { get; set; }
		public int? Angebot_Nr { get; set; }
		public bool? erledigt_pos { get; set; }//
		public bool? erledigt { get; set; }//
		public int? Nr { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung1 { get; set; }
		public int? Anzahl { get; set; }
		public DateTime? Liefertermin { get; set; }
		public Decimal? Einzelpreis { get; set; }
		public Decimal? Preiseinheit { get; set; }
		public Decimal? Gesamtpreis { get; set; }
		public DateTime? Ausdr2 { get; set; }
		public DateTime? Ausdr1 { get; set; }
		public Decimal? Gesamtkosten { get; set; }
		public bool? Stückliste { get; set; }
		public string Kostenart { get; set; }
		public Decimal? Betrag { get; set; }
		public Decimal? Gesamtpersonalkosten { get; set; }
		public string Artikelnummer { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? OriginalAnzahl { get; set; }
		public string Mandant { get; set; }
		public Decimal? Kosten { get; set; }
		public int? Lagerort_id { get; set; }//
		public string Typ { get; set; }//
		public Decimal? Gewicht { get; set; }
		public bool? Standardlieferant { get; set; }//
		public BacklogHWEntity(DataRow dataRow)
		{
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt_pos"]);
			erledigt = (dataRow["erledigt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["erledigt"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Angebot_Nr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einzelpreis"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			Ausdr2 = (dataRow["Ausdr2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ausdr2"]);
			Ausdr1 = (dataRow["Ausdr1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Ausdr1"]);
			Gesamtkosten = (dataRow["Gesamtkosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtkosten"]);
			Stückliste = (dataRow["Stückliste"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Stückliste"]);
			Kostenart = (dataRow["Kostenart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kostenart"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Gesamtpersonalkosten = (dataRow["Gesamtpersonalkosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpersonalkosten"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			OriginalAnzahl = (dataRow["OriginalAnzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OriginalAnzahl"]);
			Kosten = (dataRow["Kosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kosten"]);
		}

	}
}
