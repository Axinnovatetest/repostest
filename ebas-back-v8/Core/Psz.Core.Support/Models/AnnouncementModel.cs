using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static Psz.Core.BaseData.Enums.BomChangeEnums;

namespace Psz.Core.Support.Models
{
	public class AnnouncementModel
	{
		public DateTime? CreatedAt { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? EndDate { get; set; }
		public int Id { get; set; }
		public bool? IsActive { get; set; }
		public string Message { get; set; }
		public DateTime? StartDate { get; set; }
		public string Title { get; set; }

		public AnnouncementModel() { }

		public AnnouncementModel(DataRow dataRow)
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

		public AnnouncementModel(Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity entity)
		{
			if(entity == null)
				return;
			Id = entity.Id;
			Title = entity.Title;
		    Message = entity.Message;
			StartDate = entity.StartDate;
			EndDate = entity.EndDate;
			CreatedAt = entity.CreatedAt;
			CreatedBy= entity.CreatedBy;
			IsActive= entity.IsActive;
		
		}
		public Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables._Commun.GoLiveAnnouncementsEntity
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
