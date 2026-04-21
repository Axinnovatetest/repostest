using System;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Verpackung
{
	public class PackingModel
	{
		public DateTime? liefertermin { get; set; }
		public long nrAngebote { get; set; } //champ nr en table angebote
		public long nrAngeboteArtikel { get; set; }
		public long angeboteNr { get; set; }
		public string vornameNameFirma { get; set; }
		public string benutzer { get; set; }
		public int anzahl { get; set; }
		public string artikelnummer { get; set; }
		public int artikelNr { get; set; }
		public string bezeichnung1 { get; set; }
		public int lagerort_id { get; set; }
		public string versandinfo_von_CS { get; set; }
		public bool packstatus { get; set; }
		public string gepackt_von { get; set; }
		public DateTime? gepackt_Zeitpunkt { get; set; }
		public bool versandstatus { get; set; }
		public string versanddienstleister { get; set; }
		public string versandnummer { get; set; }
		public string postext { get; set; }
		public string packinfo_von_Lager { get; set; }
		public string versandinfo_von_Lager { get; set; }
		public string lagerort { get; set; }
		public bool gebucht { get; set; }
		public bool versand_gedruckt { get; set; }
		public string Abladestelle { get; set; }
		public bool ls_von_Versand_gedruckt { get; set; }
		public string versandarten_Auswahl { get; set; }
		public DateTime? versanddatum_Auswahl { get; set; }
		public decimal Groesse { get; set; }

		public PackingModel(Infrastructure.Data.Entities.Joins.Logistics.PackingEntity PackingEntity)
		{

			if(PackingEntity != null)
			{
				liefertermin = PackingEntity.liefertermin;
				nrAngebote = PackingEntity.nrAngebote;
				nrAngeboteArtikel = PackingEntity.nrAngeboteArtikel;
				angeboteNr = PackingEntity.angeboteNr;
				vornameNameFirma = PackingEntity.vornameNameFirma;
				benutzer = PackingEntity.benutzer;
				anzahl = PackingEntity.anzahl;
				artikelnummer = PackingEntity.artikelnummer;
				artikelNr = PackingEntity.artikelNr;
				bezeichnung1 = PackingEntity.bezeichnung1;
				lagerort_id = PackingEntity.lagerort_id;
				versandinfo_von_CS = PackingEntity.versandinfo_von_CS;
				packstatus = PackingEntity.packstatus;
				gepackt_von = PackingEntity.gepackt_von;
				gepackt_Zeitpunkt = PackingEntity.gepackt_Zeitpunkt;
				versandstatus = PackingEntity.versandstatus;
				versanddienstleister = PackingEntity.versanddienstleister;
				versandnummer = PackingEntity.versandnummer;
				postext = PackingEntity.postext;
				packinfo_von_Lager = PackingEntity.packinfo_von_Lager;
				versandinfo_von_Lager = PackingEntity.versandinfo_von_Lager;
				lagerort = PackingEntity.lagerort;
				gebucht = PackingEntity.gebucht;
				versand_gedruckt = PackingEntity.versand_gedruckt;
				Abladestelle = PackingEntity.Abladestelle;
				ls_von_Versand_gedruckt = PackingEntity.ls_von_Versand_gedruckt;
				versandarten_Auswahl = PackingEntity.versandarten_Auswahl;
				versanddatum_Auswahl = PackingEntity.versanddatum_Auswahl;
				Groesse = PackingEntity.Groesse;
			}
		}
	}
	public class PackingChooseModel
	{
		public DateTime? liefertermin { get; set; }
		public long nrAngeboteArtikel { get; set; }
		public string artikelnummer { get; set; }
		public long angeboteNr { get; set; }
		public string versandinfo_von_CS { get; set; }
		public string versandAart { get; set; }//
		public string bestellnummer { get; set; }//
		public int anzahl { get; set; }
		public string bezeichnung1 { get; set; }
		public int lagerort_id { get; set; }
		public bool packstatus { get; set; }
		public decimal exportGewicht { get; set; }
		public bool versandstatus { get; set; }
		public string versanddienstleister { get; set; }
		public string versandnummer { get; set; }
		public string postext { get; set; }
		public string lagerort { get; set; }
		public bool gebucht { get; set; }
		public bool versand_gedruckt { get; set; }
		public string Abladestelle { get; set; }
		public string verpackungsart { get; set; }
		public int? verpackungsmenge { get; set; }
		public decimal gewichtArtikel { get; set; }
		public string versandarten_Auswahl { get; set; }
		public DateTime? versanddatum_Auswahl { get; set; }
		public decimal verkaufpreis { get; set; }
		public decimal? verpackungGewicht { get; set; }
		public string lStrassePostfach { get; set; }
		public string lLandPLZORT { get; set; }
		public string lAnrede { get; set; }
		public string lName2 { get; set; }
		public string lName3 { get; set; }
		public string lAnsprechpartner { get; set; }
		public string lAbteilung { get; set; }
		public string lVornameNameFirma { get; set; }
		public decimal preis { get; set; }
		public string bezug { get; set; }
		public bool vDAGedruckt { get; set; }
		public decimal? gesammtGewicht { get; set; }



		public PackingChooseModel(Infrastructure.Data.Entities.Joins.Logistics.PackingChooseEntity PackingEntity)
		{

			if(PackingEntity != null)
			{
				liefertermin = PackingEntity.liefertermin;
				nrAngeboteArtikel = PackingEntity.nrAngeboteArtikel;
				artikelnummer = PackingEntity.artikelnummer;
				angeboteNr = PackingEntity.angeboteNr;
				versandinfo_von_CS = PackingEntity.versandinfo_von_CS;
				versandAart = PackingEntity.versandAart;
				bestellnummer = PackingEntity.bestellnummer;
				anzahl = PackingEntity.anzahl;
				bezeichnung1 = PackingEntity.bezeichnung1;
				lagerort_id = PackingEntity.lagerort_id;
				packstatus = PackingEntity.packstatus;
				exportGewicht = PackingEntity.exportGewicht;
				versandstatus = PackingEntity.versandstatus;
				versanddienstleister = PackingEntity.versanddienstleister;
				versandnummer = PackingEntity.versandnummer;
				postext = PackingEntity.postext;
				lagerort = PackingEntity.lagerort;
				gebucht = PackingEntity.gebucht;
				versand_gedruckt = PackingEntity.versand_gedruckt;
				Abladestelle = PackingEntity.Abladestelle;
				verpackungsart = PackingEntity.verpackungsart;
				verpackungsmenge = PackingEntity.verpackungsmenge;
				gewichtArtikel = PackingEntity.gewichtArtikel;
				versandarten_Auswahl = PackingEntity.versandarten_Auswahl;
				versanddatum_Auswahl = PackingEntity.versanddatum_Auswahl;
				verkaufpreis = PackingEntity.verkaufpreis;
				verpackungGewicht = PackingEntity.verpackungGewicht;
				lStrassePostfach = PackingEntity.lStrassePostfach;
				lLandPLZORT = PackingEntity.lLandPLZORT;
				lAnrede = PackingEntity.lAnrede;
				lName2 = PackingEntity.lName2;
				lName3 = PackingEntity.lName3;
				lAnsprechpartner = PackingEntity.lAnsprechpartner;
				lAbteilung = PackingEntity.lAbteilung;
				lVornameNameFirma = PackingEntity.lVornameNameFirma;
				preis = PackingEntity.preis;
				bezug = PackingEntity.bezug;
				vDAGedruckt = PackingEntity.vDAGedruckt;
			}
		}
	}
	public class PackingGlobalChooseModel
	{
		public List<DateTime?> listeDatum { get; set; }
		public List<string> listeFirma { get; set; }
		public List<KeyValuePair<string, string>> lsiteNameFirma { get; set; }
		public List<string> listeVersandenArten { get; set; }
		public List<PackingChooseModel> listeVerpackung { get; set; }
	}

	public class ListeUpdatePacking
	{
		public List<long> listeLieferscheine { get; set; }
	}

	public class ListeUpdateVDA
	{
		public List<long> listeNrAngArt { get; set; }
	}
}
