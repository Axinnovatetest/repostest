using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.MGO
{
    public class WarehouseWeekMaxCapacityEntity
    {
		public DateTime? CreateTime { get; set; }
		public int? CreateUserId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public int? LastEditUserId { get; set; }
		public string Warehouse { get; set; }
		public int? WarehouseId { get; set; }
		public int? Week { get; set; }
		public int? WeekMaxCapacity { get; set; }

        public WarehouseWeekMaxCapacityEntity() { }

        public WarehouseWeekMaxCapacityEntity(DataRow dataRow)
        {
			CreateTime = (dataRow["CreateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreateTime"]);
			CreateUserId = (dataRow["CreateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CreateUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUserId = (dataRow["LastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastEditUserId"]);
			Warehouse = (dataRow["Warehouse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warehouse"]);
			WarehouseId = (dataRow["WarehouseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WarehouseId"]);
			Week = (dataRow["Week"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Week"]);
			WeekMaxCapacity = (dataRow["WeekMaxCapacity"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WeekMaxCapacity"]);
        }
    
        public WarehouseWeekMaxCapacityEntity ShallowClone()
        {
            return new WarehouseWeekMaxCapacityEntity
            {
			CreateTime = CreateTime,
			CreateUserId = CreateUserId,
			Id = Id,
			LastEditTime = LastEditTime,
			LastEditUserId = LastEditUserId,
			Warehouse = Warehouse,
			WarehouseId = WarehouseId,
			Week = Week,
			WeekMaxCapacity = WeekMaxCapacity
            };
        }
    }
}

