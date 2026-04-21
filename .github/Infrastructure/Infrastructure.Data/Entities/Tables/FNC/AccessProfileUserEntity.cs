using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class AccessProfileUserEntity
	{
		public int AccessProfileId { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public int UserId { get; set; }

		public AccessProfileUserEntity() { }

		public AccessProfileUserEntity(DataRow dataRow)
		{
			AccessProfileId = Convert.ToInt32(dataRow["AccessProfileId"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
		}
	}
}

