using System;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_IV_Entity
	{
		public PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_IV_Entity(System.Data.DataRow dataRow)
		{
			PSZ = (dataRow["PSZ#"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ#"]);
			Artikel_Nr_des_Bauteils = (dataRow["Artikel-Nr des Bauteils"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel-Nr des Bauteils"]);
			Termin_Bestatigt1 = (dataRow["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestätigt1"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Artikel_Artikelnummer = (dataRow["Artikel_Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel_Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Fertigung_Anzahl = (dataRow["Fertigung_Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Fertigung_Anzahl"]);
			Stucklisten_Anzahl = Convert.ToDecimal(String.Format("{0:0.000}", (dataRow["Stücklisten_Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Stücklisten_Anzahl"])));
			Bruttobedarf = Convert.ToDecimal(String.Format("{0:0.000}", (dataRow["Bruttobedarf"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bruttobedarf"])));
			Bestand = Convert.ToDecimal(String.Format("{0:0.000}", (dataRow["Bestand"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestand"])));
			Termin_Materialbedarf = (dataRow["Termin_Materialbedarf"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Materialbedarf"]);
			Laufende_Summe = (dataRow["Laufende Summe"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Laufende Summe"]);
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
	}
}
