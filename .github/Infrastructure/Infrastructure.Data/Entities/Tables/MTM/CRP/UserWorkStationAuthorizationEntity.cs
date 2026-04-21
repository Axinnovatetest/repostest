using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class UserWorkStationAuthorizationEntity
	{
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public int WorkStationId { get; set; }
		public string WorkStationName { get; set; }

		public UserWorkStationAuthorizationEntity() { }

		public UserWorkStationAuthorizationEntity(DataRow dataRow)
		{
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = Convert.ToString(dataRow["UserName"]);
			WorkStationId = Convert.ToInt32(dataRow["WorkStationId"]);
			WorkStationName = Convert.ToString(dataRow["WorkStationName"]);
		}
	}
}

