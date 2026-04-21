using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> ToggleDone(int data,
			Core.Identity.Models.UserModel user)
		{
			if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
			{
				throw new Core.Exceptions.UnauthorizedException();
			}

			lock(Locks.OrdersLock)
			{
				try
				{
					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					// - 2022-07-04 - block changes for LS w/ Rechnung
					if(/*orderDb.Erledigt == true &&*/ orderDb.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY)
					{
						var invoiceEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetInvoiceByLieferschein(orderDb.Nr);
						if(invoiceEntities != null && invoiceEntities.Count > 0)
						{
							return Core.Models.ResponseModel<object>.FailureResponse(new List<string> { "ACHTUNG: Lieferschein ist erledigt.", "Buchung rückgängig nicht möglich!", "Rechnung stornieren notwendig?" });
						}
					}

					orderDb.Erledigt = orderDb.Erledigt.HasValue ? !orderDb.Erledigt.Value : true;

					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderDb);
					//logging
					var _log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.MODIFICATIONOBJECT, "CTS", user)
						.LogCTS("Erledigt", (!orderDb.Erledigt.Value).ToString(), orderDb.Erledigt.Value.ToString(), 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

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
