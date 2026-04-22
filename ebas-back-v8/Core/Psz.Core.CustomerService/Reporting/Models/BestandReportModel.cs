using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Reporting.Models
{
	public class BestandReportModel
	{
		public List<BestandReportContactModel> Contact { get; set; }
		public List<BestandReportClientsModel> Clients { get; set; }
		public List<BestandReportLagerModel> Lager { get; set; }
		public List<BestandReportDetailsModel> Details { get; set; }
	}
	public class BestandReportContactModel
	{
		public string Contact { get; set; }
		public Decimal SumWert { get; set; }
		public Decimal SumVK { get; set; }

		public BestandReportContactModel()
		{

		}

		public BestandReportContactModel(string contact, decimal sumWert, decimal sumVK)
		{
			Contact = contact;
			SumWert = sumWert;
			SumVK = sumVK;
		}
	}
	public class BestandReportClientsModel
	{
		public string Kunde { get; set; }
		public string Contact { get; set; }
		public Decimal SumWert { get; set; }
		public Decimal SumVK { get; set; }

		public BestandReportClientsModel(string client, string contact, decimal sumwert, decimal sumvk)
		{
			Kunde = client;
			Contact = contact;
			SumWert = sumwert;
			SumVK = sumvk;
		}
	}
	public class BestandReportDetailsModel
	{
		public Decimal Wert { get; set; }
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }
		public string CS_Kontakt { get; set; }
		public int Lagerort_id { get; set; }
		public int Bestand { get; set; }
		public string Lagerort { get; set; }
		public Decimal VK { get; set; }
		public Decimal Materialkosten { get; set; }
		public Decimal Arbeitskosten { get; set; }
		//
		public string Bezeichnung_1 { get; set; }

		public BestandReportDetailsModel()
		{

		}

		public BestandReportDetailsModel(Infrastructure.Data.Entities.Joins.CTS.BestandCSProEntity entity)
		{
			Wert = entity.Wert ?? 0;
			Artikelnummer = entity.Artikelnummer;
			Kunde = entity.Kunde;
			CS_Kontakt = entity.CS_Kontakt;
			Lagerort_id = entity.Lagerort_id ?? 0;
			Bestand = entity.Bestand ?? 0;
			Lagerort = entity.Lagerort;
			VK = entity.VK ?? 0;
			Materialkosten = entity.Materialkosten ?? 0;
			Arbeitskosten = entity.Arbeitskosten ?? 0;
		}

		public BestandReportDetailsModel(Infrastructure.Data.Entities.Joins.CTS.BestandAusenlagerEntity entity)
		{
			Wert = entity.Wert ?? 0;
			Artikelnummer = entity.Artikelnummer;
			Kunde = entity.Kunde;
			CS_Kontakt = entity.CS_Kontakt;
			Lagerort_id = entity.Lagerort_id ?? 0;
			Bestand = entity.Bestand ?? 0;
			Lagerort = entity.Lagerort;
			VK = entity.VK ?? 0;
			Materialkosten = entity.Materialkosten ?? 0;
			Arbeitskosten = entity.Arbeitskosten ?? 0;
			Bezeichnung_1 = entity.Bezeichnung_1;
		}
	}
	public class BestandReportLagerModel
	{
		public string Lager { get; set; }
		public string Kunde { get; set; }
		public string Contact { get; set; }
		public Decimal SumWert { get; set; }
		public Decimal SumVK { get; set; }

		public BestandReportLagerModel(string lager, string kunde, string contact, decimal sumWert, decimal sumVK)
		{
			Lager = lager;
			Kunde = kunde;
			Contact = contact;
			SumWert = sumWert;
			SumVK = sumVK;
		}
	}
}
