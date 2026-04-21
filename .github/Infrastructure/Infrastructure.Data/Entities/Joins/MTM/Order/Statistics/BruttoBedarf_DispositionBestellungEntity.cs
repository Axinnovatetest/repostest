using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{
	public class BruttoBedarf_DispositionBestellungEntity
	{
		public int? Lief_Nr { get; set; }
		public string VornameFirma { get; set; }
		public decimal? Anzahl1 { get; set; }
		public int? Projekt_Nr { get; set; }
		public DateTime? Liefertermin { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public string Bemerk { get; set; }
		public int? AB_L { get; set; }
		public BruttoBedarf_DispositionBestellungEntity()
		{

		}
		public BruttoBedarf_DispositionBestellungEntity(DataRow dataRow)
		{
			Lief_Nr = (dataRow["Lief_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lief_Nr"].ToString());
			VornameFirma = (dataRow["VornameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["VornameFirma"].ToString());
			Anzahl1 = (dataRow["Anzahl1"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl1"].ToString());
			Projekt_Nr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Projekt-Nr"].ToString());
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"].ToString());
			Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"].ToString());
			Bemerk = (dataRow["Bemerk"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerk"].ToString());
			AB_L = (dataRow["AB_L"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AB_L"].ToString());
		}
	}
}
