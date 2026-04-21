using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class LieferPlannungBestandEntity
	{
		public int? Lagerort_id { get; set; }
		public int? Bestand { get; set; }
		public string psz { get; set; }
		public string Lagerort { get; set; }

		public LieferPlannungBestandEntity(DataRow dataRow)
		{
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestand"]);
			psz = (dataRow["psz#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["psz#"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
		}
	}


	public class LieferPlannungFertigungEntity
	{
		public string psz { get; set; }
		public int? Fertigungsnummer { get; set; }
		public bool? FA_Gestartet { get; set; }
		public string Lagerort { get; set; }
		public string Kennzeichen { get; set; }
		public int? Menge_Offen { get; set; }
		public int? Originalanzahl { get; set; }
		public DateTime? Termin_Bestätigt1 { get; set; }
		public LieferPlannungFertigungEntity(DataRow dataRow)
		{
			psz = (dataRow["psz#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["psz#"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["FA_Gestartet"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Kennzeichen = (dataRow["Kennzeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kennzeichen"]);
			Menge_Offen = (dataRow["Menge Offen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Menge Offen"]);
			Originalanzahl = (dataRow["Originalanzahl"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Originalanzahl"]);
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
		}
	}
}
