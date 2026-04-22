using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class PlantBookingsLogsEntity
	{
		public int Id { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public string LastUpdateUserFullName { get; set; }
		public int? LastUpdateUserId { get; set; }
		public string LastUpdateUsername { get; set; }
		public string LogDescription { get; set; }
		public string LogObject { get; set; }
		public int? LogObjectId { get; set; }

		public PlantBookingsLogsEntity() { }

		public PlantBookingsLogsEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			LastUpdateTime = dataRow["LastUpdateTime"] == DBNull.Value ? null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserFullName = dataRow["LastUpdateUserFullName"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdateUserFullName"]);
			LastUpdateUserId = dataRow["LastUpdateUserId"] == DBNull.Value ? null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			LastUpdateUsername = dataRow["LastUpdateUsername"] == DBNull.Value ? "" : Convert.ToString(dataRow["LastUpdateUsername"]);
			LogDescription = dataRow["LogDescription"] == DBNull.Value ? "" : Convert.ToString(dataRow["LogDescription"]);
			LogObject = dataRow["LogObject"] == DBNull.Value ? "" : Convert.ToString(dataRow["LogObject"]);
			LogObjectId = dataRow["LogObjectId"] == DBNull.Value ? null : Convert.ToInt32(dataRow["LogObjectId"]);
		}

		public PlantBookingsLogsEntity ShallowClone()
		{
			return new PlantBookingsLogsEntity
			{
				Id = Id,
				LastUpdateTime = LastUpdateTime,
				LastUpdateUserFullName = LastUpdateUserFullName,
				LastUpdateUserId = LastUpdateUserId,
				LastUpdateUsername = LastUpdateUsername,
				LogDescription = LogDescription,
				LogObject = LogObject,
				LogObjectId = LogObjectId
			};
		}
	}
}
