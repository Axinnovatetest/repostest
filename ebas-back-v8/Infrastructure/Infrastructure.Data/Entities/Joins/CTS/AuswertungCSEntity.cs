using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class ErstelltRehugenEntity
	{
		public string Vorname_NameFirma { get; set; }
		public string Artikelnummer { get; set; }
		public DateTime? Datum { get; set; }
		public int? Rgnummer { get; set; }
		public int? Angebotnummer { get; set; }
		public Decimal? VKGesamtpreis { get; set; }
		public Decimal? Gesamtkupferzuschlag { get; set; }
		public Decimal? NettoBetrag { get; set; }
		public string Bezug { get; set; }
		public string CSKontakt { get; set; }
		public ErstelltRehugenEntity(DataRow dataRow)
		{
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Rgnummer = (dataRow["Rgnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Rgnummer"]);
			Angebotnummer = (dataRow["Angebotnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebotnummer"]);
			VKGesamtpreis = (dataRow["VKGesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VKGesamtpreis"]);
			Gesamtkupferzuschlag = (dataRow["Gesamtkupferzuschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtkupferzuschlag"]);
			NettoBetrag = (dataRow["NettoBetrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["NettoBetrag"]);
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			CSKontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
		}

	}
}
