using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class OrderPlacementFileEntity
	{
		public DateTime? Date { get; set; }
		public int FileId { get; set; }
		public string FileName { get; set; }
		public int Id { get; set; }
		public int? OrderId { get; set; }
		public int? OrderPlacementId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }

		public OrderPlacementFileEntity() { }

		public OrderPlacementFileEntity(DataRow dataRow)
		{
			Date = (dataRow["Date"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Date"]);
			FileId = Convert.ToInt32(dataRow["FileId"]);
			FileName = (dataRow["FileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderId"]);
			OrderPlacementId = (dataRow["OrderPlacementId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderPlacementId"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
		}
	}
}

