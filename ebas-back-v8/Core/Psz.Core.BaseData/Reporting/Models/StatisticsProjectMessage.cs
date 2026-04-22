using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Reporting.Models
{
	public class StatisticsProjectMessage
	{
		public Header ReportData { get; set; }
		public List<Item> Items { get; set; }

		// -
		public class Header
		{
			public string Title { get; set; }
			public string ReportDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
		}
		public class Item
		{
			public string AB_Datum { get; set; }
			public int Arbeitszeit_Serien_Pro_Kabesatz { get; set; }
			public string Artikelnummer { get; set; }
			public string Bemerkungen { get; set; }
			public int EAU { get; set; }
			public string EMPB { get; set; }
			public DateTime Erstanlage { get; set; }
			public string FA_Datum { get; set; }
			public int ID { get; set; }
			public string Kontakt_AV_PSZ { get; set; }
			public string Kontakt_CS_PSZ { get; set; }
			public string Kontakt_Technik_Kunde { get; set; }
			public string Kontakt_Technik_PSZ { get; set; }
			public int Kosten { get; set; }
			public string Krimp_WKZ { get; set; }
			public string Material_Eskalation_AV { get; set; }
			public string Material_Eskalation_Termin { get; set; }
			public string Material_Komplett { get; set; }
			public int Menge { get; set; }
			public int MOQ { get; set; }
			public string Projekt_betreung { get; set; }
			public string Projekt_Start { get; set; }
			public bool Projektmeldung { get; set; }
			public string Projekt_Nr { get; set; }
			public string Rapid_Prototyp { get; set; }
			public string Serie_PSZ { get; set; }
			public string SG_WKZ { get; set; }
			public string Standort_Muster { get; set; }
			public string Standort_Serie { get; set; }
			public string Summe_Arbeitszeit { get; set; }
			public string Termin_mit_Technik_abgesprochen { get; set; }
			public string TSP_Kunden { get; set; }
			public string Typ { get; set; }
			public string UL_Verpackung { get; set; }
			public string Wunschtermin_Kunde { get; set; }
			public string Zuschlag { get; set; }

			// 
			public string Kunde { get; set; }
			public string Kundenschlussel { get; set; }
		}
	}
}
