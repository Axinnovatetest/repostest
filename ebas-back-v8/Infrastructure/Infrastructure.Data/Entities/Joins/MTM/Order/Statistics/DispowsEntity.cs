using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class Dispows120Entity
	{
		/*
1	Name1		
2	Stücklisten_Artikelnummer		
3	Bezeichnung des Bauteils		
4	SummevonBruttobedarf		
5	MaxvonTermin_Materialbedarf		
6	Bestand		
7	[Bestand]-[SummevonBruttobedarf]		
8	Mindestbestellmenge		
9	Lagerort		
10	Lagerort_id		
11	Rahmen-Nr		
12	Rahmenmenge		
13	Rahmenauslauf		
14	Wenn([Betand_Obsolete]>0;Wahr;Falsch)		
*/
		public string Name1 { get; set; } //
		public string Stücklisten_Artikelnummer { get; set; }//
		public string Bezeichnung { get; set; }//
		public double SummevonBruttobedarf { get; set; } //
		public DateTime? MaxvonTermin_Materialbedarf { get; set; }
		public DateTime? Rahmenauslauf { get; set; }
		public double Bestand { get; set; }//
		public double Differenz { get; set; } //
		public double Mindestbestellmenge { get; set; } //
		public string Lagerort { get; set; }//
		public int Lagerort_id { get; set; } //
		public int ArtikelNr { get; set; } //
		public int TotalCount { get; set; } //
		public string Rahmen_Nr { get; set; } //
		public int? Rahmenmenge { get; set; }
		public Boolean obsolet { get; set; }
		public Dispows120Entity(DataRow dataRow)
		{
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lagerort_id"].ToString());
			ArtikelNr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Artikel-Nr"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Rahmenmenge"].ToString());
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : dataRow["Name1"].ToString();
			Stücklisten_Artikelnummer = (dataRow["Stücklisten_Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Stücklisten_Artikelnummer"].ToString();
			Bezeichnung = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung des Bauteils"].ToString();
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : dataRow["Lagerort"].ToString();
			Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : dataRow["Rahmen-Nr"].ToString();
			SummevonBruttobedarf = (dataRow["SummevonBruttobedarf"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["SummevonBruttobedarf"].ToString());
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Bestand"].ToString());
			Differenz = (dataRow["Differenz"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Differenz"].ToString());
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Mindestbestellmenge"].ToString());
			MaxvonTermin_Materialbedarf = (dataRow["MaxvonTermin_Materialbedarf"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["MaxvonTermin_Materialbedarf"].ToString());
			Rahmenauslauf = (dataRow["Rahmenauslauf"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Rahmenauslauf"].ToString());
			obsolet = (dataRow["obsolet"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["obsolet"].ToString());
		}
	}
	public class Dispows40Entity
	{
		/*
1	Name1		
2	Stücklisten_Artikelnummer		
3	Bezeichnung des Bauteils		
4	SummevonBruttobedarf		
5	MaxvonTermin_Materialbedarf		
6	Bestand		
7	[Bestand]-[SummevonBruttobedarf]		
8	Mindestbestellmenge		
9	Lagerort		
10	Lagerort_id		
11	Rahmen-Nr		
12	Rahmenmenge		
13	Rahmenauslauf		
14	Wenn([Betand_Obsolete]>0;Wahr;Falsch)		
*/
		public string Name1 { get; set; } //
		public string Stücklisten_Artikelnummer { get; set; }//
		public double SummevonBruttobedarf { get; set; } //
		public DateTime? MaxvonTermin_Materialbedarf { get; set; }
		public double Bestand { get; set; }//
		public double Differenz { get; set; } //
		public double Mindestbestellmenge { get; set; } //
		public string Lagerort { get; set; }//
		public int Lagerort_id { get; set; } //
		public int TotalCount { get; set; } //
		public string Rahmen_Nr { get; set; } //
		public int? Rahmenmenge { get; set; }
		public Boolean obsolet { get; set; }
		public Dispows40Entity(DataRow dataRow)
		{
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lagerort_id"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Rahmenmenge = (dataRow["Rahmenmenge"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Rahmenmenge"].ToString());
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : dataRow["Name1"].ToString();
			Stücklisten_Artikelnummer = (dataRow["Stücklisten_Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Stücklisten_Artikelnummer"].ToString();
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : dataRow["Lagerort"].ToString();
			Rahmen_Nr = (dataRow["Rahmen-Nr"] == System.DBNull.Value) ? "" : dataRow["Rahmen-Nr"].ToString();
			SummevonBruttobedarf = (dataRow["SummevonBruttobedarf"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["SummevonBruttobedarf"].ToString());
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Bestand"].ToString());
			Differenz = (dataRow["Differenz"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Differenz"].ToString());
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Mindestbestellmenge"].ToString());
			MaxvonTermin_Materialbedarf = (dataRow["MaxvonTermin_Materialbedarf"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["MaxvonTermin_Materialbedarf"].ToString());
			obsolet = (dataRow["obsolet"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["obsolet"].ToString());
		}
	}
	public class Dispows120DetailsEntity
	{
		/*
		1	PSZ#	
		2	Bezeichnung des Bauteils	
		*/
		public string PSZ { get; set; } //
		public string Bezeichnung_des_Bauteils { get; set; }//

		public Dispows120DetailsEntity(DataRow dataRow)
		{

			PSZ = (dataRow["PSZ"] == System.DBNull.Value) ? "" : dataRow["PSZ"].ToString();
			Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung des Bauteils"].ToString();

		}
	}
	public class Dispows120DetailsBestandEntity
	{
		/*
		3	Lagerort_id
		4	Lagerort
		5	Lagerort
		*/
		public int Lagerort_id { get; set; } //
		public string Lagerort { get; set; }//
		public double Bestand { get; set; } //

		public Dispows120DetailsBestandEntity(DataRow dataRow)
		{
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lagerort_id"].ToString());
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Bestand"].ToString());
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : dataRow["Lagerort"].ToString();

		}
	}
	public class Dispows120DetailsLieferantenEntity
	{
		/*
		6	Name1
		7	Standardlieferant
		8	Wiederbeschaffungszeitraum
		9	Adresse
		10	Bestell-Nr
		11	Verpackungseinheit
		12	Telefon
		13	Einkaufspreis
		14	Mindestbestellmenge
		*/

		public string Name1 { get; set; }//
		public string Adresse { get; set; }//
		public bool Standardlieferant { get; set; }//
		public int Wiederbeschaffungszeitraum { get; set; }//
		public int TotalCount { get; set; }//
		public string Bestell_Nr { get; set; }//
		public int Verpackungseinheit { get; set; }//
		public string Telefon { get; set; }//
		public double Einkaufspreis { get; set; } //
		public double Mindestbestellmenge { get; set; } //
		public int Lieferanten_Nr { get; set; } //

		public Dispows120DetailsLieferantenEntity(DataRow dataRow)
		{
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Mindestbestellmenge"].ToString());
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Einkaufspreis"].ToString());
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : dataRow["Telefon"].ToString();
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : dataRow["Name1"].ToString();
			Adresse = (dataRow["Adresse"] == System.DBNull.Value) ? "" : dataRow["Adresse"].ToString();
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : dataRow["Bestell-Nr"].ToString();
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Standardlieferant"].ToString());
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Verpackungseinheit = (dataRow["Verpackungseinheit"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Verpackungseinheit"].ToString());
			Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lieferanten-Nr"].ToString());
		}
	}
	public class Dispows120DetailsOffenBestellungenEntity
	{
		/*
		15	Lagerort_id --
		16	Bestellung-Nr --
		17	Rahmenbestellung --
		18	Bestellmenge --
		19	Offen --
		20	Liefertermin --
		21	Bestätigter_Termin --
		22	AB-Nr_Lieferant --
		23	Einzelpreis
		Nr
		*/
		public int Lagerort_id { get; set; } //
		public int Bestellung_Nr { get; set; } //
		public bool Rahmenbestellung { get; set; } //
		public string AB_Nr_Lieferant { get; set; } //
		public int Nr { get; set; }//
		public double Bestellmenge { get; set; } //
		public double Offen { get; set; } //
		public double Einzelpreis { get; set; } //
		public DateTime? Liefertermin { get; set; } //
		public DateTime? Bestätigter_Termin { get; set; } //
		public int Lieferanten_Nr { get; set; } //
		public int TotalCount { get; set; } //

		public Dispows120DetailsOffenBestellungenEntity(DataRow dataRow)
		{
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lagerort_id"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Bestellung-Nr"].ToString());
			Rahmenbestellung = (dataRow["Rahmenbestellung"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["Rahmenbestellung"].ToString());
			AB_Nr_Lieferant = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : dataRow["AB-Nr_Lieferant"].ToString();
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Nr"].ToString());
			Bestellmenge = (dataRow["Bestellmenge"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Bestellmenge"].ToString());
			Offen = (dataRow["Offen"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Offen"].ToString());
			Einzelpreis = (dataRow["Einzelpreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Einzelpreis"].ToString());
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Liefertermin"].ToString());
			Bestätigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Bestätigter_Termin"].ToString());
			Lieferanten_Nr = (dataRow["Lieferanten-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Lieferanten-Nr"].ToString());
		}
	}
	public class Dispows120DetailsBedarfeEntity
	{
		/*
		24	Termin_Bestätigt1	
		25	Fertigungsnummer	
		26	Artikel_Artikelnummer	
		27	Bezeichnung 1	
		28	Fertigung_Anzahl	
		29	Stücklisten_Anzahl	
		30	Bruttobedarf	
		31	Termin_Materialbedarf	
		32	Laufende Summe	
		33	[Bestand]-[Laufende Summe]	
		*/
		public string Psz { get; set; } //
		public string Artikel_Nr_des_Bauteils { get; set; } //
		public string Artikel_Artikelnummer { get; set; } //
		public string Fertigungsnummer { get; set; } //
		public string Bezeichnung1 { get; set; } //
		public double Fertigung_anzahl { get; set; } //
		public double Stücklisten_Anzahl { get; set; } //
		public double Verfügbar { get; set; } //
		public double Bruttobedarf { get; set; } //
		public double Bestand { get; set; } //
		public double Laufende_Summe { get; set; } //
		public int TotalCount { get; set; } //
		public DateTime? Termin_Materialbedarf { get; set; } //
		public DateTime? Termin_Bestätigt1 { get; set; } //

		public Dispows120DetailsBedarfeEntity(DataRow dataRow)
		{
			Psz = (dataRow["Psz"] == System.DBNull.Value) ? "" : dataRow["Psz"].ToString();
			Artikel_Nr_des_Bauteils = (dataRow["Artikel_Nr_des_Bauteils"] == System.DBNull.Value) ? "" : dataRow["Artikel_Nr_des_Bauteils"].ToString();
			Artikel_Artikelnummer = (dataRow["Artikel_Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikel_Artikelnummer"].ToString();
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? "" : dataRow["Fertigungsnummer"].ToString();
			Bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : dataRow["Bezeichnung1"].ToString();
			Fertigung_anzahl = (dataRow["Fertigung_anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Fertigung_anzahl"].ToString());
			Verfügbar = (dataRow["Verfügbar"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Verfügbar"].ToString());
			Stücklisten_Anzahl = (dataRow["Stücklisten_Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Stücklisten_Anzahl"].ToString());
			Bruttobedarf = (dataRow["Bruttobedarf"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Bruttobedarf"].ToString());
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Bestand"].ToString());
			Laufende_Summe = (dataRow["Laufende_Summe"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Laufende_Summe"].ToString());
			Termin_Materialbedarf = (dataRow["Termin_Materialbedarf"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Termin_Materialbedarf"].ToString());
			Termin_Bestätigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["TotalCount"].ToString());
		}
	}
	public class DispowsDetailsCurrencyEntity
	{
		/*
		currency
		*/
		public string Symbol { get; set; } //
		public int Nr { get; set; } //

		public DispowsDetailsCurrencyEntity(DataRow dataRow)
		{
			Symbol = (dataRow["Symbol"] == System.DBNull.Value) ? "" : dataRow["Symbol"].ToString();
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Nr"].ToString());
		}
	}



}
