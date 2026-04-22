using System;

namespace Psz.Core.BaseData.Models.Settings.HourlyRate
{
	public class HourlyRateRequestModel
	{
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public decimal? HourlyRate { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public string LastEditUserName { get; set; }
		public int? ProductionSiteId { get; set; }
		public string ProductionSiteName { get; set; }

		public Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity
			{
				CreationTime = CreationTime,
				CreationUserId = CreationUserId,
				CreationUserName = CreationUserName,
				HourlyRate = HourlyRate,
				Id = Id,
				LastEditTime = LastEditTime,
				LastEditUserId = LastEditUserId,
				LastEditUserName = LastEditUserName,
				ProductionSiteId = ProductionSiteId,
				ProductionSiteName = ProductionSiteName,
			};
		}
	}
	public class HourlyRateResponseModel
	{
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public decimal? HourlyRate { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public string LastEditUserName { get; set; }
		public int? ProductionSiteId { get; set; }
		public string ProductionSiteName { get; set; }
		public HourlyRateResponseModel(Infrastructure.Data.Entities.Tables.BSD.HourlyRateEntity entity)
		{
			if(entity is null)
			{
				return;
			}

			CreationTime = entity.CreationTime;
			CreationUserId = entity.CreationUserId;
			CreationUserName = entity.CreationUserName;
			HourlyRate = entity.HourlyRate;
			Id = entity.Id;
			LastEditTime = entity.LastEditTime;
			LastEditUserId = entity.LastEditUserId;
			LastEditUserName = entity.LastEditUserName;
			ProductionSiteId = entity.ProductionSiteId;
			ProductionSiteName = entity.ProductionSiteName;
		}
	}
}
