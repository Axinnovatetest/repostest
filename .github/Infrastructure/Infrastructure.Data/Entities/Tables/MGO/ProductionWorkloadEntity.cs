using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.MGO
{
    public class ProductionWorkloadEntity
    {
		public int? FaArticleCount { get; set; }
		public int? FaCount { get; set; }
		public int? FaTime { get; set; }
		public int? FaTotalQuantity { get; set; }
		public int? FaWeek { get; set; }
		public int? FaYear { get; set; }
		public int Id { get; set; }
		public int? RecordSyncId { get; set; }
		public DateTime? RecordSyncTime { get; set; }
		public int? RecordSyncUserId { get; set; }
		public int? WarehouseId { get; set; }
		public decimal? WarehouseMaxCapacity { get; set; }

        public ProductionWorkloadEntity() { }

        public ProductionWorkloadEntity(DataRow dataRow)
        {
			FaArticleCount = (dataRow["FaArticleCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaArticleCount"]);
			FaCount = (dataRow["FaCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaCount"]);
			FaTime = (dataRow["FaTime"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaTime"]);
			FaTotalQuantity = (dataRow["FaTotalQuantity"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaTotalQuantity"]);
			FaWeek = (dataRow["FaWeek"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaWeek"]);
			FaYear = (dataRow["FaYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FaYear"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			RecordSyncId = (dataRow["RecordSyncId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RecordSyncId"]);
			RecordSyncTime = (dataRow["RecordSyncTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["RecordSyncTime"]);
			RecordSyncUserId = (dataRow["RecordSyncUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RecordSyncUserId"]);
			WarehouseId = (dataRow["WarehouseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WarehouseId"]);
			WarehouseMaxCapacity = (dataRow["WarehouseMaxCapacity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["WarehouseMaxCapacity"]);
        }
    
        public ProductionWorkloadEntity ShallowClone()
        {
            return new ProductionWorkloadEntity
            {
			FaArticleCount = FaArticleCount,
			FaCount = FaCount,
			FaTime = FaTime,
			FaTotalQuantity = FaTotalQuantity,
			FaWeek = FaWeek,
			FaYear = FaYear,
			Id = Id,
			RecordSyncId = RecordSyncId,
			RecordSyncTime = RecordSyncTime,
			RecordSyncUserId = RecordSyncUserId,
			WarehouseId = WarehouseId
            };
        }
    }
}

