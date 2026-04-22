using System;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity
	{
		public Psz_Disposition_Nettobedarfsermittlung_Umbuchung_Analyse_Entity(System.Data.DataRow dataRow)
		{
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			Stucklisten_Artikelnummer = (dataRow["Stücklisten_Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stücklisten_Artikelnummer"]);
			Bezeichnung_des_Bauteils = (dataRow["Bezeichnung des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung des Bauteils"]);
			SummevonBruttobedarf = (dataRow["SummevonBruttobedarf"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SummevonBruttobedarf"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Lagerort_id"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			SummevonBestand = (dataRow["SummevonBestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["SummevonBestand"]);
			MaxvonTermin_Materialbedarf = (dataRow["MaxvonTermin_Materialbedarf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MaxvonTermin_Materialbedarf"]);
			Differenz = (dataRow["Differenz"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Differenz"]);
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
