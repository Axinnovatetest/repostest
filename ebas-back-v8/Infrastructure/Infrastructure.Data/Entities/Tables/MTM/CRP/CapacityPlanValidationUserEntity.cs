using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class CapacityPlanValidationUserEntity
	{
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public int Id { get; set; }
		public string UserEmail { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public int? ValidationLevel { get; set; }

		public CapacityPlanValidationUserEntity() { }

		public CapacityPlanValidationUserEntity(DataRow dataRow)
		{
			CountryId = Convert.ToInt32(dataRow["CountryId"]);
			CountryName = (dataRow["CountryName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CountryName"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UserEmail = (dataRow["UserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserEmail"]);
			UserId = Convert.ToInt32(dataRow["UserId"]);
			UserName = (dataRow["UserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UserName"]);
			ValidationLevel = (dataRow["ValidationLevel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ValidationLevel"]);
		}
	}
}

