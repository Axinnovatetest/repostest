using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class FAListShneidereiKabelGeschnittenEntity
	{
		public int? Fertigungsnummer { get; set; }
		public string Artikelnummer { get; set; }
		public Decimal? FA_Menge { get; set; }
		public Decimal? Erledigt { get; set; }
		public DateTime? Termin { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public bool? FA_Gestartet { get; set; }
		public FAListShneidereiKabelGeschnittenEntity(DataRow dataRow)
		{
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			FA_Menge = (dataRow["FA_Menge"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["FA_Menge"]);
			Erledigt = (dataRow["Erledigt"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Erledigt"]);
			Termin = (dataRow["Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin"]);
			Kabel_geschnitten = (dataRow["Kabel_geschnitten"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Kabel_geschnitten"]);
			FA_Gestartet = (dataRow["FA_Gestartet"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["FA_Gestartet"]);
		}
	}
}
