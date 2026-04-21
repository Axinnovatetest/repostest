using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests
{
	public class __BSD_Offer_Request_LogsEntity
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

		public __BSD_Offer_Request_LogsEntity() { }

		public __BSD_Offer_Request_LogsEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserFullName = (dataRow["LastUpdateUserFullName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastUpdateUserFullName"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			LastUpdateUsername = (dataRow["LastUpdateUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastUpdateUsername"]);
			LogDescription = (dataRow["LogDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogDescription"]);
			LogObject = (dataRow["LogObject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogObject"]);
			LogObjectId = (dataRow["LogObjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LogObjectId"]);
			ManufacturerNumber = (dataRow["ManufacturerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ManufacturerNumber"]);
			SupplierContactName = (dataRow["SupplierContactName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierContactName"]);
		}

		public __BSD_Offer_Request_LogsEntity ShallowClone()
		{
			return new __BSD_Offer_Request_LogsEntity
			{
				Id = Id,
				LastUpdateTime = LastUpdateTime,
				LastUpdateUserFullName = LastUpdateUserFullName,
				LastUpdateUserId = LastUpdateUserId,
				LastUpdateUsername = LastUpdateUsername,
				LogDescription = LogDescription,
				LogObject = LogObject,
				LogObjectId = LogObjectId,
				ManufacturerNumber = ManufacturerNumber,
				SupplierContactName = SupplierContactName
			};
		}
	}


	public class __BSD_Offer_Request_LogsWithTotalCountEntity
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
		public int? TotalCount { get; set; }

		public __BSD_Offer_Request_LogsWithTotalCountEntity() { }

		public __BSD_Offer_Request_LogsWithTotalCountEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserFullName = (dataRow["LastUpdateUserFullName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastUpdateUserFullName"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			LastUpdateUsername = (dataRow["LastUpdateUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastUpdateUsername"]);
			LogDescription = (dataRow["LogDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogDescription"]);
			LogObject = (dataRow["LogObject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogObject"]);
			LogObjectId = (dataRow["LogObjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LogObjectId"]);
			ManufacturerNumber = (dataRow["ManufacturerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ManufacturerNumber"]);
			SupplierContactName = (dataRow["SupplierContactName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierContactName"]);
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCount"]);
		}
	}

}
