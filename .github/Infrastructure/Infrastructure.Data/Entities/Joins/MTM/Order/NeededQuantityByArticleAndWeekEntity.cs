using System;


namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class NeededQuantityByArticleAndWeekEntity
	{
		public string week { get; set; }
		public decimal? NeedQuantity { get; set; }
		public string Artikelnummer { get; set; }
		public int ArtikelNr { get; set; }

		public NeededQuantityByArticleAndWeekEntity(System.Data.DataRow datarow)
		{
			ArtikelNr = (datarow["Artikel_Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Artikel_Nr"]);
			NeedQuantity = (datarow["NeedQuantity"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(datarow["NeedQuantity"]);
			//Termin_Bestätigt1 = (datarow ["Termin_Bestätigt1"] == System.DBNull.Value) ? (DateTime?) null : Convert.ToDateTime(datarow ["Termin_Bestätigt1"]);
			week = (datarow["Week"] == System.DBNull.Value) ? "" : datarow["Week"].ToString();
			Artikelnummer = (datarow["Artikelnummer"] == System.DBNull.Value) ? "" : datarow["Artikelnummer"].ToString();
		}
	}
}
