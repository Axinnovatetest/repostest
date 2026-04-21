using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
    public class __bsd_pm_logsEntity
    {
		public int Id { get; set; }
		public string LogText { get; set; }
		public DateTime? LogTime { get; set; }
		public int? ProjectId { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

        public __bsd_pm_logsEntity() { }

        public __bsd_pm_logsEntity(DataRow dataRow)
        {
			Id = Convert.ToInt32(dataRow["Id"]);
			LogText = (dataRow["LogText"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogText"]);
			LogTime = (dataRow["LogTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LogTime"]);
			ProjectId = (dataRow["ProjectId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjectId"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
        }
    
        public __bsd_pm_logsEntity ShallowClone()
        {
            return new __bsd_pm_logsEntity
            {
			Id = Id,
			LogText = LogText,
			LogTime = LogTime,
			ProjectId = ProjectId,
			UserId = UserId,
			Username = Username
            };
        }
    }
}

