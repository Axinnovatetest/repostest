using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables._Commun
{
	public class GoLiveAnnouncementsEntity
	{
		public DateTime? CreatedAt { get; set; }
		public string CreatedBy { get; set; }
		public DateTime? EndDate { get; set; }
		public int Id { get; set; }
		public bool? IsActive { get; set; }
		public string Message { get; set; }
		public DateTime? StartDate { get; set; }
		public string Title { get; set; }

		public GoLiveAnnouncementsEntity() { }

		public GoLiveAnnouncementsEntity(DataRow dataRow)
		{
			CreatedAt = (dataRow["CreatedAt"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreatedAt"]);
			CreatedBy = (dataRow["CreatedBy"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreatedBy"]);
			EndDate = (dataRow["EndDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EndDate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsActive = (dataRow["IsActive"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["IsActive"]);
			Message = (dataRow["Message"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Message"]);
			StartDate = (dataRow["StartDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]);
			Title = (dataRow["Title"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Title"]);
		}

		public GoLiveAnnouncementsEntity ShallowClone()
		{
			return new GoLiveAnnouncementsEntity
			{
				CreatedAt = CreatedAt,
				CreatedBy = CreatedBy,
				EndDate = EndDate,
				Id = Id,
				IsActive = IsActive,
				Message = Message,
				StartDate = StartDate,
				Title = Title
			};
		}
	}
}

