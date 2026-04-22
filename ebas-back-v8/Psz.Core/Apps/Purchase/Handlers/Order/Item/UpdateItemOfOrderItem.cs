using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> UpdateItemOfOrderItem(Models.Order.UpdateElementItemModel data,
			Core.Identity.Models.UserModel user)
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

					var orderElementDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
					if(orderElementDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order Element not found" }
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
					var errors = new List<string>();
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

					var discount = Convert.ToDecimal(orderElementDb.Rabatt ?? 0);
					var fixedPrice = orderElementDb.VKFestpreis ?? false;
					var cuWeight = Convert.ToDecimal(itemDb.CuGewicht ?? 0);
					var del = (itemDb.DEL ?? 0);
					var unitPriceBasis = Convert.ToDecimal(itemDb.Preiseinheit ?? 0);
					var ordredQuantity = Convert.ToDecimal(orderElementDb.AktuelleAnzahl ?? 0);

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
						ordredQuantity,
						singleCopperSurcharge);

					var vkUnitPrice = Helpers.CalculationHelper.CalculateVkUnitPrice(fixedPrice,
						verkaufspreis,
						ordredQuantity,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					var unitPrice = Helpers.CalculationHelper.CalculateUnitPrice(fixedPrice,
						unitPriceBasis,
						ordredQuantity,
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
						ordredQuantity,
						discount);

					var vKTotalPrice = Helpers.CalculationHelper.CalculateVkTotalPrice(unitPriceBasis,
						vkUnitPrice,
						ordredQuantity);

					var totalCuWeight = Helpers.CalculationHelper.CalculateTotalWeight(ordredQuantity,
						cuWeight);

					//orderElementDb.Position = data.PositionNumber;
					//orderElementDb.Wunschtermin = data.DesiredDate;
					//orderElementDb.Anzahl = (decimal)data.OrderedQuantity;
					//orderElementDb.AktuelleAnzahl = (decimal)data.OrderedQuantity;
					//orderElementDb.Abladestelle = data.ConsigneeUnloadingPoint;
					//orderElementDb.Bezeichnung2_Kunde = itemDb.Bezeichnung2;
					//orderElementDb.OriginalAnzahl = (decimal)data.OrderedQuantity;
					//orderElementDb.Freies_Format_EDI = data.FreeText;
					//orderElementDb.Bemerkungsfeld1 = data.Note1;
					//orderElementDb.Bemerkungsfeld2 = data.Note2;
					// - 2022-05-16 Bz
					orderElementDb.Bezeichnung1 = itemDb.Bezeichnung1;
					orderElementDb.Bezeichnung2 = itemDb.Bezeichnung2;
					//orderElementDb.Einheit = data.MeasureUnitQualifier;
					orderElementDb.ArtikelNr = itemDb.ArtikelNr;
					//orderElementDb.Kupferbasis = 150;
					//orderElementDb.Preiseinheit = unitPriceBasis;
					//orderElementDb.DELFixiert = itemDb.DELFixiert;
					//orderElementDb.DEL = itemDb.DEL;
					//orderElementDb.EinzelCuGewicht = itemDb.CuGewicht;
					orderElementDb.VKFestpreis = fixedPrice;
					//orderElementDb.USt = itemDb.Umsatzsteuer;
					orderElementDb.Einzelkupferzuschlag = (decimal)singleCopperSurcharge;
					orderElementDb.GesamtCuGewicht = (decimal)totalCuWeight;
					orderElementDb.Einzelpreis = (decimal)unitPrice;
					orderElementDb.VKEinzelpreis = (decimal)vkUnitPrice;
					orderElementDb.Gesamtpreis = (decimal)totalPrice;
					orderElementDb.Gesamtkupferzuschlag = (decimal)totalCopperSurcharge;
					orderElementDb.VKGesamtpreis = (decimal)vKTotalPrice;

					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(orderElementDb);

					// > ---EDI---EDI---EDI---EDI---EDI---
					// OrderElementExtension.UpdateStatus(orderElementDb.Nr);

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
