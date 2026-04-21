using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class PSZ_Eingangskontrolle_TNEntity
	{
		public int? Aktiv { get; set; }
		public decimal? Akzeptierte_Menge { get; set; }
		public int? Anzahl_Verpackungen { get; set; }
		public string Artikelnummer { get; set; }
		public int? Bestellung_Nr { get; set; }
		public int? Clock_Number { get; set; }
		public DateTime? Datum { get; set; }
		public int? Eingangslieferscheinnr { get; set; }
		public decimal? Geprufte_Prufmenge { get; set; }
		public decimal? Gesamtmenge { get; set; }  
		public string Inspektor { get; set; }
		public string Kunde { get; set; }
		public int? LagerortID { get; set; }
		public int? Laufende_Nummer { get; set; }
		public decimal? Menge { get; set; }
		public DateTime? MHDDatum { get; set; }
		public int Nummer_Verpackung { get; set; }
		public bool? Prufentscheid { get; set; }
		public decimal? Prufmenge { get; set; }
		public decimal? Pruftiefe { get; set; }
		public decimal? Reklamierte_Menge { get; set; }
		public decimal? Restmenge_Rolle_PPS { get; set; }
		public string Resultat { get; set; }
		public int? Status_Rolle { get; set; }
		public int? Verpackungsnr { get; set; }
		public decimal? WE_Anzahl_VOH { get; set; }
		public DateTime? WE_Datum_VOH { get; set; }
		public string WE_LS_VOH { get; set; }

		public int? Nach_Lager { get; set; }
		public bool? Recieve { get; set; }

		public PSZ_Eingangskontrolle_TNEntity() { }

		public PSZ_Eingangskontrolle_TNEntity(DataRow dataRow)
		{
			Aktiv = (dataRow["Aktiv"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Aktiv"]);
			Akzeptierte_Menge = (dataRow["Akzeptierte_Menge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Akzeptierte_Menge"]);
			Anzahl_Verpackungen = (dataRow["Anzahl_Verpackungen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Anzahl_Verpackungen"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Clock_Number = (dataRow["Clock Number"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Clock Number"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Eingangslieferscheinnr = (dataRow["Eingangslieferscheinnr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Eingangslieferscheinnr"]);
			Geprufte_Prufmenge = (dataRow["Geprüfte_Prüfmenge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Geprüfte_Prüfmenge"]);
			Gesamtmenge = (dataRow["Gesamtmenge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Gesamtmenge"]);
			Inspektor = (dataRow["Inspektor"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Inspektor"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			LagerortID = (dataRow["LagerortID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerortID"]);
			Laufende_Nummer = (dataRow["Laufende Nummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Laufende Nummer"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			MHDDatum = (dataRow["MHDDatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MHDDatum"]);
			Nummer_Verpackung = Convert.ToInt32(dataRow["Nummer Verpackung"]);
			Prufentscheid = (dataRow["Prüfentscheid"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Prüfentscheid"]);
			Prufmenge = (dataRow["Prüfmenge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Prüfmenge"]);
			Pruftiefe = (dataRow["Prüftiefe"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Prüftiefe"]);
			Reklamierte_Menge = (dataRow["Reklamierte Menge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Reklamierte Menge"]);
			Restmenge_Rolle_PPS = (dataRow["Restmenge_Rolle_PPS"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Restmenge_Rolle_PPS"]);
			Resultat = (dataRow["Resultat"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Resultat"]);
			Status_Rolle = (dataRow["Status_Rolle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Status_Rolle"]);
			Verpackungsnr = (dataRow["Verpackungsnr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsnr"]);
			WE_Anzahl_VOH = (dataRow["WE_Anzahl_VOH"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["WE_Anzahl_VOH"]);
			WE_Datum_VOH = (dataRow["WE_Datum_VOH"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["WE_Datum_VOH"]);
			WE_LS_VOH = (dataRow["WE_LS_VOH"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WE_LS_VOH"]);
		}

		public PSZ_Eingangskontrolle_TNEntity ShallowClone()
		{
			return new PSZ_Eingangskontrolle_TNEntity
			{
				Aktiv = Aktiv,
				Akzeptierte_Menge = Akzeptierte_Menge,
				Anzahl_Verpackungen = Anzahl_Verpackungen,
				Artikelnummer = Artikelnummer,
				Bestellung_Nr = Bestellung_Nr,
				Clock_Number = Clock_Number,
				Datum = Datum,
				Eingangslieferscheinnr = Eingangslieferscheinnr,
				Geprufte_Prufmenge = Geprufte_Prufmenge,
				Gesamtmenge = Gesamtmenge,
				Inspektor = Inspektor,
				Kunde = Kunde,
				LagerortID = LagerortID,
				Laufende_Nummer = Laufende_Nummer,
				Menge = Menge,
				MHDDatum = MHDDatum,
				Nummer_Verpackung = Nummer_Verpackung,
				Prufentscheid = Prufentscheid,
				Prufmenge = Prufmenge,
				Pruftiefe = Pruftiefe,
				Reklamierte_Menge = Reklamierte_Menge,
				Restmenge_Rolle_PPS = Restmenge_Rolle_PPS,
				Resultat = Resultat,
				Status_Rolle = Status_Rolle,
				Verpackungsnr = Verpackungsnr,
				WE_Anzahl_VOH = WE_Anzahl_VOH,
				WE_Datum_VOH = WE_Datum_VOH,
				WE_LS_VOH = WE_LS_VOH
			};
		}

		public PSZ_Eingangskontrolle_TNEntity (Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity EntityJoin, LagerAccessEnum? choice)
		{

			Aktiv = EntityJoin.Aktiv;
			Akzeptierte_Menge = EntityJoin.Akzeptierte_Menge;
			Anzahl_Verpackungen = EntityJoin.Anzahl_Verpackungen;
			Artikelnummer = EntityJoin.Artikelnummer;
			Bestellung_Nr = EntityJoin.Bestellung_Nr;
			Clock_Number = EntityJoin.Clock_Number;
			Datum = EntityJoin.Datum;
			Eingangslieferscheinnr = EntityJoin.Eingangslieferscheinnr;
			Geprufte_Prufmenge = EntityJoin.Geprufte_Prufmenge;
			Gesamtmenge = EntityJoin.Gesamtmenge;
			Inspektor = EntityJoin.Inspektor;
			Kunde = EntityJoin.Kunde;
			LagerortID = EntityJoin.LagerortID;
			Laufende_Nummer = EntityJoin.Laufende_Nummer;
			Menge = EntityJoin.Menge;
			MHDDatum = EntityJoin.MHDDatum;
			Prufentscheid = EntityJoin.Prufentscheid;
			Prufmenge = EntityJoin.Prufmenge;
			Pruftiefe = EntityJoin.Pruftiefe;
			Reklamierte_Menge = EntityJoin.Reklamierte_Menge;
			Restmenge_Rolle_PPS = EntityJoin.Restmenge_Rolle_PPS;
			Resultat = EntityJoin.Resultat;
			Status_Rolle = EntityJoin.Status_Rolle;
			WE_Anzahl_VOH = EntityJoin.WE_Anzahl_VOH;
			WE_Datum_VOH = EntityJoin.WE_Datum_VOH;
			WE_LS_VOH = EntityJoin.WE_LS_VOH;
			if(choice == LagerAccessEnum.TN || choice == LagerAccessEnum.BETN || choice == LagerAccessEnum.WS || choice == LagerAccessEnum.GZTN)
			{
				Nummer_Verpackung = EntityJoin.Nummer_Verpackung;
			}
			else
			{
				Verpackungsnr = EntityJoin.Verpackungsnr;
			}
		}
	}

	public class PSZ_Eingangskontrolle_TNMinimalEntity
	{
		public int? Aktiv { get; set; }
		public string Artikelnummer { get; set; }
		public int? Gesamtmenge { get; set; }
		public decimal? Menge { get; set; }
		public int Nummer_Verpackung { get; set; }
		public decimal? Restmenge_Rolle_PPS { get; set; }
		public int? Status_Rolle { get; set; }
		public int? Verpackungsnr { get; set; }
		//Add 04-04-2025 By Ridha
		public string Inspector { get; set; }
		public string Remarque { get; set; }
		public DateTime? Datum { get; set; }

		public PSZ_Eingangskontrolle_TNMinimalEntity() { }

		public PSZ_Eingangskontrolle_TNMinimalEntity(DataRow dataRow)
		{
			Aktiv = (dataRow["Aktiv"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Aktiv"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Gesamtmenge = (dataRow["Gesamtmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Gesamtmenge"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			Nummer_Verpackung = Convert.ToInt32(dataRow["Verpackungsnr"]);
			Restmenge_Rolle_PPS = (dataRow["Restmenge_Rolle_PPS"] == System.DBNull.Value) ? Convert.ToDecimal(dataRow["Menge"]) : Convert.ToDecimal(dataRow["Restmenge_Rolle_PPS"]);
			Status_Rolle = (dataRow["Status_Rolle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Status_Rolle"]);
			Verpackungsnr = (dataRow["Verpackungsnr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsnr"]);
			Inspector = (dataRow["Inspektor"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Inspektor"]);
			Remarque = (dataRow["Resultat"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Resultat"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
		}
		public PSZ_Eingangskontrolle_TNMinimalEntity(DataRow dataRow, LagerAccessEnum choice)
		{
			Aktiv = (dataRow["Aktiv"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Aktiv"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Gesamtmenge = (dataRow["Gesamtmenge"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Gesamtmenge"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			Restmenge_Rolle_PPS = (dataRow["Restmenge_Rolle_PPS"] == System.DBNull.Value) ? Convert.ToDecimal(dataRow["Menge"]) : Convert.ToDecimal(dataRow["Restmenge_Rolle_PPS"]);
			Status_Rolle = (dataRow["Status_Rolle"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Status_Rolle"]);
			
			if(choice == LagerAccessEnum.TN || choice == LagerAccessEnum.BETN || choice == LagerAccessEnum.WS || choice == LagerAccessEnum.GZTN)
			{
				Nummer_Verpackung = Convert.ToInt32(dataRow["Verpackungsnr"]);
			}
			else
			{
				Verpackungsnr = (dataRow["Verpackungsnr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Verpackungsnr"]);
			}
		}
	}


	public enum LagerAccessEnum
	{
		[Description("TN")]
		TN = 7,
		[Description("BE_TN")]
		BETN = 60,
		[Description("GZ-TN")]
		GZTN = 102,
		[Description("WS")]
		WS = 42,
		[Description("Albanien")]
		Albanien = 26,
		[Description("Eigenfertigung")]
		Eigenfertigung = 6,
		[Description("Fertigung D")]
		FertigungD = 15
	}
}

