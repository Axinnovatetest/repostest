using System;

namespace Psz.Core.FinanceControl.Models.Budget.Order
{
	public class PlaceHistoryModel
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public string OrderPlacedEmailMessage { get; set; }
		public string OrderPlacedEmailTitle { get; set; }
		public int? OrderPlacedReportFileId { get; set; }
		public string OrderPlacedSendingEmail { get; set; }
		public string OrderPlacedSupplierEmail { get; set; }
		public DateTime? OrderPlacedTime { get; set; }
		public string OrderPlacedUserEmail { get; set; }
		public int? OrderPlacedUserId { get; set; }
		public string OrderPlacedUserName { get; set; }
		public string SupplierEmail { get; set; }
		public string SupplierNummer { get; set; }
		public string OrderPlacementCCEmail { get; set; }

		public PlaceHistoryModel()
		{

		}
		public PlaceHistoryModel(Infrastructure.Data.Entities.Tables.FNC.OrderPlacementEntity orderPlacementEntity)
		{
			if(orderPlacementEntity == null)
				return;

			Id = orderPlacementEntity.Id;
			OrderId = orderPlacementEntity.OrderId;
			OrderPlacedEmailMessage = orderPlacementEntity.OrderPlacedEmailMessage;
			OrderPlacedEmailTitle = orderPlacementEntity.OrderPlacedEmailTitle;
			OrderPlacedReportFileId = orderPlacementEntity.OrderPlacedReportFileId;
			OrderPlacedSendingEmail = orderPlacementEntity.OrderPlacedSendingEmail;
			OrderPlacedSupplierEmail = orderPlacementEntity.OrderPlacedSupplierEmail;
			OrderPlacedTime = orderPlacementEntity.OrderPlacedTime;
			OrderPlacedUserEmail = orderPlacementEntity.OrderPlacedUserEmail;
			OrderPlacedUserId = orderPlacementEntity.OrderPlacedUserId;
			OrderPlacedUserName = orderPlacementEntity.OrderPlacedUserName;
			SupplierEmail = orderPlacementEntity.SupplierEmail;
			SupplierNummer = orderPlacementEntity.SupplierNummer;
			OrderPlacementCCEmail = orderPlacementEntity.OrderPlacementCCEmail;
		}
	}
}
