using System;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{

	public class OrdersForArticlesByWeekEntity
	{
		public decimal? orderedQuantitiy { get; set; }
		public int artikelNr { get; set; }
		public string WeekO { get; set; }
		public OrdersForArticlesByWeekEntity(System.Data.DataRow datarow)
		{
			orderedQuantitiy = (datarow["orderedQuantitiy"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(datarow["orderedQuantitiy"]);
			WeekO = (datarow["WeekO"] == System.DBNull.Value) ? "" : datarow["WeekO"].ToString();
			artikelNr = (datarow["artikelNr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["artikelNr"]);
		}
	}
	public class FaultyOrdersEntity
	{
		public int Nr { get; set; }
		public int Bestellung_Nr { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public int TotalCount { get; set; }
		public string Supplier { get; set; }
		public FaultyOrdersEntity(System.Data.DataRow datarow)
		{
			TotalCount = (datarow["TotalCount"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["TotalCount"]);
			Nr = (datarow["Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Nr"]);
			Bestellung_Nr = (datarow["Bestellung-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Bestellung-Nr"]);
			Bestatigter_Termin = (datarow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(datarow["Bestätigter_Termin"]);
			Supplier = (datarow["Supplier"] == System.DBNull.Value) ? "" : (datarow["Supplier"]).ToString();
		}
	}




}
