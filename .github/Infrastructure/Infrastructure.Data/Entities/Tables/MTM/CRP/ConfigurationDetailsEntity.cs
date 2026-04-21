using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class ConfigurationDetailsEntity
	{
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public int DepartmentWeekNumber { get; set; }
		public int HeaderId { get; set; }
		public int Id { get; set; }
		public bool IsLowerThanThreshold { get; set; }
		public bool? Validated { get; set; }
		public DateTime? ValidatedDate { get; set; }
		public int? ValidatedUserId { get; set; }

		public ConfigurationDetailsEntity() { }

		public ConfigurationDetailsEntity(DataRow dataRow)
		{
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			DepartmentId = Convert.ToInt32(dataRow["DepartmentId"]);
			DepartmentName = Convert.ToString(dataRow["DepartmentName"]);
			DepartmentWeekNumber = Convert.ToInt32(dataRow["DepartmentWeekNumber"]);
			HeaderId = Convert.ToInt32(dataRow["HeaderId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IsLowerThanThreshold = Convert.ToBoolean(dataRow["IsLowerThanThreshold"]);
			Validated = (dataRow["Validated"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Validated"]);
			ValidatedDate = (dataRow["ValidatedDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ValidatedDate"]);
			ValidatedUserId = (dataRow["ValidatedUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ValidatedUserId"]);
		}
	}
}

