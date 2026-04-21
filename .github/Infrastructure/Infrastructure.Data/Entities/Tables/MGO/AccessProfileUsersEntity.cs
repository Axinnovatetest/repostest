using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MGO
{
	public class AccessProfileUsersEntity
	{
		public int? AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public string UserEmail { get; set; }
		public int? UserId { get; set; }
		public string UserName { get; set; }

		public AccessProfileUsersEntity() { }

		public AccessProfileUsersEntity(DataRow dataRow)
		{
			AccessProfileId = (dataRow["AccessProfileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AccessProfileId"]);
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserEmail = (dataRow["UserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserEmail"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
		}

		public AccessProfileUsersEntity ShallowClone()
		{
			return new AccessProfileUsersEntity
			{
				AccessProfileId = AccessProfileId,
				AccessProfileName = AccessProfileName,
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				Id = Id,
				UserEmail = UserEmail,
				UserId = UserId,
				UserName = UserName
			};
		}
	}
}

