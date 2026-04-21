using System;

namespace Psz.Core.Logistics.Models.Verpackung
{
	public class VDAModel
	{
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung2 { get; set; }
		public string customerItemNumber { get; set; }
		public decimal groesse { get; set; }
		public string lVornameNameFirma { get; set; }
		public string lStrassePostfach { get; set; }
		public string lLandPLZORT { get; set; }
		public string verpackungsart { get; set; }
		public int? verpackungsmenge { get; set; }
		public string abladestelle { get; set; }
		public bool packstatus { get; set; }
		public DateTime? liefertermin { get; set; }
		public int anzahl { get; set; }
		public string bezug { get; set; }
		public string ihrZeichen { get; set; }
		public long angeboteNr { get; set; }
		public string index_Kunde { get; set; }
		public bool versand_gedruckt { get; set; }
		public long nrAngeboteArtikel { get; set; }
		public bool vDAGedruckt { get; set; }
		public VDAModel(Infrastructure.Data.Entities.Joins.Logistics.VDAEntity PackingEntity)
		{

			if(PackingEntity != null)
			{
				artikelnummer = PackingEntity.artikelnummer;
				bezeichnung1 = PackingEntity.bezeichnung1;
				bezeichnung2 = PackingEntity.bezeichnung2;
				customerItemNumber = PackingEntity.customerItemNumber;
				groesse = PackingEntity.groesse;
				lVornameNameFirma = PackingEntity.lVornameNameFirma;
				lStrassePostfach = PackingEntity.lStrassePostfach;
				lLandPLZORT = PackingEntity.lLandPLZORT;
				verpackungsart = PackingEntity.verpackungsart;
				verpackungsmenge = PackingEntity.verpackungsmenge;
				abladestelle = PackingEntity.abladestelle;
				packstatus = PackingEntity.packstatus;
				liefertermin = PackingEntity.liefertermin;
				anzahl = PackingEntity.anzahl;
				bezug = PackingEntity.bezug;
				ihrZeichen = PackingEntity.ihrZeichen;
				angeboteNr = PackingEntity.angeboteNr;
				index_Kunde = PackingEntity.index_Kunde;
				versand_gedruckt = PackingEntity.versand_gedruckt;
				nrAngeboteArtikel = PackingEntity.nrAngeboteArtikel;
				vDAGedruckt = PackingEntity.vDAGedruckt;




			}
		}
	}
}
