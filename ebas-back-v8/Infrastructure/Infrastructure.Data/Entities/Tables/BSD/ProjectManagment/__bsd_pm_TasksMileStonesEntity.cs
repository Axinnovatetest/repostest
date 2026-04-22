using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.BSD
{
    public class __bsd_pm_TasksMileStonesEntity
    {
		public int Id { get; set; }
		public int? IdMileStone { get; set; }
		public int? IdTask { get; set; }
		public string TaskName { get; set; }

        public __bsd_pm_TasksMileStonesEntity() { }

        public __bsd_pm_TasksMileStonesEntity(DataRow dataRow)
        {
			Id = Convert.ToInt32(dataRow["Id"]);
			IdMileStone = (dataRow["IdMileStone"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdMileStone"]);
			IdTask = (dataRow["IdTask"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["IdTask"]);
			TaskName = (dataRow["TaskName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["TaskName"]);
        }
    
        public __bsd_pm_TasksMileStonesEntity ShallowClone()
        {
            return new __bsd_pm_TasksMileStonesEntity
            {
			Id = Id,
			IdMileStone = IdMileStone,
			IdTask = IdTask,
			TaskName = TaskName
            };
        }
    }
}

