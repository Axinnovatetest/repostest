using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CPL
{
	public class Capital_requests_engeneering_teamsEntity
	{
		public DateTime? AddTime { get; set; }
		public int? AddUserId { get; set; }
		public int Id { get; set; }
		public string Plant { get; set; }
		public int? PlantId { get; set; }
		public DateTime? UpdateTime { get; set; }
		public int? UpdateUserId { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

		public Capital_requests_engeneering_teamsEntity() { }

		public Capital_requests_engeneering_teamsEntity(DataRow dataRow)
		{
			AddTime = (dataRow["AddTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["AddTime"]);
			AddUserId = (dataRow["AddUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AddUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Plant = (dataRow["Plant"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Plant"]);
			PlantId = (dataRow["PlantId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PlantId"]);
			UpdateTime = (dataRow["UpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["UpdateTime"]);
			UpdateUserId = (dataRow["UpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UpdateUserId"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}

		public Capital_requests_engeneering_teamsEntity ShallowClone()
		{
			return new Capital_requests_engeneering_teamsEntity
			{
				AddTime = AddTime,
				AddUserId = AddUserId,
				Id = Id,
				Plant = Plant,
				PlantId = PlantId,
				UpdateTime = UpdateTime,
				UpdateUserId = UpdateUserId,
				UserId = UserId,
				Username = Username
			};
		}
	}
}

