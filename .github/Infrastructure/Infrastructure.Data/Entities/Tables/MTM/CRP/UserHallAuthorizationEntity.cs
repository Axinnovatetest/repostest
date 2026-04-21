using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class UserHallAuthorizationEntity
	{
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }

		public UserHallAuthorizationEntity() { }

		public UserHallAuthorizationEntity(DataRow dataRow)
		{
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			HallId = Convert.ToInt32(dataRow["HallId"]);
			HallName = Convert.ToString(dataRow["HallName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = Convert.ToString(dataRow["UserName"]);
		}
	}
}

