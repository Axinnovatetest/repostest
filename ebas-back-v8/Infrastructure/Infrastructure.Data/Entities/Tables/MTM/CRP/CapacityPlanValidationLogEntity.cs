using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class CapacityPlanValidationLogEntity
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int Id { get; set; }
		public int? ValidationLevel { get; set; }
		public int? ValidationStatus { get; set; }
		public string ValidationStatusName { get; set; }
		public DateTime? ValidationTime { get; set; }
		public int? ValidationUserId { get; set; }
		public int Year { get; set; }
		public string ValidationReason { get; set; }

		public CapacityPlanValidationLogEntity() { }

		public CapacityPlanValidationLogEntity(DataRow dataRow)
		{
			CountryId = Convert.ToInt32(dataRow["CountryId"]);
			CountryName = Convert.ToString(dataRow["CountryName"]);
			HallId = Convert.ToInt32(dataRow["HallId"]);
			HallName = Convert.ToString(dataRow["HallName"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ValidationLevel = (dataRow["ValidationLevel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ValidationLevel"]);
			ValidationStatus = (dataRow["ValidationStatus"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ValidationStatus"]);
			ValidationStatusName = (dataRow["ValidationStatusName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ValidationStatusName"]);
			ValidationTime = (dataRow["ValidationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidationTime"]);
			ValidationUserId = (dataRow["ValidationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ValidationUserId"]);
			Year = Convert.ToInt32(dataRow["Year"]);
			ValidationReason = (dataRow["ValidationReason"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ValidationReason"]);
		}
	}
}

