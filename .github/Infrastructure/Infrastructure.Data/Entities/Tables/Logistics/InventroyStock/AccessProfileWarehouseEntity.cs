using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
    public class AccessProfileWarehouseEntity
    {
		public int AccessProfileId { get; set; }
		public int Id { get; set; }
		public DateTime? LastEditTime { get; set; }
		public string LastEditUser { get; set; }
		public int WarehouseId { get; set; }

        public AccessProfileWarehouseEntity() { }

        public AccessProfileWarehouseEntity(DataRow dataRow)
        {
			AccessProfileId = Convert.ToInt32(dataRow["AccessProfileId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LastEditTime = (dataRow["LastEditTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastEditTime"]);
			LastEditUser = (dataRow["LastEditUser"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastEditUser"]);
			WarehouseId = Convert.ToInt32(dataRow["WarehouseId"]);
        }
    
        public AccessProfileWarehouseEntity ShallowClone()
        {
            return new AccessProfileWarehouseEntity
            {
			AccessProfileId = AccessProfileId,
			Id = Id,
			LastEditTime = LastEditTime,
			LastEditUser = LastEditUser,
			WarehouseId = WarehouseId
            };
        }
    }
}

