using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AnsprechpartnerEntity
	{
		public string Abteilung { get; set; } // Department 
		public string Anrede { get; set; } // Adress 
		public string Ansprechpartner { get; set; } // contact person
		public bool? Auswahl_AB_BW { get; set; } // Selection AB/BW
		public string Bemerkung { get; set; } // Notes
		public string Briefanrede { get; set; } // Salutation
		public string EMail { get; set; } // EmailAdress
		public string FAX { get; set; } // Fax
		public DateTime? Geburtstag { get; set; } // Birthday 
		public string Land { get; set; } // Country
		public string Mobil { get; set; } // Mobile
		public int Nr { get; set; } // Id
		public int? Nummer { get; set; } // Number
		public string Ort { get; set; } // City
		public string PLZ { get; set; } // postcode 
		public string Position { get; set; } // Position
		public bool? Serienbrief { get; set; } //FormLetter
		public int? Sprache { get; set; } // Language
		public string StraBe { get; set; } // Street
		public string Telefon { get; set; } // PhoneNumber
		public string Titel { get; set; } // Title
		public string Zu_Handen { get; set; } // Attention

		public AnsprechpartnerEntity() { }
		public AnsprechpartnerEntity(DataRow dataRow)
		{
			Abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			Anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			Ansprechpartner = (dataRow["Ansprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ansprechpartner"]);
			Auswahl_AB_BW = (dataRow["auswahl_AB_BW"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["auswahl_AB_BW"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Briefanrede = (dataRow["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Briefanrede"]);
			EMail = (dataRow["eMail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["eMail"]);
			FAX = (dataRow["FAX"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FAX"]);
			Geburtstag = (dataRow["Geburtstag"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Geburtstag"]);
			Land = (dataRow["Land"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land"]);
			Mobil = (dataRow["Mobil"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mobil"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Nummer = (dataRow["Nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nummer"]);
			Ort = (dataRow["Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ort"]);
			PLZ = (dataRow["PLZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PLZ"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
			Serienbrief = (dataRow["Serienbrief"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Serienbrief"]);
			Sprache = (dataRow["Sprache"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Sprache"]);
			StraBe = (dataRow["Straße"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Titel = (dataRow["Titel"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Titel"]);
			Zu_Handen = (dataRow["zu_Händen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["zu_Händen"]);
		}
	}
}

