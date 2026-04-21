using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class WorkAreaAuthorizationEntity
	{
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public int WorkAreaId { get; set; }
		public string WorkAreaName { get; set; }

		public WorkAreaAuthorizationEntity() { }

		public WorkAreaAuthorizationEntity(DataRow dataRow)
		{
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = Convert.ToString(dataRow["UserName"]);
			WorkAreaId = Convert.ToInt32(dataRow["WorkAreaId"]);
			WorkAreaName = Convert.ToString(dataRow["WorkAreaName"]);
		}
	}
}

