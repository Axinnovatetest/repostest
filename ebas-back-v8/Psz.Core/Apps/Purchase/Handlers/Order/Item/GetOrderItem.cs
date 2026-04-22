using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Models.Order.Element.OrderItemModel GetOrderItem(int elementId)
		{
			try
			{
				return GetOrderItems(new List<int>() { elementId }).FirstOrDefault();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
