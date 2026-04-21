using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.STG
{
	public class DepartmentEntity
	{
		public DateTime? ArchiveTime { get; set; }
		public long? ArchiveUserId { get; set; }
		public long CompanyId { get; set; }
		public string CompanyName { get; set; }
		public DateTime CreationTime { get; set; }
		public long CreationUserId { get; set; }
		public DateTime? DeleteTime { get; set; }
		public long? DeleteUserId { get; set; }
		public string Description { get; set; }
		public string HeadUserEmail { get; set; }
		public long HeadUserId { get; set; }
		public string HeadUserName { get; set; }
		public long Id { get; set; }
		public bool? IsArchived { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? IsFNC { get; set; }
		public bool? IsSTG { get; set; }
		public bool? IsWPL { get; set; }
		public DateTime? LastEditTime { get; set; }
		public long? LastEditUserId { get; set; }
		public string Name { get; set; }

		public DepartmentEntity() { }

		public DepartmentEntity(DataRow dataRow)
		{
			ArchiveTime = (dataRow["ArchiveTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ArchiveTime"]);
			ArchiveUserId = (dataRow["ArchiveUserId"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["ArchiveUserId"]);
			CompanyId = Convert.ToInt64(dataRow["CompanyId"]);
			CompanyName = Convert.ToString(dataRow["CompanyName"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt64(dataRow["CreationUserId"]);
			DeleteTime = (dataRow["DeleteTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeleteTime"]);
			DeleteUserId = (dataRow["DeleteUserId"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["DeleteUserId"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			HeadUserEmail = (dataRow["HeadUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HeadUserEmail"]);
			HeadUserId = Convert.ToInt64(dataRow["HeadUserId"]);
			HeadUserName = Convert.ToString(dataRow["HeadUserName"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			IsArchived = (dataRow["IsArchived"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsArchived"]);
			IsDeleted = (dataRow["IsDeleted"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsDeleted"]);
			IsFNC = (dataRow["IsFNC"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsFNC"]);
			IsSTG = (dataRow["IsSTG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsSTG"]);
			IsWPL = (dataRow["IsWPL"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsWPL"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["LastEditUserId"]);
			Name = Convert.ToString(dataRow["Name"]);
		}
	}
}

