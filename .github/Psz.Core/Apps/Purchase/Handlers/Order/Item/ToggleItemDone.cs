using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> ToggleItemDone(int data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{

				var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
				try
				{
					botransaction.beginTransaction();

					#region // -- transaction-based logic -- //
					if(user == null || !user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated)
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var orderItemDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data);
					if(orderItemDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order Element not found" }
						};
					}


					#region > OrderItem
					orderItemDb.erledigt_pos = orderItemDb.erledigt_pos.HasValue ? !orderItemDb.erledigt_pos.Value : true;
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(orderItemDb, botransaction.connection, botransaction.transaction);

					// -
					var _object = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderItemDb.AngebotNr ?? -1);
					var _toLog = new Psz.Core.CustomerService.Helpers.LogHelper(_object.Nr, (int)_object.Angebot_Nr, int.TryParse(_object.Projekt_Nr, out var val) ? val : 0, _object.Typ, Psz.Core.CustomerService.Helpers.LogHelper.LogType.MODIFICATIONPOS, "CTS", user);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(
						_toLog.LogCTS("Done", $"{!orderItemDb.erledigt_pos}", $"{orderItemDb.erledigt_pos}", orderItemDb?.Position ?? -1, orderItemDb.Nr), botransaction.connection, botransaction.transaction);
					#endregion

					// > ---EDI---EDI---EDI---EDI---EDI---
					// OrderElementExtension.UpdateStatus(orderElementDbResponse.Body.Nr);


					#endregion // -- transaction-based logic -- //
					//CTS-02-Änderung EDI Haken AB
					// - 2025-09-03 - ticket #47174 - this should not apply to to EDI orders bc it sets the order to booked and they cannot set it back
					if(_object.Typ == "Auftragsbestätigung" && (_object.EDI_Dateiname_CSV ?? "").Length == 0)
					{
						var elements = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderItemDb.AngebotNr ?? -1, botransaction.connection, botransaction.transaction, false);
						var elementsIds = elements.Select(e => e.erledigt_pos).ToList();
						if(elementsIds.All(erledigt_pos => erledigt_pos == true))
						{
							Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateErledigtWithTransaction(orderItemDb.AngebotNr ?? -1, botransaction.connection, botransaction.transaction);
						}

					}
					//CTS-02-Änderung EDI Haken AB

					//TODO: handle transaction state (success or failure)
					if(botransaction.commit())
					{
						return Core.Models.ResponseModel<object>.SuccessResponse();

					}
					else
					{
						return Core.Models.ResponseModel<object>.FailureResponse("Transaction error");
					}
				} catch(Exception e)
				{
					botransaction.rollback();
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
