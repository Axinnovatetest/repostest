using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public partial class Element
		{
			public static Core.Models.ResponseModel<int> QuickCreateItem(Models.Order.Element.QuickCreateItemModel data,
				Core.Identity.Models.UserModel user)
			{
				lock(Locks.OrdersLock)
				{
					try
					{
						if(user == null || (!user.Access.CustomerService.ModuleActivated && !user.Access.Purchase.ModuleActivated))
						{
							throw new Core.Exceptions.UnauthorizedException();
						}

						return QuickCreateItemInternal(data, user);
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(e);
						throw;
					}
				}
			}

			internal static Core.Models.ResponseModel<int> QuickCreateItemInternal(Models.Order.Element.QuickCreateItemModel data,
				Core.Identity.Models.UserModel user)
			{
				lock(Locks.OrderItemsLock)
				{

					var orderData = Handlers.Order.Get(data.OrderId, false);
					if(orderData == null)
					{
						return new Core.Models.ResponseModel<int>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(data.ItemNumber);
					if(itemDb == null)
					{
						return new Core.Models.ResponseModel<int>()
						{
							Errors = new List<string>() { "Item not found" }
						};
					}

					int newPositionNumber = getNextPositionNumber(orderData);

					var createItemsData = new Models.Order.Element.NotCalculatedOrderElementsModel()
					{
						OrderId = orderData.Id,
						Items = new List<Models.Order.Element.CreateItemModel>()
					{
						new Models.Order.Element.CreateItemModel()
						{
							Id = -1,
							ItemType = data.ItemTypeId,
							PositionNumber = newPositionNumber,
							ItemNumber = data.ItemNumber,
							OrderedQuantity = data.Quantity ?? 1,
							DesiredDate = orderData.DesiredDate ?? DateTime.Now.AddDays(+30),
							CustomerItemNumber = (orderData.CustomerNumber ?? 0).ToString(),
							UnloadingPoint = string.Empty,
							FreeText = string.Empty,
							CurrentItemPriceCalculationNet = 0m,
							ItemDescription = itemDb.Bezeichnung2,
							LineItemAmount = 0m,
							UnitPriceBasis = 1,
							Consignee = null,
							Index_Kunde = itemDb.Index_Kunde,
							Index_Kunde_Datum = itemDb.Index_Kunde_Datum
						}
					}
					};

					var calculatedElementsResponse = CreateItems(createItemsData, user);

					if(!calculatedElementsResponse.Success)
					{
						return new Core.Models.ResponseModel<int>()
						{
							Errors = calculatedElementsResponse.Errors
						};
					}

					// -- Open Order
					var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId);
					orderEntity.Gebucht = false;
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderEntity);
					// -

					return Core.Models.ResponseModel<int>.SuccessResponse(calculatedElementsResponse.Body.First());
				}
			}

			private static int getNextPositionNumber(Models.Order.OrderModel orderData)
			{
				var newPositionNumber = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetMaxPositionNumberByOrderId(orderData.Id)
					+ 10;

				//var newPositionNumberString = newPositionNumber.ToString();

				//return int.Parse(newPositionNumberString.Substring(1, newPositionNumberString.Length - 1) + "0");
				return newPositionNumber;
			}
		}
	}
}
