using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Change
		{
			public static Core.Models.ResponseModel<int> AcceptNewItem(int itemChangeId,
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
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = new List<string>() { "Item Change already used or overwritten" }
							};
						}

						if((Enums.OrderEnums.OrderChangeItemTypes)itemChangeDb.Type
							!= Enums.OrderEnums.OrderChangeItemTypes.New)
						{
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = new List<string>() { "Wrong Item Change Type" }
							};
						}

						var orderChangeDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.Get(itemChangeDb.OrderChangeId);
						if(orderChangeDb == null)
						{
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = new List<string>() { "Order Change not found" }
							};
						}

						var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(itemChangeDb.OrderId);
						if(orderDb == null)
						{
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = new List<string>() { "Order not found" }
							};
						}

						if(orderDb.Neu_Order == false)
						{
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = new List<string>() { "Order is validated" }
							};
						}

						var orderItemsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb.Nr);
						var orderItemDb = orderItemsDb.Find(e => e.Position == itemChangeDb.PositionNumber);
						if(orderItemDb != null)
						{
							return new Core.Models.ResponseModel<int>(-1)
							{
								Errors = new List<string>() { "An other Order Item with the same Position number exists" }
							};
						}

						#region > Insert New Order Item
						var _id = 0; // for filtering later in orderElementExtension
						var calculateRequestData = new Models.Order.Element.NotCalculatedOrderElementsModel()
						{
							OrderId = orderDb.Nr,
							Elements = new List<Models.Order.Element.NotCalculatedElementModel>()
							{
								new Models.Order.Element.NotCalculatedElementModel()
								{
									ChangeType = itemChangeDb.Type,
									CurrentItemPriceCalculationNet = itemChangeDb.CurrentItemPriceCalculationNet,
									CustomerItemNumber = itemChangeDb.CustomerItemNumber,
									DesiredDate = itemChangeDb.DesiredDate,
									FreeText = itemChangeDb.Notes,
									Id = _id,
									ItemDescription = itemChangeDb.ItemDescription,
									ItemNumber = itemChangeDb.ItemNumber,
									MeasureUnitQualifier = itemChangeDb.MeasureUnitQualifier,
									OrderedQuantity = itemChangeDb.OrderedQuantity,
									PositionNumber = itemChangeDb.PositionNumber,
									UnitPriceBasis = itemChangeDb.UnitPriceBasis,
									UnloadingPoint = orderChangeDb.ConsigneeUnloadingPoint
								}
							}
						};
						_id += 1;

						var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderDb.Kunden_Nr ?? -1);
						var createElementsResponse = Element.CreateOrderElementsInternal(calculateRequestData, adressDb.Kundennummer ?? -1, user);

						if(!createElementsResponse.Success)
						{
							return new Core.Models.ResponseModel<int>()
							{
								Errors = createElementsResponse.Errors
							};
						}

						var insertedItemId = createElementsResponse.Body.FirstOrDefault();
						#endregion

						#region > Update Item Change
						itemChangeDb.Status = (int)Enums.OrderEnums.OrderChangeItemStatus.Accepted;
						itemChangeDb.ActionTime = DateTime.Now;
						itemChangeDb.ActionUserId = user.Id;
						itemChangeDb.ActionUsername = user.Username;

						Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.Update(itemChangeDb);
						#endregion

						#region update Order
						if(orderDb.Gebucht == true)
						{
							Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateGebucht(orderDb.Nr, false);
						}
						#endregion Order

						return Core.Models.ResponseModel<int>.SuccessResponse(insertedItemId);
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
