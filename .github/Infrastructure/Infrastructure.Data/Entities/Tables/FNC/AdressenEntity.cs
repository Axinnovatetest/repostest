using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AdressenEntity
	{
		public string Abteilung { get; set; }
		public double? Adresstyp { get; set; }
		public string Anrede { get; set; }
		public bool Auswahl { get; set; }
		public string Bemerkungen { get; set; }
		public string Briefanrede { get; set; }
		public string eMail { get; set; }
		public DateTime? erfasst { get; set; }
		public string Fax { get; set; }
		public string Funktion { get; set; }
		public int? Kundennummer { get; set; }
		public string Land { get; set; }
		public int? Lieferantennummer { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public int Nr { get; set; }
		public string Ort { get; set; }
		public int? Personalnummer { get; set; }
		public string PLZ_Postfach { get; set; }
		public string PLZ_StraBe { get; set; }
		public string Postfach { get; set; }
		public bool Postfach_bevorzugt { get; set; }
		public string Sortierbegriff { get; set; }
		public bool sperren { get; set; }
		public string StraBe { get; set; }
		public string stufe { get; set; }
		public string Telefon { get; set; }
		public string Titel { get; set; }
		public string upsize_ts { get; set; }
		public string von { get; set; }
		public string Vorname { get; set; }
		public string WWW { get; set; }

		public AdressenEntity() { }

		public AdressenEntity(DataRow dataRow)
		{
			Abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			Adresstyp = (dataRow["Adresstyp"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Adresstyp"]);
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			Auswahl = Convert.ToBoolean(dataRow["Auswahl"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Briefanrede = (dataRow["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Briefanrede"]);
			eMail = (dataRow["eMail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["eMail"]);
			erfasst = (dataRow["erfasst"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["erfasst"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			Funktion = (dataRow["Funktion"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Funktion"]);
			Kundennummer = (dataRow["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundennummer"]);
			Land = (dataRow["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land"]);
			Lieferantennummer = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferantennummer"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			Personalnummer = (dataRow["Personalnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personalnummer"]);
			PLZ_Postfach = (dataRow["PLZ_Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PLZ_Postfach"]);
			PLZ_StraBe = (dataRow["PLZ_Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PLZ_Straße"]);
			Postfach = (dataRow["Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Postfach"]);
			Postfach_bevorzugt = Convert.ToBoolean(dataRow["Postfach bevorzugt"]);
			Sortierbegriff = (dataRow["Sortierbegriff"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sortierbegriff"]);
			sperren = Convert.ToBoolean(dataRow["sperren"]);
			StraBe = (dataRow["Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße"]);
			stufe = (dataRow["stufe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["stufe"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Titel = (dataRow["Titel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Titel"]);
			upsize_ts = (dataRow["upsize_ts"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["upsize_ts"]);
			von = (dataRow["von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["von"]);
			Vorname = (dataRow["Vorname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname"]);
			WWW = (dataRow["WWW"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WWW"]);
		}
	}
}
