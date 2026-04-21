using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class UserDepartementAuthorizationEntity
	{
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int DepartementId { get; set; }
		public string DepartementName { get; set; }
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }

		public UserDepartementAuthorizationEntity() { }

		public UserDepartementAuthorizationEntity(DataRow dataRow)
		{
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			DepartementId = Convert.ToInt32(dataRow["DepartementId"]);
			DepartementName = Convert.ToString(dataRow["DepartementName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = Convert.ToString(dataRow["UserName"]);
		}
	}
}

