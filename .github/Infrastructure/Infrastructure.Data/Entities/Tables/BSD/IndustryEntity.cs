using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class IndustryEntity
	{
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public string Name { get; set; }
		public int? Type { get; set; }

		public IndustryEntity() { }
		public IndustryEntity(DataRow dataRow)
		{
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			Name = Convert.ToString(dataRow["Name"]);
			Type = Convert.ToInt32(dataRow["_Type"]);
		}
	}
}

