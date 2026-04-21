using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.STG
{
	public class OrderTypesEntity
	{
		public string Description { get; set; }
		public int Id { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public string Type { get; set; }

		public OrderTypesEntity() { }

		public OrderTypesEntity(DataRow dataRow)
		{
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			Type = Convert.ToString(dataRow["Type"]);
		}
	}
}

