using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class E_RechnungArchive3Entity
	{
		public int ID { get; set; }
		public DateTime? Belegdatum { get; set; }
		public decimal? Betrag { get; set; }
		public decimal? Ausdr1 { get; set; }
		public decimal? Ausdr2 { get; set; }
		public decimal? Ausdr3 { get; set; }
		public string Ausdr4 { get; set; }
		public bool? Ausdr5 { get; set; }

		public E_RechnungArchive3Entity() { }
		public E_RechnungArchive3Entity(DataRow dataRow)
		{
			ID = Convert.ToInt32(dataRow["ID"]);
			Belegdatum = (dataRow["Belegdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Belegdatum"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Ausdr1 = (dataRow["Ausdr1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ausdr1"]);
			Ausdr2 = (dataRow["Ausdr2"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ausdr2"]);
			Ausdr3 = (dataRow["Ausdr3"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Ausdr3"]);
			Ausdr4 = (dataRow["Ausdr4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ausdr4"]);
			Ausdr5 = (dataRow["Ausdr5"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Ausdr5"]);
		}
	}

	public class E_RechnungArchive4Entity
	{
		public int? Kunden_Nr { get; set; }
		public string Typ { get; set; }
		public DateTime? Datum { get; set; }
		public int? Personal_Nr { get; set; }
		public int? Artikel_Nr { get; set; }
		public decimal? Anzahl { get; set; }
		public decimal? gesamt { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public decimal? USt { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string Mandant { get; set; }
		public E_RechnungArchive4Entity() { }

		public E_RechnungArchive4Entity(DataRow dataRow)
		{
			Kunden_Nr = (dataRow["Kunden-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Kunden-Nr"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Personal_Nr = (dataRow["Personal-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personal-Nr"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			gesamt = (dataRow["gesamt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["gesamt"]);
			Angebot_Nr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Projekt-Nr"]);
			USt = (dataRow["USt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["USt"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);

		}
	}
}
