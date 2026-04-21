using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class OrderUnvalidationEntity
	{
		public int Id { get; set; }
		public int OrderArticleCount { get; set; }
		public int OrderId { get; set; }
		public int OrderProjectId { get; set; }
		public decimal OrderTotalAmount { get; set; }
		public string OrderType { get; set; }
		public int OrderUserId { get; set; }
		public int UnvalidationLevel { get; set; }
		public string UnvalidationNotes { get; set; }
		public DateTime UnvalidationTime { get; set; }
		public int UserId { get; set; }

		public OrderUnvalidationEntity() { }

		public OrderUnvalidationEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			OrderArticleCount = Convert.ToInt32(dataRow["OrderArticleCount"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderProjectId = Convert.ToInt32(dataRow["OrderProjectId"]);
			OrderTotalAmount = Convert.ToDecimal(dataRow["OrderTotalAmount"]);
			OrderType = Convert.ToString(dataRow["OrderType"]);
			OrderUserId = Convert.ToInt32(dataRow["OrderUserId"]);
			UnvalidationLevel = Convert.ToInt32(dataRow["UnvalidationLevel"]);
			UnvalidationNotes = (dataRow["UnvalidationNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UnvalidationNotes"]);
			UnvalidationTime = Convert.ToDateTime(dataRow["UnvalidationTime"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
		}
	}
}

