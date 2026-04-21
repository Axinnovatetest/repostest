

namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class StockWarningsPoViewEntity
	{
		public int? Nr { get; set; }
		public int? BestellungNr { get; set; }
		public decimal? Quantity { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public DateTime? ConfirmedDate { get; set; }
		public DateTime? CreationDate { get; set; }
		public StockWarningsPoViewEntity()
		{

		}
		public StockWarningsPoViewEntity(DataRow dataRow, bool includeCreationDate = false)
		{
			Nr = dataRow["Nr"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			BestellungNr = dataRow["Bestellung-Nr"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Bestellung-Nr"]);
			Quantity = dataRow["Anzahl"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Anzahl"]);
			DeliveryDate = dataRow["Liefertermin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			ConfirmedDate = dataRow["Bestätigter_Termin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Bestätigter_Termin"]);
			if(includeCreationDate)
				CreationDate = dataRow["Datum"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
		}
	}
}