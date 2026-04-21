namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class CustomerSummaryEntity
	{
		public string CustomerName { get; set; }
		public string DocumentType { get; set; }
		public string DocumentNumber { get; set; }
		public int DocumentAngebotNr { get; set; }
		public string ArticleNumber { get; set; }
		public string ArticleDesignation { get; set; }
		public int OpenQuantity { get; set; }
		public int FANumber { get; set; }
		public DateTime? Date { get; set; }
		public int Week { get; set; }
		public int Year { get; set; }
		public decimal? UnitPrice { get; set; }
		public decimal? TotalPrice { get; set; }
		public int? ArticleId { get; set; }
		public int? AngeboteNR { get; set; }
		public int? CustomerId { get; set; }
		public int TotalCount { get; set; }

		public CustomerSummaryEntity(DataRow dataRow)
		{
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			DocumentType = (dataRow["DocumentType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DocumentType"]);
			DocumentNumber = (dataRow["DocumentNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DocumentNumber"]);
			DocumentAngebotNr = (dataRow["DocumentAngebotNr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["DocumentAngebotNr"]);
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ArticleId"]);
			AngeboteNR = (dataRow["AngeboteNR"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["AngeboteNR"]);
			CustomerId = (dataRow["CustomerId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["CustomerId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			ArticleDesignation = (dataRow["ArticleDesignation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleDesignation"]);
			OpenQuantity = (dataRow["OpenQuantity"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["OpenQuantity"]);
			FANumber = (dataRow["FANumber"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["FANumber"]);
			Date = (dataRow["Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Date"]);
			Week = (dataRow["Week"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Week"]);
			Year = (dataRow["Year"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Year"]);
			UnitPrice = (dataRow["UnitPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["UnitPrice"]);
			TotalPrice = (dataRow["TotalPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalPrice"]);
			TotalCount = Convert.ToInt32(dataRow["TotalCount"]);
		}
	}
}
