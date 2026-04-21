using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.PlantBookings;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class LogsDataRequestModel: IPaginatedRequestModel
	{
		public string SearchValue { get; set; }
	}
	public class LogsDataResponseModel
	{
		public int Id { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public string LastUpdateUserFullName { get; set; }
		public int? LastUpdateUserId { get; set; }
		public string LastUpdateUsername { get; set; }
		public string LogDescription { get; set; }
		public string LogObject { get; set; }
		public int? LogObjectId { get; set; }
		public bool isRemoved { get; set; }

		public LogsDataResponseModel(Infrastructure.Data.Entities.Tables.Logistics.PlantBookingsLogsEntity __lgtplantbookingslogsEntity)
		{
			if(__lgtplantbookingslogsEntity == null)
				return;

			Id = __lgtplantbookingslogsEntity.Id;
			LastUpdateTime = __lgtplantbookingslogsEntity.LastUpdateTime;
			LastUpdateUserFullName = __lgtplantbookingslogsEntity.LastUpdateUserFullName;
			LastUpdateUserId = __lgtplantbookingslogsEntity.LastUpdateUserId;
			LastUpdateUsername = __lgtplantbookingslogsEntity.LastUpdateUsername;
			LogDescription = __lgtplantbookingslogsEntity.LogDescription;
			LogObject = __lgtplantbookingslogsEntity.LogObject;
			LogObjectId = __lgtplantbookingslogsEntity.LogObjectId;
			isRemoved = __lgtplantbookingslogsEntity.LogDescription.ToLower().Contains("Delete".ToLower())?true:false;
		}
	}
}
public class LogsResponseModel: IPaginatedResponseModel<LogsDataResponseModel>
{
}
