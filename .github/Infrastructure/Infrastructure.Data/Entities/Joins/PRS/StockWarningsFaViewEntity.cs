

namespace Infrastructure.Data.Entities.Joins.PRS
{
	public class StockWarningsFaViewEntity
	{
		public int? IdFertigung { get; set; }
		public int? Fertigungsnummer { get; set; }
		public DateTime? Termin { get; set; }
		public decimal? Quantity { get; set; }
		public StockWarningsFaViewEntity()
		{

		}
		public StockWarningsFaViewEntity(DataRow dataRow)
		{
			IdFertigung = dataRow["ID"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["ID"]);
			Fertigungsnummer = dataRow["Fertigungsnummer"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Termin = dataRow["Termin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin"]);
			Quantity = dataRow["Qty"] == DBNull.Value ? (int?)null : Convert.ToInt32(dataRow["Qty"]);
		}
	}
}