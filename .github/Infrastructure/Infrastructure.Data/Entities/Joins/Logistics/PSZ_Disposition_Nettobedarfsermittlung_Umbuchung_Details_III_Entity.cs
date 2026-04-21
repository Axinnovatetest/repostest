using System;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Entity
	{
		public PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_III_Entity(System.Data.DataRow dataRow)
		{
			PSZ = (dataRow["PSZ#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ#"]);
			Artikel_Nr_des_Bauteils = (dataRow["Artikel-Nr des Bauteils"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Artikel-Nr des Bauteils"]);
			Vorname_NameFirma = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			Standardlieferant = (dataRow["Standardlieferant"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Standardlieferant"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Telefon = (dataRow["Telefon"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Telefon"]);
			Fax = (dataRow["Fax"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Fax"]);
			Wiederbeschaffungszeitraum = (dataRow["Wiederbeschaffungszeitraum"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Wiederbeschaffungszeitraum"]);
			Mindestbestellmenge = (dataRow["Mindestbestellmenge"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Mindestbestellmenge"]);
			Bestellungen_Bestellung_Nr = (dataRow["Bestellungen_Bestellung-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestellungen_Bestellung-Nr"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
			bestellte_Artikel_Bestellung_Nr = (dataRow["bestellte Artikel_Bestellung-Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["bestellte Artikel_Bestellung-Nr"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			AB_Nr_Lieferant = (dataRow["AB-Nr_Lieferant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AB-Nr_Lieferant"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Lagerort_id"]);
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
