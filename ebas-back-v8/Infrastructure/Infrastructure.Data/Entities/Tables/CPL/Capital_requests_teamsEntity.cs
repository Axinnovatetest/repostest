using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CPL
{
	public class Capital_requests_teamsEntity
	{
		public DateTime? AddTime { get; set; }
		public int? AddUser { get; set; }
		public int Id { get; set; }
		public int? PlantId { get; set; }
		public string Team { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

		public Capital_requests_teamsEntity() { }

		public Capital_requests_teamsEntity(DataRow dataRow)
		{
			AddTime = (dataRow["AddTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AddTime"]);
			AddUser = (dataRow["AddUser"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AddUser"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			PlantId = (dataRow["PlantId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PlantId"]);
			Team = (dataRow["Team"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Team"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}

		public Capital_requests_teamsEntity ShallowClone()
		{
			return new Capital_requests_teamsEntity
			{
				AddTime = AddTime,
				AddUser = AddUser,
				Id = Id,
				PlantId = PlantId,
				Team = Team,
				UserId = UserId,
				Username = Username
			};
		}
	}
}

