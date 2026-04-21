using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static List<Models.Order.Element.OrderItemModel> GetOrderItems(int orderId)
		{
			try
			{
				var elementsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderId, false);
				var elementsIds = elementsDb.Select(e => e.Nr).ToList();

				return GetOrderItems(elementsIds);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static List<Models.Order.Element.OrderItemModel> GetOrderItems(List<int> elementsIds)
		{
			try
			{
				var response = new List<Models.Order.Element.OrderItemModel>();

				var orderItemsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(elementsIds);
				var orderDB = (orderItemsDb != null && orderItemsDb.Count > 0) ? Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderItemsDb[0].AngebotNr ?? -1) : null;

				var orderItemsIds = orderItemsDb?.Select(e => e.Nr)?.ToList();
				var orderItemsExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderItemsIds(orderItemsIds);

				var itemsIds = orderItemsDb?.Where(e => e.ArtikelNr.HasValue)?.Select(e => e.ArtikelNr.Value)?.ToList();
				var itemsDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(itemsIds);

				var storaLocationsIds = orderItemsDb?.Where(e => e.Lagerort_id.HasValue)?.Select(e => e.Lagerort_id.Value)?.ToList();
				var storageLocationsDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(storaLocationsIds);

				var ordersIds = orderItemsDb?.Where(e => e.AngebotNr.HasValue)?.Select(e => e.AngebotNr.Value)?.ToList();
				var ordersChangesDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.GetByOrdersIds(ordersIds);
				var ordersChangesIds = ordersChangesDb?.Select(e => e.Id)?.ToList();

				string IslSoRAB = string.Empty;
				var AngeboteNr = (orderItemsDb != null && orderItemsDb.Count > 0)
					? orderItemsDb[0].AngebotNr
					: null;
				var orderType = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(AngeboteNr ?? -1)?.Typ;
				var fertigungEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(orderItemsDb?.Select(x => x.Fertigungsnummer ?? -1)?.ToList())?.Where(x => x.Kennzeichen?.ToLower()?.Trim() != "storno");

				var adressenDB = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderDB?.Kunden_Nr ?? -1);
				foreach(var orderItemDb in orderItemsDb)
				{
					var itemDb = orderItemDb.ArtikelNr.HasValue
						? itemsDb.Find(e => e.ArtikelNr == orderItemDb.ArtikelNr.Value)
						: null;

					var fertigungEntity = fertigungEntities?.FirstOrDefault(x => x.Fertigungsnummer == orderItemDb.Fertigungsnummer);
					// > ---EDI---EDI---EDI---EDI---EDI---
					// var pendingOrderItemsChangesDb = pendingOrdersChangesItemsDb.Where(e => e.OrderId == elementDb.AngebotNr).ToList();
					Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity OroginalOrderOrLSItem = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity();
					var storageLocationDb = storageLocationsDb?.Find(e => e.LagerortId == orderItemDb.Lagerort_id);
					var orderItemsExtensionDb = orderItemsExtensionsDb?.Find(e => e.OrderItemId == orderItemDb.Nr);
					if(orderItemDb.LSPoszuABPos.HasValue && orderItemDb.LSPoszuABPos.Value != 0 && orderItemDb.LSPoszuABPos.Value != -1)
					{
						OroginalOrderOrLSItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(orderItemDb.LSPoszuABPos ?? -1);
					}
					var CalculatedValue = (orderItemDb.Preiseinheit.HasValue && orderItemDb.Preiseinheit.Value > 0) ?
								(orderItemDb.OriginalAnzahl ?? 0 / orderItemDb.Preiseinheit.Value) * orderItemDb.Einzelpreis ?? 0 * (1 - orderItemDb.Rabatt ?? 0)
								: 0;

					var rahmenposition = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity();
					var rahmen = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity();
					if(orderItemDb.ABPoszuRAPos.HasValue && orderItemDb.ABPoszuRAPos.Value != 0 && orderItemDb.ABPoszuRAPos.Value != -1)
					{
						rahmenposition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(orderItemDb.ABPoszuRAPos ?? -1);
						rahmen = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(rahmenposition?.AngebotNr ?? -1);
					}
					var order = new Models.Order.Element.OrderItemModel()
					{
						Id = orderItemDb.Nr,
						OrderNumber = orderItemDb.AngebotNr?.ToString(),
						OrderId = orderItemDb.AngebotNr ?? -1,
						Done = (orderItemDb.erledigt_pos ?? false),
						// >>>>>>>>>>>>>>>>
						ItemId = orderItemDb.ArtikelNr ?? -1,
						ItemNumber = itemDb?.ArtikelNummer,
						RP = orderItemDb.RP ?? false,
						Position = orderItemDb.Position ?? 0,
						OpenQuantity_Quantity = Convert.ToDecimal(orderItemDb.Anzahl ?? 0),
						DesiredDate = orderItemDb.Wunschtermin,
						DeliveryDate = orderItemDb.Liefertermin,
						StorageLocationId = storageLocationDb != null ? storageLocationDb.LagerortId : -1,
						StorageLocationName = storageLocationDb?.Lagerort,
						FixedUnitPrice = orderItemDb.VKFestpreis ?? false, // <<<<<< !
						UnitPrice = Convert.ToDecimal(orderItemDb.VKEinzelpreis ?? 0),
						FixedTotalPrice = orderItemDb.VKFestpreis ?? false,
						TotalPrice = Convert.ToDecimal(orderItemDb.VKGesamtpreis ?? 0),
						UnitPriceBasis = Convert.ToDecimal(orderItemDb.Preiseinheit ?? 0),
						Discount = Convert.ToDecimal(orderItemDb.Rabatt ?? 0),
						VAT = Convert.ToDecimal(orderItemDb.USt ?? 0),
						// >>>>>>>>>>>>>>
						Designation1 = orderItemDb?.Bezeichnung1,
						Designation2 = orderItemDb?.Bezeichnung2,
						Designation3 = itemDb?.Bezeichnung3,
						MeasureUnitQualifier = itemDb?.Einheit,
						DrawingIndex = itemDb?.Zeichnungsnummer,
						CopperBase = orderItemDb.Kupferbasis ?? 0,
						DelFixed = orderItemDb.DELFixiert ?? false,
						DelNote = orderItemDb.DEL ?? 0,
						CopperWeight = Convert.ToDecimal(orderItemDb.EinzelCuGewicht ?? 0),
						CopperSurcharge = Convert.ToDecimal(orderItemDb.Einzelkupferzuschlag ?? 0),
						ProductionNumber = (orderItemDb.Fertigungsnummer ?? 0),
						// >>>>>>>>>>>>>>>>
						UnloadingPoint = orderItemDb.Abladestelle,
						OpenQuantity_UnitPrice = Convert.ToDecimal(orderItemDb.Einzelpreis ?? 0),
						OpenQuantity_TotalPrice = Convert.ToDecimal(orderItemDb.Gesamtpreis ?? 0),
						OpenQuantity_CopperWeight = Convert.ToDecimal(orderItemDb.GesamtCuGewicht ?? 0),
						OpenQuantity_CopperSurcharge = Convert.ToDecimal(orderItemDb.Gesamtkupferzuschlag ?? 0),
						OriginalOrderQuantity = Convert.ToDecimal(orderItemDb.OriginalAnzahl ?? 0),
						OriginalOrderAmount = (orderType == "Lieferschein") ? CalculatedValue : Convert.ToDecimal(orderItemDb.VKGesamtpreis ?? 0),
						DeliveredQuantity = (orderType == "Lieferschein") ? OroginalOrderOrLSItem?.Geliefert ?? 0
						: Convert.ToDecimal(orderItemDb.Geliefert ?? 0),
						// >>>>>>>>>>>>>>>>
						FreeText = orderItemDb.Freies_Format_EDI,
						Note1 = orderItemDb.Bemerkungsfeld1,
						Note2 = orderItemDb.Bemerkungsfeld2,

						Version = (orderItemsExtensionDb?.Version ?? 0),
						Index_Kunde = orderItemDb.Index_Kunde,
						Index_Kunde_Datum = orderItemDb.Index_Kunde_Datum,
						LS_ZU_AB = orderItemDb.LSPoszuABPos,
						CSInterneBemerkung = orderItemDb.CSInterneBemerkung,
						ItemTypeId = orderItemDb.Typ,

						RahmenPosId = orderItemDb.ABPoszuRAPos,
						RahmenId = rahmenposition?.AngebotNr,
						RahmenVorfallNr = rahmen?.Angebot_Nr,
						CustomerItemNumber = orderDB != null && orderDB.Kunden_Nr.HasValue ? orderDB.Kunden_Nr.Value.ToString() : "",
						ItemCustomerDescription = adressenDB?.Name1,
						Postext = orderItemDb.POSTEXT,
						//- 2022-06-20
						ProductionId = fertigungEntity?.ID ?? -1,
						OrderType = orderDB?.Typ,
						OrderIsManualCreation = !orderDB.Neu_Order.HasValue && string.IsNullOrWhiteSpace(orderDB.EDI_Dateiname_CSV)
					};

					response.Add(order);
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
