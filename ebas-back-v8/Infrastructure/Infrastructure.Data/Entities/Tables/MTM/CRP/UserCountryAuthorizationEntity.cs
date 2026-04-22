using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class UserCountryAuthorizationEntity
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int Id { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }

		public UserCountryAuthorizationEntity() { }

		public UserCountryAuthorizationEntity(DataRow dataRow)
		{
			CountryId = Convert.ToInt32(dataRow["CountryId"]);
			CountryName = Convert.ToString(dataRow["CountryName"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = Convert.ToString(dataRow["UserName"]);
		}
	}
}

