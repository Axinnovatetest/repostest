using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class Auschusskosten_Technik_InfoEntity
	{
		public DateTime? Datum { get; set; }
		public string Typ { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal? Anzahl { get; set; }
		public string Einheit { get; set; }
		public string Lagerort { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? Grund { get; set; }
		public string Bemerkung { get; set; }
		public decimal Kosten { get; set; }
		public int totalRows { get; set; }
		public int Rollennummer { get; set; }
		public Auschusskosten_Technik_InfoEntity() { }
		public Auschusskosten_Technik_InfoEntity(DataRow dataRow)
		{
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Grund = (dataRow["Grund"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Grund"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Kosten = (dataRow["Kosten"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Kosten"]);
			totalRows = (dataRow["totalRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalRows"]);
			Rollennummer = (dataRow["Rollennummer"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Rollennummer"]);

		}
	}
}
