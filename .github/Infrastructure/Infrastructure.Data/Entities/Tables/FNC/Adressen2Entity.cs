using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{

	public class AdressenEntity2
	{
		public string Abteilung { get; set; }
		public int? Adresstyp { get; set; }
		public string Anrede { get; set; }
		public bool? Auswahl { get; set; }
		public string Bemerkung { get; set; }
		public string Bemerkungen { get; set; }
		public string Briefanrede { get; set; }
		public bool? Dienstag__Anliefertag_ { get; set; }
		public bool? Donnerstag__Anliefertag_ { get; set; }
		public int? DUNS { get; set; }
		public bool? EDI_Aktiv { get; set; }
		public string eMail { get; set; }
		public DateTime? erfasst { get; set; }
		public string Fax { get; set; }
		public bool? Freitag__Anliefertag_ { get; set; }
		public string Funktion { get; set; }
		public int? Kundennummer { get; set; }
		public string Land { get; set; }
		public int? Lieferantennummer { get; set; }
		public bool? Mittwoch__Anliefertag_ { get; set; }
		public bool? Montag__Anliefertag_ { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public int Nr { get; set; }
		public string Ort { get; set; }
		public bool? PendingValidation { get; set; }
		public int? Personalnummer { get; set; }
		public string PLZ_Postfach { get; set; }
		public string PLZ_Strasse { get; set; }
		public string Postfach { get; set; }
		public bool? Postfach_bevorzugt { get; set; }
		public string Sortierbegriff { get; set; }
		public bool? sperren { get; set; }
		public string Strasse { get; set; }
		public string stufe { get; set; }
		public string Telefon { get; set; }
		public string Titel { get; set; }
		public string von { get; set; }
		public string Vorname { get; set; }
		public string WWW { get; set; }

		public AdressenEntity2() { }

		public AdressenEntity2(DataRow dataRow)
		{
			Abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			Adresstyp = (dataRow["Adresstyp"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Adresstyp"]);
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			Auswahl = (dataRow["Auswahl"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Auswahl"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Briefanrede = (dataRow["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Briefanrede"]);
			Dienstag__Anliefertag_ = (dataRow["Dienstag (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Dienstag (Anliefertag)"]);
			Donnerstag__Anliefertag_ = (dataRow["Donnerstag (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Donnerstag (Anliefertag)"]);
			DUNS = (dataRow["DUNS"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DUNS"]);
			EDI_Aktiv = (dataRow["EDI-Aktiv"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EDI-Aktiv"]);
			eMail = (dataRow["eMail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["eMail"]);
			erfasst = (dataRow["erfasst"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["erfasst"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			Freitag__Anliefertag_ = (dataRow["Freitag (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Freitag (Anliefertag)"]);
			Funktion = (dataRow["Funktion"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Funktion"]);
			Kundennummer = (dataRow["Kundennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kundennummer"]);
			Land = (dataRow["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land"]);
			Lieferantennummer = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferantennummer"]);
			Mittwoch__Anliefertag_ = (dataRow["Mittwoch (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mittwoch (Anliefertag)"]);
			Montag__Anliefertag_ = (dataRow["Montag (Anliefertag)"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Montag (Anliefertag)"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			PendingValidation = (dataRow["PendingValidation"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["PendingValidation"]);
			Personalnummer = (dataRow["Personalnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personalnummer"]);
			PLZ_Postfach = (dataRow["PLZ_Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PLZ_Postfach"]);
			PLZ_Strasse = (dataRow["PLZ_Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PLZ_Straße"]);
			Postfach = (dataRow["Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Postfach"]);
			Postfach_bevorzugt = (dataRow["Postfach bevorzugt"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Postfach bevorzugt"]);
			Sortierbegriff = (dataRow["Sortierbegriff"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sortierbegriff"]);
			sperren = (dataRow["sperren"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["sperren"]);
			Strasse = (dataRow["Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße"]);
			stufe = (dataRow["stufe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["stufe"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Titel = (dataRow["Titel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Titel"]);
			von = (dataRow["von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["von"]);
			Vorname = (dataRow["Vorname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname"]);
			WWW = (dataRow["WWW"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WWW"]);
		}

		public AdressenEntity2 ShallowClone()
		{
			return new AdressenEntity2
			{
				Abteilung = Abteilung,
				Adresstyp = Adresstyp,
				Anrede = Anrede,
				Auswahl = Auswahl,
				Bemerkung = Bemerkung,
				Bemerkungen = Bemerkungen,
				Briefanrede = Briefanrede,
				Dienstag__Anliefertag_ = Dienstag__Anliefertag_,
				Donnerstag__Anliefertag_ = Donnerstag__Anliefertag_,
				DUNS = DUNS,
				EDI_Aktiv = EDI_Aktiv,
				eMail = eMail,
				erfasst = erfasst,
				Fax = Fax,
				Freitag__Anliefertag_ = Freitag__Anliefertag_,
				Funktion = Funktion,
				Kundennummer = Kundennummer,
				Land = Land,
				Lieferantennummer = Lieferantennummer,
				Mittwoch__Anliefertag_ = Mittwoch__Anliefertag_,
				Montag__Anliefertag_ = Montag__Anliefertag_,
				Name1 = Name1,
				Name2 = Name2,
				Name3 = Name3,
				Nr = Nr,
				Ort = Ort,
				PendingValidation = PendingValidation,
				Personalnummer = Personalnummer,
				PLZ_Postfach = PLZ_Postfach,
				PLZ_Strasse = PLZ_Strasse,
				Postfach = Postfach,
				Postfach_bevorzugt = Postfach_bevorzugt,
				Sortierbegriff = Sortierbegriff,
				sperren = sperren,
				Strasse = Strasse,
				stufe = stufe,
				Telefon = Telefon,
				Titel = Titel,
				von = von,
				Vorname = Vorname,
				WWW = WWW
			};
		}
	}



}
