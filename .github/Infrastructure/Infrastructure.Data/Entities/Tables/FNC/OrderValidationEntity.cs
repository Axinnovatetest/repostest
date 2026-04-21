using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class OrderValidationEntity
	{
		public int Id { get; set; }
		public int OrderArticleCount { get; set; }
		public int OrderId { get; set; }
		public int OrderProjectId { get; set; }
		public decimal OrderTotalAmount { get; set; }
		public string OrderType { get; set; }
		public int OrderUserId { get; set; }
		public string UserEmail { get; set; }
		public int UserId { get; set; }
		public string Username { get; set; }
		public int ValidationLevel { get; set; }
		public string ValidationNotes { get; set; }
		public DateTime ValidationTime { get; set; }

		public OrderValidationEntity() { }

		public OrderValidationEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			OrderArticleCount = Convert.ToInt32(dataRow["OrderArticleCount"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderProjectId = Convert.ToInt32(dataRow["OrderProjectId"]);
			OrderTotalAmount = Convert.ToDecimal(dataRow["OrderTotalAmount"]);
			OrderType = Convert.ToString(dataRow["OrderType"]);
			OrderUserId = Convert.ToInt32(dataRow["OrderUserId"]);
			UserEmail = (dataRow["UserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserEmail"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
			ValidationLevel = Convert.ToInt32(dataRow["ValidationLevel"]);
			ValidationNotes = (dataRow["ValidationNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ValidationNotes"]);
			ValidationTime = Convert.ToDateTime(dataRow["ValidationTime"]);
		}
	}
}

