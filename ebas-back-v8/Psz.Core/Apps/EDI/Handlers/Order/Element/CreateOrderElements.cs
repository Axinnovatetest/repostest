using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public partial class Element
		{
			public static Core.Models.ResponseModel<List<int>> CreateOrderElements(Models.Order.Element.NotCalculatedOrderElementsModel data,
				Core.Identity.Models.UserModel user)
			{
				lock(Locks.OrdersLock)
				{
					try
					{
						if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
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

						if(orderDb.Neu_Order == false)
						{
							return new Core.Models.ResponseModel<List<int>>()
							{
								Errors = new List<string>() { "Order is validated" }
							};
						}
						var adressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderDb.Kunden_Nr ?? -1);
						return CreateOrderElementsInternal(data, adressDb.Kundennummer ?? -1, user);
					} catch(Exception e)
					{
						Infrastructure.Services.Logging.Logger.Log(e);
						throw;
					}
				}
			}

			internal static Core.Models.ResponseModel<List<int>> CreateOrderElementsInternal(Models.Order.Element.NotCalculatedOrderElementsModel data, int customerNumber,
				Core.Identity.Models.UserModel user)
			{
				var calculatedElementsResponse = CalculateElementsData(data, customerNumber);

				if(!calculatedElementsResponse.Success)
				{
					return new Core.Models.ResponseModel<List<int>>(null)
					{
						Errors = calculatedElementsResponse.Errors
					};
				}

				var insertedIds = new List<int>();
				var orderId = -1;
				var primaryPositionNr = -1;
				var primaryPosition = -1;
				var primaryPositionArt = -1;
				var PLACEHOLDER = -9999;
				var addressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(customerNumber);
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(addressDb?.Nr ?? -1);

				foreach(var itemData in calculatedElementsResponse.Body)
				{
					var orderItemDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity()
					{
						Typ = itemData.ItemTypeId,
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

						Bezeichnung1 = itemData.CustomerItemNumber,
						Bezeichnung2 = itemData.ItemCustomerDescription,
						Bezeichnung3 = "-",
						Einheit = itemData.MeasureUnitQualifier,
						ArtikelNr = itemData.ItemId,

						Kupferbasis = 150,

						Preiseinheit = itemData.UnitPriceBasis == 0 ? 1 : itemData.UnitPriceBasis, // - 2022-05-30 - init to 1 to respect DB Constraint
						DELFixiert = itemData.DelFixed,
						DEL = itemData.DelNote,
						EinzelCuGewicht = itemData.CopperWeight,
						VKFestpreis = itemData.FixedUnitPrice,
						USt = customerDb.Umsatzsteuer_berechnen.HasValue && customerDb.Umsatzsteuer_berechnen.Value ? 0.19m : 0m, // - 2023-07-05 - itemData.VAT,
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

						PositionZUEDI = itemData.Id == 0 ? (int?)null : PLACEHOLDER, // <<< will be updated at the end of the loop

						// - Ridha 2021-07-01
						Fertigungsnummer = 0,

						// - 2022-03-11 tracking Lager.Bestand 
						Index_Kunde = itemData.Index_Kunde,
						Index_Kunde_Datum = itemData.Index_Kunde_Datum,
						CSInterneBemerkung = itemData.CSInterneBemerkung,
					};

					var insertedId = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Insert(orderItemDb);
					if(itemData.Id == 0) // Primary prosition
					{
						// Update secondary positions before going to next primary position
						if(primaryPositionNr != -1)
						{
							// >>>> Logging
							Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace,
								$">>> Updating values >>> orderId:{orderId}, primaryPosition:{primaryPosition}, primaryPositionArt:{primaryPositionArt}, primaryPositionNr:{primaryPositionNr}, PLACEHOLDER:{PLACEHOLDER}");

							// Update secondary positions
							Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdatePrimaryPosition(orderId, primaryPosition, primaryPositionArt, primaryPositionNr, PLACEHOLDER);
						}

						orderId = itemData.OrderId;
						primaryPosition = itemData.Position;
						primaryPositionArt = itemData.ItemId;
						primaryPositionNr = insertedId;

						// >>>> Logging
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $">>> Primary Position >>> {JsonConvert.SerializeObject(itemData)}");
					}

					var originalData = data.Elements.Find(e => e.Id == itemData.Id && e.PositionNumber == itemData.Position);
					if(originalData != null)
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"{JsonConvert.SerializeObject(itemData)}");
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"{JsonConvert.SerializeObject(originalData)}");

						var orderItemExtensionDb = new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity
						{
							Id = -1,

							OrderId = data.OrderId,
							OrderItemId = insertedId,
							Status = (int)Enums.OrderElementEnums.OrderElementStatus.Original,
							OriginalQuantity = originalData.OrderedQuantity,
							OriginalGesamtpreis = originalData.LineItemAmount, // <<<< Ignore this when Position is Secondary
							OriginalVKGesamtpreis = -1, //// <<<<
							DesiredDate = originalData.DesiredDate,
							CreationDate = DateTime.Now,
							CreationUserId = (user?.Id ?? -1),
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = (user?.Id ?? -1),
							LastUpdateUsername = user?.Username ?? "-",
							Version = 0,
							PrimaryPositionNumber = itemData.Id == 0 ? (int?)null : itemData.Position
						};
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.Insert(orderItemExtensionDb);
					}

					OrderElementExtension.SetStatus(insertedId);

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
							OrderType = (int)Enums.OrderEnums.OrderTypes.Order,
							OrderElementId = insertedId,
						};

						Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.Insert(consigneeItemDb);
					}

					insertedIds.Add(insertedId);
				}

				// Update LAST secondary positions
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdatePrimaryPosition(orderId, primaryPosition, primaryPositionArt, primaryPositionNr, PLACEHOLDER);



				return Core.Models.ResponseModel<List<int>>.SuccessResponse(insertedIds);
			}

			internal static Core.Models.ResponseModel<List<int>> CreateOrderElementsInternal(Models.Order.Element.NotCalculatedOrderElementsModel data, int customerNumber,
				Core.Identity.Models.UserModel user, Infrastructure.Services.Utils.TransactionsManager botransaction)
			{
				var calculatedElementsResponse = CalculateElementsData(data, customerNumber);

				if(!calculatedElementsResponse.Success)
				{
					return new Core.Models.ResponseModel<List<int>>(null)
					{
						Errors = calculatedElementsResponse.Errors
					};
				}

				var insertedIds = new List<int>();
				var orderId = -1;
				var primaryPositionNr = -1;
				var primaryPosition = -1;
				var primaryPositionArt = -1;
				var PLACEHOLDER = -9999;
				var addressDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(customerNumber);
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(addressDb?.Nr ?? -1);

				foreach(var itemData in calculatedElementsResponse.Body)
				{
					var orderItemDb = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity()
					{
						Typ = itemData.ItemTypeId,
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

						Bezeichnung1 = itemData.CustomerItemNumber,
						Bezeichnung2 = itemData.ItemCustomerDescription,
						Bezeichnung3 = "-",
						Einheit = itemData.MeasureUnitQualifier,
						ArtikelNr = itemData.ItemId,

						Kupferbasis = 150,

						Preiseinheit = itemData.UnitPriceBasis == 0 ? 1 : itemData.UnitPriceBasis, // - 2022-05-30 - init to 1 to respect DB Constraint
						DELFixiert = itemData.DelFixed,
						DEL = itemData.DelNote,
						EinzelCuGewicht = itemData.CopperWeight,
						VKFestpreis = itemData.FixedUnitPrice,
						USt = customerDb.Umsatzsteuer_berechnen.HasValue && customerDb.Umsatzsteuer_berechnen.Value ? 0.19m : 0m, // - 2023-07-05 - itemData.VAT,
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

						PositionZUEDI = itemData.Id == 0 ? (int?)null : PLACEHOLDER, // <<< will be updated at the end of the loop

						// - Ridha 2021-07-01
						Fertigungsnummer = 0,

						// - 2022-03-11 tracking Lager.Bestand 
						Index_Kunde = itemData.Index_Kunde,
						Index_Kunde_Datum = itemData.Index_Kunde_Datum,
						CSInterneBemerkung = itemData.CSInterneBemerkung,
					};

					var insertedId = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.InsertWithTransaction(orderItemDb, botransaction.connection, botransaction.transaction);
					if(itemData.Id == 0) // Primary prosition
					{
						// Update secondary positions before going to next primary position
						if(primaryPositionNr != -1)
						{
							// >>>> Logging
							Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace,
								$">>> Updating values >>> orderId:{orderId}, primaryPosition:{primaryPosition}, primaryPositionArt:{primaryPositionArt}, primaryPositionNr:{primaryPositionNr}, PLACEHOLDER:{PLACEHOLDER}");

							// Update secondary positions
							Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdatePrimaryPosition(orderId, primaryPosition, primaryPositionArt, primaryPositionNr, PLACEHOLDER, botransaction.connection, botransaction.transaction);
						}

						orderId = itemData.OrderId;
						primaryPosition = itemData.Position;
						primaryPositionArt = itemData.ItemId;
						primaryPositionNr = insertedId;

						// >>>> Logging
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $">>> Primary Position >>> {JsonConvert.SerializeObject(itemData)}");
					}

					var originalData = data.Elements.Find(e => e.Id == itemData.Id && e.PositionNumber == itemData.Position);
					if(originalData != null)
					{
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"{JsonConvert.SerializeObject(itemData)}");
						Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"{JsonConvert.SerializeObject(originalData)}");

						var orderItemExtensionDb = new Infrastructure.Data.Entities.Tables.PRS.OrderItemExtensionEntity
						{
							Id = -1,

							OrderId = data.OrderId,
							OrderItemId = insertedId,
							Status = (int)Enums.OrderElementEnums.OrderElementStatus.Original,
							OriginalQuantity = originalData.OrderedQuantity,
							OriginalGesamtpreis = originalData.LineItemAmount, // <<<< Ignore this when Position is Secondary
							OriginalVKGesamtpreis = -1, //// <<<<
							DesiredDate = originalData.DesiredDate,
							CreationDate = DateTime.Now,
							CreationUserId = (user?.Id ?? -1),
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = (user?.Id ?? -1),
							LastUpdateUsername = user?.Username ?? "-",
							Version = 0,
							PrimaryPositionNumber = itemData.Id == 0 ? (int?)null : itemData.Position
						};
						Infrastructure.Data.Access.Tables.PRS.OrderItemExtensionAccess.InsertWithTransaction(orderItemExtensionDb, botransaction.connection, botransaction.transaction);
					}

					OrderElementExtension.SetStatus(insertedId, botransaction);

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
							OrderType = (int)Enums.OrderEnums.OrderTypes.Order,
							OrderElementId = insertedId,
						};

						Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.Insert(consigneeItemDb, botransaction.connection, botransaction.transaction);
					}

					insertedIds.Add(insertedId);
				}

				// Update LAST secondary positions
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdatePrimaryPosition(orderId, primaryPosition, primaryPositionArt, primaryPositionNr, PLACEHOLDER, botransaction.connection, botransaction.transaction);



				return Core.Models.ResponseModel<List<int>>.SuccessResponse(insertedIds);
			}
		}
	}
}
