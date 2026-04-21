namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ArticleMinStockUpdateHistoryEntity
	{
		public int? ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public int Id { get; set; }
		public int? LagerId { get; set; }
		public int? NewMinStock { get; set; }
		public int? OldMinStock { get; set; }
		public DateTime? UpdateDate { get; set; }
		public int? UpdateUserId { get; set; }
		public string UpdateUserName { get; set; }

		public ArticleMinStockUpdateHistoryEntity() { }

		public ArticleMinStockUpdateHistoryEntity(DataRow dataRow)
		{
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			ArticleNumber = (dataRow["ArticleNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleNumber"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LagerId = (dataRow["LagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerId"]);
			NewMinStock = (dataRow["NewMinStock"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NewMinStock"]);
			OldMinStock = (dataRow["OldMinStock"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OldMinStock"]);
			UpdateDate = (dataRow["UpdateDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateDate"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
			UpdateUserName = (dataRow["UpdateUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UpdateUserName"]);
		}

		public ArticleMinStockUpdateHistoryEntity ShallowClone()
		{
			return new ArticleMinStockUpdateHistoryEntity
			{
				ArticleId = ArticleId,
				ArticleNumber = ArticleNumber,
				Id = Id,
				LagerId = LagerId,
				NewMinStock = NewMinStock,
				OldMinStock = OldMinStock,
				UpdateDate = UpdateDate,
				UpdateUserId = UpdateUserId,
				UpdateUserName = UpdateUserName
			};
		}
	}
}

