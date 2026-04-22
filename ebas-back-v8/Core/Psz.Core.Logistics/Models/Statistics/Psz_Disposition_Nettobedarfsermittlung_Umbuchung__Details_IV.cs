using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV
	{
		public Psz_Disposition_Nettobedarfsermittlung_Umbuchung__Details_IV(Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_IV_Entity _data)
		{
			PSZ = _data.PSZ;
			Artikel_Nr_des_Bauteils = _data.Artikel_Nr_des_Bauteils;
			Termin_Bestatigt1 = _data.Termin_Bestatigt1;
			Fertigungsnummer = _data.Fertigungsnummer;
			Artikel_Artikelnummer = _data.Artikel_Artikelnummer;
			Bezeichnung1 = _data.Bezeichnung1;
			Fertigung_Anzahl = _data.Fertigung_Anzahl;
			Stucklisten_Anzahl = _data.Stucklisten_Anzahl;
			Bruttobedarf = _data.Bruttobedarf;
			Bestand = _data.Bestand;
			Termin_Materialbedarf = _data.Termin_Materialbedarf;
			Laufende_Summe = _data.Laufende_Summe;
			Soustraction = _data.Bestand - _data.Laufende_Summe;
		}
		public string PSZ { get; set; }
		public string Artikel_Nr_des_Bauteils { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public int Fertigungsnummer { get; set; }
		public string Artikel_Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int Fertigung_Anzahl { get; set; }
		public decimal Stucklisten_Anzahl { get; set; }
		public decimal Bruttobedarf { get; set; }
		public decimal Bestand { get; set; }
		public DateTime? Termin_Materialbedarf { get; set; }
		public decimal Laufende_Summe { get; set; }
		public decimal Soustraction { get; set; }
	}



}
