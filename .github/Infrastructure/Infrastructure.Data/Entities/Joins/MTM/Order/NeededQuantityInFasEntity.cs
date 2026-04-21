using System;


namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class NeededQuantityInFasEntity
	{
		public string Week { get; set; }
		public decimal? NeededQuantity { get; set; }
		public decimal? Stock { get; set; }
		public string Artikelnummer { get; set; }
		public int? Reserved_Fertigun { get; set; }
		public int? Reserved_Article { get; set; }
		public NeededQuantityInFasEntity(System.Data.DataRow datarow)
		{
			NeededQuantity = (datarow["NeededQuantity"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(datarow["NeededQuantity"]);
			Artikelnummer = (datarow["Artikelnummer"] == System.DBNull.Value) ? "" : datarow["Artikelnummer"].ToString();
			Reserved_Article = (datarow["Reserved_Article"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Reserved_Article"]);
			Stock = (datarow["Stock"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(datarow["Stock"]);
			Reserved_Fertigun = (datarow["Reserved_Fertigun"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Reserved_Fertigun"]);
			Week = (datarow["Week"] == System.DBNull.Value) ? "" : datarow["Week"].ToString();
		}
	}
}
