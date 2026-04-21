using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> DeleteArchive(int id,
			Core.Identity.Models.UserModel user)
		{
			if(user == null || !user.Access.Purchase.ModuleActivated)
			{
				throw new Core.Exceptions.UnauthorizedException();
			}

			lock(Locks.OrdersLock)
			{
				try
				{
					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(id);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					if(Helpers.ItemHelper.CanDeleteOrder(id, orderDb.Angebot_Nr ?? -1, out var msg))
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.DeleteByOrder(id);
						Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Delete(id);
					}
					else
					{
						if(Helpers.ItemHelper.CanArchiveOrderByAngebote(orderDb.Angebot_Nr))
						{
							//orderDb.Nr_sto = 1;
							//Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderDb);
						}
					}

					return Core.Models.ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
