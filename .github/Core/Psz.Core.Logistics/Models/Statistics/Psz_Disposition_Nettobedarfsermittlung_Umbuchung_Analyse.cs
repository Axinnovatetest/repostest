using System;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse
	{
		public Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse(Infrastructure.Data.Entities.Joins.Logistics.Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity _data)
		{
			Name1 = _data.Name1;
			Stucklisten_Artikelnummer = _data.Stucklisten_Artikelnummer;
			Bezeichnung_des_Bauteils = _data.Bezeichnung_des_Bauteils;
			SummevonBruttobedarf = _data.SummevonBruttobedarf;
			Lagerort_id = _data.Lagerort_id;
			Lagerort = _data.Lagerort;
			SummevonBestand = _data.SummevonBestand;
			MaxvonTermin_Materialbedarf = _data.MaxvonTermin_Materialbedarf;
			Differenz = _data.Differenz;
		}
		public string Name1 { get; set; }
		public string Stucklisten_Artikelnummer { get; set; }
		public string Bezeichnung_des_Bauteils { get; set; }
		public decimal SummevonBruttobedarf { get; set; }
		public int? Lagerort_id { get; set; }
		public string Lagerort { get; set; }
		public decimal SummevonBestand { get; set; }
		public DateTime? MaxvonTermin_Materialbedarf { get; set; }
		public decimal Differenz { get; set; }
	}
}
