using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class PrsSupplierArticleEntity
	{
		public bool? aktiv { get; set; }
		public int? Address_Nr { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bestell_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public int? Lieferantennummer { get; set; }
		public decimal? Mindestbestellmenge { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public bool? Standardlieferant { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }
		public int? SupplierId { get; set; }

		public PrsSupplierArticleEntity() { }

		public PrsSupplierArticleEntity(DataRow dataRow)
		{
			aktiv = (dataRow["aktiv"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["aktiv"]);
			Address_Nr = (dataRow["Address_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Address_Nr"]);
			SupplierId = (dataRow["SupplierId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SupplierId"]);
			Artikel_Nr = Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Lieferantennummer = (dataRow["Lieferantennummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lieferantennummer"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Mindestbestellmenge"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Name2 = (dataRow["Name2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name2"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Standardlieferant"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
		}

		public PrsSupplierArticleEntity ShallowClone()
		{
			return new PrsSupplierArticleEntity
			{
				aktiv = aktiv,
				Artikel_Nr = Artikel_Nr,
				Artikelnummer = Artikelnummer,
				Bestell_Nr = Bestell_Nr,
				Bezeichnung_1 = Bezeichnung_1,
				Bezeichnung_2 = Bezeichnung_2,
				Einkaufspreis = Einkaufspreis,
				Lieferantennummer = Lieferantennummer,
				Mindestbestellmenge = Mindestbestellmenge,
				Name1 = Name1,
				Name2 = Name2,
				Standardlieferant = Standardlieferant,
				Wiederbeschaffungszeitraum = Wiederbeschaffungszeitraum
			};
		}
	}
}
