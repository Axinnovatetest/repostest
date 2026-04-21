using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class ProductionOrderChangeHistoryEntity
	{
		public DateTime? ChangeDate { get; set; }
		public DateTime? ConfirmedDeadline { get; set; }
		public DateTime? PreviousDeadline { get; set; }
		public int? ProductionOrderNumber { get; set; }
		public string ProductionOrderStatus { get; set; }
		public Single? ProductionOrderTime { get; set; }
		public int? ProductionOrderWarehouseId { get; set; }
		public int? ID { get; set; }

		public ProductionOrderChangeHistoryEntity() { }

		public ProductionOrderChangeHistoryEntity(DataRow dataRow)
		{
			ChangeDate = (dataRow["ChangeDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ChangeDate"]);
			ConfirmedDeadline = (dataRow["ConfirmedDeadline"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ConfirmedDeadline"]);
			PreviousDeadline = (dataRow["PreviousDeadline"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["PreviousDeadline"]);
			ProductionOrderNumber = (dataRow["ProductionOrderNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderNumber"]);
			ProductionOrderStatus = (dataRow["ProductionOrderStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionOrderStatus"]);
			ProductionOrderTime = (dataRow["ProductionOrderTime"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["ProductionOrderTime"]);
			ProductionOrderWarehouseId = (dataRow["ProductionOrderWarehouseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderWarehouseId"]);
			ID = (dataRow["ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID"]);
		}
		public ProductionOrderChangeHistoryEntity ShallowClone()
		{
			return new ProductionOrderChangeHistoryEntity
			{
				ChangeDate = ChangeDate,
				ConfirmedDeadline = ConfirmedDeadline,
				PreviousDeadline = PreviousDeadline,
				ProductionOrderNumber = ProductionOrderNumber,
				ProductionOrderStatus = ProductionOrderStatus,
				ProductionOrderTime = ProductionOrderTime,
				ProductionOrderWarehouseId = ProductionOrderWarehouseId
			};
		}
	}
	public class ProductionOrderChangeHistoryWarehouseEntity
	{
		public int? ProductionOrderCount { get; set; }
		public string ProductionOrderStatus { get; set; }
		public decimal? ProductionOrderTime { get; set; }
		public int? ProductionOrderWarehouseId { get; set; }

		public ProductionOrderChangeHistoryWarehouseEntity() { }

		public ProductionOrderChangeHistoryWarehouseEntity(DataRow dataRow)
		{
			ProductionOrderCount = (dataRow["ProductionOrderCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderCount"]);
			ProductionOrderStatus = (dataRow["ProductionOrderStatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ProductionOrderStatus"]);
			ProductionOrderTime = (dataRow["ProductionOrderTime"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionOrderTime"]);
			ProductionOrderWarehouseId = (dataRow["ProductionOrderWarehouseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderWarehouseId"]);
		}
	}
	public class ProductionOrderChangeHistoryWarehouseYearWeekEntity
	{
		public int? ProductionOrderCount { get; set; }
		public Single? ProductionOrderTime { get; set; }
		public int? ProductionOrderWarehouseId { get; set; }
		public int? ChangeDateYear { get; set; }
		public int? ChangeDateWeek { get; set; }
		public int ProductionOrderChangeId { get; set; }

		public ProductionOrderChangeHistoryWarehouseYearWeekEntity() { }

		public ProductionOrderChangeHistoryWarehouseYearWeekEntity(DataRow dataRow)
		{
			ChangeDateWeek = (dataRow["ChangeDateWeek"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ChangeDateWeek"]);
			ChangeDateYear = (dataRow["ChangeDateYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ChangeDateYear"]);
			ProductionOrderCount = (dataRow["ProductionOrderCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderCount"]);
			ProductionOrderTime = (dataRow["ProductionOrderTime"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["ProductionOrderTime"]);
			ProductionOrderWarehouseId = (dataRow["ProductionOrderWarehouseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderWarehouseId"]);
			ProductionOrderChangeId = (dataRow["ProductionOrderChangeId"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ProductionOrderChangeId"]);
		}
	}
	public class ProductionOrderChangeHistoryWarehouseYearWeekFullEntity
	{
		public int? ProductionOrderCountIn { get; set; }
		public int? ProductionOrderCountOut { get; set; }
		public decimal? ProductionOrderTimeIn { get; set; }
		public decimal? ProductionOrderTimeOut { get; set; }
		public int? ProductionOrderWarehouseId { get; set; }
		public int? ChangeDateYear { get; set; }
		public int? ChangeDateWeek { get; set; }

		public ProductionOrderChangeHistoryWarehouseYearWeekFullEntity() { }

		public ProductionOrderChangeHistoryWarehouseYearWeekFullEntity(DataRow dataRow)
		{
			ChangeDateWeek = (dataRow["ChangeDateWeek"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ChangeDateWeek"]);
			ChangeDateYear = (dataRow["ChangeDateYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ChangeDateYear"]);
			ProductionOrderCountIn = (dataRow["ProductionOrderCountIn"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderCountIn"]);
			ProductionOrderCountOut = (dataRow["ProductionOrderCountOut"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderCountOut"]);
			ProductionOrderTimeIn = (dataRow["ProductionOrderTimeIn"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionOrderTimeIn"]);
			ProductionOrderTimeOut = (dataRow["ProductionOrderTimeOut"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionOrderTimeOut"]);
			ProductionOrderWarehouseId = (dataRow["ProductionOrderWarehouseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderWarehouseId"]);
		}
	}

	public class FaChangesWeekYearHoursLeftEntity
	{
		public int? AffectedWeek { get; set; }
		public int? AffectedYear { get; set; }
		public int? Lager { get; set; }
		public decimal? HoursLeft { get; set; }
		public FaChangesWeekYearHoursLeftEntity() { }

		public FaChangesWeekYearHoursLeftEntity(DataRow dataRow)
		{
			AffectedWeek = (dataRow["AffectedWeek"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AffectedWeek"]);
			AffectedYear = (dataRow["AffectedYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AffectedYear"]);
			HoursLeft = (dataRow["HoursLeft"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["HoursLeft"]);
			Lager = (dataRow["Lager"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lager"]);
		}
	}
}
