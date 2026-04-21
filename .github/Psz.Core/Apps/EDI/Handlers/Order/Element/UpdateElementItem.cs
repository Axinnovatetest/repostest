using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> UpdateElementItem(Models.Order.UpdateElementItemModel data,
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
					//var technicArticles = Program.BSD.TechnicArticleIds;
					if(orderElementDb.ArtikelNr != itemDb.ArtikelNr && !HorizonsHelper.ArticleIsTechnic(itemDb.ArtikelNr))
					{
						DateTime _newDate, _oldDate;
						_newDate = _oldDate = orderElementDb.Liefertermin ?? orderElementDb.Wunschtermin ?? new DateTime(1900, 1, 1);
						var horizonCheck = false;
						var messages = new List<string>();
						horizonCheck = orderDb.Typ == Psz.Core.Apps.Purchase.Enums.OrderEnums.TypeToData(Purchase.Enums.OrderEnums.Types.Confirmation)
							? HorizonsHelper.userHasABPosHorizonRight(_newDate, _oldDate, user, out messages)
							: HorizonsHelper.userHasFRCPosHorizonRight(_newDate, _oldDate, user, out messages);
						if(!horizonCheck)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = messages
							};
						}
					}

					var errors = new List<string>();
					if(errors.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = errors
						};
					}
					// - 2022-12-15 - if Article has changed - update!
					#region change Article ?
					if(orderElementDb.ArtikelNr != itemDb.ArtikelNr)
					{
						orderElementDb.DELFixiert = itemDb.DELFixiert;
						orderElementDb.DEL = itemDb.DEL;
						orderElementDb.Einheit = itemDb.Einheit;
					}
					// -
					#endregion

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
					var kupferbasis = 150;

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
						ordredQuantity,
						singleCopperSurcharge);

					var vkUnitPrice = Helpers.CalculationHelper.CalculateVkUnitPrice(orderElementDb.Typ == (int)CustomerService.Enums.OrderEnums.ItemType.Serie, fixedPrice,
						verkaufspreis,
						ordredQuantity,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					var unitPrice = Helpers.CalculationHelper.CalculateUnitPrice(orderElementDb.Typ == (int)CustomerService.Enums.OrderEnums.ItemType.Serie, fixedPrice,
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
					var articleProductionExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(itemDb.ArtikelNr);
					var articleProductionPlace = articleProductionExtensionEntity?.ProductionPlace1_Id;
					var storageLocation = new KeyValuePair<int, string>();
					if(articleProductionPlace != null)
					{
						storageLocation = Enums.OrderEnums.GetArticleHauplager((Enums.OrderEnums.ArticleProductionPlace)articleProductionPlace);
					}
					//orderElementDb.ArtikelNr = itemDb.ArtikelNr;
					orderElementDb.VKFestpreis = fixedPrice;
					orderElementDb.Einzelkupferzuschlag = singleCopperSurcharge;
					orderElementDb.GesamtCuGewicht = totalCuWeight;
					orderElementDb.Einzelpreis = unitPrice;
					orderElementDb.VKEinzelpreis = vkUnitPrice;
					orderElementDb.Gesamtpreis = totalPrice;
					orderElementDb.Gesamtkupferzuschlag = totalCopperSurcharge;
					orderElementDb.VKGesamtpreis = vKTotalPrice;
					orderElementDb.Index_Kunde = null;
					orderElementDb.Index_Kunde_Datum = null;
					orderElementDb.Lagerort_id = storageLocation.Key;
					// - 2022-05-16 Bz
					orderElementDb.Bezeichnung1 = itemDb.Bezeichnung1;
					orderElementDb.Bezeichnung2 = itemDb.Bezeichnung2;
					// - 
					// - 2022-12-15 - if Article has changed - update!
					#region change Article ?
					if(orderElementDb.ArtikelNr != itemDb.ArtikelNr)
					{
						orderElementDb.Bezeichnung1 = itemDb.Bezeichnung1;
						orderElementDb.Bezeichnung2 = itemDb.Bezeichnung2;
						orderElementDb.Bezeichnung3 = itemDb.Bezeichnung3;
						// - 
						orderElementDb.Index_Kunde = itemDb.Index_Kunde;
						orderElementDb.Index_Kunde_Datum = itemDb.Index_Kunde_Datum;
						// -
						orderElementDb.DELFixiert = itemDb.DELFixiert;
						orderElementDb.DEL = itemDb.DEL;
						orderElementDb.Einheit = itemDb.Einheit;
					}
					orderElementDb.ArtikelNr = itemDb.ArtikelNr;
					// -
					#endregion

					var Order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderElementDb.AngebotNr ?? -1);
					var OldorderElementDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(orderElementDb);
					//logging
					var OlditemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(OldorderElementDb.ArtikelNr ?? -1);
					var _log = new LogHelper(Order.Nr, (int)Order.Angebot_Nr, int.TryParse(Order.Projekt_Nr, out var val) ? val : 0, Order.Typ, LogHelper.LogType.MODIFICATIONPOS, "CTS", user)
						.LogCTS("Artikel", OlditemDb.ArtikelNummer, itemDb.ArtikelNummer, (int)OldorderElementDb.Position, OldorderElementDb.Nr);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
					OrderElementExtension.SetStatus(orderElementDb.Nr);

					return Core.Models.ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
		public static Core.Models.ResponseModel<object> UpdateElementItem(Models.Order.UpdateElementItemModel data,
			Core.Identity.Models.UserModel user, Infrastructure.Services.Utils.TransactionsManager botransaction)
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
					//var technicArticles = Program.BSD.TechnicArticleIds;
					if(orderElementDb.ArtikelNr != itemDb.ArtikelNr && !HorizonsHelper.ArticleIsTechnic(itemDb.ArtikelNr))
					{
						DateTime _newDate, _oldDate;
						_newDate = _oldDate = orderElementDb.Liefertermin ?? orderElementDb.Wunschtermin ?? new DateTime(1900, 1, 1);
						var horizonCheck = false;
						var messages = new List<string>();
						horizonCheck = orderDb.Typ == Psz.Core.Apps.Purchase.Enums.OrderEnums.TypeToData(Purchase.Enums.OrderEnums.Types.Confirmation)
							? HorizonsHelper.userHasABPosHorizonRight(_newDate, _oldDate, user, out messages)
							: HorizonsHelper.userHasFRCPosHorizonRight(_newDate, _oldDate, user, out messages);
						if(!horizonCheck)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = messages
							};
						}
					}

					var errors = new List<string>();
					if(errors.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = errors
						};
					}
					// - 2022-12-15 - if Article has changed - update!
					#region change Article ?
					if(orderElementDb.ArtikelNr != itemDb.ArtikelNr)
					{
						orderElementDb.DELFixiert = itemDb.DELFixiert;
						orderElementDb.DEL = itemDb.DEL;
						orderElementDb.Einheit = itemDb.Einheit;
					}
					// -
					#endregion

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
					var kupferbasis = 150;

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
						ordredQuantity,
						singleCopperSurcharge);

					var vkUnitPrice = Helpers.CalculationHelper.CalculateVkUnitPrice(orderElementDb.Typ == (int)CustomerService.Enums.OrderEnums.ItemType.Serie, fixedPrice,
						verkaufspreis,
						ordredQuantity,
						me1,
						me2,
						me3,
						me4,
						pm2,
						pm3,
						pm4);

					var unitPrice = Helpers.CalculationHelper.CalculateUnitPrice(orderElementDb.Typ == (int)CustomerService.Enums.OrderEnums.ItemType.Serie, fixedPrice,
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
					var articleProductionExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(itemDb.ArtikelNr);
					var articleProductionPlace = articleProductionExtensionEntity?.ProductionPlace1_Id;
					var storageLocation = new KeyValuePair<int, string>();
					if(articleProductionPlace != null)
					{
						storageLocation = Enums.OrderEnums.GetArticleHauplager((Enums.OrderEnums.ArticleProductionPlace)articleProductionPlace);
					}
					//orderElementDb.ArtikelNr = itemDb.ArtikelNr;
					orderElementDb.VKFestpreis = fixedPrice;
					orderElementDb.Einzelkupferzuschlag = singleCopperSurcharge;
					orderElementDb.GesamtCuGewicht = totalCuWeight;
					orderElementDb.Einzelpreis = unitPrice;
					orderElementDb.VKEinzelpreis = vkUnitPrice;
					orderElementDb.Gesamtpreis = totalPrice;
					orderElementDb.Gesamtkupferzuschlag = totalCopperSurcharge;
					orderElementDb.VKGesamtpreis = vKTotalPrice;
					orderElementDb.Index_Kunde = null;
					orderElementDb.Index_Kunde_Datum = null;
					orderElementDb.Lagerort_id = storageLocation.Key;
					// - 2022-05-16 Bz
					orderElementDb.Bezeichnung1 = itemDb.Bezeichnung1;
					orderElementDb.Bezeichnung2 = itemDb.Bezeichnung2;
					// - 
					// - 2022-12-15 - if Article has changed - update!
					#region change Article ?
					if(orderElementDb.ArtikelNr != itemDb.ArtikelNr)
					{
						orderElementDb.Bezeichnung1 = itemDb.Bezeichnung1;
						orderElementDb.Bezeichnung2 = itemDb.Bezeichnung2;
						orderElementDb.Bezeichnung3 = itemDb.Bezeichnung3;
						// - 
						orderElementDb.Index_Kunde = itemDb.Index_Kunde;
						orderElementDb.Index_Kunde_Datum = itemDb.Index_Kunde_Datum;
						// -
						orderElementDb.DELFixiert = itemDb.DELFixiert;
						orderElementDb.DEL = itemDb.DEL;
						orderElementDb.Einheit = itemDb.Einheit;
					}
					orderElementDb.ArtikelNr = itemDb.ArtikelNr;
					// -
					#endregion

					var Order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderElementDb.AngebotNr ?? -1);
					var OldorderElementDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(orderElementDb, botransaction.connection, botransaction.transaction);
					//logging
					var OlditemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(OldorderElementDb.ArtikelNr ?? -1);
					var _log = new LogHelper(Order.Nr, (int)Order.Angebot_Nr, int.TryParse(Order.Projekt_Nr, out var val) ? val : 0, Order.Typ, LogHelper.LogType.MODIFICATIONPOS, "CTS", user)
						.LogCTS("Artikel", OlditemDb.ArtikelNummer, itemDb.ArtikelNummer, (int)OldorderElementDb.Position, OldorderElementDb.Nr);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
					OrderElementExtension.SetStatus(orderElementDb.Nr, botransaction);

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
