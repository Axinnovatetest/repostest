using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public enum AdressenTypEnum
	{
		Standard = 1,
		Interessant = 2,
		Lieferadresse = 3,
	}
	public class AdressenEntity
	{
		public string Abteilung { get; set; }
		public int? Adresstyp { get; set; }
		public string Anrede { get; set; }
		public bool? Auswahl { get; set; }
		public string Bemerkung { get; set; }
		public string Bemerkungen { get; set; }
		public string Briefanrede { get; set; }
		public bool? Dienstag_Anliefertag { get; set; }
		public bool? Donnerstag_Anliefertag { get; set; }
		public string EMail { get; set; }
		public Nullable<DateTime> Erfasst { get; set; }
		public string Fax { get; set; }
		public bool? Freitag_Anliefertag { get; set; }
		public string Funktion { get; set; }
		public int? Kundennummer { get; set; }
		public string Land { get; set; }
		public int? Lieferantennummer { get; set; }
		public bool? Mittwoch_Anliefertag { get; set; }
		public bool? Montag_Anliefertag { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public int Nr { get; set; }
		public string Ort { get; set; }
		public int? Personalnummer { get; set; }
		public string PLZ_Postfach { get; set; }
		public string PLZ_StraBe { get; set; }
		public string Postfach { get; set; }
		public bool? Postfach_bevorzugt { get; set; }
		public string Sortierbegriff { get; set; }
		public bool? Sperren { get; set; }
		public string StraBe { get; set; }
		public string Stufe { get; set; }
		public string Telefon { get; set; }
		public string Titel { get; set; }
		public string Von { get; set; }
		public string Vorname { get; set; }
		public string WWW { get; set; }
		public string Duns { get; set; }
		public bool? EDI_Aktiv { get; set; }
		public bool? PendingValidation { get; set; }
		public string StorageLocation { get; set; }
		public string UnloadingPoint { get; set; }
		public AdressenEntity() { }
		public AdressenEntity(DataRow dr)

		{
			Abteilung = (dr["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Abteilung"]);
			Adresstyp = (dr["Adresstyp"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["Adresstyp"]);
			Anrede = (dr["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Anrede"]);
			Auswahl = (dr["Auswahl"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["Auswahl"]);
			Bemerkung = (dr["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bemerkung"]);
			Bemerkungen = (dr["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Bemerkungen"]);
			Briefanrede = (dr["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Briefanrede"]);
			Dienstag_Anliefertag = (dr["Dienstag (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["Dienstag (Anliefertag)"]);
			Donnerstag_Anliefertag = (dr["Donnerstag (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["Donnerstag (Anliefertag)"]);
			EMail = (dr["eMail"] == System.DBNull.Value) ? "" : Convert.ToString(dr["eMail"]);
			Erfasst = (dr["erfasst"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["erfasst"]);
			Fax = (dr["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Fax"]);
			Freitag_Anliefertag = (dr["Freitag (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["Freitag (Anliefertag)"]);
			Funktion = (dr["Funktion"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Funktion"]);
			Kundennummer = (dr["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["Kundennummer"]);
			Land = (dr["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Land"]);
			Lieferantennummer = (dr["Lieferantennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["Lieferantennummer"]);
			Mittwoch_Anliefertag = (dr["Mittwoch (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["Mittwoch (Anliefertag)"]);
			Montag_Anliefertag = (dr["Montag (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["Montag (Anliefertag)"]);
			Name1 = (dr["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Name1"]);
			Name2 = (dr["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Name2"]);
			Name3 = (dr["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Name3"]);
			Nr = Convert.ToInt32(dr["Nr"]);
			Ort = (dr["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Ort"]);
			Personalnummer = (dr["Personalnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dr["Personalnummer"]);
			PLZ_Postfach = (dr["PLZ_Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["PLZ_Postfach"]);
			PLZ_StraBe = (dr["PLZ_Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dr["PLZ_Straße"]);
			Postfach = (dr["Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Postfach"]);
			Postfach_bevorzugt = (dr["Postfach bevorzugt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["Postfach bevorzugt"]);
			Sortierbegriff = (dr["Sortierbegriff"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Sortierbegriff"]);
			Sperren = (dr["sperren"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["sperren"]);
			StraBe = (dr["Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Straße"]);
			Stufe = (dr["stufe"] == System.DBNull.Value) ? "" : Convert.ToString(dr["stufe"]);
			Telefon = (dr["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Telefon"]);
			Titel = (dr["Titel"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Titel"]);
			Von = (dr["von"] == System.DBNull.Value) ? "" : Convert.ToString(dr["von"]);
			Vorname = (dr["Vorname"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Vorname"]);
			WWW = (dr["WWW"] == System.DBNull.Value) ? "" : Convert.ToString(dr["WWW"]);
			Duns = (dr["Duns"] == System.DBNull.Value) ? "" : Convert.ToString(dr["Duns"]);
			EDI_Aktiv = (dr["EDI-Aktiv"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["EDI-Aktiv"]);
			PendingValidation = (dr["PendingValidation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dr["PendingValidation"]);
			StorageLocation = (dr["StorageLocation"] == System.DBNull.Value) ? "" : Convert.ToString(dr["StorageLocation"]);
			UnloadingPoint = (dr["UnloadingPoint"] == System.DBNull.Value) ? "" : Convert.ToString(dr["UnloadingPoint"]);
		}
	}
}