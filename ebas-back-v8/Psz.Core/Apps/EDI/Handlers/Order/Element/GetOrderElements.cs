using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static List<Models.Order.Element.OrderElementModel> GetOrderElements(int orderId, bool ignoreDeleted = true)
		{
			try
			{
				var elementsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderId, ignoreDeleted);
				var elementsIds = elementsDb.Select(e => e.Nr).ToList();

				return GetElements(elementsIds);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
