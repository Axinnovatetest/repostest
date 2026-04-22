using System;

namespace Psz.Core.Logistics.Reporting.Models
{
	public class VDAEtikettenModel
	{
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung1BarreCode { get; set; }
		public string bezeichnung2 { get; set; }
		public string customerItemNumber { get; set; }
		public decimal groesse { get; set; }
		public decimal gewicht { get; set; }
		public string lVornameNameFirma { get; set; }
		public string lStrassePostfach { get; set; }
		public string lLandPLZORT { get; set; }
		public string verpackungsart { get; set; }
		public int verpackungsmenge { get; set; }
		public string abladestelle { get; set; }
		public bool packstatus { get; set; }
		public string liefertermin { get; set; }
		public int anzahl { get; set; }
		public string anzahlBarreCode { get; set; }
		public string bezug { get; set; }
		public string ihrZeichen { get; set; }
		public long angeboteNr { get; set; }
		public string angeboteNrBarreCode { get; set; }
		public string index_Kunde { get; set; }
		public bool versand_gedruckt { get; set; }
		public long nrAngeboteArtikel { get; set; }
		public bool vDAGedruckt { get; set; }
		public VDAEtikettenModel()
		{
		}
		public VDAEtikettenModel(Infrastructure.Data.Entities.Joins.Logistics.VDAEntity lsDetailsEntity)
		{
			if(lsDetailsEntity != null)
			{
				artikelnummer = lsDetailsEntity.artikelnummer;
				bezeichnung1 = lsDetailsEntity.bezeichnung1.Length >= 7 ? lsDetailsEntity.bezeichnung1.Substring(0, 7) : lsDetailsEntity.bezeichnung1;
				bezeichnung1BarreCode = lsDetailsEntity.artikelnummer.Substring(0, 3) == "968" ? (lsDetailsEntity.bezeichnung1.Length >= 7 ? "P" + lsDetailsEntity.bezeichnung1.Substring(0, 7) : "P" + lsDetailsEntity.bezeichnung1) : (lsDetailsEntity.bezeichnung1.Length >= 7 ? lsDetailsEntity.bezeichnung1.Substring(0, 7) : lsDetailsEntity.bezeichnung1);
				bezeichnung2 = lsDetailsEntity.bezeichnung2;
				customerItemNumber = lsDetailsEntity.customerItemNumber;
				groesse = Math.Round(lsDetailsEntity.groesse, 0);
				gewicht = Math.Round((lsDetailsEntity.groesse * lsDetailsEntity.anzahl / 1000), 3);
				lVornameNameFirma = lsDetailsEntity.lVornameNameFirma;
				lStrassePostfach = lsDetailsEntity.lStrassePostfach;
				lLandPLZORT = lsDetailsEntity.lLandPLZORT;
				verpackungsart = lsDetailsEntity.verpackungsart;
				verpackungsmenge = lsDetailsEntity.verpackungsmenge == null ? 0 : (int)lsDetailsEntity.verpackungsmenge;
				abladestelle = lsDetailsEntity.abladestelle;
				packstatus = lsDetailsEntity.packstatus;
				liefertermin = lsDetailsEntity.liefertermin == null ? "" : lsDetailsEntity.liefertermin.Value.ToString("dd-MM-yyyy");
				anzahl = lsDetailsEntity.anzahl;
				anzahlBarreCode = lsDetailsEntity.artikelnummer.Substring(0, 3) == "968" ? "Q" + lsDetailsEntity.anzahl.ToString() : lsDetailsEntity.anzahl.ToString();
				bezug = lsDetailsEntity.bezug.Length >= 11 ? lsDetailsEntity.bezug.Substring(0, 11) : lsDetailsEntity.bezug;
				ihrZeichen = lsDetailsEntity.ihrZeichen;
				angeboteNr = lsDetailsEntity.angeboteNr;
				angeboteNrBarreCode = lsDetailsEntity.artikelnummer != null && lsDetailsEntity.artikelnummer != "" && lsDetailsEntity.artikelnummer.Length >= 3 && lsDetailsEntity.artikelnummer.Substring(0, 3) == "968" ? "N" + lsDetailsEntity.angeboteNr.ToString() : lsDetailsEntity.angeboteNr.ToString();
				index_Kunde = lsDetailsEntity.index_Kunde;
				versand_gedruckt = lsDetailsEntity.versand_gedruckt;
				nrAngeboteArtikel = lsDetailsEntity.nrAngeboteArtikel;
				vDAGedruckt = lsDetailsEntity.vDAGedruckt;




			}
		}
	}
}
