using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<Models.Order.OrderModel> Get(int id,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				var order = Get(id, true);
				if(order == null)
				{
					throw new Core.Exceptions.NotFoundException();
				}

				return Core.Models.ResponseModel<Models.Order.OrderModel>.SuccessResponse(order);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<List<Models.Order.OrderModel>> Get(bool? validated,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				return Core.Models.ResponseModel<List<Models.Order.OrderModel>>.SuccessResponse(Get(true, validated));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static Models.Order.OrderModel Get(int id,
			bool includeChanges)
		{
			try
			{
				return Get(new List<int>() { id }, includeChanges).FirstOrDefault();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Order.OrderModel> Get(bool includeChanges,
			bool? validated = null)
		{
			try
			{
				if(!validated.HasValue)
				{
					return Get(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(), includeChanges);
				}
				else if(validated.Value)
				{
					return Get(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrder(false), includeChanges);
				}
				else
				{
					return Get(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrder(true), includeChanges);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Order.OrderModel> Get(List<int> ids,
			bool includeChanges)
		{
			try
			{
				var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(ids);

				return Get(ordersDb, includeChanges);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Order.OrderModel> Get(List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> ordersDb, bool includeChanges, bool includeActions = true)
		{
			try
			{
				var response = new List<Models.Order.OrderModel>();

				var customersNummers = ordersDb
					.Where(e => e.Kunden_Nr.HasValue)
					.Select(e => e.Kunden_Nr.Value)
					.ToList();

				var ordersIds = ordersDb.Select(e => e.Nr).ToList();
				var ordersExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrdersIds(ordersIds);
				var BlanketsExtensionsDb = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNrs(ordersIds);

				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNummers);
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNummers);

				foreach(var orderDb in ordersDb)
				{
					var customerDb = orderDb.Kunden_Nr.HasValue
						? customersDb.Find(e => e.Nummer == orderDb.Kunden_Nr.Value)
						: null;
					var adressDb = orderDb.Kunden_Nr.HasValue
						? adressesDb.Find(e => e.Nr == orderDb.Kunden_Nr.Value)
						: null;
					var orderExtensionDb = ordersExtensionsDb?.Find(e => e.OrderId == orderDb.Nr);
					var BlanketExtensionsDb = BlanketsExtensionsDb?.Find(e => e.AngeboteNr == orderDb.Nr);
					var rahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(orderDb.Nr);
					//Deflor
					int? dlfHeader = null;
					int? dlfItem = null;
					if(orderDb.nr_dlf.HasValue && orderDb.nr_dlf.Value != -1 && orderDb.nr_dlf.Value != 0)
					{

						var dlfLineItemPlan = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(orderDb.nr_dlf ?? -1);
						if(dlfLineItemPlan != null)
						{
							var dlfLineItem = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(dlfLineItemPlan?.LineItemId ?? -1);
							var dlfHeaderentity = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(dlfLineItem?.HeaderId ?? -1);
							dlfHeader = Convert.ToInt32(dlfHeaderentity?.Id ?? -1);
							dlfItem = Convert.ToInt32(dlfLineItem?.Id ?? -1);
						}

					}
					//rechnung
					System.DateTime? rechSentTime = null;
					if(orderDb.Typ == Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Invoice))
					{
						var rechAuto = Infrastructure.Data.Access.Tables.CTS.__E_rechnung_CreatedAccess.GetByRechnungNr(orderDb.Nr);
						if(rechAuto != null)
							rechSentTime = rechAuto.SentTime;
					}

					var order = new Models.Order.OrderModel()
					{
						Id = orderDb.Nr,
						CustomerId = customerDb?.Nr,
						CustomerNumber = orderDb.Kunden_Nr, // customerDb?.Nummer
						AdressCustomerNumber = adressDb?.Kundennummer, // adressDb.Kunden_Nr,

						AbId = (orderDb.Ab_id == orderDb.Nr) ? -1 : orderDb.Ab_id,
						abGebucht = (orderDb.Ab_id == orderDb.Nr) ? null : Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(orderDb.Ab_id ?? -1)?.Gebucht,
						Belegkreis = orderDb.Belegkreis,
						Conditions = orderDb.Konditionen,
						Contact = orderDb.Ansprechpartner,
						CountryPostcode = orderDb.Land_PLZ_Ort,
						Department = orderDb.Abteilung,
						ProjectNumber = orderDb.Projekt_Nr,
						Done = orderDb.Erledigt,
						Booked = orderDb.Gebucht,


						Date = orderDb.Datum,
						DesiredDate = orderDb.Wunschtermin,
						DueDate = orderDb.Falligkeit,
						DeliveryDate = orderDb.Liefertermin,
						ShippingDate = orderDb.Versanddatum_Auswahl,

						Freetext = orderDb.Freitext,
						Freetext2 = orderDb.Freie_Text,
						Name = orderDb.Vorname_NameFirma,
						Name2 = orderDb.Name2,
						Name3 = orderDb.Name3,
						New = orderDb.Neu,
						NewOrder = orderDb.Neu_Order ?? false,
						NrAuf = orderDb.Nr_auf,
						NrBv = orderDb.Nr_BV,
						NrGut = orderDb.Nr_gut,
						NrKanban = orderDb.Nr_Kanban,
						NrLie = orderDb.Nr_lie,
						NrPro = orderDb.Nr_pro,
						NrRa = orderDb.Nr_RA,
						NrRec = orderDb.Nr_rec,
						NrSto = orderDb.Nr_sto,
						OrderTitle = orderDb.LBriefanrede,
						Payment = orderDb.Zahlungsweise,
						PersonalNumber = orderDb.Personal_Nr,
						RepairNumber = orderDb.Reparatur_nr,
						Shipping = orderDb.Versandart,
						ShippingAddress = orderDb.Lieferadresse,
						StreetPOBox = orderDb.Straße_Postfach,
						SuppliedNumber = orderDb.Ihr_Zeichen,
						Type = orderDb.Typ,
						Vat = orderDb.USt_Berechnen ?? false,

						LClientName = orderDb.LVorname_NameFirma,
						LContact = orderDb.LAnsprechpartner,
						LCountryPostcode = orderDb.LLand_PLZ_Ort,
						LCountryZIPLocation = orderDb.LLand_PLZ_Ort, // << same as above ??
						LDepartment = orderDb.LAbteilung,
						LName = orderDb.LVorname_NameFirma,
						LName2 = orderDb.LName2,
						LName3 = orderDb.LName3,
						LOrderTitle = orderDb.LBriefanrede,
						LStreetMailbox = orderDb.LStraße_Postfach,
						LStreetPOBox = orderDb.LStraße_Postfach,
						LType = orderDb.LAnrede,
						UnloadingPoint = orderDb.UnloadingPoint,
						StorageLocation = orderDb.StorageLocation,

						Version = (orderExtensionDb?.Version ?? 0),
						//ValidationTime = orderExtensionDb?.ValidationTime,
						//ValidationUserId = orderExtensionDb?.ValidationUserId,
						//ValidationUser = orderExtensionDb != null
						//    ? validationUsersDb.Find(e => e.Id == orderExtensionDb.ValidationUserId)?.Name
						//    : null,

						VorfailNr = orderDb.Angebot_Nr.HasValue ? orderDb.Angebot_Nr.ToString() : string.Empty,

						DocumentNumber = orderDb.Bezug,
						IsManualCreation = !orderDb.Neu_Order.HasValue && string.IsNullOrWhiteSpace(orderDb.EDI_Dateiname_CSV),
						// - 2022-11-30 - allow ignore CanArchive & CanDelete checks - perf!!!!
						CanArchive = includeActions == false ? false : ((!orderDb.Angebot_Nr.HasValue || orderDb.Angebot_Nr.HasValue && orderDb.Angebot_Nr.Value <= 0)
							? true
							: Helpers.ItemHelper.CanArchiveOrderByAngebote(orderDb.Angebot_Nr)),
						CanDelete = includeActions == false ? false : ((string.IsNullOrEmpty(orderDb.Projekt_Nr) || string.IsNullOrWhiteSpace(orderDb.Projekt_Nr))
							? true
							: Helpers.ItemHelper.CanDeleteOrder(orderDb.Nr, orderDb.Angebot_Nr ?? -1, out var msg)),

						// > Consignee and Buyer
						//Buyer = new Models.Order.OrderModel.BuyerModel(orderExtBuyerDb),
						//Consignee = new Models.Order.ConsigneeModel(orderExtConsigneeDb),

						Changes = new Models.Order.OrderModel.ChangesModel(),
						RahmenCustomer = rahmenExtensionEntity?.Warenemfanger,
						RahmenSupplier = rahmenExtensionEntity?.Auftraggeber,
						RahmenType = rahmenExtensionEntity?.BlanketTypeId,
						RechnungType = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.GetByKundennummer(orderDb.Kunden_Nr ?? -1)?.Typ,
						Nr_DlfHeader = dlfHeader,
						Nr_DlfItem = dlfItem,
						RechnungSent = orderDb.rec_sent ?? false,
						RechnungSentDate = rechSentTime,

						// - 2023-01-18
						UstIdCustomer = $"USt-ID-Nr.: {customerDb?.EG___Identifikationsnummer}",
						//- 26-03-2025 change for fix ticket #43561
						LsAddressNr = orderDb.LsAddressNr ?? -1,

					};

					#region > Changes
					//// FIXME: Remove this!!!
					//// includeChanges = false;
					///
					//if (includeChanges)
					//{
					//    #region > Global
					//    var latestGlobalChangeDb = ordersChangesDb.FindAll(e => e.OrderId == orderDb.Nr && e.GlobalStatus == globalChangePendingStatus)
					//        .OrderBy(e => e.CreationTime)
					//        .LastOrDefault();
					//    if (latestGlobalChangeDb != null)
					//    {
					//        order.Changes.GlobalChangeId = latestGlobalChangeDb.Id;
					//        if (latestGlobalChangeDb.Notes != order.Freetext)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "Freetext",
					//                Value = latestGlobalChangeDb.Notes
					//            });
					//        }
					//        if (latestGlobalChangeDb.Reference != order.DocumentNumber)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "DocumentNumber",
					//                Value = latestGlobalChangeDb.Reference
					//            });
					//        }

					//        if (latestGlobalChangeDb.CustomerContactName != order.Contact)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "Contact",
					//                Value = latestGlobalChangeDb.CustomerContactName
					//            });
					//        }
					//        if (latestGlobalChangeDb.CustomerPurchasingDepartment != order.Department)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "Department",
					//                Value = latestGlobalChangeDb.CustomerPurchasingDepartment
					//            });
					//        }
					//        if (latestGlobalChangeDb.CustomerStreetPostalCode != order.StreetPOBox)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "StreetPOBox",
					//                Value = latestGlobalChangeDb.CustomerStreetPostalCode
					//            });
					//        }
					//        if (latestGlobalChangeDb.CustomerStreetCityPostalCode != order.CountryPostcode)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "CountryPostcode",
					//                Value = latestGlobalChangeDb.CustomerStreetCityPostalCode
					//            });
					//        }

					//        if (latestGlobalChangeDb.ConsigneeContactName != order.LContact)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "LContact",
					//                Value = latestGlobalChangeDb.ConsigneeContactName
					//            });
					//        }
					//        if (latestGlobalChangeDb.ConsigneeName != order.LName)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "LName",
					//                Value = latestGlobalChangeDb.ConsigneeName
					//            });
					//        }
					//        if (latestGlobalChangeDb.ConsigneeName != order.LClientName)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "LClientName",
					//                Value = latestGlobalChangeDb.ConsigneeName
					//            });
					//        }
					//        if (latestGlobalChangeDb.ConsigneePurchasingDepartment != order.LDepartment)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "LDepartment",
					//                Value = latestGlobalChangeDb.ConsigneePurchasingDepartment
					//            });
					//        }
					//        if (latestGlobalChangeDb.ConsigneeStreetPostalCode != order.LStreetMailbox)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "LStreetMailbox",
					//                Value = latestGlobalChangeDb.ConsigneeStreetPostalCode
					//            });
					//        }
					//        if (latestGlobalChangeDb.ConsigneeStreetPostalCode != order.LStreetPOBox)
					//        {
					//            order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
					//            {
					//                Key = "LStreetPOBox",
					//                Value = latestGlobalChangeDb.ConsigneeStreetPostalCode
					//            });
					//        }
					//    }
					//    #endregion

					//    #region > Items
					//    var pendingChangesItemsDb = ordersChangesItemsDb.FindAll(e => e.OrderId == orderDb.Nr && e.Status == itemChangePendingStatus);

					//    order.Changes.NewItems = pendingChangesItemsDb.Count(e => e.Type == itemChangeTypeNew);
					//    order.Changes.CanceledItems = pendingChangesItemsDb.Count(e => e.Type == itemChangeTypeCanceled);
					//    order.Changes.ChangedItems = pendingChangesItemsDb
					//        .Where(e => e.Type == itemChangeTypeChanged)
					//        .DistinctBy(e => e.ItemNumber)
					//        .Count();
					//    #endregion
					//}
					#endregion

					response.Add(order);
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static Core.Models.ResponseModel<List<Models.Order.OrderModel>> GetByCustomer(int customerId,
			bool? validated = false,
			Core.Identity.Models.UserModel user = null)
		{
			try
			{
				return Core.Models.ResponseModel<List<Models.Order.OrderModel>>.SuccessResponse(GetByCustomer(customerId, validated));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Order.OrderModel> GetByCustomer(int customerId,
			bool? validated)
		{
			try
			{
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customerId);
				if(customerDb == null)
				{
					Infrastructure.Services.Logging.Logger.Log("customerDb == null");
					throw new Exception("Customer Not found");
				}

				if(!validated.HasValue)
				{
					var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get()
						.FindAll(a => a.Kunden_Nr == customerDb.Nummer)?.ToList();
					return Get(ordersDb, true);
				}
				else if(validated.Value)
				{
					var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrder(false)
						.FindAll(a => a.Kunden_Nr == customerDb.Nummer)?.ToList();
					return Get(ordersDb, true);
				}
				else
				{
					var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrder(true)
						.FindAll(a => a.Kunden_Nr == customerDb.Nummer)?.ToList();
					return Get(ordersDb, true);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		//public static Core.Models.ResponseModel<List<Models.Customers.CustomerOrdersCountModel>> GetCountByCustomers(List<int> customersNumbers,
		//    bool? validated = null, 
		//    Core.Identity.Models.UserModel user = null)
		//{
		//    try
		//    {
		//        if (!validated.HasValue)
		//        {
		//            return Core.Models.ResponseModel<List<Models.Customers.CustomerOrdersCountModel>>.SuccessResponse(GetCountByCustomers(customersNumbers));
		//        }
		//        else if (validated.Value)
		//        {
		//            return Core.Models.ResponseModel<List<Models.Customers.CustomerOrdersCountModel>>.SuccessResponse(GetValidatedCountByCustomers(customersNumbers));
		//        }
		//        else
		//        {
		//            return Core.Models.ResponseModel<List<Models.Customers.CustomerOrdersCountModel>>.SuccessResponse(GetUnvalidatedCountByCustomers(customersNumbers));
		//        }
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//internal static List<Models.Customers.CustomerOrdersCountModel> GetCountByCustomers(List<int> customersNumbers)
		//{
		//    try
		//    {
		//        var customersDb = Infrastructure.Data.Access.Tables.EDI.KundenAccess.GetByNummers(customersNumbers);
		//        var ordersDb = Infrastructure.Data.Access.Tables.EDI.AngeboteAccess.Get()?.FindAll(a => customersNumbers.Exists(i => i == a.Kunden_Nr))?.ToList();

		//        return GetCountByCustomer(ordersDb, customersDb);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//internal static List<Models.Customers.CustomerOrdersCountModel> GetValidatedCountByCustomers(List<int> customersNumbers)
		//{
		//    try
		//    {
		//        var customersDb = Infrastructure.Data.Access.Tables.EDI.KundenAccess.GetByNummers(customersNumbers);
		//        var ordersDb = Infrastructure.Data.Access.Tables.EDI.AngeboteAccess.GetValidated()?.FindAll(a => customersNumbers.Exists(i => i == a.Kunden_Nr))?.ToList();

		//        return GetCountByCustomer(ordersDb, customersDb);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//internal static List<Models.Customers.CustomerOrdersCountModel> GetUnvalidatedCountByCustomers(List<int> customersNumbers)
		//{
		//    try
		//    {
		//        var customersDb = Infrastructure.Data.Access.Tables.EDI.KundenAccess.GetByNummers(customersNumbers);
		//        var ordersDb = Infrastructure.Data.Access.Tables.EDI.AngeboteAccess.GetUnvalidated()?.FindAll(a => customersNumbers.Exists(i => i == a.Kunden_Nr))?.ToList();

		//        return GetCountByCustomer(ordersDb, customersDb);
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
		//internal static List<Models.Customers.CustomerOrdersCountModel> GetCountByCustomer(List<Infrastructure.Data.Entities.Tables.EDI.AngeboteEntity> ordersDb, 
		//    List<Infrastructure.Data.Entities.Tables.EDI.KundenEntity> customersDb)
		//{
		//    try
		//    {
		//        var response = new List<Models.Customers.CustomerOrdersCountModel>();
		//        if (ordersDb == null)
		//            return response;

		//        var customersNumbers = customersDb
		//            .Where(e => e.Nummer.HasValue)
		//            .Select(e => e.Nummer.Value).ToList();
		//        var adressesDb = Infrastructure.Data.Access.Tables.EDI.AdressenAccess.Get(customersNumbers);

		//        List<Infrastructure.Data.Entities.Tables.EDI.AngeboteEntity> _orders = null;
		//        foreach (var customerDb in customersDb)
		//        {
		//            var adressDb = adressesDb.Find(e => e.Nr == customerDb.Nummer);
		//            _orders = ordersDb.FindAll(x => x.Kunden_Nr == customerDb.Nummer);

		//            response.Add(new Models.Customers.CustomerOrdersCountModel()
		//            {
		//                Id = customerDb.Nr,
		//                Type = adressDb?.Anrede,
		//                CustomerNumber = customerDb.Nummer ?? -1,

		//                Name = adressDb?.Name1,
		//                Name2 = adressDb?.Name2,
		//                Name3 = adressDb?.Name3,

		//                Contact = "",
		//                Department = customerDb.RG_Abteilung,
		//                CountryPostcode = customerDb.RG_Land_PLZ_ORT,
		//                StreetPOBox = customerDb.RG_Strasse_Postfach,

		//                Count = _orders == null ? 0 : _orders.Count
		//            });
		//        }

		//        return response;
		//    }
		//    catch (Exception e)
		//    {
		//        Infrastructure.Services.Logging.Logger.Log(e);
		//        throw e;
		//    }
		//}
	}
}
