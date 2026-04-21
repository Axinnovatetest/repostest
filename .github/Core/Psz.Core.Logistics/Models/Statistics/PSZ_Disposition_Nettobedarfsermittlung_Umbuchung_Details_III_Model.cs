using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Model
	{
		public PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Model(Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Entity _data)
		{
			PSZ = _data.PSZ;
			Artikel_Nr_des_Bauteils = _data.Artikel_Nr_des_Bauteils;
			Vorname_NameFirma = _data.Vorname_NameFirma;
			Standardlieferant = _data.Standardlieferant;
			Bestell_Nr = _data.Bestell_Nr;
			Einkaufspreis = _data.Einkaufspreis;
			Telefon = _data.Telefon;
			Fax = _data.Fax;
			Wiederbeschaffungszeitraum = _data.Wiederbeschaffungszeitraum;
			Mindestbestellmenge = _data.Mindestbestellmenge;
			Bestellungen_Bestellung_Nr = _data.Bestellungen_Bestellung_Nr;
			Anzahl = _data.Anzahl;
			Liefertermin = _data.Liefertermin;
			Bestatigter_Termin = _data.Bestatigter_Termin;
			bestellte_Artikel_Bestellung_Nr = _data.bestellte_Artikel_Bestellung_Nr;
			Name1 = _data.Name1;
			AB_Nr_Lieferant = _data.AB_Nr_Lieferant;
			Lagerort_id = _data.Lagerort_id;
		}
		public string PSZ { get; set; }
		public int Artikel_Nr_des_Bauteils { get; set; }
		public string Vorname_NameFirma { get; set; }
		public int Standardlieferant { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal Einkaufspreis { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public int Wiederbeschaffungszeitraum { get; set; }
		public decimal Mindestbestellmenge { get; set; }
		public int? Bestellungen_Bestellung_Nr { get; set; }
		public decimal? Anzahl { get; set; }
		public DateTime? Liefertermin { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public int bestellte_Artikel_Bestellung_Nr { get; set; }
		public string Name1 { get; set; }
		public string AB_Nr_Lieferant { get; set; }
		public int? Lagerort_id { get; set; }
	}
}
