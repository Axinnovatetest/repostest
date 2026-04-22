using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FADruck
{
	public class FAReport1PositionsEntity
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public Decimal? Anzahl { get; set; }
		public string Arbeitsanweisung { get; set; }
		public string Fertiger { get; set; }
		public DateTime? Termin_Soll { get; set; }
		public string Bemerkungen { get; set; }
		public string Lagerort { get; set; }
		public bool? ESD_Schutz { get; set; }
		public FAReport1PositionsEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Arbeitsanweisung = (dataRow["Arbeitsanweisung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Arbeitsanweisung"]);
			Fertiger = (dataRow["Fertiger"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fertiger"]);
			Termin_Soll = (dataRow["Termin_Soll"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Soll"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			ESD_Schutz = (dataRow["ESD_Schutz"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["ESD_Schutz"]);
		}
	}
}
