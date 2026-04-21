using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Models.Budget.Order.Statistics
{
	// 08-02-2024 Personalize -->  15h 18

	public class CheapestOrderModel
	{
		public int OrderId { get; set; }

		public string OrderNum { get; set; } = string.Empty;

		public decimal OrderCost { get; set; }

		public string CurrencyName { get; set; }
		public CheapestOrderModel(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity entity, decimal orderAmount)
		{
			if(entity == null)
			{ return; }

			OrderId = entity.OrderId;
			OrderNum = entity.OrderNumber;
			CurrencyName = entity.CurrencyName;
			OrderCost = orderAmount;
		}
	}
}
