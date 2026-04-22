using Infrastructure.Data.Entities.Tables.Logistics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables._Commun
{
	public class GoLiveAnnouncements_ReadEntity
	{
		public int? AnnouncementId { get; set; }
		public int Id { get; set; }
		public DateTime? ReadAt { get; set; }
		public string Username { get; set; }
		public int? UserId { get; set; }

		public GoLiveAnnouncements_ReadEntity() { }

		public GoLiveAnnouncements_ReadEntity(DataRow dataRow)
		{
			AnnouncementId = (dataRow["AnnouncementId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AnnouncementId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ReadAt = (dataRow["ReadAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ReadAt"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
		}

		public GoLiveAnnouncements_ReadEntity ShallowClone()
		{
			return new GoLiveAnnouncements_ReadEntity
			{
				AnnouncementId = AnnouncementId,
				Id = Id,
				ReadAt = ReadAt,
				Username = Username,
				UserId = UserId
			};
		}
	}
}

