using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> DeleteOrderItem(Models.Order.Element.DeleteItemModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					if(user == null || !user.Access.Purchase.ModuleActivated
						|| !user.Access.Purchase.OrderUpdate
						|| !user.Access.CustomerService.ModuleActivated
						|| !user.Access.CustomerService.EDIOrderEdit)
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					var orderElementDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
					if(orderElementDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order Element not found" }
						};
					}
					if(orderElementDb.Fertigungsnummer > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Element already in production" }
						};
					}

					if(orderElementDb.AngebotNr != orderDb.Nr)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Element is not Element of Order" }
						};
					}

					// - 2022-05-16 - check for sent delivery-notes on Position
					var deliveryNoteEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveryNotesByAB(orderDb.Nr);
					if(deliveryNoteEntities != null && deliveryNoteEntities.Count > 0)
					{
						var deliveredPositionEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyAngeboteNrs(deliveryNoteEntities.Select(x => x.Nr).ToList())
							?.Where(x => x.ArtikelNr == orderElementDb.ArtikelNr && x.Anzahl > 0)
							?.ToList();
						if(deliveredPositionEntities != null && deliveredPositionEntities.Count > 0)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { $"Element has delivered quantity in LS" }
							};
						}
					}

					// - 2022-05-16 - soft/hard delete
					if(orderDb.Neu_Order.HasValue) // - EDI => soft-delete
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.SoftDelete(orderElementDb.Nr);
						EDI.Handlers.OrderElementExtension.SetStatus(orderElementDb.Nr);
					}
					else // - Manual AB => hard-delete
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Delete(orderElementDb.Nr);
					}

					//logging
					var _log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.DELETIONPOS, "CTS - EDI", user)
						.LogCTS(null, null, $"Article: {orderElementDb.ArtikelNr}| Quantity: {orderElementDb.Anzahl}", (int)orderElementDb.Position, orderElementDb.Nr);
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
