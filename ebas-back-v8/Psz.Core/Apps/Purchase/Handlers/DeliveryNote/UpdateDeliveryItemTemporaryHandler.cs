using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.DeliveryNote
{
	public class UpdateDeliveryItemTemporaryHandler
	{
		public static Core.Models.ResponseModel<Models.DeliveryNote.UpdateDeliveryItemModel> UpdateDeliveryItem(Models.DeliveryNote.UpdateDeliveryItemModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					if(user == null
						|| !(
							(user.Access.CustomerService.ModuleActivated && user.Access.CustomerService.DeliveryNoteEdit)
							|| (user.Access.CustomerService.ModuleActivated && user.Access.CustomerService.DeliveryNoteCreate)
							|| user.SuperAdministrator || user.IsGlobalDirector))
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId);
					if(string.IsNullOrEmpty(data.ItemNumber) || string.IsNullOrWhiteSpace(data.ItemNumber))
					{
						return new Core.Models.ResponseModel<Models.DeliveryNote.UpdateDeliveryItemModel>()
						{
							Errors = new List<string>() { "article number must not be empty !" }
						};
					}

					var customerDb = orderDb.Kunden_Nr.HasValue
						? Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderDb.Kunden_Nr.Value)
						: null;
					if(customerDb == null)
					{
						return new Core.Models.ResponseModel<Models.DeliveryNote.UpdateDeliveryItemModel>()
						{
							Errors = new List<string>() { "Customer not found" }
						};
					}

					var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(data.ItemNumber);
					if(itemDb == null)
					{
						return new Core.Models.ResponseModel<Models.DeliveryNote.UpdateDeliveryItemModel>()
						{
							Errors = new List<string>() { "Item not found" }
						};
					}
					if(itemDb.Freigabestatus.ToUpper() == "O")
					{
						return new Core.Models.ResponseModel<Models.DeliveryNote.UpdateDeliveryItemModel>()
						{
							Errors = new List<string>() { "Item is 'Obsolete'" }
						};
					}

					Infrastructure.Data.Entities.Tables.INV.LagerorteEntity storageLocationDb = null;
					if(data.StorageLocationId > 0)
					{
						storageLocationDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get((int)data.StorageLocationId);
						if(storageLocationDb == null)
						{
							return new Core.Models.ResponseModel<Models.DeliveryNote.UpdateDeliveryItemModel>()
							{
								Errors = new List<string>() { "Storage Location not found" }
							};
						}
					}

					//var orderItemExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOderIdAndOrderItemId(orderDb.Nr, orderItemDb.Nr);

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

					var itemPricingGroupDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(itemDb.ArtikelNr);

					var conditionAssignementTableDb = customerDb.Konditionszuordnungs_Nr.HasValue
						? Infrastructure.Data.Access.Tables.PRS.KonditionsZuordnungstabelleEntity.Get(customerDb.Konditionszuordnungs_Nr.Value)
						: null;

					var discount = data.Discount ?? 0;
					var fixedPrice = data.FixedTotalPrice ?? false;
					var unitPriceBasis = data.UnitPriceBasis ?? 0;
					var cuWeight = Convert.ToDecimal(itemDb.CuGewicht ?? 0);
					var del = data.DelNote.HasValue ? Convert.ToInt32(data.DelNote.Value) : 0;
					//(itemDb.DEL ?? 0);

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


					var singleCopperSurcharge = CalculateSingleCopperSurcharge(fixedPrice,
						del,
						cuWeight,
						data.CopperBase.HasValue ? Convert.ToInt32(data.CopperBase.Value) : 0);

					var totalCopperSurcharge = Helpers.CalculationHelper.CalculateTotalCopperSurcharge(fixedPrice,
						data.OrderedQuantity,
						singleCopperSurcharge);

					var calc = (data.FixedUnitPrice.HasValue && data.FixedUnitPrice.Value) ? data.UnitPrice : verkaufspreis;

					var vkUnitPrice = Helpers.CalculationHelper.CalculateVkUnitPrice(fixedPrice,
						calc,
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
					var OriginalOrderQuantity = (data.positionViewMode == 1 || data.positionViewMode == 0) ? data.OpenQuantity_Quantity : data.OriginalOrderQuantity;
					var UnitPriceBasis = data.UnitPriceBasis ?? 0;

					Models.DeliveryNote.UpdateDeliveryItemModel response = new Models.DeliveryNote.UpdateDeliveryItemModel()
					{
						ItemNumber = itemDb.ArtikelNummer,
						CopperSurcharge = singleCopperSurcharge,
						OpenQuantity_CopperSurcharge = totalCopperSurcharge,
						UnitPrice = vkUnitPrice,
						TotalPrice = vKTotalPrice,
						OpenQuantity_UnitPrice = unitPrice,
						OpenQuantity_TotalPrice = totalPrice,
						//
						Id = data.Id,
						ItemId = data.ItemId,
						OrderId = data.OrderId,
						OrderNumber = data.OrderNumber,
						CreateDate = data.CreateDate,
						//TODO:--- Param Fields
						ItemTypeId = data.ItemTypeId ?? -1,
						OpenQuantity_Quantity = data.OpenQuantity_Quantity,
						//TODO:--- Get fields
						Designation1 = data.Designation1,
						Designation2 = data.Designation2,
						Designation3 = data.Designation3,
						DelNote = data.DelNote,
						CustomerItemNumber = data.CustomerItemNumber,
						Discount = data.Discount,
						VAT = data.VAT,
						ItemCustomerDescription = data.ItemCustomerDescription,
						//TODO:--- Calculated fields
						OriginalOrderQuantity = OriginalOrderQuantity,
						OriginalOrderAmount = data.OriginalOrderAmount,
						CopperBase = data.CopperBase,
						CopperWeight = data.CopperWeight,
						CurrentItemPriceCalculationNet = data.CurrentItemPriceCalculationNet,
						OpenQuantity_CopperWeight = totalCuWeight,
						UnitPriceBasis = data.UnitPriceBasis,
						//TODO:--- Changed fields
						DelieveryId = data.DelieveryId,
						Version = data.Version,
						Position = data.Position,
						StorageLocationId = data.StorageLocationId ?? -1,
						StorageLocationName = data.StorageLocationName,
						DelFixed = data.DelFixed,
						ChangeType = data.ChangeType ?? -1,
						DeliveryDate = data.DeliveryDate,
						DesiredDate = data.DesiredDate,
						Done = data.Done,
						FixedUnitPrice = data.FixedUnitPrice,
						FixedTotalPrice = data.FixedTotalPrice,
						RP = data.RP,
						FreeText = data.FreeText,
						Note1 = data.Note1,
						Note2 = data.Note2,
						ProductionNumber = data.ProductionNumber,
						//TODO:--- New fields
						positionViewMode = data.positionViewMode,
						DelNew = data.DelNew,
						DelUpdate = data.DelUpdate,
						index = data.index,
						//!CS Info
						Versandinfo_von_CS = data.Versandinfo_von_CS,
						//!Packing
						Packstatus = data.Packstatus,
						Gepackt_von = data.Gepackt_von,
						Gepackt_Zeitpunkt = data.Gepackt_Zeitpunkt,
						Packinfo_von_Lager = data.Packinfo_von_Lager,
						//!Shipping
						Versandstatus = data.Versandstatus,
						Versanddienstleister = data.Versanddienstleister,
						Versandnummer = data.Versandnummer,
						Versandinfo_von_Lager = data.Versandinfo_von_Lager,
						UnloadingPoint = data.UnloadingPoint, //Abladestelle !EDI
						EDI_PREIS_KUNDE = data.EDI_PREIS_KUNDE,
						EDI_PREISEINHEIT = data.EDI_PREISEINHEIT,
						//TODO:--- Position Table fields
						originalPosition = data.originalPosition,
						ArticleQuantity = data.ArticleQuantity,
						OrderedQuantity = data.OrderedQuantity,
						DeliveredQuantity = data.DeliveredQuantity,
						DesiredQuantity = data.DesiredQuantity,
						DesiredUnitPrice = data.DesiredUnitPrice,
						ApprovedQuantity = data.ApprovedQuantity,
						ApprovedUnitPrice = data.ApprovedUnitPrice,
						// ????---- fields
						MeasureUnitQualifier = data.MeasureUnitQualifier,
						DrawingIndex = data.DrawingIndex,
						HasPendingChange = data.HasPendingChange,
						HasPendingCancel = data.HasPendingCancel,
						termin_eingehalten = data.termin_eingehalten,
						//
						CalculatedValue = (data.UnitPriceBasis.HasValue && data.UnitPriceBasis.Value != 0) ?
						(OriginalOrderQuantity / data.UnitPriceBasis.Value) * unitPrice * (1 - data.Discount ?? 0) : 0,
						Index_Kunde = data.Index_Kunde,
						Index_Kunde_Datum = data.Index_Kunde_Datum,
						Postext = data.Postext,
						CSInterneBemerkung = data.CSInterneBemerkung,

					};
					#endregion
					return Core.Models.ResponseModel<Models.DeliveryNote.UpdateDeliveryItemModel>.SuccessResponse(response);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}


		internal static decimal CalculateSingleCopperSurcharge(bool fixedPrice,
			int del,
			decimal cuGewicht,
			int kupferbasis)
		{
			return !fixedPrice
				? decimal.Round((Convert.ToDecimal((del * 1.01m) - kupferbasis) / 100m) * cuGewicht, 2)
				: 0;
		}
	}
}
