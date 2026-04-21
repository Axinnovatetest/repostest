using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class XMLLineItemPlanModel
	{
		public long Id { get; set; }
		public long LineItemId { get; set; }
		public decimal? PlanningQuantityCumulativeQuantity { get; set; }
		public string PlanningQuantityFrequencyIdentifier { get; set; }
		public decimal? PlanningQuantityQuantity { get; set; }
		public DateTime? PlanningQuantityRequestedShipmentDate { get; set; }
		public string PlanningQuantityUnitOfMeasure { get; set; }
		public DateTime? PlanningQuantityWeeklyPeriodEndDate { get; set; }
		public int PositionNumber { get; set; }
		public int? PlanningQuantityDeliveryPlanStatusIdentifier { get; set; }
		// -
		public DateTime? ProductionDate { get; set; }
		public int? ProductionId { get; set; }
		public int? ProductionUserId { get; set; }
		public string ProductionUserName { get; set; }
		// - 
		public DateTime? OrderDate { get; set; }
		public int? OrderId { get; set; }
		public int? OrderItemId { get; set; }
		public int? OrderUserId { get; set; }
		public string OrderUserName { get; set; }
		public int OrdersCount { get; set; }
		public decimal RestQuantity { get; set; }

		public XMLLineItemPlanModel(Models.Delfor.DocumentDetailsForecastDataForecastLineItemPlanningQuantity itemPlanningQuantity, int lineItemId, int positionNumber)
		{
			if(itemPlanningQuantity == null)
				return;

			Id = -1;
			LineItemId = lineItemId;
			PositionNumber = positionNumber;

			PlanningQuantityCumulativeQuantity = itemPlanningQuantity.CumulativeQuantity;
			PlanningQuantityFrequencyIdentifier = itemPlanningQuantity.FrequencyIdentifier?.Trim();
			PlanningQuantityQuantity = itemPlanningQuantity.PlanningQuantity;
			PlanningQuantityRequestedShipmentDate = itemPlanningQuantity.RequestedShipmentDate?.DateTime;
			PlanningQuantityUnitOfMeasure = itemPlanningQuantity.QuantityUnitOfMeasure?.Trim();
			PlanningQuantityWeeklyPeriodEndDate = itemPlanningQuantity.WeeklyPeriodEndDate?.DateTime;
			PlanningQuantityDeliveryPlanStatusIdentifier = itemPlanningQuantity.DeliveryPlanStatusIdentifier;
			// -
			ProductionDate = null;
			ProductionId = null;
			ProductionUserId = null;
			ProductionUserName = null;
			// -
			OrderDate = null;
			OrderId = null;
			OrderItemId = null;
			OrderUserId = null;
			OrderUserName = null;
		}
		public XMLLineItemPlanModel(Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity lineItemPlanEntity, Infrastructure.Data.Entities.Tables.COR.UserEntity orderUserEntity, Infrastructure.Data.Entities.Tables.COR.UserEntity prodUserEntity, int abCount, decimal restQuantity)
		{
			if(lineItemPlanEntity == null)
				return;

			Id = lineItemPlanEntity.Id;
			LineItemId = lineItemPlanEntity.LineItemId;
			PlanningQuantityCumulativeQuantity = lineItemPlanEntity.PlanningQuantityCumulativeQuantity;
			PlanningQuantityFrequencyIdentifier = lineItemPlanEntity.PlanningQuantityFrequencyIdentifier;
			PlanningQuantityQuantity = lineItemPlanEntity.PlanningQuantityQuantity;
			PlanningQuantityRequestedShipmentDate = lineItemPlanEntity.PlanningQuantityRequestedShipmentDate;
			PlanningQuantityUnitOfMeasure = lineItemPlanEntity.PlanningQuantityUnitOfMeasure;
			PlanningQuantityWeeklyPeriodEndDate = lineItemPlanEntity.PlanningQuantityWeeklyPeriodEndDate;
			PositionNumber = lineItemPlanEntity.PositionNumber;
			PlanningQuantityDeliveryPlanStatusIdentifier = lineItemPlanEntity.PlanningQuantityDeliveryPlanStatusIdentifier;

			// -
			ProductionDate = lineItemPlanEntity?.ProductionDate;
			ProductionId = lineItemPlanEntity?.ProductionId;
			ProductionUserId = lineItemPlanEntity?.ProductionUserId;
			ProductionUserName = prodUserEntity?.Username;

			// -
			OrderDate = lineItemPlanEntity?.OrderDate;
			OrderId = lineItemPlanEntity?.OrderId;//-->for linking to AB
			OrderItemId = lineItemPlanEntity?.OrderItemId;
			OrderUserId = lineItemPlanEntity?.OrderUserId;
			OrderUserName = orderUserEntity?.Username;

			//souilmi -->for showing in table
			OrdersCount = abCount;// Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(lineItemPlanEntity?.OrderId ?? -1)?.Angebot_Nr ?? null;
			RestQuantity = restQuantity;
		}

		public Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity
			{
				Id = Id,
				LineItemId = LineItemId,
				PlanningQuantityCumulativeQuantity = PlanningQuantityCumulativeQuantity,
				PlanningQuantityFrequencyIdentifier = PlanningQuantityFrequencyIdentifier,
				PlanningQuantityQuantity = PlanningQuantityQuantity,
				PlanningQuantityRequestedShipmentDate = PlanningQuantityRequestedShipmentDate,
				PlanningQuantityUnitOfMeasure = PlanningQuantityUnitOfMeasure,
				PlanningQuantityWeeklyPeriodEndDate = PlanningQuantityWeeklyPeriodEndDate,
				PositionNumber = PositionNumber,
				PlanningQuantityDeliveryPlanStatusIdentifier = PlanningQuantityDeliveryPlanStatusIdentifier,
				ProductionDate = ProductionDate,
				ProductionId = ProductionId,
				ProductionUserId = ProductionUserId,
				OrderDate = OrderDate,
				OrderId = OrderId,
				OrderItemId = OrderItemId,
				OrderUserId = OrderUserId
			};
		}
	}
}
