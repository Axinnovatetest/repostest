using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class LineItemPlanModel
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

		public LineItemPlanModel(Models.Delfor.DocumentDetailsForecastDataForecastLineItemPlanningQuantity itemPlanningQuantity, int lineItemId, int positionNumber)
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
		}
		public LineItemPlanModel(Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity lineItemPlanEntity)
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
			};
		}
	}
}
