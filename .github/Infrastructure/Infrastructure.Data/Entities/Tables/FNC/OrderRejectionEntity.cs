using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class OrderRejectionEntity
	{
		public int Id { get; set; }
		public int OrderArticleCount { get; set; }
		public int OrderId { get; set; }
		public int OrderProjectId { get; set; }
		public decimal OrderTotalAmount { get; set; }
		public string OrderType { get; set; }
		public int OrderUserId { get; set; }
		public int RejectionLevel { get; set; }
		public string RejectionNotes { get; set; }
		public DateTime RejectionTime { get; set; }
		public int UserId { get; set; }

		public OrderRejectionEntity() { }

		public OrderRejectionEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			OrderArticleCount = Convert.ToInt32(dataRow["OrderArticleCount"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderProjectId = Convert.ToInt32(dataRow["OrderProjectId"]);
			OrderTotalAmount = Convert.ToDecimal(dataRow["OrderTotalAmount"]);
			OrderType = Convert.ToString(dataRow["OrderType"]);
			OrderUserId = Convert.ToInt32(dataRow["OrderUserId"]);
			RejectionLevel = Convert.ToInt32(dataRow["RejectionLevel"]);
			RejectionNotes = (dataRow["RejectionNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RejectionNotes"]);
			RejectionTime = Convert.ToDateTime(dataRow["RejectionTime"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
		}
	}
}

