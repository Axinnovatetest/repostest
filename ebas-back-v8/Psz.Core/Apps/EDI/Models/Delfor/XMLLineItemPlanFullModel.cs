using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class XMLLineItemPlanFullModel
	{
		// - LineItem props
		public DateTime? CallOffDateTime { get; set; }
		public int? CallOffNumber { get; set; }
		public decimal? CumulativeReceivedQuantity { get; set; }
		public decimal? CumulativeScheduledQuantity { get; set; }
		public string CustomersItemMaterialNumber { get; set; }
		public string DrawingRevisionNumber { get; set; }
		public string DocumentNumber { get; set; }
		public long HeaderId { get; set; }
		//public long Id { get; set; }
		public decimal? LastReceivedQuantity { get; set; }
		public DateTime? MaterialAuthorizationDate { get; set; }
		public decimal? MaterialAuthorizationQuantity { get; set; }
		public DateTime? PlanningHorizionEnd { get; set; }
		public DateTime? PlanningHorizionStart { get; set; }
		public int PositionNumber { get; set; }
		public DateTime? PreviousCallOffDate { get; set; }
		public int? PreviousCallOffNumber { get; set; }
		public DateTime? ProductionAuthorizationDateTime { get; set; }
		public decimal? ProductionAuthorizationQuantity { get; set; }
		public string SuppliersItemMaterialNumber { get; set; }
		public DateTime? PlanningQuantityWeeklyPeriodEndDate { get; set; }
		public string BuyersInternalProductGroupCode { get; set; }
		public DateTime? LastASNDate { get; set; }
		public DateTime? LastASNDeliveryDate { get; set; }
		public string LastASNNumber { get; set; }

		// - LineItemPlan props
		public long Id { get; set; }
		public long LineItemId { get; set; }
		public decimal? PlanningQuantityCumulativeQuantity { get; set; }
		public string PlanningQuantityFrequencyIdentifier { get; set; }
		public decimal? PlanningQuantityQuantity { get; set; }
		public DateTime? PlanningQuantityRequestedShipmentDate { get; set; }
		public string PlanningQuantityUnitOfMeasure { get; set; }
		public int? PlanningQuantityDeliveryPlanStatusIdentifier { get; set; }

		public XMLLineItemPlanFullModel(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity lineItemEntity,
			Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity lineItemPlanEntity)
		{
			if(lineItemEntity == null || lineItemPlanEntity == null)
				return;

			CallOffDateTime = lineItemEntity.CallOffDateTime;
			CallOffNumber = lineItemEntity.CallOffNumber;
			CumulativeReceivedQuantity = lineItemEntity.CumulativeReceivedQuantity;
			CumulativeScheduledQuantity = lineItemEntity.CumulativeScheduledQuantity;
			CustomersItemMaterialNumber = lineItemEntity.CustomersItemMaterialNumber;
			HeaderId = lineItemEntity.HeaderId;
			//Id = lineItemEntity.Id;
			LastReceivedQuantity = lineItemEntity.LastReceivedQuantity;
			MaterialAuthorizationDate = lineItemEntity.MaterialAuthorizationDate;
			MaterialAuthorizationQuantity = lineItemEntity.MaterialAuthorizationQuantity;
			PlanningHorizionEnd = lineItemEntity.PlanningHorizionEnd;
			PlanningHorizionStart = lineItemEntity.PlanningHorizionStart;
			PositionNumber = lineItemEntity.PositionNumber;
			PreviousCallOffDate = lineItemEntity.PreviousCallOffDate;
			PreviousCallOffNumber = lineItemEntity.PreviousCallOffNumber;
			ProductionAuthorizationDateTime = lineItemEntity.ProductionAuthorizationDateTime;
			ProductionAuthorizationQuantity = lineItemEntity.ProductionAuthorizationQuantity;
			SuppliersItemMaterialNumber = lineItemEntity.SuppliersItemMaterialNumber;
			DrawingRevisionNumber = lineItemEntity.DrawingRevisionNumber;
			DocumentNumber = lineItemEntity.DocumentNumber;
			BuyersInternalProductGroupCode = lineItemEntity.BuyersInternalProductGroupCode;
			LastASNDate = lineItemEntity.LastASNDate;
			LastASNDeliveryDate = lineItemEntity.LastASNDeliveryDate;
			LastASNNumber = lineItemEntity.LastASNNumber;

			// -
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
		}
	}
}
