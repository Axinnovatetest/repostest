using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
    public class InsideSalesChecksLogsEntity
    {
		public int Id { get; set; }
		public int? NewRecordCount { get; set; }
		public DateTime? RecordTime { get; set; }

        public InsideSalesChecksLogsEntity() { }

        public InsideSalesChecksLogsEntity(DataRow dataRow)
        {
			Id = Convert.ToInt32(dataRow["Id"]);
			NewRecordCount = (dataRow["NewRecordCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["NewRecordCount"]);
			RecordTime = (dataRow["RecordTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["RecordTime"]);
        }
    
        public InsideSalesChecksLogsEntity ShallowClone()
        {
            return new InsideSalesChecksLogsEntity
            {
			Id = Id,
			NewRecordCount = NewRecordCount,
			RecordTime = RecordTime
            };
        }
    }
}

