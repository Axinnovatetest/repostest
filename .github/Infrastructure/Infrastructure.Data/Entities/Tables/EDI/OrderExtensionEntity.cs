using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class OrderExtensionEntity
	{
		public int Id { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public int LastUpdateUserId { get; set; }
		public string LastUpdateUsername { get; set; }
		public int OrderId { get; set; }
		public string RecipientId { get; set; }
		public string SenderDuns { get; set; }
		public string SenderId { get; set; }
		public DateTime EdiValidationTime { get; set; }
		public int EdiValidationUserId { get; set; }
		public int Version { get; set; }

		public OrderExtensionEntity() { }

		public OrderExtensionEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdateTime = Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = Convert.ToInt32(dataRow["LastUpdateUserId"]);
			LastUpdateUsername = Convert.ToString(dataRow["LastUpdateUsername"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			RecipientId = (dataRow["RecipientId"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["RecipientId"]);
			SenderDuns = (dataRow["SenderDuns"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SenderDuns"]);
			SenderId = (dataRow["SenderId"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SenderId"]);
			EdiValidationTime = Convert.ToDateTime(dataRow["ValidationTime"]);
			EdiValidationUserId = Convert.ToInt32(dataRow["ValidationUserId"]);
			Version = Convert.ToInt32(dataRow["Version"]);
		}
	}
}

