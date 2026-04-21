using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class HourlyRateEntity
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

		public HourlyRateEntity() { }

		public HourlyRateEntity(DataRow dataRow)
		{
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			CreationUserName = (dataRow["CreationUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CreationUserName"]);
			HourlyRate = (dataRow["HourlyRate"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["HourlyRate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			LastEditUserName = (dataRow["LastEditUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastEditUserName"]);
			ProductionSiteId = (dataRow["ProductionSiteId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionSiteId"]);
			ProductionSiteName = (dataRow["ProductionSiteName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionSiteName"]);
		}

		public HourlyRateEntity ShallowClone()
		{
			return new HourlyRateEntity
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
				ProductionSiteName = ProductionSiteName
			};
		}
	}
}

