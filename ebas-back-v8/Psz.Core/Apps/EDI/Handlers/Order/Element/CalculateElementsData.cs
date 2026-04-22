using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Element
		{
			public static Core.Models.ResponseModel<List<Models.Order.Element.OrderElementModel>> CalculateElementsData(Models.Order.Element.NotCalculatedOrderElementsModel data, int customerNumber)
			{
				var itemsSuppliersNumbers = data.Elements
					.Where(e => !string.IsNullOrWhiteSpace(e.ItemNumber))
					.Select(e => e.ItemNumber?.Trim())
					.ToList();
				var itemsCustomersNumbers = data.Elements
					.Where(e => !string.IsNullOrEmpty(e.CustomerItemNumber))
					.Select(e => e.CustomerItemNumber?.Trim()?.TrimStart('0'))
					.ToList();

				var itemsDbBySuppliersNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(itemsSuppliersNumbers, itemsCustomersNumbers);
				var itemsDbByCustomerNumbers = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByBezeichnung1(itemsCustomersNumbers);

				#region > Check Items
				var itemsErrors = new List<string>();
				foreach(var lineItem in data.Elements)
				{
					// 2023-01-20 - check StdEdi
					var stdEdiArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(customerNumber, lineItem.CustomerItemNumber);
					if(stdEdiArticleEntity != null)
					{
						// - we're good to go
						continue;
					}

					var itemDbs = !string.IsNullOrWhiteSpace(lineItem.ItemNumber)
							? itemsDbBySuppliersNumbers.FindAll(e => e.ArtikelNummer == lineItem.ItemNumber).ToList()
							: null;
					if(itemDbs == null || itemDbs.Count <= 0)
					{
						itemDbs = !string.IsNullOrEmpty(lineItem.CustomerItemNumber)
							? itemsDbByCustomerNumbers.FindAll(e => e.Bezeichnung1?.ToLower().Trim().Contains(lineItem.CustomerItemNumber?.ToLower().Trim()?.TrimStart('0')) == true)?.ToList()
							: null;
					}

					if(itemDbs == null || itemDbs.Count <= 0)
					{
						itemsErrors.Add("Position number " + lineItem.PositionNumber + ": Article not found.");
						continue;
					}
					//if(itemDbs.Count > 0)
					//{
					//    Infrastructure.Services.Logging.Logger.Log(string.Join('-', itemDbs.Select(e => e.Freigabestatus).ToList()));
					//    if(itemDbs.All(e => e.Freigabestatus == "O"))
					//    {
					//        itemsErrors.Add("Position number " + lineItem.PositionNumber + ": Article is obsolete");
					//        continue;
					//    }
					//}

					if(lineItem.UnitPriceBasis <= 0)
					{
						itemsErrors.Add("Position number " + lineItem.PositionNumber + ": UnitPriceBasis " + lineItem.UnitPriceBasis + " is invalid");
					}

					if(lineItem.OrderedQuantity <= 0)
					{
						itemsErrors.Add("Position number " + lineItem.PositionNumber + ": Ordered Quantity " + lineItem.OrderedQuantity + " is invalid");
					}

					if(lineItem.CurrentItemPriceCalculationNet < 0)
					{
						itemsErrors.Add("Position number " + lineItem.PositionNumber + ": Current Item Price Calculation Net " + lineItem.CurrentItemPriceCalculationNet + " is invalid");
					}
				}

				if(itemsErrors.Count > 0)
				{
					return new Core.Models.ResponseModel<List<Models.Order.Element.OrderElementModel>>()
					{
						Errors = itemsErrors
					};
				}
				#endregion

				var itemsPricingGroupsNrs = itemsDbBySuppliersNumbers.Select(e => e.ArtikelNr).ToList();
				itemsPricingGroupsNrs.AddRange(itemsDbByCustomerNumbers.Select(e => e.ArtikelNr));

				var itemsPricingGroupsDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(itemsPricingGroupsNrs);

				var storageLocationsDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get();

				var responseElements = new List<Models.Order.Element.OrderElementModel>();


				foreach(var itemData in data.Elements)
				{
					Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity itemDb = null;
					// 2023-01-20 - check StdEdi
					var stdEdiArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetDefaultEdi(customerNumber, itemData.CustomerItemNumber);
					if(stdEdiArticleEntity != null)
					{
						itemDb = stdEdiArticleEntity;
					}
					else
					{
						// - Search Article by PSZ number, if any
						if(!string.IsNullOrWhiteSpace(itemData.ItemNumber))
						{
							itemDb = !string.IsNullOrWhiteSpace(itemData.ItemNumber)
									? itemsDbBySuppliersNumbers.Find(e => e.ArtikelNummer == itemData.ItemNumber)
									: null;
						}
						else
						{
							itemDb = !string.IsNullOrWhiteSpace(itemData.CustomerItemNumber)
									   ? itemsDbBySuppliersNumbers.Find(e => e.Bezeichnung1?.ToLower().Trim().Contains(itemData.CustomerItemNumber?.ToLower().Trim()?.TrimStart('0')) == true)
									   : null;
						}

						if(itemDb == null)
						{
							itemDb = !string.IsNullOrEmpty(itemData.CustomerItemNumber)
								? itemsDbByCustomerNumbers.Find(e => e.Bezeichnung1?.ToLower().Trim().Contains(itemData.CustomerItemNumber?.ToLower().Trim()?.TrimStart('0')) == true)
								: null;
						}
						// - 2023-01-22 - if XML article Obsolete, take next in same CustomerItemNumber (Bz1 is used here)
						if(itemDb != null && itemDb.Freigabestatus?.ToLower()?.Trim() == "o")
						{
							var articleXmlEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(customerNumber, itemDb.ArtikelNummer?.Substring(0, itemDb.ArtikelNummer?.IndexOf('-') ?? 0), itemData.CustomerItemNumber ?? itemDb.CustomerItemNumber)
								?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
							itemDb = articleXmlEntities.OrderByDescending(x => x.ArtikelNr).FirstOrDefault(x => x.Freigabestatus?.ToLower()?.Trim() != "o") ?? itemDb;
						}
					}

					if(itemDb == null)
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Warn, "itemDb == null");
						continue;
					}

					var discount = 0m;
					var unitPriceBasis = Convert.ToDecimal(itemDb.Preiseinheit ?? 0m);
					var fixedPrice = itemDb.VKFestpreis ?? false;
					var cuWeight = Convert.ToDecimal(itemDb.CuGewicht ?? 0);
					var del = (itemDb.DEL ?? 0);
					var freeText = itemData.FreeText;

					var me1 = 0m;
					var me2 = 0m;
					var me3 = 0m;
					var me4 = 0m;
					var pm1 = 0m;
					var pm2 = 0m;
					var pm3 = 0m;
					var pm4 = 0m;
					var verkaufspreis = 0m;
					var kupferbasis = 150;

					var itemPricingGroupDb = itemsPricingGroupsDb.Find(e => e.Artikel_Nr == itemDb.ArtikelNr);
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
						cuWeight,
						kupferbasis);

					var totalCopperSurcharge = Helpers.CalculationHelper.CalculateTotalCopperSurcharge(fixedPrice,
						itemData.OrderedQuantity,
						singleCopperSurcharge);

					var vkUnitPrice = Helpers.CalculationHelper.CalculateVkUnitPrice(true, fixedPrice,
						verkaufspreis,
						itemData.OrderedQuantity,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					var unitPrice = Helpers.CalculationHelper.CalculateUnitPrice(true, fixedPrice,
						unitPriceBasis,
						itemData.OrderedQuantity,
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
						itemData.OrderedQuantity,
						discount);

					var vKTotalPrice = Helpers.CalculationHelper.CalculateVkTotalPrice(unitPriceBasis,
						vkUnitPrice,
						itemData.OrderedQuantity);

					var totalCuWeight = Helpers.CalculationHelper.CalculateTotalWeight(itemData.OrderedQuantity,
						cuWeight);

					var element = new Models.Order.Element.OrderElementModel
					{
						ItemTypeId = itemData.ItemTypeId,
						ChangeType = itemData.ChangeType,
						ItemCustomerDescription = itemData.ItemDescription,
						CurrentItemPriceCalculationNet = itemData.CurrentItemPriceCalculationNet,
						CustomerItemNumber = itemData.CustomerItemNumber,
						// >>>>>>>>>>>>>>>>>
						Id = itemData.Id, // <<--
						OrderNumber = data.OrderId.ToString(),
						OrderId = data.OrderId,
						Done = false,
						// >>>>>>>>>>>>>>>>
						ItemId = itemDb.ArtikelNr,
						ItemNumber = itemDb.ArtikelNummer,
						RP = false,
						Position = itemData.PositionNumber,
						OpenQuantity_Quantity = itemData.OrderedQuantity,
						DesiredDate = itemData.DesiredDate,
						DeliveryDate = null,
						StorageLocationId = -1, // <<--
						StorageLocationName = "", // <<--
						FixedUnitPrice = fixedPrice,
						UnitPrice = vkUnitPrice,
						FixedTotalPrice = fixedPrice,
						TotalPrice = vKTotalPrice,
						UnitPriceBasis = Convert.ToDecimal(itemDb.Preiseinheit ?? 0),
						Discount = 0m,
						VAT = Convert.ToDecimal(itemDb.Umsatzsteuer ?? 0),
						// >>>>>>>>>>>>>>
						Designation1 = itemDb.Bezeichnung1,
						Designation2 = itemDb.Bezeichnung2,
						Designation3 = itemDb.Bezeichnung3,
						MeasureUnitQualifier = itemDb.Einheit,
						DrawingIndex = itemDb.Zeichnungsnummer,
						CopperBase = kupferbasis, // 150,
						DelFixed = itemDb.DELFixiert ?? false,
						DelNote = itemDb.DEL ?? 0,
						CopperWeight = Convert.ToDecimal(itemDb.CuGewicht ?? 0),
						CopperSurcharge = singleCopperSurcharge,
						ProductionNumber = 0,
						// >>>>>>>>>>>>>>>>
						UnloadingPoint = itemData.UnloadingPoint,
						OpenQuantity_UnitPrice = unitPrice,
						OpenQuantity_TotalPrice = totalPrice,
						OpenQuantity_CopperWeight = totalCuWeight,
						OpenQuantity_CopperSurcharge = totalCopperSurcharge,
						OriginalOrderQuantity = itemData.OrderedQuantity,
						OriginalOrderAmount = totalPrice,
						DeliveredQuantity = 0,
						// >>>>>>>>>>>>>>>>
						FreeText = itemData.FreeText,
						Note1 = itemData.FreeText,
						Note2 = itemData.FreeText,
						Consignee = itemData.Consignee,

						// - 
						Index_Kunde = itemDb.Index_Kunde,
						Index_Kunde_Datum = itemDb.Index_Kunde_Datum,
					};

					responseElements.Add(element);
				}

				return Core.Models.ResponseModel<List<Models.Order.Element.OrderElementModel>>
					.SuccessResponse(responseElements);
			}
		}
	}
}
