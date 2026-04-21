using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public partial class Element
		{
			public static Core.Models.ResponseModel<List<int>> CreateOrderElements(Models.Order.Element.NotCalculatedOrderElementsModel data,
				Core.Identity.Models.UserModel user = null)
			{
				lock(Locks.OrdersLock)
				{
					try
					{
						if(user == null || !user.Access.Purchase.ModuleActivated)
						{
							throw new Core.Exceptions.UnauthorizedException();
						}

						var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId);
						if(orderDb == null)
						{
							return new Core.Models.ResponseModel<List<int>>()
							{
								Errors = new List<string>() { "Order not found" }
							};
						}

						//if (orderDb.Neu_Order == false)
						//{
						//    return new Core.Models.ResponseModel<List<int>>()
						//    {
						//        Errors = new List<string>() { "Order is validated" }
						//    };
						//}

						return CreateItems(data, user);
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(e);
						throw;
					}
				}
			}

			internal static Core.Models.ResponseModel<List<int>> CreateItems(Models.Order.Element.NotCalculatedOrderElementsModel data,
				Core.Identity.Models.UserModel user)
			{
				var calculatedElementsResponse = CalculateItemsData(data);

				if(!calculatedElementsResponse.Success)
				{
					return new Core.Models.ResponseModel<List<int>>(null)
					{
						Errors = calculatedElementsResponse.Errors
					};
				}

				var insertedIds = new List<int>();

				foreach(var itemData in calculatedElementsResponse.Body)
				{
					var orderElementDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity()
					{
						// TODO Verif: Order Element Type :  Prototyp - Erstmuster - Nullserie - Serie + Bezug +Bemerkung [Type Position]
						Typ = itemData.ItemTypeId, // (int)Enums.OrderEnums.Types.Confirmation, // <<<  
						AngebotNr = itemData.OrderId,
						Position = itemData.Position,
						Wunschtermin = itemData.DesiredDate,
						Anzahl = itemData.OpenQuantity_Quantity,
						Abladestelle = itemData.UnloadingPoint,
						Bezeichnung2_Kunde = itemData.ItemCustomerDescription,
						OriginalAnzahl = itemData.OpenQuantity_Quantity,
						Freies_Format_EDI = itemData.FreeText,

						EDI_PREIS_KUNDE = itemData.CurrentItemPriceCalculationNet,
						EDI_PREISEINHEIT = itemData.UnitPriceBasis,
						EDI_Quantity_Ordered = itemData.OpenQuantity_Quantity,
						EDI_Historie_Nr = null,

						LieferanweisungP_FTXDIN_TEXT = "",
						Bemerkungsfeld1 = itemData.FreeText,
						Bemerkungsfeld2 = itemData.FreeText,

						Bezeichnung1 = itemData.Designation1,
						Bezeichnung2 = itemData.Designation2,
						Bezeichnung3 = itemData.Designation3,
						Einheit = itemData.MeasureUnitQualifier,
						ArtikelNr = itemData.ItemId,

						Kupferbasis = 150,

						Preiseinheit = itemData.UnitPriceBasis == 0 ? 1 : itemData.UnitPriceBasis,// - 2022-05-30 - init to 1 to respect DB Constraint
						DELFixiert = itemData.DelFixed,
						DEL = itemData.DelNote,
						EinzelCuGewicht = itemData.CopperWeight,
						VKFestpreis = itemData.FixedUnitPrice,
						USt = itemData.VAT,
						Einzelkupferzuschlag = itemData.CopperSurcharge,
						GesamtCuGewicht = itemData.OpenQuantity_CopperWeight,
						Einzelpreis = itemData.OpenQuantity_UnitPrice,
						VKEinzelpreis = itemData.UnitPrice,
						Gesamtpreis = itemData.OriginalOrderAmount,
						Gesamtkupferzuschlag = itemData.OpenQuantity_CopperSurcharge,
						VKGesamtpreis = itemData.TotalPrice,
						erledigt_pos = false,
						// Fertigungsnummer = itemData.f // > ReferencedDocument, ignored for now

						Geliefert = 0, // << compatibility with psz soft
						Rabatt = 0, // << compatibility with psz soft
						Index_Kunde = itemData.Index_Kunde,
						Index_Kunde_Datum = itemData.Index_Kunde_Datum,
						CSInterneBemerkung = itemData.CSInterneBemerkung,
						POSTEXT = itemData.Postext,
					};

					var insertedId = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Insert(orderElementDb);

					var originalData = data.Items.Find(e => e.Id == itemData.Id);
					if(originalData != null)
					{
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity
						{
							Id = -1,

							OrderId = data.OrderId,
							OrderItemId = insertedId,
							Status = (int)Enums.OrderElementEnums.OrderElementStatus.Original,
							OriginalQuantity = originalData.OrderedQuantity,
							OriginalGesamtpreis = originalData.LineItemAmount,
							OriginalVKGesamtpreis = -1,
							DesiredDate = itemData.DesiredDate,
							CreationDate = DateTime.Now,
							CreationUserId = (user?.Id ?? -1),

							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = (user?.Id ?? -1),
							LastUpdateUsername = (user?.Username ?? "-"),
							Version = 0,
						});
					}

					// > ---EDI---EDI---EDI---EDI---EDI---
					// OrderElementExtension.UpdateStatus(insertedId);

					if(itemData.Consignee != null)
					{
						var consigneeItemDb = new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity
						{
							Id = -1,
							OrderId = itemData.OrderId,
							DUNS = itemData.Consignee?.ConsigneeDUNS,
							City = itemData.Consignee?.ConsigneeCity,
							ContactFax = itemData.Consignee?.ConsigneeContactFax,
							ContactName = itemData.Consignee?.ConsigneeContactName,
							ContatTelephone = itemData.Consignee?.ConsigneeContactTelephone,
							CountryName = itemData.Consignee?.ConsigneeCountryName,
							Name = itemData.Consignee?.ConsigneeName,
							Name2 = itemData.Consignee?.ConsigneeName2,
							Name3 = itemData.Consignee?.ConsigneeName3,
							PartyIdentificationCodeListQualifier = itemData.Consignee?.ConsigneeIdentificationCodeListQualifier,
							PostalCode = itemData.Consignee?.ConsigneePostalCode,
							PurchasingDepartment = itemData.Consignee?.ConsigneePurchasingDepartment,
							StorageLocation = itemData.Consignee?.ConsigneeStorageLocation,
							Street = itemData.Consignee?.ConsigneeStreet,
							UnloadingPoint = itemData.Consignee?.ConsigneeUnloadingPoint,
							OrderType = (int)Enums.OrderEnums.EdiDocumentTypes.Order,
							OrderElementId = insertedId
						};

						Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.Insert(consigneeItemDb);
					}
					//logging
					var InsertedItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(insertedId);
					var Order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get((int)InsertedItem.AngebotNr);
					var _log = new LogHelper(Order.Nr, (int)Order.Angebot_Nr, int.TryParse(Order.Projekt_Nr, out var val) ? val : 0, Order.Typ, LogHelper.LogType.CREATIONPOS, "CTS", user)
						.LogCTS(null, null, null, (int)InsertedItem.Position, InsertedItem.Nr);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

					insertedIds.Add(insertedId);
				}

				return Core.Models.ResponseModel<List<int>>.SuccessResponse(insertedIds);
			}
		}
	}
}
