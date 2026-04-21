using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.WPL
{
	public class HallAuthorizationEntity
	{
		public int HallId { get; set; }
		public int Id { get; set; }
		public int UserId { get; set; }

		public HallAuthorizationEntity() { }
		public HallAuthorizationEntity(DataRow dataRow)
		{
			HallId = Convert.ToInt32(dataRow["Hall_Id"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserId = Convert.ToInt32(dataRow["User_Id"]);
		}
	}
}
