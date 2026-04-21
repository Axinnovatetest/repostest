using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class UserCountryEntity
	{
		public int CountryId { get; set; }
		public int Id { get; set; }
		public int UserId { get; set; }

		public UserCountryEntity() { }
		public UserCountryEntity(DataRow dataRow)
		{
			CountryId = Convert.ToInt32(dataRow["Country_Id"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = Convert.ToInt32(dataRow["User_Id"]);
		}
	}
}
