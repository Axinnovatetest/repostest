using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class FA_NPEX_ReportDetailsModel
	{
		public int Fertigungsnummer { get; set; }
		public string Kunde { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public int Anzahl { get; set; }
		public DateTime Termin_Fertigstellung { get; set; }
		public string Bemerkung { get; set; }
		public Decimal Preis { get; set; }
		public string Stucklisten_Artikelnummer { get; set; }
		public string Bezeichnung_des_Bauteils { get; set; }
		public int Artikel_Nr_des_Bauteils { get; set; }
		public Decimal Stucklisten_Anzahl { get; set; }
		public Decimal Bedarf { get; set; }
		public Decimal Bestand { get; set; }
		public string Freigabestatus { get; set; }
		public DateTime Termin_Bestätigt1 { get; set; }
		public bool Erstmuster { get; set; }
		public FA_NPEX_ReportDetailsModel()
		{

		}

	}
	public class FA_NPEX_ReportCustomersModel
	{
		public string Kunde { get; set; }
		public int Count { get; set; }
		public FA_NPEX_ReportCustomersModel(string kunde, int count)
		{
			Kunde = kunde;
			Count = count;
		}
	}
	public class FA_NPEX_ReportArticleModel
	{
		public string Kunde { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Freigabestatus { get; set; }
		public int Fertigungsnummer { get; set; }

		public FA_NPEX_ReportArticleModel(string kunde, string artikelnummer, string bezeichnung_1, string freigabestatus, int fertigungsnummer)
		{
			Kunde = kunde;
			Artikelnummer = artikelnummer;
			Bezeichnung_1 = bezeichnung_1;
			Freigabestatus = freigabestatus;
			Fertigungsnummer = fertigungsnummer;
		}
	}
	public class FA_NPEX_ReportOrderModel
	{
		public string Kunde { get; set; }
		public string Artikelnummer { get; set; }
		public int Fertigungsnummer { get; set; }
		public int Anzahl { get; set; }
		public DateTime Termin_Fertigstellung { get; set; }
		public DateTime Termin_Bestätigt1 { get; set; }
		public string Bemerkung { get; set; }
		public bool Erstmuster { get; set; }
		public Decimal Preis { get; set; }

		public FA_NPEX_ReportOrderModel(string kunde, string artikelnummer, int fertigungsnummer, int anzahl, DateTime termin_Fertigstellung, DateTime termin_Bestätigt1, string bemerkung, bool erstmuster, decimal preis)
		{
			Kunde = kunde;
			Artikelnummer = artikelnummer;
			Fertigungsnummer = fertigungsnummer;
			Anzahl = anzahl;
			Termin_Fertigstellung = termin_Fertigstellung;
			Termin_Bestätigt1 = termin_Bestätigt1;
			Bemerkung = bemerkung;
			Erstmuster = erstmuster;
			Preis = preis;
		}
	}
	public class FA_NPEX_ReportModel
	{
		public List<FA_NPEX_ReportDetailsModel> Details { get; set; }
		public List<FA_NPEX_ReportCustomersModel> Kunden { get; set; }
		public List<FA_NPEX_ReportArticleModel> Articles { get; set; }
		public List<FA_NPEX_ReportOrderModel> Orders { get; set; }
	}
	public class FA_NPEX_ReportEntryModel
	{
		public int Lager { get; set; }
		public string Kunde { get; set; }
		public List<string> Articles { get; set; }
	}
}
