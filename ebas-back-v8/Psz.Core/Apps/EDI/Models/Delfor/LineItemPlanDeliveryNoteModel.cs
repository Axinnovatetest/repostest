using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class LineItemPlanDeliveryNoteModel
	{
		public long LineItemPlanId { get; set; }
		public int DeliveryNoteId { get; set; }
		public int DeliveryNotePosId { get; set; }
		public decimal LineItemQuantity { get; set; }
		public decimal DeliveryNotePosQuantity { get; set; }
		public int? DeliveryNotePosNumber { get; set; }
		public int DeliveryNotePosArticleId { get; set; }
		public string DeliveryNotePosArticleNumber { get; set; }
		public DateTime DeliveryNoteCreationTime { get; set; }
		public int DeliveryNoteNumber { get; set; }
		public DateTime MinDate { get; set; }
		public LineItemPlanDeliveryNoteModel(int lineItemPlanId, decimal lineItemQuantity, string articleNumber, DateTime minDate,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity lineItemPlan_OrdersEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angeboteneArtikelEntity
			)
		{
			LineItemPlanId = lineItemPlanId;
			DeliveryNoteId = lineItemPlan_OrdersEntity?.Nr ?? -1;
			LineItemQuantity = lineItemQuantity;
			DeliveryNotePosQuantity = angeboteneArtikelEntity?.Anzahl ?? 0;
			DeliveryNotePosId = angeboteneArtikelEntity?.Nr ?? 0;
			DeliveryNotePosNumber = angeboteneArtikelEntity?.Position;
			DeliveryNotePosArticleId = angeboteneArtikelEntity?.ArtikelNr ?? 0;
			DeliveryNotePosArticleNumber = articleNumber;
			DeliveryNoteCreationTime = lineItemPlan_OrdersEntity.Datum ?? DateTime.MinValue;
			DeliveryNoteNumber = lineItemPlan_OrdersEntity?.Angebot_Nr ?? -1;
			MinDate = minDate;
		}
	}
}
