using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class LineItemPlanEntity
	{
		public long Id { get; set; }
		public long LineItemId { get; set; }
		public DateTime? OrderDate { get; set; }
		public int? OrderId { get; set; }
		public int? OrderItemId { get; set; }
		public int? OrderUserId { get; set; }
		public decimal? PlanningQuantityChange { get; set; }
		public decimal? PlanningQuantityCumulativeQuantity { get; set; }
		public int? PlanningQuantityDeliveryPlanStatusIdentifier { get; set; }
		public string PlanningQuantityFrequencyIdentifier { get; set; }
		public decimal? PlanningQuantityQuantity { get; set; }
		public DateTime? PlanningQuantityRequestedShipmentDate { get; set; }
		public string PlanningQuantityUnitOfMeasure { get; set; }
		public DateTime? PlanningQuantityWeeklyPeriodEndDate { get; set; }
		public int PositionNumber { get; set; }
		public DateTime? ProductionConfirmationDate { get; set; }
		public int? ProductionConfirmationId { get; set; }
		public int? ProductionConfirmationItemId { get; set; }
		public int? ProductionConfirmationUserId { get; set; }
		public DateTime? ProductionDate { get; set; }
		public int? ProductionId { get; set; }
		public int? ProductionOrderId { get; set; }
		public int? ProductionOrderUserId { get; set; }
		public int? ProductionUserId { get; set; }

		public LineItemPlanEntity() { }

		public LineItemPlanEntity(DataRow dataRow)
		{
			Id = Convert.ToInt64(dataRow["Id"]);
			LineItemId = Convert.ToInt64(dataRow["LineItemId"]);
			OrderDate = (dataRow["OrderDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OrderDate"]);
			OrderId = (dataRow["OrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderId"]);
			OrderItemId = (dataRow["OrderItemId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderItemId"]);
			OrderUserId = (dataRow["OrderUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderUserId"]);
			PlanningQuantityChange = (dataRow["PlanningQuantityChange"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PlanningQuantityChange"]);
			PlanningQuantityCumulativeQuantity = (dataRow["PlanningQuantityCumulativeQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PlanningQuantityCumulativeQuantity"]);
			PlanningQuantityDeliveryPlanStatusIdentifier = (dataRow["PlanningQuantityDeliveryPlanStatusIdentifier"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PlanningQuantityDeliveryPlanStatusIdentifier"]);
			PlanningQuantityFrequencyIdentifier = (dataRow["PlanningQuantityFrequencyIdentifier"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PlanningQuantityFrequencyIdentifier"]);
			PlanningQuantityQuantity = (dataRow["PlanningQuantityQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PlanningQuantityQuantity"]);
			PlanningQuantityRequestedShipmentDate = (dataRow["PlanningQuantityRequestedShipmentDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["PlanningQuantityRequestedShipmentDate"]);
			PlanningQuantityUnitOfMeasure = (dataRow["PlanningQuantityUnitOfMeasure"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PlanningQuantityUnitOfMeasure"]);
			PlanningQuantityWeeklyPeriodEndDate = (dataRow["PlanningQuantityWeeklyPeriodEndDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["PlanningQuantityWeeklyPeriodEndDate"]);
			PositionNumber = Convert.ToInt32(dataRow["PositionNumber"]);
			ProductionConfirmationDate = (dataRow["ProductionConfirmationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ProductionConfirmationDate"]);
			ProductionConfirmationId = (dataRow["ProductionConfirmationId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionConfirmationId"]);
			ProductionConfirmationItemId = (dataRow["ProductionConfirmationItemId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionConfirmationItemId"]);
			ProductionConfirmationUserId = (dataRow["ProductionConfirmationUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionConfirmationUserId"]);
			ProductionDate = (dataRow["ProductionDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ProductionDate"]);
			ProductionId = (dataRow["ProductionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionId"]);
			ProductionOrderId = (dataRow["ProductionOrderId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderId"]);
			ProductionOrderUserId = (dataRow["ProductionOrderUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionOrderUserId"]);
			ProductionUserId = (dataRow["ProductionUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProductionUserId"]);
		}

		public LineItemPlanEntity ShallowClone()
		{
			return new LineItemPlanEntity
			{
				Id = Id,
				LineItemId = LineItemId,
				OrderDate = OrderDate,
				OrderId = OrderId,
				OrderItemId = OrderItemId,
				OrderUserId = OrderUserId,
				PlanningQuantityChange = PlanningQuantityChange,
				PlanningQuantityCumulativeQuantity = PlanningQuantityCumulativeQuantity,
				PlanningQuantityDeliveryPlanStatusIdentifier = PlanningQuantityDeliveryPlanStatusIdentifier,
				PlanningQuantityFrequencyIdentifier = PlanningQuantityFrequencyIdentifier,
				PlanningQuantityQuantity = PlanningQuantityQuantity,
				PlanningQuantityRequestedShipmentDate = PlanningQuantityRequestedShipmentDate,
				PlanningQuantityUnitOfMeasure = PlanningQuantityUnitOfMeasure,
				PlanningQuantityWeeklyPeriodEndDate = PlanningQuantityWeeklyPeriodEndDate,
				PositionNumber = PositionNumber,
				ProductionConfirmationDate = ProductionConfirmationDate,
				ProductionConfirmationId = ProductionConfirmationId,
				ProductionConfirmationItemId = ProductionConfirmationItemId,
				ProductionConfirmationUserId = ProductionConfirmationUserId,
				ProductionDate = ProductionDate,
				ProductionId = ProductionId,
				ProductionOrderId = ProductionOrderId,
				ProductionOrderUserId = ProductionOrderUserId,
				ProductionUserId = ProductionUserId
			};
		}
	}
}

