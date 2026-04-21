
namespace Psz.Core.CRP.Models.Delfor
{
	public class DelforLineItemModel
	{
		public DateTime? CallOffDateTime { get; set; }
		public int? CallOffNumber { get; set; }
		public decimal? CumulativeReceivedQuantity { get; set; }
		public decimal? CumulativeScheduledQuantity { get; set; }
		public string CustomersItemMaterialNumber { get; set; }
		public string DrawingRevisionNumber { get; set; }
		public string DocumentNumber { get; set; }
		public long HeaderId { get; set; }
		public long Id { get; set; }
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
		public string BuyersInternalProductGroupCode { get; set; }
		public DateTime? LastASNDate { get; set; }
		public DateTime? LastASNDeliveryDate { get; set; }
		public string LastASNNumber { get; set; }

		//souilmi 12/09/2022
		public int? NextVersionId { get; set; }
		public int? PreviousVersionId { get; set; }



		public DelforLineItemModel(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity lineItemEntity)
		{
			if(lineItemEntity == null)
				return;

			CallOffDateTime = lineItemEntity.CallOffDateTime;
			CallOffNumber = lineItemEntity.CallOffNumber;
			CumulativeReceivedQuantity = lineItemEntity.CumulativeReceivedQuantity;
			CumulativeScheduledQuantity = lineItemEntity.CumulativeScheduledQuantity;
			CustomersItemMaterialNumber = lineItemEntity.CustomersItemMaterialNumber;
			HeaderId = lineItemEntity.HeaderId;
			Id = lineItemEntity.Id;
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
		}
		public Infrastructure.Data.Entities.Tables.CTS.LineItemEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.LineItemEntity
			{
				CallOffDateTime = CallOffDateTime,
				CallOffNumber = CallOffNumber,
				CumulativeReceivedQuantity = CumulativeReceivedQuantity,
				CumulativeScheduledQuantity = CumulativeScheduledQuantity,
				CustomersItemMaterialNumber = CustomersItemMaterialNumber,
				HeaderId = HeaderId,
				Id = Id,
				LastReceivedQuantity = LastReceivedQuantity,
				MaterialAuthorizationDate = MaterialAuthorizationDate,
				MaterialAuthorizationQuantity = MaterialAuthorizationQuantity,
				PlanningHorizionEnd = PlanningHorizionEnd,
				PlanningHorizionStart = PlanningHorizionStart,
				PositionNumber = PositionNumber,
				PreviousCallOffDate = PreviousCallOffDate,
				PreviousCallOffNumber = PreviousCallOffNumber,
				ProductionAuthorizationDateTime = ProductionAuthorizationDateTime,
				ProductionAuthorizationQuantity = ProductionAuthorizationQuantity,
				SuppliersItemMaterialNumber = SuppliersItemMaterialNumber,
				DrawingRevisionNumber = DrawingRevisionNumber,
				DocumentNumber = DocumentNumber,
				BuyersInternalProductGroupCode = BuyersInternalProductGroupCode,
				LastASNDate = LastASNDate,
				LastASNDeliveryDate = LastASNDeliveryDate,
				LastASNNumber = LastASNNumber
			};
		}
	}
}
