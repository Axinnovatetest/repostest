using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class BomTzResponseModel
	{
		public string BestandSuffix { get; set; }
		public List<Item> Items { get; set; }
		public BomTzResponseModel(string bestandSuffix, List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz> bomTzs)
		{
			BestandSuffix = bestandSuffix;
			Items = bomTzs?.Select(x => new Item(x))?.ToList();
		}
	}
	public class Item
	{
		public decimal? Anzahl { get; set; }
		public string ArtikelNrFG { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Bestand { get; set; }
		public string Bestell_Nr { get; set; }
		public string Bezeichnung_des_Bauteils { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Gesamtbestand { get; set; }
		public decimal? Kupferzahl { get; set; }
		public decimal? Mindestbestellmenge { get; set; }
		public string Name1 { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }
		public Item(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_BomTz bS_BomTz)
		{
			if(bS_BomTz == null)
				return;

			// -
			Anzahl = bS_BomTz.Anzahl;
			ArtikelNrFG = bS_BomTz.ArtikelNrFG;
			Artikelnummer = bS_BomTz.Artikelnummer;
			Bestand = bS_BomTz.Bestand;
			Bestell_Nr = bS_BomTz.Bestell_Nr;
			Bezeichnung_des_Bauteils = bS_BomTz.Bezeichnung_des_Bauteils;
			Einkaufspreis = bS_BomTz.Einkaufspreis;
			Gesamtbestand = bS_BomTz.Gesamtbestand;
			Kupferzahl = bS_BomTz.Kupferzahl;
			Mindestbestellmenge = bS_BomTz.Mindestbestellmenge;
			Name1 = bS_BomTz.Name1;
			Wiederbeschaffungszeitraum = bS_BomTz.Wiederbeschaffungszeitraum;
		}
	}
}
