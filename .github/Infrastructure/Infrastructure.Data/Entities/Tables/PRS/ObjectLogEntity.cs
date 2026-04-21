using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class ObjectLogEntity
	{
		public int Id { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? LastUpdateUserId { get; set; }
		public string LastUpdateUsername { get; set; }
		public string LastUpdateUserFullName { get; set; }
		public string LogDescription { get; set; }
		public string LogObject { get; set; }
		public int LogObjectId { get; set; }

		public ObjectLogEntity() { }

		public ObjectLogEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			LastUpdateUsername = (dataRow["LastUpdateUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastUpdateUsername"]);
			LastUpdateUserFullName = (dataRow["LastUpdateUserFullName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastUpdateUserFullName"]);
			LogDescription = (dataRow["LogDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogDescription"]);
			LogObject = (dataRow["LogObject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogObject"]);
			LogObjectId = Convert.ToInt32(dataRow["LogObjectId"]);
		}
	}
}

