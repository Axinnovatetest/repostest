using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Statistiken_AngeboteEntity
	{
		public int? Adress_Nr { get; set; }
		public int? Angebot_Nr { get; set; }
		public decimal? Anzahl { get; set; }
		public int? Artikel_Nr { get; set; }
		public int? Artikel_Nr_nach { get; set; }
		public int? Bestellung_Nr { get; set; }
		public DateTime? Datum { get; set; }
		public bool? fibu_export { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public int ID { get; set; }
		public int? Lagerort_ID { get; set; }
		public int? Lagerort_ID_nach { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string Mandant { get; set; }
		public int? NR_Angebotene_Artikel { get; set; }
		public int? Personal_Nr { get; set; }
		public int? Projekt_Nr { get; set; }
		public string Typ { get; set; }
		public decimal? USt { get; set; }

		public Statistiken_AngeboteEntity() { }

		public Statistiken_AngeboteEntity(DataRow dataRow)
		{
			Adress_Nr = (dataRow["Adreß-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Adreß-Nr"]);
			Angebot_Nr = (dataRow["Angebot_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot_Nr"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Artikel_Nr = (dataRow["Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikel_Nr_nach = (dataRow["Artikel-Nr_nach"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr_nach"]);
			Bestellung_Nr = (dataRow["Bestellung_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellung_Nr"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			fibu_export = (dataRow["fibu_export"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["fibu_export"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Lagerort_ID = (dataRow["Lagerort_ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_ID"]);
			Lagerort_ID_nach = (dataRow["Lagerort_ID_nach"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_ID_nach"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Mandant = (dataRow["Mandant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Mandant"]);
			NR_Angebotene_Artikel = (dataRow["NR-Angebotene_Artikel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NR-Angebotene_Artikel"]);
			Personal_Nr = (dataRow["Personal-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Personal-Nr"]);
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Projekt-Nr"]);
			Typ = (dataRow["Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ"]);
			USt = (dataRow["USt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["USt"]);
		}
	}
}

