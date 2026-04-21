using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
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
		internal static List<Models.Order.OrderModel> Get(bool includeChanges, bool? validated = null)
		{
			try
			{
				if(!validated.HasValue)
				{
					return Get(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTyp(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION), includeChanges);
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
			bool includeChanges, bool? validated = null)
		{
			try
			{
				var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(ids);

				return Get(ordersDb, includeChanges, validated);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Order.OrderModel> Get(IEnumerable<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> ordersDb,
			bool includeChanges, bool? validated = null)
		{
			try
			{
				if(!validated.HasValue)
				{
					ordersDb = ordersDb?.Where(e => e.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION);
				}
				else if(validated.Value)
				{
					ordersDb = ordersDb?.Where(e => e.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION && e.Neu_Order == false);
				}
				else
				{
					ordersDb = ordersDb?.Where(e => e.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION && e.Neu_Order == true);
				}


				var response = new List<Models.Order.OrderModel>();

				var customersNummers = ordersDb?.Where(e => e.Kunden_Nr.HasValue).Select(e => e.Kunden_Nr.Value).ToList();

				var ordersIds = ordersDb?.Select(e => e.Nr).ToList();
				var ordersExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrdersIds(ordersIds) ?? new List<Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity>();
				// - 2022-11-23 - discard Order w/o Extension (these are coming from P3000)
				// -- 2022-11-24 - allow view/edit but NOT validate

				var ordersExtConsigneesDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionConsigneeAccess.GetByOrdersType(ordersIds, (int)Enums.OrderEnums.OrderTypes.Undefined);
				var ordersExtBuyersDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionBuyerAccess.GetByOrdersType(ordersIds, (int)Enums.OrderEnums.OrderTypes.Undefined);

				var validationUsersIds = ordersExtensionsDb.Select(e => e.EdiValidationUserId).ToList();
				var validationUsersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validationUsersIds);
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNummers);
				//var adressesDb = Infrastructure.Data.Access.Tables.EDI.AdressenAccess.GetByKundenNummers(customersIds);

				var globalChangePendingStatus = (int)Enums.OrderEnums.GlobalOrderChangeStatus.Pending;
				var itemChangePendingStatus = (int)Enums.OrderEnums.OrderChangeItemStatus.Pending;
				var itemChangeTypeNew = (int)Enums.OrderEnums.OrderChangeItemTypes.New;
				var itemChangeTypeCanceled = (int)Enums.OrderEnums.OrderChangeItemTypes.Canceled;
				var itemChangeTypeChanged = (int)Enums.OrderEnums.OrderChangeItemTypes.Changed;
				var ordersChangesDb = includeChanges
					? Infrastructure.Data.Access.Tables.PRS.OrderChangeAccess.GetByOrdersIds(ordersIds)
					: null;
				var ordersChangesIds = ordersChangesDb?.Select(e => e.Id)?.ToList();
				var ordersChangesItemsDb = Infrastructure.Data.Access.Tables.PRS.OrderChangeItemAccess.GetByOrderChangeIds(ordersChangesIds);

				if(ordersDb == null || ordersDb?.Count()<=0)
				{
					return response;
				}

				foreach(var orderDb in ordersDb)
				{
					var customerDb = orderDb.Kunden_Nr.HasValue
						? customersDb.Find(e => e.Nummer == orderDb.Kunden_Nr.Value)
						: null;
					//var adressDb = orderDb.Kunden_Nr.HasValue
					//    ? adressesDb.Find(e => e.Kundennummer == orderDb.Kunden_Nr.Value)
					//    : null;
					var orderExtensionDb = ordersExtensionsDb.Find(e => e.OrderId == orderDb.Nr);
					var orderExtBuyerDb = ordersExtBuyersDb.Find(e => e.OrderId == orderDb.Nr);
					var orderExtConsigneeDb = ordersExtConsigneesDb.Find(e => e.OrderId == orderDb.Nr);

					var order = new Models.Order.OrderModel()
					{
						Id = orderDb.Nr,

						CustomerId = customerDb?.Nr,
						CustomerNumber = orderDb.Kunden_Nr, // customerDb?.Nummer
						AdressCustomerNumber = orderDb.Unser_Zeichen,

						ProjectNumber = orderDb.Projekt_Nr,
						VorfailNr = orderDb.Angebot_Nr.HasValue ? orderDb.Angebot_Nr.ToString() : string.Empty, // <<<<< To update
						AbId = orderDb.Ab_id,
						Belegkreis = orderDb.Belegkreis,
						Conditions = orderDb.Konditionen,
						Contact = orderDb.Ansprechpartner,
						CountryPostcode = orderDb.Land_PLZ_Ort,
						Department = orderDb.Abteilung,
						DesiredDate = orderDb.Wunschtermin,
						DueDate = orderDb.Falligkeit,
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
						OrderTitle = orderDb.Briefanrede,
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

						ValidationTime = orderExtensionDb?.EdiValidationTime,
						ValidationUserId = orderExtensionDb?.EdiValidationUserId,
						ValidationUser = orderExtensionDb != null
							? validationUsersDb.Find(e => e.Id == orderExtensionDb.EdiValidationUserId)?.Username
							: null,

						DocumentNumber = orderDb.Bezug,
						IsManualCreation = !string.IsNullOrWhiteSpace(orderDb.Bezug)
											&& orderDb.Bezug.StartsWith(Order.ManaulDocumentPrefix),

						// > Consignee and Buyer
						Buyer = new Models.Order.OrderModel.BuyerModel(orderExtBuyerDb),
						Consignee = new Models.Order.ConsigneeModel(orderExtConsigneeDb),

						Changes = new Models.Order.OrderModel.ChangesModel(),
						// 2022-07-14 - indicate absence of Consignee and per Position Delivery Address
						///<see cref="MissingConsignee"/>
						MissingConsignee = orderExtConsigneeDb != null && orderExtConsigneeDb.UnloadingPoint == "-1",
						Date = orderDb.Datum,
						// - 2023-01-18
						UstIdCustomer = $"USt-ID-Nr.: {customerDb.EG___Identifikationsnummer}"
					};


					// FIXME: Remove this!!!
					// includeChanges = false;
					#region > Changes
					if(includeChanges)
					{
						#region > Global
						var latestGlobalChangeDb = ordersChangesDb?.FindAll(e => e.OrderId == orderDb.Nr && e.GlobalStatus == globalChangePendingStatus)?
							.OrderBy(e => e.CreationTime)?
							.LastOrDefault();
						if(latestGlobalChangeDb != null)
						{
							order.Changes.GlobalChangeId = latestGlobalChangeDb.Id;
							if(latestGlobalChangeDb.Notes != order.Freetext)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "Freetext",
									Value = latestGlobalChangeDb.Notes
								});
							}
							if(latestGlobalChangeDb.Reference != order.DocumentNumber)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "DocumentNumber",
									Value = latestGlobalChangeDb.Reference
								});
							}

							if(latestGlobalChangeDb.CustomerContactName != order.Contact)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "Contact",
									Value = latestGlobalChangeDb.CustomerContactName
								});
							}
							if(latestGlobalChangeDb.CustomerPurchasingDepartment != order.Department)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "Department",
									Value = latestGlobalChangeDb.CustomerPurchasingDepartment
								});
							}
							if(latestGlobalChangeDb.CustomerStreetPostalCode != order.StreetPOBox)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "StreetPOBox",
									Value = latestGlobalChangeDb.CustomerStreetPostalCode
								});
							}
							if(latestGlobalChangeDb.CustomerStreetCityPostalCode != order.CountryPostcode)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "CountryPostcode",
									Value = latestGlobalChangeDb.CustomerStreetCityPostalCode
								});
							}

							if(latestGlobalChangeDb.ConsigneeContactName != order.LContact)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "LContact",
									Value = latestGlobalChangeDb.ConsigneeContactName
								});
							}
							if(latestGlobalChangeDb.ConsigneeName != order.LName)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "LName",
									Value = latestGlobalChangeDb.ConsigneeName
								});
							}
							if(latestGlobalChangeDb.ConsigneeName != order.LClientName)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "LClientName",
									Value = latestGlobalChangeDb.ConsigneeName
								});
							}
							if(latestGlobalChangeDb.ConsigneePurchasingDepartment != order.LDepartment)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "LDepartment",
									Value = latestGlobalChangeDb.ConsigneePurchasingDepartment
								});
							}
							if(latestGlobalChangeDb.ConsigneeStreetPostalCode != order.LStreetMailbox)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "LStreetMailbox",
									Value = latestGlobalChangeDb.ConsigneeStreetPostalCode
								});
							}
							if(latestGlobalChangeDb.ConsigneeStreetPostalCode != order.LStreetPOBox)
							{
								order.Changes.GlobalChanges.Add(new Models.Order.OrderModel.ChangesModel.GlobalChange()
								{
									Key = "LStreetPOBox",
									Value = latestGlobalChangeDb.ConsigneeStreetPostalCode
								});
							}
						}
						#endregion

						#region > Items
						var pendingChangesItemsDb = ordersChangesItemsDb.FindAll(e => e.OrderId == orderDb.Nr && e.Status == itemChangePendingStatus);

						order.Changes.NewItems = pendingChangesItemsDb.Count(e => e.Type == itemChangeTypeNew);
						order.Changes.CanceledItems = pendingChangesItemsDb.Count(e => e.Type == itemChangeTypeCanceled);
						order.Changes.ChangedItems = pendingChangesItemsDb?.Where(e => e.Type == itemChangeTypeChanged)
							?.DistinctBy(e => e.ItemNumber)
							?.Count() ?? 0;
						#endregion
					}
					#endregion

					response.Add(order);
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}

		public static Core.Models.ResponseModel<List<Models.Order.OrderModel>> GetByCustomer(int customerId,
			bool? validated = false,
			Core.Identity.Models.UserModel user = null, EDI.Models.Order.GetRequestModel data = null)
		{
			try
			{
				return Core.Models.ResponseModel<List<Models.Order.OrderModel>>.SuccessResponse(GetByCustomer(new List<int> { customerId }, validated, data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<List<Models.Order.OrderModel>> GetByCustomer(List<int> customerId,
			bool? validated = false,
			Core.Identity.Models.UserModel user = null, EDI.Models.Order.GetRequestModel data = null)
		{
			try
			{
				return Core.Models.ResponseModel<List<Models.Order.OrderModel>>.SuccessResponse(GetByCustomer(customerId, validated, data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Order.OrderModel> GetByCustomer(List<int> customerIds,
			bool? validated, EDI.Models.Order.GetRequestModel data )
		{
			if(customerIds == null || customerIds.Count <= 0)
				return new List<Models.Order.OrderModel>();

			try
			{

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data?.PageSize > 0 ? ((data?.RequestedPage ?? 0) * (data?.PageSize ?? 0)) : 0,
					RequestRows = data?.PageSize ?? 10
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data?.SortField))
				{
					var sortFieldName = "[Fälligkeit]";
					switch(data?.SortField?.ToLower())
					{
						default:
						case "duedate":
							sortFieldName = "[Fälligkeit]";
							break;
						case "documentnumber":
							sortFieldName = "Bezug";
							break;
						case "type":
							sortFieldName = "[Typ]";
							break;
						case "name":
							sortFieldName = "[Vorname/NameFirma]";
							break;
						case "projectnumber":
							sortFieldName = "[Projekt-Nr]";
							break;
						case "vorfailnr":
							sortFieldName = "[Angebot-Nr]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data?.SortDesc ?? false,
					};
				}
				#endregion

				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customerIds);
				if(customersDb == null)
				{
					Infrastructure.Services.Logging.Logger.Log("customerDb == null");
					throw new Exception("Customer Not found");
				}

				var customerNummers = customersDb?.Select(x => x.Nummer ?? -1)?.ToList();
				if(!validated.HasValue)
				{
					var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetConfirmationByKundenNr(customerNummers, data?.SearchText, data?.StartDate, data?.EndDate, dataSorting, dataPaging)
						?.Distinct();
					return Get(ordersDb, true);
				}
				else if(validated.Value)
				{
					var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrder(false, customerNummers, data?.SearchText, data?.StartDate, data?.EndDate, dataSorting, dataPaging);
					return Get(ordersDb, true);
				}
				else
				{
					// - 2022-12-14 - filter from DB
					var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrder(true, customerNummers, data?.SearchText, data?.StartDate, data?.EndDate, dataSorting, dataPaging);
					return Get(ordersDb, true);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public static Core.Models.ResponseModel<List<Models.Customers.CustomerOrdersCountModel>> GetCountByCustomers(IEnumerable<int> customersNumbers,
			bool? validated = null, Core.Identity.Models.UserModel user = null)
		{
			try
			{
				if(!validated.HasValue)
				{
					return Core.Models.ResponseModel<List<Models.Customers.CustomerOrdersCountModel>>.SuccessResponse(GetCountByCustomers(customersNumbers));
				}
				else if(validated.Value)
				{
					return Core.Models.ResponseModel<List<Models.Customers.CustomerOrdersCountModel>>.SuccessResponse(GetValidatedCountByCustomers(customersNumbers));
				}
				else
				{
					return Core.Models.ResponseModel<List<Models.Customers.CustomerOrdersCountModel>>.SuccessResponse(GetUnvalidatedCountByCustomers(customersNumbers));
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Customers.CustomerOrdersCountModel> GetCountByCustomers(IEnumerable<int> customersNumbers)
		{
			try
			{
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers);
				var counts = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrderCounts(null, customersNumbers);

				return GetCountByCustomer( customersDb, counts);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Customers.CustomerOrdersCountModel> GetValidatedCountByCustomers(IEnumerable<int> customersNumbers)
		{
			try
			{
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers);
				// - 2022-12-14 - filter from DB
				var counts = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrderCounts(false, customersNumbers);

				return GetCountByCustomer(	customersDb, counts);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Customers.CustomerOrdersCountModel> GetUnvalidatedCountByCustomers(IEnumerable<int> customersNumbers)
		{
			try
			{
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers);
				// - 2022-12-14 - filter from DB

				var counts = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrderCounts(true, customersNumbers);

				return GetCountByCustomer(customersDb, counts);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Customers.CustomerOrdersCountModel> GetCountByCustomer(List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> customersDb, IEnumerable<KeyValuePair<int, int>> counts)
		{
			try
			{
				var response = new List<Models.Customers.CustomerOrdersCountModel>();
				if(customersDb == null)
					return response;

				var customersNumbers = customersDb
					.Where(e => e.Nummer.HasValue)
					.Select(e => e.Nummer.Value).ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNumbers);

				// - 2022-11-23 - discard Order w/o Extension (these are coming from P3000)
				// -- 2022-11-24 - allow view/edit but NOT validate

				foreach(var customerDb in customersDb)
				{
					var adressDb = adressesDb.Find(e => e.Nr == customerDb.Nummer);

					response.Add(new Models.Customers.CustomerOrdersCountModel()
					{
						Id = customerDb.Nr,
						Type = adressDb?.Anrede,
						CustomerNumber = customerDb.Nummer ?? -1,
						AdressCustomerNumber = adressDb?.Kundennummer,

						Name = adressDb?.Name1,
						Name2 = adressDb?.Name2,
						Name3 = adressDb?.Name3,

						Contact = "",
						Department = customerDb.RG_Abteilung,
						CountryPostcode = customerDb.RG_Land_PLZ_ORT,
						StreetPOBox = customerDb.RG_Strasse_Postfach,

						Count = counts?.FirstOrDefault(x=> x.Key == customerDb.Nummer).Value ?? 0
					});
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		// Creation

	}
}
