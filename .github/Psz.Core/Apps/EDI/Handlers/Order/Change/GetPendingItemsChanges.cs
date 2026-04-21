using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Change
		{
			public static Core.Models.ResponseModel<Models.Order.Change.OrderItemsChangesModel> GetPendingItemsChanges(int orderId,
				Core.Identity.Models.UserModel user)
			{
				try
				{
					if(user == null
						|| !user.Access.CustomerService.ModuleActivated)
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderId);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<Models.Order.Change.OrderItemsChangesModel>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					var orderElementsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb.Nr, false);

					var ordersChangesDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.GetByOrderId(orderDb.Nr);
					var ordersChangesIds = ordersChangesDb.Select(e => e.Id).ToList();
					var itemChangePendingStatus = (int)Enums.OrderEnums.OrderChangeItemStatus.Pending;
					var itemChangeTypeNew = (int)Enums.OrderEnums.OrderChangeItemTypes.New;
					var itemChangeTypeChanged = (int)Enums.OrderEnums.OrderChangeItemTypes.Changed;
					var itemChangeTypeCanceled = (int)Enums.OrderEnums.OrderChangeItemTypes.Canceled;
					var pendingOrdersChangesItemsDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.GetByOrderChangeIds(ordersChangesIds)
						.FindAll(e => e.Status == itemChangePendingStatus);

					#region > filter Update Changes
					var newAndCanceledChangesItemsDb = pendingOrdersChangesItemsDb.FindAll(e => e.Type == itemChangeTypeNew
						|| e.Type == itemChangeTypeCanceled);

					var latestUniqueChangedItemsChangesDb = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();

					foreach(var changedItemChangeDb in pendingOrdersChangesItemsDb
						.Where(e => e.Type == itemChangeTypeChanged)/*
                        .OrderByDescending(e => e.CreationTime)*/)
					{
						if(!latestUniqueChangedItemsChangesDb.Exists(e => e.ItemNumber == changedItemChangeDb.ItemNumber
							|| e.CustomerItemNumber == changedItemChangeDb.CustomerItemNumber))
						{
							latestUniqueChangedItemsChangesDb.Add(changedItemChangeDb);
						}
					}

					pendingOrdersChangesItemsDb = new List<Infrastructure.Data.Entities.Tables.PRS.OrderChangeItemEntity>();
					pendingOrdersChangesItemsDb.AddRange(newAndCanceledChangesItemsDb);
					pendingOrdersChangesItemsDb.AddRange(latestUniqueChangedItemsChangesDb);
					#endregion

					var calculateElementsRequestData = new Models.Order.Element.NotCalculatedOrderElementsModel()
					{
						OrderId = orderDb.Nr,
						Elements = new List<Models.Order.Element.NotCalculatedElementModel>()
					};

					foreach(var pendingItemChangeDb in pendingOrdersChangesItemsDb)
					{
						var orderChangeDb = ordersChangesDb.Find(e => e.Id == pendingItemChangeDb.OrderChangeId);

						calculateElementsRequestData.Elements.Add(new Models.Order.Element.NotCalculatedElementModel()
						{
							CurrentItemPriceCalculationNet = pendingItemChangeDb.CurrentItemPriceCalculationNet,
							CustomerItemNumber = pendingItemChangeDb.CustomerItemNumber,
							DesiredDate = pendingItemChangeDb.DesiredDate,
							FreeText = pendingItemChangeDb.Notes,
							ItemDescription = pendingItemChangeDb.ItemDescription,
							ItemNumber = pendingItemChangeDb.ItemNumber,
							MeasureUnitQualifier = pendingItemChangeDb.MeasureUnitQualifier,
							OrderedQuantity = pendingItemChangeDb.OrderedQuantity,
							PositionNumber = pendingItemChangeDb.PositionNumber,
							UnitPriceBasis = pendingItemChangeDb.UnitPriceBasis,
							UnloadingPoint = orderChangeDb?.ConsigneeUnloadingPoint,

							Id = pendingItemChangeDb.Id,
							ChangeType = pendingItemChangeDb.Type
						});
					}
					var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderDb.Kunden_Nr ?? -1);
					var calculateElementsResponse = Element.CalculateElementsData(calculateElementsRequestData, adressDb?.Kundennummer ?? -1);

					if(!calculateElementsResponse.Success)
					{
						return new Core.Models.ResponseModel<Models.Order.Change.OrderItemsChangesModel>()
						{
							Errors = calculateElementsResponse.Errors
						};
					}

					var response = new Core.Models.ResponseModel<Models.Order.Change.OrderItemsChangesModel>()
					{
						Success = true,
						Body = new Models.Order.Change.OrderItemsChangesModel()
					};

					foreach(var calculatedElement in calculateElementsResponse.Body)
					{
						switch((Enums.OrderEnums.OrderChangeItemTypes)calculatedElement.ChangeType)
						{
							case Enums.OrderEnums.OrderChangeItemTypes.New:
								response.Body.NewElements.Add(calculatedElement);
								break;

							case Enums.OrderEnums.OrderChangeItemTypes.Canceled:
								response.Body.CanceledElements.Add(calculatedElement);
								break;

							case Enums.OrderEnums.OrderChangeItemTypes.Changed:
								{
									var orderElementDb = orderElementsDb.Find(e => e.Position == calculatedElement.Position);
									if(orderElementDb == null)
									{
										Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "orderElementDb == null");
										break;
									}

									var orderElement = Handlers.Order.GetElement(orderElementDb.Nr);
									if(orderElement == null)
									{
										Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "orderElement == null");
										break;
									}

									var changeData = new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel();
									changeData.ChangedItem = calculatedElement;
									changeData.OriginalItem = orderElement;

									changeData.Changes = new List<Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel>();

									if(changeData.ChangedItem.CopperBase != changeData.OriginalItem.CopperBase)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "CopperBase",
											Value = changeData.ChangedItem.CopperBase
										});
									}
									if(changeData.ChangedItem.CopperSurcharge != changeData.OriginalItem.CopperSurcharge)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "CopperSurcharge",
											Value = changeData.ChangedItem.CopperSurcharge
										});
									}
									if(changeData.ChangedItem.CopperWeight != changeData.OriginalItem.CopperWeight)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "CopperWeight",
											Value = changeData.ChangedItem.CopperWeight
										});
									}
									if(changeData.ChangedItem.DelFixed != changeData.OriginalItem.DelFixed)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "DelFixed",
											Value = changeData.ChangedItem.DelFixed
										});
									}
									if(changeData.ChangedItem.DeliveredQuantity != changeData.OriginalItem.DeliveredQuantity)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "DeliveredQuantity",
											Value = changeData.ChangedItem.DeliveredQuantity
										});
									}
									if(changeData.ChangedItem.DeliveryDate != changeData.OriginalItem.DeliveryDate)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "DeliveryDate",
											Value = changeData.ChangedItem.DeliveryDate
										});
									}
									if(changeData.ChangedItem.DelNote != changeData.OriginalItem.DelNote)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "DelNote",
											Value = changeData.ChangedItem.DelNote
										});
									}
									if(changeData.ChangedItem.Designation1 != changeData.OriginalItem.Designation1)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "Designation1",
											Value = changeData.ChangedItem.Designation1
										});
									}
									if(changeData.ChangedItem.Designation2 != changeData.OriginalItem.Designation2)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "Designation2",
											Value = changeData.ChangedItem.Designation2
										});
									}
									if(changeData.ChangedItem.Designation3 != changeData.OriginalItem.Designation3)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "Designation3",
											Value = changeData.ChangedItem.Designation3
										});
									}
									if(changeData.ChangedItem.DesiredDate != changeData.OriginalItem.DesiredDate)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "DesiredDate",
											Value = changeData.ChangedItem.DesiredDate
										});
									}
									if(changeData.ChangedItem.Discount != changeData.OriginalItem.Discount)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "Discount",
											Value = changeData.ChangedItem.Discount
										});
									}
									if(changeData.ChangedItem.Done != changeData.OriginalItem.Done)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "Done",
											Value = changeData.ChangedItem.Done
										});
									}
									if(changeData.ChangedItem.DrawingIndex != changeData.OriginalItem.DrawingIndex)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "DrawingIndex",
											Value = changeData.ChangedItem.DrawingIndex
										});
									}
									if(changeData.ChangedItem.FixedTotalPrice != changeData.OriginalItem.FixedTotalPrice)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "FixedTotalPrice",
											Value = changeData.ChangedItem.FixedTotalPrice
										});
									}
									if(changeData.ChangedItem.FixedUnitPrice != changeData.OriginalItem.FixedUnitPrice)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "FixedUnitPrice",
											Value = changeData.ChangedItem.FixedUnitPrice
										});
									}
									if(changeData.ChangedItem.FreeText != changeData.OriginalItem.FreeText)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "FreeText",
											Value = changeData.ChangedItem.FreeText
										});
									}
									if(changeData.ChangedItem.MeasureUnitQualifier != changeData.OriginalItem.MeasureUnitQualifier)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "MeasureUnitQualifier",
											Value = changeData.ChangedItem.MeasureUnitQualifier
										});
									}
									if(changeData.ChangedItem.Note1 != changeData.OriginalItem.Note1)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "Note1",
											Value = changeData.ChangedItem.Note1
										});
									}
									if(changeData.ChangedItem.Note2 != changeData.OriginalItem.Note2)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "Note2",
											Value = changeData.ChangedItem.Note2
										});
									}
									if(changeData.ChangedItem.OpenQuantity_CopperSurcharge != changeData.OriginalItem.OpenQuantity_CopperSurcharge)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "OpenQuantity_CopperSurcharge",
											Value = changeData.ChangedItem.OpenQuantity_CopperSurcharge
										});
									}
									if(changeData.ChangedItem.OpenQuantity_CopperWeight != changeData.OriginalItem.OpenQuantity_CopperWeight)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "OpenQuantity_CopperWeight",
											Value = changeData.ChangedItem.OpenQuantity_CopperWeight
										});
									}
									if(changeData.ChangedItem.OpenQuantity_Quantity != changeData.OriginalItem.OpenQuantity_Quantity)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "OpenQuantity_Quantity",
											Value = changeData.ChangedItem.OpenQuantity_Quantity
										});
									}
									if(changeData.ChangedItem.OpenQuantity_TotalPrice != changeData.OriginalItem.OpenQuantity_TotalPrice)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "OpenQuantity_TotalPrice",
											Value = changeData.ChangedItem.OpenQuantity_TotalPrice
										});
									}
									if(changeData.ChangedItem.OpenQuantity_UnitPrice != changeData.OriginalItem.OpenQuantity_UnitPrice)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "OpenQuantity_UnitPrice",
											Value = changeData.ChangedItem.OpenQuantity_UnitPrice
										});
									}
									if(changeData.ChangedItem.OriginalOrderAmount != changeData.OriginalItem.OriginalOrderAmount)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "OriginalOrderAmount",
											Value = changeData.ChangedItem.OriginalOrderAmount
										});
									}
									if(changeData.ChangedItem.OriginalOrderQuantity != changeData.OriginalItem.OriginalOrderQuantity)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "OriginalOrderQuantity",
											Value = changeData.ChangedItem.OriginalOrderQuantity
										});
									}
									if(changeData.ChangedItem.ProductionNumber != changeData.OriginalItem.ProductionNumber)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "ProductionNumber",
											Value = changeData.ChangedItem.ProductionNumber
										});
									}
									if(changeData.ChangedItem.RP != changeData.OriginalItem.RP)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "RP",
											Value = changeData.ChangedItem.RP
										});
									}
									if(changeData.ChangedItem.StorageLocationId != changeData.OriginalItem.StorageLocationId)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "StorageLocationId",
											Value = changeData.ChangedItem.StorageLocationId
										});
									}
									if(changeData.ChangedItem.StorageLocationName != changeData.OriginalItem.StorageLocationName)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "StorageLocationName",
											Value = changeData.ChangedItem.StorageLocationName
										});
									}
									if(changeData.ChangedItem.TotalPrice != changeData.OriginalItem.TotalPrice)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "TotalPrice",
											Value = changeData.ChangedItem.TotalPrice
										});
									}
									if(changeData.ChangedItem.UnitPrice != changeData.OriginalItem.UnitPrice)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "UnitPrice",
											Value = changeData.ChangedItem.UnitPrice
										});
									}
									if(changeData.ChangedItem.UnitPriceBasis != changeData.OriginalItem.UnitPriceBasis)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "UnitPriceBasis",
											Value = changeData.ChangedItem.UnitPriceBasis
										});
									}
									if(changeData.ChangedItem.UnloadingPoint != changeData.OriginalItem.UnloadingPoint)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "UnloadingPoint",
											Value = changeData.ChangedItem.UnloadingPoint
										});
									}
									if(changeData.ChangedItem.VAT != changeData.OriginalItem.VAT)
									{
										changeData.Changes.Add(new Models.Order.Change.OrderItemsChangesModel.ChangedElementModel.ChangeModel()
										{
											Key = "VAT",
											Value = changeData.ChangedItem.VAT
										});
									}
									response.Body.ChangedElements.Add(changeData);
									break;
								}
						}
					}

					return response;
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
