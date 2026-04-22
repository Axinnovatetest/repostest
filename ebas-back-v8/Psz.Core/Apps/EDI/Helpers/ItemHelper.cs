using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Helpers
{
	public class ItemHelper
	{
		public static Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity GetItemByOrderIds(string itemNumber,
			string customerItemNumber)
		{
			var itemDb = !string.IsNullOrEmpty(itemNumber)
				? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(itemNumber)
				 : null;

			if(itemDb == null || itemDb.Freigabestatus == "O")
			{
				itemDb = !string.IsNullOrEmpty(customerItemNumber)
					? Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByBezeichnung1(new List<string>() { customerItemNumber?.TrimStart('0') }).FirstOrDefault()
					: null;
			}

			if(itemDb == null || itemDb.Freigabestatus == "O")
			{
				return null;
			}

			return itemDb;
		}
	}
}
