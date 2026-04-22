using System.Collections.Generic;

namespace Psz.Core.Logistics.Reporting.Models
{
	public class ChoosePackingReportModel
	{

		public List<HeaderReportModel> Headers { get; set; }
		public List<PackingReportDetailsModel> Details { get; set; }
	}
	public class HeaderReportModel
	{
		public string datum { get; set; }
		public string verpackung { get; set; }
		public string kunde { get; set; }
		public string lStrassePostfach { get; set; }
		public string lName2 { get; set; }
		public string lName3 { get; set; }
		public string lAnsprechpartner { get; set; }
		public string lAbteilung { get; set; }
		public decimal gesammtPreis { get; set; }
		public string lLandPLZOrt { get; set; }



		public HeaderReportModel()
		{

		}

		public HeaderReportModel(string datum, string verpackung, string kunde, string lStrassePostfach, string lName2, string lName3, string lAnsprechpartner, string lAbteilung, string lLandPLZOrt)
		{
			this.datum = datum;
			this.verpackung = verpackung;
			this.kunde = kunde;
			this.lStrassePostfach = lStrassePostfach;
			this.lName2 = lName2;
			this.lName3 = lName3;
			this.lAnsprechpartner = lAnsprechpartner;
			this.lAbteilung = lAbteilung;
			this.lLandPLZOrt = lLandPLZOrt;
		}
		public HeaderReportModel(string datum, string verpackung, string kunde, decimal gesammtPreis)
		{
			this.datum = datum;
			this.verpackung = verpackung;
			this.kunde = kunde;
			this.gesammtPreis = gesammtPreis;

		}
	}



	public class PackingReportDetailsModel
	{
		public string datum { get; set; }
		public string kunde { get; set; }
		public string verpackung { get; set; }
		public string lAnrede { get; set; }
		public string lStrassePostfach { get; set; }
		public string lName2 { get; set; }
		public string lName3 { get; set; }
		public string lAnsprechpartner { get; set; }
		public string lAbteilung { get; set; }
		public string lLandPLZOrt { get; set; }
		public long lsNumber { get; set; }
		public int anzahl { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung { get; set; }
		public string lagerort { get; set; }
		public string bestellnummer { get; set; }
		public string bezug { get; set; }
		public string bemerkung { get; set; }
		public int verpackungsmenge { get; set; }
		public string verpackungsart { get; set; }
		public decimal gewicht { get; set; }
		public decimal gesammtGewicht { get; set; }
		public decimal gesammtPreis { get; set; }
		public string versandinfo_von_CS { get; set; }


		public PackingReportDetailsModel()
		{

		}
		public PackingReportDetailsModel(Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity entity)
		{
			this.datum = entity.versanddatum_Auswahl != null ? entity.versanddatum_Auswahl.Value.ToString("dd-MM-yyyy") : "";
			this.kunde = entity.lVornameNameFirma ?? "";
			this.verpackung = entity.versandarten_Auswahl ?? "";
			this.lAnrede = entity.lAnrede;
			this.lStrassePostfach = entity.lStrassePostfach;
			this.lName2 = entity.lName2;
			this.lName3 = entity.lName3;
			this.lAnsprechpartner = entity.lAnsprechpartner;
			this.lAbteilung = entity.lAbteilung;
			this.lLandPLZOrt = entity.lLandPLZORT;
			this.lsNumber = entity.angeboteNr;
			this.anzahl = entity.anzahl;
			this.artikelnummer = entity.artikelnummer;
			this.bezeichnung = entity.bezeichnung1;
			this.lagerort = entity.lagerort;
			this.bestellnummer = entity.bestellnummer;
			this.bezug = entity.bezug;
			this.bemerkung = entity.bestellnummer;
			this.verpackungsart = entity.verpackungsart ?? "";
			this.verpackungsmenge = entity.verpackungsmenge ?? 0;
			this.gewicht = entity.verpackungsart == null || entity.verpackungsart == "" || entity.verpackungsart.ToLower().Contains("gitterbox") || entity.verpackungsart.ToLower().Contains("um-") ? entity.gewichtArtikel * entity.anzahl / 1000 : entity.anzahl <= this.verpackungsmenge ? entity.gewichtArtikel * entity.anzahl / 1000 + (decimal)(entity.verpackungGewicht == null ? 0 : entity.verpackungGewicht) / 1000 : entity.verpackungsmenge == null ? -1001 : (decimal)entity.gewichtArtikel * entity.anzahl / 1000 + (decimal)(entity.verpackungGewicht == null ? 0 : (decimal)entity.verpackungGewicht / 1000 * (this.verpackungsmenge == 0 ? 0 : (decimal)entity.anzahl / (decimal)entity.verpackungsmenge));
			this.gesammtGewicht = entity.verpackungsart == null || entity.verpackungsart == "" || entity.verpackungsart.ToLower().Contains("gitterbox") || entity.verpackungsart.ToLower().Contains("um-") ? entity.gewichtArtikel * entity.anzahl / 1000 : entity.anzahl <= this.verpackungsmenge ? entity.gewichtArtikel * entity.anzahl / 1000 + (decimal)(entity.verpackungGewicht == null ? 0 : entity.verpackungGewicht) / 1000 : entity.verpackungsmenge == null ? 0 : (decimal)entity.gewichtArtikel * entity.anzahl / 1000 + (decimal)(entity.verpackungGewicht == null ? 0 : (decimal)entity.verpackungGewicht / 1000 * (this.verpackungsmenge == 0 ? 0 : (decimal)entity.anzahl / (decimal)entity.verpackungsmenge));
			this.gesammtPreis = entity.anzahl * entity.preis;
			this.versandinfo_von_CS = entity.versandinfo_von_CS;


		}

		public PackingReportDetailsModel(string datum, string verpackungsart, string kunde, long lsNumber, string artikelnummer)
		{
			this.datum = datum;
			this.kunde = kunde;
			this.verpackung = verpackungsart;
			this.lsNumber = lsNumber;
			this.artikelnummer = artikelnummer;
		}
	}
}
