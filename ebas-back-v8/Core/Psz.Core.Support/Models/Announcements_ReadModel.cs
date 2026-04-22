using Infrastructure.Data.Entities.Tables._Commun;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Models
{
	public class Announcements_ReadModel
	{
		public int? AnnouncementId { get; set; }
		public int Id { get; set; }
		public DateTime? ReadAt { get; set; }
		public string Username { get; set; }

		public Announcements_ReadModel() { }

		public Announcements_ReadModel(DataRow dataRow)
		{
			AnnouncementId = (dataRow["AnnouncementId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AnnouncementId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ReadAt = (dataRow["ReadAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ReadAt"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}

		public Announcements_ReadModel(Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncements_ReadEntity entity)
		{
			AnnouncementId = entity.AnnouncementId;
			Id = entity.Id;
			Username = entity.Username;
			ReadAt = entity.ReadAt;

		}
	}
}
