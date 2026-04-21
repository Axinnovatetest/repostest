using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Change
		{
			public static Core.Models.ResponseModel<object> AcceptItemCancel(int itemChangeId,
				Core.Identity.Models.UserModel user)
			{
				lock(Locks.OrdersLock)
				{
					try
					{
						if(user == null
							|| !user.Access.Purchase.ModuleActivated)
						{
							throw new Core.Exceptions.UnauthorizedException();
						}

						var itemChangeDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.Get(itemChangeId);
						if(itemChangeDb == null)
						{
							throw new Core.Exceptions.NotFoundException();
						}

						if((Enums.OrderEnums.OrderChangeItemStatus)itemChangeDb.Status
							!= Enums.OrderEnums.OrderChangeItemStatus.Pending)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Item Change already used" }
							};
						}

						if((Enums.OrderEnums.OrderChangeItemTypes)itemChangeDb.Type
							!= Enums.OrderEnums.OrderChangeItemTypes.Canceled)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Wrong Item Change Type" }
							};
						}

						var orderChangeDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Get(itemChangeDb.OrderChangeId);
						if(orderChangeDb == null)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Order Change not found" }
							};
						}

						var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(itemChangeDb.OrderId);
						if(orderDb == null)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Order not found" }
							};
						}

						// - 2022-11-24 - do not reset Order to Neu if imported from P3000
						if(!string.IsNullOrEmpty(orderDb.EDI_Dateiname_CSV) && orderDb.EDI_Dateiname_CSV.Trim().ToLower().Substring(orderDb.EDI_Dateiname_CSV.Trim().Length - 4, 4) == ".csv")
						{
							//return new Core.Models.ResponseModel<int>(customerDb.Nr)
							//{
							//	Errors = new List<string>() { "Order file is CSV, it might be imported from P3000" }
							//};
						}
						else
						{
							if(orderDb.Neu_Order == false)
							{
								return new Core.Models.ResponseModel<object>()
								{
									Errors = new List<string>() { "Order is validated" }
								};
							}
						}
						//var itemDb = Helpers.ItemHelper.GetItemByOrderIds(itemChangeDb.ItemNumber, 
						//    itemChangeDb.CustomerItemNumber);
						//if (itemDb == null)
						//{
						//    return new Core.Models.ResponseModel<object>()
						//    {
						//        Errors = new List<string>() { "Item not found" }
						//    };
						//}

						var orderItemsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb.Nr);
						var orderItemDb = orderItemsDb.Find(e => e.Position == itemChangeDb.PositionNumber);
						if(orderItemDb == null)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Order Item not found" }
							};
						}

						// - 2022-11-25 - prevent deleting Pos w/ FAs not closed or canceled
						if(orderItemDb.Fertigungsnummer.HasValue && orderItemDb.Fertigungsnummer.Value > 0)
						{
							var fa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(orderItemDb.Fertigungsnummer ?? -1);
							if(fa != null && (fa.Kennzeichen?.ToLower()?.Trim() == "offen"))
							{
								return Core.Models.ResponseModel<object>.FailureResponse($"Order Position {orderItemDb.Position}: please cancel open FA [{fa.Fertigungsnummer}] first.");
							}
						}


						#region > Delete Order Item
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.SoftDelete(orderItemDb.Nr);
						OrderElementExtension.SetStatus(orderItemDb.Nr);

						// - Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Delete(orderItemDb.Nr);
						#endregion

						#region > Update Item Change
						itemChangeDb.Status = (int)Enums.OrderEnums.OrderChangeItemStatus.Accepted;
						itemChangeDb.ActionTime = DateTime.Now;
						itemChangeDb.ActionUserId = user.Id;
						itemChangeDb.ActionUsername = user.Username;

						Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.Update(itemChangeDb);
						#endregion

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
}
