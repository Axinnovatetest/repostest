using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class MahnwesenEntity
	{
		public int? Adress_id { get; set; }
		public string Anrede { get; set; }
		public DateTime? Belegdatum { get; set; }
		public int? Belegnummer { get; set; }
		public string Belegtyp { get; set; }
		public string Bemerkungen { get; set; }
		public decimal? Betrag { get; set; }
		public float? Betrag_FW { get; set; }
		public DateTime? Datum_letzte_Zahlung { get; set; }
		public int ID { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public int? Mahnstufe { get; set; }
		public string Mandant { get; set; }
		public bool? Markierung { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public int? Projekt_Nr { get; set; }
		public string Strasse_Postfach { get; set; }
		public string Vorname_NameFirma { get; set; }
		public DateTime? Zahlungsfrist { get; set; }

		public MahnwesenEntity() { }

		public MahnwesenEntity(DataRow dataRow)
		{
			Adress_id = (dataRow["Adress_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Adress_id"]);
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			Belegdatum = (dataRow["Belegdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Belegdatum"]);
			Belegnummer = (dataRow["Belegnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegnummer"]);
			Belegtyp = (dataRow["Belegtyp"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Belegtyp"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Betrag_FW = (dataRow["Betrag_FW"] == System.DBNull.Value) ? (float?)null : Convert.ToSingle(dataRow["Betrag_FW"]);
			Datum_letzte_Zahlung = (dataRow["Datum_letzte_Zahlung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum_letzte_Zahlung"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Land_PLZ_Ort = (dataRow["Land/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land/PLZ/Ort"]);
			Mahnstufe = (dataRow["Mahnstufe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Mahnstufe"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
			Markierung = (dataRow["Markierung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Markierung"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Projekt-Nr"]);
			Strasse_Postfach = (dataRow["Straße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße/Postfach"]);
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			Zahlungsfrist = (dataRow["Zahlungsfrist"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Zahlungsfrist"]);
		}
	}
}

