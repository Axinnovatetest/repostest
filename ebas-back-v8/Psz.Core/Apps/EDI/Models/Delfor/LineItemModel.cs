using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class LineItemModel
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

		public LineItemModel(Models.Delfor.DocumentDetailsForecastDataForecastLineItem documentDetails, long headerId, string documentNumber)
		{
			if(documentDetails == null)
				return;

			HeaderId = headerId;
			DocumentNumber = documentNumber;
			Id = -1;
			PositionNumber = documentDetails.PositionNumber;

			CallOffDateTime = documentDetails.ReferencedDocumentsDatesValues?.CallOffDate?.DateTime;
			CallOffNumber = documentDetails.ReferencedDocumentsDatesValues?.CallOffNumber;
			PreviousCallOffDate = documentDetails.ReferencedDocumentsDatesValues?.PreviousCallOffDate?.DateTime;
			PreviousCallOffNumber = documentDetails.ReferencedDocumentsDatesValues.PreviousCallOfNumber;

			CumulativeReceivedQuantity = documentDetails.ReferencedDocumentsDatesValues?.CumulativeReceivedQuantity;
			CumulativeScheduledQuantity = documentDetails.ReferencedDocumentsDatesValues?.CumulativeScheduledQuantity;
			LastReceivedQuantity = documentDetails.ReferencedDocumentsDatesValues?.LastReceivedQuantity;
			MaterialAuthorizationDate = documentDetails.ReferencedDocumentsDatesValues?.MaterialAuthorization?.AuthorizationEndDate?.DateTime;
			MaterialAuthorizationQuantity = documentDetails.ReferencedDocumentsDatesValues?.MaterialAuthorization?.AuthorizationQuantity;
			PlanningHorizionEnd = documentDetails.ReferencedDocumentsDatesValues?.PlanningHorizionEndDate?.DateTime;
			PlanningHorizionStart = documentDetails.ReferencedDocumentsDatesValues?.PlanningHorizionStartDate.DateTime;
			ProductionAuthorizationDateTime = documentDetails.ReferencedDocumentsDatesValues?.ProductionAuthorization?.AuthorizationEndDate?.DateTime;
			ProductionAuthorizationQuantity = documentDetails.ReferencedDocumentsDatesValues.ProductionAuthorization?.AuthorizationQuantity;

			// -
			CustomersItemMaterialNumber = documentDetails.AdditionalInformation?.CustomersItemMaterialNumber?.Trim();
			SuppliersItemMaterialNumber = documentDetails.AdditionalInformation?.SuppliersItemMaterialNumber?.Trim();
			DrawingRevisionNumber = documentDetails.AdditionalInformation?.DrawingRevisionNumber?.Trim();
		}
		public LineItemModel(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity lineItemEntity)
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
				DocumentNumber = DocumentNumber
			};
		}
	}
}
