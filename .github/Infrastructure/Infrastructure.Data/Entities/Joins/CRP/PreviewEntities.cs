

namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class PreviewQuantitiesEntity
	{
		public int ArticleId { get; set; }
		public int Year { get; set; }
		public int Week { get; set; }
		public decimal ABQuantity { get; set; }
		public decimal FCQuantity { get; set; }
		public decimal LPQuantity { get; set; }
		public decimal FAQuantity { get; set; }
		public PreviewQuantitiesEntity(DataRow dataRow)
		{
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ArticleId"]);
			Week = (dataRow["Week"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Week"]);
			Year = (dataRow["Year"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Year"]);
			ABQuantity = (dataRow["ABQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["ABQuantity"]);
			FCQuantity = (dataRow["FCQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["FCQuantity"]);
			LPQuantity = (dataRow["LPQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["LPQuantity"]);
			FAQuantity = (dataRow["FAQuantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["FAQuantity"]);
		}
		public PreviewQuantitiesEntity()
		{
				
		}
	}
	public class WeekEntitiesEntity
	{
		public int EntityId { get; set; }
		public int CustomerNumber { get; set; }
		public string EntityNumber { get; set; }
		public bool? IsManual { get; set; }
		public decimal Quantity { get; set; }
		public WeekEntitiesEntity(DataRow dataRow)
		{
			EntityId = (dataRow["EntityId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["EntityId"]);
			EntityNumber = (dataRow["EntityNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["EntityNumber"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["CustomerNumber"]);
			IsManual = (dataRow["IsManual"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["IsManual"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Quantity"]);
		}
	}
}
