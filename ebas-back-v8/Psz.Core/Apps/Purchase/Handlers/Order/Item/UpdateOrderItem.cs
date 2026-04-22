using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> UpdateOrderItem(Models.Order.UpdateItemModel data, Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					if(user == null
						|| !user.Access.CustomerService.ModuleActivated
						|| !user.Access.CustomerService.EDIOrderEdit
						|| !user.Access.Purchase.ModuleActivated
						|| !user.Access.Purchase.OrderUpdate)
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

					var orderItemDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
					if(orderItemDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order Element not found" }
						};
					}

					if(orderItemDb.AngebotNr != orderDb.Nr)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Element is not Element of Order" }
						};
					}

					var customerDb = orderDb.Kunden_Nr.HasValue
						? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderDb.Kunden_Nr.Value)
						: null;
					if(customerDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Customer not found" }
						};
					}

					var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(data.ItemNumber);
					if(itemDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Item not found" }
						};
					}
					if(itemDb.Freigabestatus.ToUpper() == "O")
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Item is 'Obsolete'" }
						};
					}

					Infrastructure.Data.Entities.Tables.INV.LagerorteEntity storageLocationDb = null;
					if(data.StorageLocationId > 0)
					{
						storageLocationDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(data.StorageLocationId);
						if(storageLocationDb == null)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { "Storage Location not found" }
							};
						}
					}

					var orderItemExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOderIdAndOrderItemId(orderDb.Nr, orderItemDb.Nr);

					// > Check Version
					//if (orderItemExtensionDb != null && data.Version != orderItemExtensionDb.Version)
					//{
					//    return new Core.Models.ResponseModel<object>()
					//    {
					//        Errors = new List<string>() { "You are not using the latest data version" }
					//    };
					//}

					var errors = new List<string>();

					if(data.UnitPriceBasis <= 0)
					{
						errors.Add("UnitPriceBasis " + data.UnitPriceBasis + " is invalid");
					}

					if(data.OrderedQuantity <= 0)
					{
						errors.Add("Ordered Quantity " + data.OrderedQuantity + " is invalid");
					}

					// > Linked positions
					if(orderItemDb.PositionZUEDI != null && (int)orderItemDb.PositionZUEDI > 0)
					{
						var originalPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get((int)orderItemDb.PositionZUEDI);
						if(originalPosition == null)
						{
							errors.Add("Original Position " + orderItemDb.PositionZUEDI + " is not found");
						}
						else
						{
							if(originalPosition.ArtikelNr != orderItemDb.ArtikelNr)
							{
								errors.Add("Cannot change Article for duplicated Position.");
							}
						}
					}
					var itemWsamePositionNr = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNrAndPosition(data.Id, data.OrderId, data.PositionNumber);
					if(itemWsamePositionNr != null && itemWsamePositionNr.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { $"Position [{data.PositionNumber}] already exists in Order" }
						};
					}

					if(errors.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = errors
						};
					}

					var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemDb.ArtikelNr);

					var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
						? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
						: null;

					var discount = data.Discount;
					var fixedPrice = data.IsFixedPrice;
					var unitPriceBasis = data.UnitPriceBasis;
					var cuWeight = Convert.ToDecimal(itemDb.CuGewicht ?? 0);
					var del = (itemDb.DEL ?? 0);

					var me1 = 0m;
					var me2 = 0m;
					var me3 = 0m;
					var me4 = 0m;
					var pm1 = 0m;
					var pm2 = 0m;
					var pm3 = 0m;
					var pm4 = 0m;
					var verkaufspreis = 0m;
					if(itemPricingGroupDb != null)
					{
						me1 = Convert.ToDecimal(itemPricingGroupDb.ME1 ?? 0m);
						me2 = Convert.ToDecimal(itemPricingGroupDb.ME2 ?? 0m);
						me3 = Convert.ToDecimal(itemPricingGroupDb.ME3 ?? 0m);
						me4 = Convert.ToDecimal(itemPricingGroupDb.ME4 ?? 0m);
						pm1 = Convert.ToDecimal(itemPricingGroupDb.PM1 ?? 0m);
						pm2 = Convert.ToDecimal(itemPricingGroupDb.PM2 ?? 0m);
						pm3 = Convert.ToDecimal(itemPricingGroupDb.PM3 ?? 0m);
						pm4 = Convert.ToDecimal(itemPricingGroupDb.PM4 ?? 0m);
						verkaufspreis = Convert.ToDecimal(itemPricingGroupDb.Verkaufspreis ?? 0m);
					}

					var singleCopperSurcharge = Helpers.CalculationHelper.CalculateSingleCopperSurcharge(fixedPrice,
						del,
						cuWeight);

					var totalCopperSurcharge = Helpers.CalculationHelper.CalculateTotalCopperSurcharge(fixedPrice,
						data.OrderedQuantity,
						singleCopperSurcharge);

					var vkUnitPrice = Helpers.CalculationHelper.CalculateVkUnitPrice(fixedPrice,
						verkaufspreis,
						data.OrderedQuantity,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					var unitPrice = Helpers.CalculationHelper.CalculateUnitPrice(fixedPrice,
						unitPriceBasis,
						data.OrderedQuantity,
						vkUnitPrice,
						verkaufspreis,
						singleCopperSurcharge,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					var totalPrice = Helpers.CalculationHelper.CalculateTotalPrice(unitPriceBasis,
						unitPrice,
						data.OrderedQuantity,
						discount);

					var vKTotalPrice = Helpers.CalculationHelper.CalculateVkTotalPrice(unitPriceBasis,
						vkUnitPrice,
						data.OrderedQuantity);

					var totalCuWeight = Helpers.CalculationHelper.CalculateTotalWeight(data.OrderedQuantity,
						cuWeight);

					#region > OrderItem
					orderItemDb.Typ = data.ItemTypeId;
					orderItemDb.Position = data.PositionNumber;
					orderItemDb.Wunschtermin = data.DesiredDate;
					orderItemDb.Anzahl = data.OrderedQuantity;
					orderItemDb.AktuelleAnzahl = data.OrderedQuantity;
					orderItemDb.Abladestelle = data.ConsigneeUnloadingPoint;
					orderItemDb.Bezeichnung2_Kunde = itemDb.Bezeichnung2;
					orderItemDb.Freies_Format_EDI = data.FreeText;
					orderItemDb.Bemerkungsfeld1 = data.Note1;
					orderItemDb.Bemerkungsfeld2 = data.Note2;
					orderItemDb.Bezeichnung1 = data.Designation1;
					//itemDb.Bezeichnung1;
					orderItemDb.Bezeichnung2 = data.Designation2;
					//itemDb.Bezeichnung2;
					orderItemDb.Einheit = data.MeasureUnitQualifier;
					orderItemDb.ArtikelNr = itemDb.ArtikelNr;
					orderItemDb.Kupferbasis = 150;
					orderItemDb.Preiseinheit = unitPriceBasis == 0 ? 1 : unitPriceBasis; // - 2022-05-30 - init to 1 to respect DB Constraint
					orderItemDb.DELFixiert = itemDb.DELFixiert;
					orderItemDb.DEL = itemDb.DEL;
					orderItemDb.EinzelCuGewicht = itemDb.CuGewicht;
					orderItemDb.VKFestpreis = fixedPrice;
					orderItemDb.USt = itemDb.Umsatzsteuer;
					orderItemDb.Einzelkupferzuschlag = singleCopperSurcharge;
					orderItemDb.GesamtCuGewicht = totalCuWeight;
					orderItemDb.Einzelpreis = unitPrice;
					orderItemDb.VKEinzelpreis = vkUnitPrice;
					orderItemDb.Gesamtpreis = totalPrice;
					orderItemDb.Gesamtkupferzuschlag = totalCopperSurcharge;
					orderItemDb.VKGesamtpreis = vKTotalPrice;
					orderItemDb.Lagerort_id = storageLocationDb != null
						? storageLocationDb.LagerortId
						: (int?)null;
					orderItemDb.Liefertermin = data.DeliveryDate;
					orderItemDb.RP = data.RP;
					orderItemDb.Index_Kunde = data.Index_Kunde;
					orderItemDb.Index_Kunde_Datum = data.Index_Kunde_Datum;
					orderItemDb.POSTEXT = data.Postext;
					orderItemDb.CSInterneBemerkung = data.CSInterneBemerkung;
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(orderItemDb);
					#endregion

					#region > OrderItemExtension
					if(orderItemExtensionDb == null)
					{
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity
						{
							Id = -1,

							OrderId = data.OrderId,
							OrderItemId = orderItemDb.Nr,
							Status = (int)Enums.OrderElementEnums.OrderElementStatus.Original,
							OriginalQuantity = 0m,
							OriginalGesamtpreis = 0m,
							OriginalVKGesamtpreis = -1,
							DesiredDate = null,
							CreationDate = DateTime.Now,
							CreationUserId = (user?.Id ?? -1),

							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = (user?.Id ?? -1),
							LastUpdateUsername = user.Username,
							Version = 0,
						});
					}
					else
					{
						orderItemExtensionDb.Version += 1;
						orderItemExtensionDb.LastUpdateTime = DateTime.Now;
						orderItemExtensionDb.LastUpdateUserId = (user?.Id ?? -1);
						orderItemExtensionDb.LastUpdateUsername = user.Username;
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.Update(orderItemExtensionDb);
					}
					#endregion

					// > ---EDI---EDI---EDI---EDI---EDI---
					// OrderElementExtension.UpdateStatus(orderElementDbResponse.Body.Nr);

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
