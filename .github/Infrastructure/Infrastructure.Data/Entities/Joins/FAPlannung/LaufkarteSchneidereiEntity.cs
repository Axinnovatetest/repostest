using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class LaufkarteSchneidereiEntity
	{
		public string Klassifizierung { get; set; }
		public string Gewerk { get; set; }
		public string Artikelnummer { get; set; }
		public Decimal? Anzahl { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Bezeichnung { get; set; }
		public string FGArtikelBZ1 { get; set; }
		public string Artikelfamilie_Kunde { get; set; }

		public LaufkarteSchneidereiEntity(DataRow dataRow)
		{
			Klassifizierung = (dataRow["Klassifizierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Klassifizierung"]);
			Gewerk = (dataRow["Gewerk"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			FGArtikelBZ1 = (dataRow["FGArtikelBZ1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FGArtikelBZ1"]);
			Artikelfamilie_Kunde = (dataRow["Artikelfamilie_Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelfamilie_Kunde"]);
		}
	}
}
