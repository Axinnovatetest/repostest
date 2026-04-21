using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class BSD_AccessProfileLagerEntity
	{
		public int? AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }
		public int Id { get; set; }
		public int? LagerId { get; set; }

		public BSD_AccessProfileLagerEntity() { }

		public BSD_AccessProfileLagerEntity(DataRow dataRow)
		{
			AccessProfileId = (dataRow["AccessProfileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AccessProfileId"]);
			AccessProfileName = (dataRow["AccessProfileName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["AccessProfileName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LagerId = (dataRow["LagerId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LagerId"]);
		}
	}
}

