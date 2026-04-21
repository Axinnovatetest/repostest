using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
    public class __bsd_pm_MileStonesEntity
    {
		public string Comment { get; set; }
		public DateTime? CreationTime { get; set; }
		public int? CreationUserId { get; set; }
		public DateTime? EndDate { get; set; }
		public int Id { get; set; }
		public DateTime? StartDate { get; set; }

        public __bsd_pm_MileStonesEntity() { }

        public __bsd_pm_MileStonesEntity(DataRow dataRow)
        {
			Comment = (dataRow["Comment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Comment"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CreationUserId = (dataRow["CreationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreationUserId"]);
			EndDate = (dataRow["EndDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["EndDate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			StartDate = (dataRow["StartDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["StartDate"]);
        }
    
        public __bsd_pm_MileStonesEntity ShallowClone()
        {
            return new __bsd_pm_MileStonesEntity
            {
			Comment = Comment,
			CreationTime = CreationTime,
			CreationUserId = CreationUserId,
			EndDate = EndDate,
			Id = Id,
			StartDate = StartDate
            };
        }
    }
}

