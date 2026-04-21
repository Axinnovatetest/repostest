using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.STG
{
	public class LanguageEntity
	{
		public string Code { get; set; }
		public DateTime CreationDate { get; set; }
		public int CreationUserId { get; set; }
		public DateTime? DeleteDate { get; set; }
		public int? DeleteUserID { get; set; }
		public string Description { get; set; }
		public int Id { get; set; }
		public bool IsArchived { get; set; }
		public DateTime? LastEditDate { get; set; }
		public int? LastUserId { get; set; }
		public string Name { get; set; }

		public LanguageEntity() { }

		public LanguageEntity(DataRow dataRow)
		{
			Code = Convert.ToString(dataRow["Code"]);
			CreationDate = Convert.ToDateTime(dataRow["CreationDate"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			DeleteDate = (dataRow["DeleteDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeleteDate"]);
			DeleteUserID = (dataRow["DeleteUserID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["DeleteUserID"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsArchived = Convert.ToBoolean(dataRow["IsArchived"]);
			LastEditDate = (dataRow["LastEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditDate"]);
			LastUserId = (dataRow["LastUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUserId"]);
			Name = Convert.ToString(dataRow["Name"]);
		}
	}
}

