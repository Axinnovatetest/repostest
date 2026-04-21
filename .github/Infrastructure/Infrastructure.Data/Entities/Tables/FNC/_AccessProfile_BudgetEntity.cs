using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class _AccessProfile_BudgetEntity
	{
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public bool IsArchived { get; set; }
		public int? LastUpdateId { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public string Name { get; set; }

		public _AccessProfile_BudgetEntity() { }

		public _AccessProfile_BudgetEntity(DataRow dataRow)
		{
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArchiveUserId"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsArchived = Convert.ToBoolean(dataRow["IsArchived"]);
			LastUpdateId = (dataRow["LastUpdateId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateId"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			Name = Convert.ToString(dataRow["Name"]);
		}
	}
}

