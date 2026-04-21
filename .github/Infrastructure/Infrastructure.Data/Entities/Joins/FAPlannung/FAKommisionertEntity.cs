using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class FAKommisionertEntity
	{
		public DateTime? Geplanter_Termin { get; set; }
		public string Artikelnummer { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Halle { get; set; }
		public bool? Teilweise_kommisioniert { get; set; }
		public Decimal? FA_Menge { get; set; }
		public Decimal? Erledigt { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Artikelkurztext { get; set; }
		public string Bemerkung { get; set; }

		public FAKommisionertEntity(DataRow dataRow)
		{
			Geplanter_Termin = (dataRow["Geplanter Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Geplanter Termin"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Halle = (dataRow["Halle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Halle"]);
			Teilweise_kommisioniert = (dataRow["Teilweise kommisioniert"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Teilweise kommisioniert"]);
			FA_Menge = (dataRow["FA Menge"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["FA Menge"]);
			Erledigt = (dataRow["Erledigt"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Erledigt"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Artikelkurztext = (dataRow["Artikelkurztext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelkurztext"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
		}
	}
}
