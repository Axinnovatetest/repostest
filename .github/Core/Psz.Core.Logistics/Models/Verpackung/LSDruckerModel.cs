using System;

namespace Psz.Core.Logistics.Models.Verpackung
{
	public class LSDruckerModel
	{
		public decimal gesamtgewicht { get; set; }
		public long skontotage { get; set; }
		public decimal skonto { get; set; }
		public long nettotage { get; set; }
		public string Artikelnummer { get; set; }
		public string fax { get; set; }
		public string ursprungsland { get; set; }
		public string textAuftragsbestätigung { get; set; }
		public string textLieferschein { get; set; }
		public string textRechnung { get; set; }
		public string textGutschrift { get; set; }
		public bool delFixiert { get; set; }
		public decimal grosse { get; set; }
		public string ZolltarifNr { get; set; }
		public string index_Kunde { get; set; }
		public DateTime? index_Kunde_Datum { get; set; }
		public long aB_Pos_zu_RA_Pos { get; set; }
		public long rA_OriginalAnzahl { get; set; }
		public long rA_Abgerufen { get; set; }
		public long rA_Offen { get; set; }
		public bool erledigt_pos { get; set; }
		public bool Versandstatus { get; set; }
		public bool gebucht { get; set; }
		public bool lS_von_Versand_gedruckt { get; set; }
		public string typ { get; set; }
		public long angeboteNr { get; set; }
		public int artikelNr { get; set; }
		public string bezeichnung1 { get; set; }
		public string bezeichnung2 { get; set; }
		public string bezeichnung3 { get; set; }
		public string einheit { get; set; }
		public decimal anfangLagerBestand { get; set; }
		public decimal anzahl { get; set; }
		public decimal originalAnzahl { get; set; }
		public decimal geliefert { get; set; }
		public decimal aktuelleAnzahl { get; set; }
		public LSDruckerModel(Infrastructure.Data.Entities.Joins.Logistics.LSDruckerEntity PackingEntity)
		{

			if(PackingEntity != null)
			{
				gesamtgewicht = PackingEntity.gesamtgewicht;
				skontotage = PackingEntity.skontotage;
				skonto = PackingEntity.skonto;
				nettotage = PackingEntity.nettotage;
				Artikelnummer = PackingEntity.Artikelnummer;
				fax = PackingEntity.fax;
				ursprungsland = PackingEntity.ursprungsland;
				textAuftragsbestätigung = PackingEntity.textAuftragsbestätigung;
				textLieferschein = PackingEntity.textLieferschein;
				textRechnung = PackingEntity.textRechnung;
				textGutschrift = PackingEntity.textGutschrift;
				delFixiert = PackingEntity.delFixiert;
				grosse = PackingEntity.grosse;
				ZolltarifNr = PackingEntity.ZolltarifNr;
				index_Kunde = PackingEntity.index_Kunde;
				index_Kunde_Datum = PackingEntity.index_Kunde_Datum;
				aB_Pos_zu_RA_Pos = PackingEntity.aB_Pos_zu_RA_Pos;
				rA_OriginalAnzahl = PackingEntity.rA_OriginalAnzahl;
				rA_Abgerufen = PackingEntity.rA_Abgerufen;
				rA_Offen = PackingEntity.rA_Offen;
				erledigt_pos = PackingEntity.erledigt_pos;
				Versandstatus = PackingEntity.Versandstatus;
				gebucht = PackingEntity.gebucht;
				lS_von_Versand_gedruckt = PackingEntity.lS_von_Versand_gedruckt;
				typ = PackingEntity.typ;
				angeboteNr = PackingEntity.angeboteNr;
				artikelNr = PackingEntity.artikelNr;
				bezeichnung1 = PackingEntity.bezeichnung1;
				bezeichnung2 = PackingEntity.bezeichnung2;
				bezeichnung3 = PackingEntity.bezeichnung3;
				einheit = PackingEntity.einheit;
				anfangLagerBestand = PackingEntity.anfangLagerBestand;
				anzahl = PackingEntity.anzahl;
				originalAnzahl = PackingEntity.originalAnzahl;
				geliefert = PackingEntity.geliefert;
				aktuelleAnzahl = PackingEntity.aktuelleAnzahl;
			}
		}
	}
}
