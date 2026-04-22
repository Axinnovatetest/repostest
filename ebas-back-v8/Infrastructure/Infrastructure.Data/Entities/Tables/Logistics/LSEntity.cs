using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class LSEntity
	{
		public long angeboteNr { get; set; }
		public long projektNr { get; set; }
		public DateTime? liefertermin { get; set; }
		public int kundenNr { get; set; }
		public string anrede { get; set; }
		public string vornameNameFirma { get; set; }
		public string name2 { get; set; }
		public string name3 { get; set; }
		public string ansprechpartner { get; set; }
		public string abteilung { get; set; }
		public string landPLZOrt { get; set; }
		public string strassePostfach { get; set; }
		public string briefanrede { get; set; }
		public string lanrede { get; set; }
		public string lVornameNameFirma { get; set; }
		public string lName2 { get; set; }
		public string lName3 { get; set; }
		public string lAnsprechpartner { get; set; }
		public string labteilung { get; set; }
		public string lLandPLZOrt { get; set; }
		public string lStrassePostfach { get; set; }
		public string lBriefanrede { get; set; }
		public int personelNr { get; set; }
		public string ihrZeichen { get; set; }
		public string unserZeichen { get; set; }
		public string bezug { get; set; }
		public string versandart { get; set; }
		public DateTime? datum { get; set; }
		public string freitext { get; set; }
		public string typ { get; set; }
		public string ablastelle { get; set; }
		public string textLieferschein { get; set; }
		public decimal ust { get; set; }
		public bool rp { get; set; }
		public LSEntity()
		{

		}
		public LSEntity(DataRow dataRow)
		{
			angeboteNr = dataRow["AngebotNr"] == DBNull.Value ? 0 : Convert.ToInt64(dataRow["AngebotNr"]);
			projektNr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Projekt-Nr"]);
			liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Index_Kunde_Datum"]);
			kundenNr = (dataRow["Kunden-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Kunden-Nr"]);
			anrede = (dataRow["Anrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Anrede"]);
			vornameNameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			name3 = (dataRow["Name3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name3"]);
			ansprechpartner = (dataRow["Ansprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ansprechpartner"]);
			abteilung = (dataRow["Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abteilung"]);
			landPLZOrt = (dataRow["Land/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Land/PLZ/Ort"]);
			strassePostfach = (dataRow["Straße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Straße/Postfach"]);
			briefanrede = (dataRow["Briefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Briefanrede"]);
			lanrede = (dataRow["LAnrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAnrede"]);
			lVornameNameFirma = (dataRow["LVorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LVorname/NameFirma"]);
			lName2 = (dataRow["LName2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName2"]);
			lName3 = (dataRow["LName3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LName3"]);
			lAnsprechpartner = (dataRow["LAnsprechpartner"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAnsprechpartner"]);
			labteilung = (dataRow["LAbteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LAbteilung"]);
			lLandPLZOrt = (dataRow["LLand/PLZ/Ort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LLand/PLZ/Ort"]);
			lStrassePostfach = (dataRow["LStraße/Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LStraße/Postfach"]);
			lBriefanrede = (dataRow["LBriefanrede"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LBriefanrede"]);
			personelNr = (dataRow["Personal-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Personal-Nr"]);
			ihrZeichen = (dataRow["Ihr Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ihr Zeichen"]);
			unserZeichen = (dataRow["Unser Zeichen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Unser Zeichen"]);
			bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			freitext = (dataRow["Freitext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freitext"]);
			typ = (dataRow["typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["typ"]);
			textLieferschein = (dataRow["TextLieferschein"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TextLieferschein"]);


		}
	}
}
