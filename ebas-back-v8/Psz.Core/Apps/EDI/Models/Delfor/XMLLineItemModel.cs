using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class XMLLineItemModel
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
		//souilmi 03/11/2022
		public int PSZArtikelNr { get; set; }
		public string ArticleDescription { get; set; }
		public string DeliveryAdress { get; set; }
		public string ConsigneeAddress { get; set; }
		//
		public int? HeaderVersion { get; set; }
		public int? HeaderPreviousVersion { get; set; }
		// - 2023-03-23
		//public string UnloadingPoint { get; set; }
		//public string StorageLocation { get; set; }


		public XMLLineItemModel(Models.Delfor.DocumentDetailsForecastDataForecastLineItem documentDetails, long headerId, int headerVersion, int headerPreviousVersion, string documentNumber)
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
			PreviousCallOffNumber = documentDetails.ReferencedDocumentsDatesValues?.PreviousCallOfNumber;

			CumulativeReceivedQuantity = documentDetails.ReferencedDocumentsDatesValues?.CumulativeReceivedQuantity;
			CumulativeScheduledQuantity = documentDetails.ReferencedDocumentsDatesValues?.CumulativeScheduledQuantity;
			LastReceivedQuantity = documentDetails.ReferencedDocumentsDatesValues?.LastReceivedQuantity;
			MaterialAuthorizationDate = documentDetails.ReferencedDocumentsDatesValues?.MaterialAuthorization?.AuthorizationEndDate?.DateTime;
			MaterialAuthorizationQuantity = documentDetails.ReferencedDocumentsDatesValues?.MaterialAuthorization?.AuthorizationQuantity;
			PlanningHorizionEnd = documentDetails.ReferencedDocumentsDatesValues?.PlanningHorizionEndDate?.DateTime;
			PlanningHorizionStart = documentDetails.ReferencedDocumentsDatesValues?.PlanningHorizionStartDate?.DateTime;
			ProductionAuthorizationDateTime = documentDetails.ReferencedDocumentsDatesValues?.ProductionAuthorization?.AuthorizationEndDate?.DateTime;
			ProductionAuthorizationQuantity = documentDetails.ReferencedDocumentsDatesValues?.ProductionAuthorization?.AuthorizationQuantity;

			// -
			CustomersItemMaterialNumber = documentDetails.AdditionalInformation?.CustomersItemMaterialNumber?.Trim();
			SuppliersItemMaterialNumber = documentDetails.AdditionalInformation?.SuppliersItemMaterialNumber?.Trim();
			DrawingRevisionNumber = documentDetails.AdditionalInformation?.DrawingRevisionNumber?.Trim();
			// -
			BuyersInternalProductGroupCode = documentDetails.AdditionalInformation?.BuyersInternalProductGroupCode;
			LastASNDate = documentDetails.ReferencedDocumentsDatesValues?.LastASNDate?.DateTime;
			LastASNDeliveryDate = documentDetails.ReferencedDocumentsDatesValues?.LastASNDeliveryDate?.DateTime;
			LastASNNumber = documentDetails.ReferencedDocumentsDatesValues?.LastASNNumber;
			HeaderVersion = headerVersion;
			HeaderPreviousVersion = headerPreviousVersion;
			// - 2023-03-23
			//UnloadingPoint = documentDetails.BusinessEntities?.Consignee?.UnloadingPoint;
			//StorageLocation = documentDetails.BusinessEntities?.Consignee?.StorageLocation;
		}
		public XMLLineItemModel(Infrastructure.Data.Entities.Tables.CTS.LineItemEntity lineItemEntity)
		{
			if(lineItemEntity == null)
				return;

			var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(lineItemEntity.SuppliersItemMaterialNumber);
			var headerEntity = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(lineItemEntity.HeaderId);
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
			PSZArtikelNr = article?.ArtikelNr ?? -1;
			ArticleDescription = article?.Bezeichnung2;
			DeliveryAdress = $"{headerEntity?.ConsigneePostCode?.Trim()} {headerEntity?.ConsigneeCity?.Trim()}".Trim();
			HeaderVersion = lineItemEntity.HeaderVersion;
			HeaderPreviousVersion = lineItemEntity.HeaderPreviousVersion;
			//UnloadingPoint = lineItemEntity.UnloadingPoint;
			//StorageLocation = lineItemEntity.StorageLocation;
			PSZArtikelNr = lineItemEntity.ArticleId ?? -1;
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
				LastASNNumber = LastASNNumber,
				HeaderPreviousVersion = HeaderPreviousVersion,
				HeaderVersion = HeaderVersion,
				//UnloadingPoint = UnloadingPoint,
				//StorageLocation = StorageLocation,
				ArticleId = PSZArtikelNr
			};
		}
	}
}
