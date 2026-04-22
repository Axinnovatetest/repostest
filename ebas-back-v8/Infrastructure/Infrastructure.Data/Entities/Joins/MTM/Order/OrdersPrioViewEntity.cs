using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class OrdersPrioViewEntity
	{
		public int Nr { get; set; }
		public int? Bestellung_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public string Typ { get; set; }
		public string Vorname_NameFirma { get; set; }
		public DateTime? Datum { get; set; }
		public string Konditionen { get; set; }
		public string Bearbeiter { get; set; }
		public bool? gebucht { get; set; }
		public int? Position { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public DateTime? Liefertermin { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public string Lagerort { get; set; }
		public bool? ProjectPurchase { get; set; }
		public bool? StandardSupplierViolation { get; set; }
		public OrdersPrioViewEntity(DataRow dataRow)
		{
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Bestellung_Nr = (dataRow["Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Konditionen = (dataRow["Konditionen"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Konditionen"]);
			Bearbeiter = (dataRow["Name"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name"]);
			gebucht = (dataRow["gebucht"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["gebucht"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			ProjectPurchase = (dataRow["ProjectPurchase"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["ProjectPurchase"]);
			StandardSupplierViolation = (dataRow["StandardSupplierViolation"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["StandardSupplierViolation"]);
		}
	}
}
