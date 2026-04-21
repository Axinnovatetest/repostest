using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{

		internal static List<Models.Order.Element.OrderElementModel> GetElements(List<int> elementsIds, bool UpdateChoice = false)
		{
			const int MAX_DEC = 6;
			try
			{
				var response = new List<Models.Order.Element.OrderElementModel>();

				var elementsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(elementsIds);
				var itemsIds = elementsDb.Where(e => e.ArtikelNr.HasValue).Select(e => e.ArtikelNr.Value).ToList();
				var itemsDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(itemsIds);

				var storaLocationsIds = elementsDb.Where(e => e.Lagerort_id.HasValue).Select(e => e.Lagerort_id.Value).ToList();
				var storageLocationsDb = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get(storaLocationsIds);
				var ordersIds = elementsDb.Where(e => e.AngebotNr.HasValue).Select(e => e.AngebotNr.Value).ToList();
				var ordersChangesDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.GetByOrdersIds(ordersIds);
				var ordersChangesIds = ordersChangesDb.Select(e => e.Id).ToList();
				var itemChangePendingStatus = (int)Enums.OrderEnums.OrderChangeItemStatus.Pending;
				var itemChangeTypeChanged = (int)Enums.OrderEnums.OrderChangeItemTypes.Changed;
				var itemChangeTypeCanceled = (int)Enums.OrderEnums.OrderChangeItemTypes.Canceled;
				var pendingOrdersChangesItemsDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.GetByOrderChangeIds(ordersChangesIds)
					.FindAll(e => e.Status == itemChangePendingStatus);

				var orderElementExts = Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.GetByOrderItemsIds(elementsDb.Select(e => e.Nr).ToList());
				var fertigungEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(elementsDb?.Select(x => x.Fertigungsnummer ?? -1)?.ToList());
				foreach(var elementDb in elementsDb)
				{
					var itemDb = elementDb.ArtikelNr.HasValue
						? itemsDb.Find(e => e.ArtikelNr == elementDb.ArtikelNr.Value)
						: null;

					var pendingOrderItemsChangesDb = pendingOrdersChangesItemsDb.Where(e => e.OrderId == elementDb.AngebotNr).ToList();

					var storageLocationDb = storageLocationsDb.Find(e => e.LagerortId == elementDb.Lagerort_id);
					var ArticleProductionExtension = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(itemDb?.ArtikelNr ?? -1);
					var ArticleProductionPlace = ArticleProductionExtension?.ProductionPlace1_Id;

					var fertigungEntity = fertigungEntities?.FirstOrDefault(x => x.Fertigungsnummer == elementDb.Fertigungsnummer);
					var elementDbExtension = orderElementExts?.Find(x => x.OrderItemId == elementDb.Nr);
					var _prodPlace = -1;
					if(ArticleProductionPlace != null)
					{
						// - 2022-12-22 - return EnumValue
						foreach(int i in Enum.GetValues(typeof(Common.Enums.ArticleEnums.ArticleProductionPlace)))
						{
							if(i == ArticleProductionPlace.Value)
							{
								_prodPlace = i;
								break;
							}
						}
						//switch((Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace)ArticleProductionPlace)
						//{
						//	case Common.Enums.ArticleEnums.ArticleProductionPlace.AL:
						//		_prodPlace = 26;
						//		break;
						//	case Common.Enums.ArticleEnums.ArticleProductionPlace.TN:
						//		_prodPlace = 7;
						//		break;
						//	case Common.Enums.ArticleEnums.ArticleProductionPlace.BETN:
						//		_prodPlace = 60;
						//		break;
						//	case Common.Enums.ArticleEnums.ArticleProductionPlace.WS:
						//		_prodPlace = 42;
						//		break;
						//	case Common.Enums.ArticleEnums.ArticleProductionPlace.DE:
						//		_prodPlace = 15;
						//		break;
						//	case Common.Enums.ArticleEnums.ArticleProductionPlace.CZ:
						//		_prodPlace = 6;
						//		break;
						//	default:
						//		break;
						//}
					}
					var rahmenposition = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity();
					var rahmen = new Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity();
					if(elementDb.ABPoszuRAPos.HasValue && elementDb.ABPoszuRAPos.Value != 0 && elementDb.ABPoszuRAPos.Value != -1)
					{
						rahmenposition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(elementDb.ABPoszuRAPos ?? -1);
						rahmen = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(rahmenposition?.AngebotNr ?? -1);
					}
					// - 2025-04-03 - Groetsch Ticket #43732 - init Pos with Serie if empty
					if(!elementDb.Typ.HasValue)
					{
						var articleSalesEntities = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNr(elementDb.ArtikelNr ?? 0);
						if(articleSalesEntities?.Count > 0 && articleSalesEntities.Exists(x => x.ArticleSalesType == CustomerService.Enums.OrderEnums.ItemType.Serie.GetDescription()))
						{
							elementDb.Typ = (int)CustomerService.Enums.OrderEnums.ItemType.Serie;
						}
					}
					var salesType = CustomerService.Enums.OrderEnums.ConvertToMTDSalesItemType(((CustomerService.Enums.OrderEnums.ItemType)(elementDb.Typ ?? (int)CustomerService.Enums.OrderEnums.ItemType.Serie)).GetDescription());
					var articleSalesEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.GetByArticleNrAndTypeId(elementDb.ArtikelNr ?? 0, (int)salesType);
					var order = new Models.Order.Element.OrderElementModel()
					{
						ItemTypeId = elementDb.Typ,
						ItemTypeName = elementDb.Typ.HasValue ? GetDescription((CustomerService.Enums.OrderEnums.ItemType)elementDb.Typ) : "",
						Id = elementDb.Nr,
						OrderNumber = elementDb.AngebotNr?.ToString(),
						OrderId = elementDb.AngebotNr ?? -1,
						Done = (elementDb.erledigt_pos ?? false),
						// >>>>>>>>>>>>>>>>
						ItemId = elementDb.ArtikelNr ?? -1,
						ItemNumber = itemDb?.ArtikelNummer,
						RP = elementDb.RP ?? false,
						Position = elementDb.Position ?? 0,
						OpenQuantity_Quantity = Convert.ToDecimal(Math.Round(elementDb.Anzahl ?? 0, MAX_DEC)),
						DesiredDate = elementDb.Wunschtermin,
						DeliveryDate = elementDb.Liefertermin,
						StorageLocationId = storageLocationDb != null ? storageLocationDb.LagerortId : -1,

						StorageLocationName = storageLocationDb?.Lagerort,
						FixedUnitPrice = elementDb.EKPreise_Fix ?? false, // <<<<<< !
						UnitPrice = Convert.ToDecimal(Math.Round(elementDb.VKEinzelpreis ?? 0, MAX_DEC)),
						FixedTotalPrice = elementDb.VKFestpreis ?? false,
						TotalPrice = Convert.ToDecimal(Math.Round(elementDb.VKGesamtpreis ?? 0, MAX_DEC)),
						UnitPriceBasis = Convert.ToDecimal(Math.Round(elementDb.Preiseinheit ?? 0, MAX_DEC)),
						Discount = Convert.ToDecimal(Math.Round(elementDb.Rabatt ?? 0, MAX_DEC)),
						VAT = Convert.ToDecimal(Math.Round(elementDb.USt ?? 0, MAX_DEC)),
						// >>>>>>>>>>>>>>
						Designation1 = elementDb?.Bezeichnung1,
						Designation2 = elementDb?.Bezeichnung2,
						Designation3 = itemDb?.Bezeichnung3,
						MeasureUnitQualifier = itemDb?.Einheit,
						DrawingIndex = itemDb?.Index_Kunde,
						CopperBase = elementDb.Kupferbasis ?? 0,
						DelFixed = elementDb.DELFixiert ?? false,
						DelNote = elementDb.DEL ?? 0,
						CopperWeight = Convert.ToDecimal(Math.Round(elementDb.EinzelCuGewicht ?? 0, MAX_DEC)),
						CopperSurcharge = Convert.ToDecimal(Math.Round(elementDb.Einzelkupferzuschlag ?? 0, MAX_DEC)),
						ProductionNumber = (elementDb.Fertigungsnummer ?? 0),
						// >>>>>>>>>>>>>>>>
						UnloadingPoint = elementDb.Abladestelle,
						OpenQuantity_UnitPrice = Convert.ToDecimal(Math.Round(elementDb.Einzelpreis ?? 0, MAX_DEC)),
						OpenQuantity_TotalPrice = Convert.ToDecimal(Math.Round(elementDb.Gesamtpreis ?? 0, MAX_DEC)),
						OpenQuantity_CopperWeight = Convert.ToDecimal(Math.Round(elementDb.GesamtCuGewicht ?? 0, MAX_DEC)),
						OpenQuantity_CopperSurcharge = Convert.ToDecimal(Math.Round(elementDb.Gesamtkupferzuschlag ?? 0, MAX_DEC)),
						OriginalOrderQuantity = Convert.ToDecimal(Math.Round(elementDb.OriginalAnzahl ?? 0, MAX_DEC)),
						OriginalOrderAmount = (decimal)getOriginalPrice(elementDb, elementDbExtension),
						DeliveredQuantity = Convert.ToDecimal(Math.Round(elementDb.Geliefert ?? 0, MAX_DEC)),
						// >>>>>>>>>>>>>>>>
						FreeText = elementDb.Freies_Format_EDI,
						Note1 = elementDb.Bemerkungsfeld1,
						Note2 = elementDb.Bemerkungsfeld2,
						//  >> CHANGES >>>>
						HasPendingChange = pendingOrderItemsChangesDb.Exists(e => e.Type == itemChangeTypeChanged),
						HasPendingCancel = pendingOrderItemsChangesDb.Exists(e => e.Type == itemChangeTypeCanceled),
						originalPosition = elementDb.PositionZUEDI,
						Postext = elementDb.POSTEXT,
						Index_Kunde = elementDb.Index_Kunde,
						Index_Kunde_Datum = elementDb.Index_Kunde_Datum,
						ManufacturingFacilityId = _prodPlace != -1 ? _prodPlace : null,
						CSInterneBemerkung = elementDb.CSInterneBemerkung,
						// -2022-06-20
						ProductionId = fertigungEntity?.ID ?? -1,

						RahmenPosId = elementDb.ABPoszuRAPos,
						RahmenId = rahmenposition?.AngebotNr,
						RahmenNumber = rahmen?.Angebot_Nr ?? 0,
						RahmenPositionNumber = rahmenposition.Position ?? 0,
						DeliveryTime = articleSalesEntity?.Lieferzeit ?? 0,
						LotSize = articleSalesEntity?.Losgroesse ?? 0,
						ExternalStatus = itemDb?.Freigabestatus
					};

					if(UpdateChoice && (order.DeliveryDate is null))
					{
						order.DeliveryDate = order.DesiredDate;
					}
					if(UpdateChoice && (order.StorageLocationName is null))
					{
						order.StorageLocationName = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetLagerort(order.StorageLocationId);
					}
					response.Add(order);
				}

				return sortElements(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static decimal getOriginalPrice(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity orderElement,
			Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity orderElementExtension)
		{
			var order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderElement.AngebotNr != null ? orderElement.AngebotNr.Value : -1);
			// if not EDI order
			if(order?.EDI_Order_Neu == null || order?.EDI_Order_Neu == false)
				return ((decimal?)orderElement?.Gesamtpreis) ?? 0;

			// if Primary Position
			if(orderElementExtension != null && orderElementExtension.PrimaryPositionNumber == null)
				return orderElementExtension?.OriginalGesamtpreis ?? 0;

			return 0;
		}
		internal static List<Models.Order.Element.OrderElementModel> sortElements(List<Models.Order.Element.OrderElementModel> elements)
		{
			var PLACEHOLDER = -9999;
			elements = elements?.OrderBy(x => x.Position)?.ToList();
			var primaryElements = elements?.FindAll(x => x.originalPosition == null)?.OrderBy(x => x.Position)?.ToList();
			var secondaryElements = elements?.FindAll(x => x.originalPosition != null)?.OrderBy(x => x.Position)?.ToList();

			if(elements == null || elements.Count <= 0
				|| primaryElements == null || primaryElements.Count <= 0
				|| secondaryElements == null || secondaryElements.Count <= 0)
				return elements;


			var _elements = new List<Models.Order.Element.OrderElementModel>();
			if(primaryElements.Count == 1)
			{
				_elements.AddRange(primaryElements);
				_elements.AddRange(secondaryElements);
			}
			else
			{
				foreach(var item in primaryElements)
				{
					_elements.Add(item);
					_elements.AddRange(secondaryElements.FindAll(x => x.originalPosition == item.Id || x.Position == item.Position /*Compatibility with */)?.OrderBy(x => x.Position)?.ToList());
				}

				// - 2022-05-05 - secondary Positions w/o primary link
				var detachedSecondaryPos = secondaryElements.FindAll(x => !_elements.Exists(y => y.Id == x.Id))?.ToList();
				if(detachedSecondaryPos != null && detachedSecondaryPos.Count > 0)
				{
					_elements.AddRange(detachedSecondaryPos);
				}
			}

			return _elements;
		}


		public static string GetDescription(Enum value)
		{
			Type type = value.GetType();
			string name = Enum.GetName(type, value);
			if(name != null)
			{
				FieldInfo field = type.GetField(name);
				if(field != null)
				{
					DescriptionAttribute attr =
						   Attribute.GetCustomAttribute(field,
							 typeof(DescriptionAttribute)) as DescriptionAttribute;
					if(attr != null)
					{
						return attr.Description;
					}
				}
			}
			return null;
		}
	}
}
