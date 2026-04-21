using System;

namespace Psz.Core.Apps.EDI.Models.Delfor
{
	public class LineItemPlanOrderModel
	{
		public long LineItemPlanId { get; set; }
		public int OrderId { get; set; }
		public decimal LineItemQuantity { get; set; }
		public decimal OrderQuantity { get; set; }
		public DateTime CreationTime { get; set; }
		public int CreationUserId { get; set; }
		public int OrderNumber { get; set; }
		public LineItemPlanOrderModel(Infrastructure.Data.Entities.Tables.CTS.LineItemPlanEntity lineItemPlanEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity lineItemPlan_OrdersEntity, decimal orderTotalQty)
		{
			LineItemPlanId = lineItemPlanEntity?.LineItemId ?? -1;
			OrderId = lineItemPlan_OrdersEntity?.Nr ?? -1;
			LineItemQuantity = lineItemPlanEntity?.PlanningQuantityQuantity ?? 0;
			OrderQuantity = orderTotalQty;
			CreationTime = lineItemPlan_OrdersEntity.Datum ?? DateTime.MinValue;
			OrderNumber = lineItemPlan_OrdersEntity?.Angebot_Nr ?? -1;
		}
	}
}
