using System;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class Disposition_Reserviert_Menge_UmbuchungEntity
	{
		public Disposition_Reserviert_Menge_UmbuchungEntity(System.Data.DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Menge_Reserviert = (dataRow["Menge_Reserviert"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Menge_Reserviert"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Type"]);
			Lagerort_ID = (dataRow["Lagerort_ID"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerort_ID"]);
		}
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal Menge_Reserviert { get; set; }
		public int? Type { get; set; }
		public int Lagerort_ID { get; set; }
	}
}
