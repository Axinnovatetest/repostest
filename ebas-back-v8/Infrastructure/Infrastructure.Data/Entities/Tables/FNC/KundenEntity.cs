using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	//  public class KundenEntity
	//  {
	//      public int? Belegkreis { get; set; }
	//      public string Bemerkungen { get; set; }
	//      public string Branche { get; set; }
	//      public bool? Bruttofakturierung { get; set; }
	//      public string Debitoren_Nr { get; set; }
	//      public string EG_Identifikationsnummer { get; set; }
	//      public double? Eilzuschlag { get; set; }
	//      public bool? Factoring { get; set; }
	//      public int? Fibu_rahmen { get; set; }
	//      public bool? gesperrt_für_weitere_Lieferungen { get; set; }
	//public string Grund { get; set; }
	//      public string Grund_für_Sperre { get; set; }
	//      public int? Karenztage { get; set; }
	//      public int? Konditionszuordnungs_Nr { get; set; }
	//public double? Kreditlimit { get; set; }
	//      public string Kundengruppe { get; set; }
	//      public string Lieferantenummer_Kunden { get; set; }
	//      public bool? Lieferscheinadresse { get; set; }
	//      public int? LSADR { get; set; }
	//      public bool? LSADRANG { get; set; }
	//      public bool? LSADRAUF { get; set; }
	//      public bool? LSADRGUT { get; set; }
	//      public bool? LSADRPROF { get; set; }
	//      public bool? LSADRRG { get; set; }
	//      public bool? LSADRSTO { get; set; }
	//      public bool? LSRG { get; set; }
	//      public double? Mahngebühr_1 { get; set; }
	//public double? Mahngebühr_2 { get; set; }
	//public double? Mahngebühr_3 { get; set; }
	//public bool? Mahnsperre { get; set; }
	//      public double? Mindermengenzuschlag { get; set; }
	//      public int Nr { get; set; }
	//      public int? Nummer { get; set; }
	//      public bool? OPOS { get; set; }
	//      public int? Preisgruppe { get; set; }
	//      public int? Preisgruppe2 { get; set; }
	//      public int? Rabattgruppe { get; set; }
	//      public bool? Regelmäßig_anschreiben { get; set; }
	//public string RG_Abteilung { get; set; }
	//public string RG_Land_PLZ_ORT { get; set; }
	//public string RG_Strasse_Postfach { get; set; }
	//public int? Sprache { get; set; }
	//      public string Standardversand { get; set; }
	//      public bool? Umsatzsteuer_berechnen { get; set; }
	//      public string Versandart { get; set; }
	//      public double? Verzugszinsen { get; set; }
	//      public int? Verzugszinsen_ab_Mahnstufe { get; set; }
	//      public int? Währung { get; set; }
	//      public int? Zahlung_erwartet_nach { get; set; }
	//      public int? Zahlungskondition { get; set; }
	//      public int? Zahlungsmoral { get; set; }
	//      public string Zahlungsweise { get; set; }
	//      public double? Zielaufschlag { get; set; }
	//      public bool? Zolltarif_Nr { get; set; }

	//      public KundenEntity() { }
	//      public KundenEntity(DataRow dataRow)
	//      {
	//          Belegkreis = (dataRow["Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
	//          Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
	//          Branche = (dataRow["Branche"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Branche"]);
	//          Bruttofakturierung = (dataRow["Bruttofakturierung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bruttofakturierung"]);
	//          Debitoren_Nr = (dataRow["Debitoren-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Debitoren-Nr"]);
	//          EG_Identifikationsnummer = (dataRow["EG - Identifikationsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EG - Identifikationsnummer"]);
	//          Eilzuschlag = (dataRow["Eilzuschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Eilzuschlag"]);
	//          Factoring = (dataRow["Factoring"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Factoring"]);
	//          Fibu_rahmen = (dataRow["fibu_rahmen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["fibu_rahmen"]);
	//          gesperrt_für_weitere_Lieferungen = (dataRow["gesperrt für weitere Lieferungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gesperrt für weitere Lieferungen"]);
	//          Grund = (dataRow["Grund"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund"]);
	//          Grund_für_Sperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
	//          Karenztage = (dataRow["Karenztage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Karenztage"]);
	//          Konditionszuordnungs_Nr = (dataRow["Konditionszuordnungs-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Konditionszuordnungs-Nr"]);
	//          Kreditlimit = (dataRow["Kreditlimit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Kreditlimit"]);
	//          Kundengruppe = (dataRow["Kundengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundengruppe"]);
	//          Lieferantenummer_Kunden = (dataRow["Lieferantenummer (Kunden)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantenummer (Kunden)"]);
	//          Lieferscheinadresse = (dataRow["Lieferscheinadresse"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Lieferscheinadresse"]);
	//          LSADR = (dataRow["LSADR"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LSADR"]);
	//          LSADRANG = (dataRow["LSADRANG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRANG"]);
	//          LSADRAUF = (dataRow["LSADRAUF"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRAUF"]);
	//          LSADRGUT = (dataRow["LSADRGUT"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRGUT"]);
	//          LSADRPROF = (dataRow["LSADRPROF"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRPROF"]);
	//          LSADRRG = (dataRow["LSADRRG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRRG"]);
	//          LSADRSTO = (dataRow["LSADRSTO"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRSTO"]);
	//          LSRG = (dataRow["LSRG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSRG"]);
	//          Mahngebühr_1 = (dataRow["Mahngebühr 1"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mahngebühr 1"]);
	//          Mahngebühr_2 = (dataRow["Mahngebühr 2"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mahngebühr 2"]);
	//          Mahngebühr_3 = (dataRow["Mahngebühr 3"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mahngebühr 3"]);
	//          Mahnsperre = (dataRow["Mahnsperre"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mahnsperre"]);
	//          Mindermengenzuschlag = (dataRow["Mindermengenzuschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mindermengenzuschlag"]);
	//          Nr = Convert.ToInt32(dataRow["Nr"]);
	//          Nummer = (dataRow["nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nummer"]);
	//          OPOS = (dataRow["OPOS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OPOS"]);
	//          Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
	//          Preisgruppe2 = (dataRow["Preisgruppe2"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe2"]);
	//          Rabattgruppe = (dataRow["Rabattgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Rabattgruppe"]);
	//          Regelmäßig_anschreiben = (dataRow["Regelmäßig anschreiben ?"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Regelmäßig anschreiben ?"]);
	//          RG_Abteilung = (dataRow["RG-Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RG-Abteilung"]);
	//          RG_Land_PLZ_ORT = (dataRow["RG-Land/PLZ/ORT"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RG-Land/PLZ/ORT"]);
	//          RG_Strasse_Postfach = (dataRow["RG-Strasse-Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RG-Strasse-Postfach"]);
	//          Sprache = (dataRow["Sprache"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Sprache"]);
	//          Standardversand = (dataRow["Standardversand"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardversand"]);
	//          Umsatzsteuer_berechnen = (dataRow["Umsatzsteuer berechnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Umsatzsteuer berechnen"]);
	//          Versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
	//          Verzugszinsen = (dataRow["Verzugszinsen"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Verzugszinsen"]);
	//          Verzugszinsen_ab_Mahnstufe = (dataRow["Verzugszinsen ab Mahnstufe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verzugszinsen ab Mahnstufe"]);
	//          Währung = (dataRow["Währung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Währung"]);
	//          Zahlung_erwartet_nach = (dataRow["Zahlung erwartet nach"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Zahlung erwartet nach"]);
	//          Zahlungskondition = (dataRow["Zahlungskondition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Zahlungskondition"]);
	//          Zahlungsmoral = (dataRow["Zahlungsmoral"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Zahlungsmoral"]);
	//          Zahlungsweise = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
	//          Zielaufschlag = (dataRow["Zielaufschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Zielaufschlag"]);
	//          Zolltarif_Nr = (dataRow["zolltarif_nr"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["zolltarif_nr"]);
	//      }

	//  }


	public class KundenEntity
	{
		public int? Belegkreis { get; set; }
		public string Bemerkungen { get; set; }
		public string Branche { get; set; }
		public bool? Bruttofakturierung { get; set; }
		public string Debitoren_Nr { get; set; }
		public string EG___Identifikationsnummer { get; set; }
		public double? Eilzuschlag { get; set; }
		public bool? Factoring { get; set; }
		public int? fibu_rahmen { get; set; }
		public bool? gesperrt_für_weitere_Lieferungen { get; set; }
		public string Grund { get; set; }
		public string Grund_für_Sperre { get; set; }
		public int? Karenztage { get; set; }
		public int? Konditionszuordnungs_Nr { get; set; }
		public double? Kreditlimit { get; set; }
		public string Kundengruppe { get; set; }
		public string Lieferantenummer__Kunden_ { get; set; }
		public bool? Lieferscheinadresse { get; set; }
		public int? LSADR { get; set; }
		public bool? LSADRANG { get; set; }
		public bool? LSADRAUF { get; set; }
		public bool? LSADRGUT { get; set; }
		public bool? LSADRPROF { get; set; }
		public bool? LSADRRG { get; set; }
		public bool? LSADRSTO { get; set; }
		public bool? LSRG { get; set; }
		public double? Mahngebühr_1 { get; set; }
		public double? Mahngebühr_2 { get; set; }
		public double? Mahngebühr_3 { get; set; }
		public bool? Mahnsperre { get; set; }
		public double? Mindermengenzuschlag { get; set; }
		public int Nr { get; set; }
		public int? Nummer { get; set; }
		public bool? OPOS { get; set; }
		public int? Preisgruppe { get; set; }
		public int? Preisgruppe2 { get; set; }
		public int? Rabattgruppe { get; set; }
		public bool? Regelmäßig_anschreiben__ { get; set; }
		public string RG_Abteilung { get; set; }
		public string RG_Land_PLZ_ORT { get; set; }
		public string RG_Strasse_Postfach { get; set; }
		public int? Sprache { get; set; }
		public string Standardversand { get; set; }
		public bool? Umsatzsteuer_berechnen { get; set; }
		public string Versandart { get; set; }
		public double? Verzugszinsen { get; set; }
		public int? Verzugszinsen_ab_Mahnstufe { get; set; }
		public int? Währung { get; set; }
		public int? Zahlung_erwartet_nach { get; set; }
		public int? Zahlungskondition { get; set; }
		public int? Zahlungsmoral { get; set; }
		public string Zahlungsweise { get; set; }
		public double? Zielaufschlag { get; set; }
		public bool? zolltarif_nr { get; set; }

		public KundenEntity() { }

		public KundenEntity(DataRow dataRow)
		{
			Belegkreis = (dataRow["Belegkreis"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Belegkreis"]);
			Bemerkungen = (dataRow["Bemerkungen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkungen"]);
			Branche = (dataRow["Branche"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Branche"]);
			Bruttofakturierung = (dataRow["Bruttofakturierung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bruttofakturierung"]);
			Debitoren_Nr = (dataRow["Debitoren-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Debitoren-Nr"]);
			EG___Identifikationsnummer = (dataRow["EG - Identifikationsnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EG - Identifikationsnummer"]);
			Eilzuschlag = (dataRow["Eilzuschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Eilzuschlag"]);
			Factoring = (dataRow["Factoring"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Factoring"]);
			fibu_rahmen = (dataRow["fibu_rahmen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["fibu_rahmen"]);
			gesperrt_für_weitere_Lieferungen = (dataRow["gesperrt für weitere Lieferungen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["gesperrt für weitere Lieferungen"]);
			Grund = (dataRow["Grund"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund"]);
			Grund_für_Sperre = (dataRow["Grund für Sperre"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund für Sperre"]);
			Karenztage = (dataRow["Karenztage"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Karenztage"]);
			Konditionszuordnungs_Nr = (dataRow["Konditionszuordnungs-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Konditionszuordnungs-Nr"]);
			Kreditlimit = (dataRow["Kreditlimit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Kreditlimit"]);
			Kundengruppe = (dataRow["Kundengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kundengruppe"]);
			Lieferantenummer__Kunden_ = (dataRow["Lieferantenummer (Kunden)"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantenummer (Kunden)"]);
			Lieferscheinadresse = (dataRow["Lieferscheinadresse"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Lieferscheinadresse"]);
			LSADR = (dataRow["LSADR"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LSADR"]);
			LSADRANG = (dataRow["LSADRANG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRANG"]);
			LSADRAUF = (dataRow["LSADRAUF"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRAUF"]);
			LSADRGUT = (dataRow["LSADRGUT"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRGUT"]);
			LSADRPROF = (dataRow["LSADRPROF"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRPROF"]);
			LSADRRG = (dataRow["LSADRRG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRRG"]);
			LSADRSTO = (dataRow["LSADRSTO"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSADRSTO"]);
			LSRG = (dataRow["LSRG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["LSRG"]);
			Mahngebühr_1 = (dataRow["Mahngebühr 1"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mahngebühr 1"]);
			Mahngebühr_2 = (dataRow["Mahngebühr 2"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mahngebühr 2"]);
			Mahngebühr_3 = (dataRow["Mahngebühr 3"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mahngebühr 3"]);
			Mahnsperre = (dataRow["Mahnsperre"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Mahnsperre"]);
			Mindermengenzuschlag = (dataRow["Mindermengenzuschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Mindermengenzuschlag"]);
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Nummer = (dataRow["nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nummer"]);
			OPOS = (dataRow["OPOS"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["OPOS"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			Preisgruppe2 = (dataRow["Preisgruppe2"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe2"]);
			Rabattgruppe = (dataRow["Rabattgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Rabattgruppe"]);
			Regelmäßig_anschreiben__ = (dataRow["Regelmäßig anschreiben ?"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Regelmäßig anschreiben ?"]);
			RG_Abteilung = (dataRow["RG-Abteilung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RG-Abteilung"]);
			RG_Land_PLZ_ORT = (dataRow["RG-Land/PLZ/ORT"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RG-Land/PLZ/ORT"]);
			RG_Strasse_Postfach = (dataRow["RG-Strasse-Postfach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RG-Strasse-Postfach"]);
			Sprache = (dataRow["Sprache"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Sprache"]);
			Standardversand = (dataRow["Standardversand"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standardversand"]);
			Umsatzsteuer_berechnen = (dataRow["Umsatzsteuer berechnen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Umsatzsteuer berechnen"]);
			Versandart = (dataRow["Versandart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Versandart"]);
			Verzugszinsen = (dataRow["Verzugszinsen"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Verzugszinsen"]);
			Verzugszinsen_ab_Mahnstufe = (dataRow["Verzugszinsen ab Mahnstufe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verzugszinsen ab Mahnstufe"]);
			Währung = (dataRow["Währung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Währung"]);
			Zahlung_erwartet_nach = (dataRow["Zahlung erwartet nach"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Zahlung erwartet nach"]);
			Zahlungskondition = (dataRow["Zahlungskondition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Zahlungskondition"]);
			Zahlungsmoral = (dataRow["Zahlungsmoral"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Zahlungsmoral"]);
			Zahlungsweise = (dataRow["Zahlungsweise"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zahlungsweise"]);
			Zielaufschlag = (dataRow["Zielaufschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Zielaufschlag"]);
			zolltarif_nr = (dataRow["zolltarif_nr"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["zolltarif_nr"]);
		}
	}
}
