using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.ROH
{
	public  class GetLogChangesOfferRequestResponseModel
	{
		public int Id { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public string LastUpdateUserFullName { get; set; }
		public int? LastUpdateUserId { get; set; }
		public string LastUpdateUsername { get; set; }
		public string LogDescription { get; set; }
		public string LogObject { get; set; }
		public int? LogObjectId { get; set; }
		public string ManufacturerNumber { get; set; }
		public string SupplierContactName { get; set; }

		public GetLogChangesOfferRequestResponseModel(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsEntity data)
		{
			Id = data.Id;
			LastUpdateTime = data.LastUpdateTime;
			LastUpdateUserFullName = data.LastUpdateUserFullName;
			LastUpdateUserId = data.LastUpdateUserId;
			LastUpdateUsername = data.LastUpdateUsername;
			LogDescription = data.LogDescription;
			LogObject = data.LogObject;
			LogObjectId = data.LogObjectId;
			ManufacturerNumber = data.ManufacturerNumber;
			SupplierContactName = data.SupplierContactName;
		}
	}


	public class GetLogChangesOfferRequestWithTotalCountResponseModel
	{
		public int Id { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public string LastUpdateUserFullName { get; set; }
		public string LogTitle { get; set; }
		public int? LastUpdateUserId { get; set; }
		public string LastUpdateUsername { get; set; }
		public string LogDescription { get; set; }
		public string LogObject { get; set; }
		public int? LogObjectId { get; set; }
		public int? TotalCount { get; set; }
		public string ManufacturerNumber { get; set; }
		public string SupplierContactName { get; set; }

		public GetLogChangesOfferRequestWithTotalCountResponseModel(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.__BSD_Offer_Request_LogsWithTotalCountEntity data)
		{
			Id = data.Id;
			ManufacturerNumber = data.ManufacturerNumber;
			SupplierContactName = data.SupplierContactName;
			LastUpdateTime = data.LastUpdateTime;
			LastUpdateUserFullName = data.LastUpdateUserFullName;
			LastUpdateUserId = data.LastUpdateUserId;
			LastUpdateUsername = data.LastUpdateUsername;
			LogDescription = data.LogDescription;
			LogObject = data.LogObject;
			LogObjectId = data.LogObjectId;
			TotalCount = data.TotalCount;
		}
	}


	public class GetOfferRequestLogsRequestModel: IPaginatedRequestModel
	{
		public string FilterText { get; set; }
	}
}
