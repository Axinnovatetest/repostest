using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers.OrderError
{
	public partial class OrderError
	{
		public static List<Models.OrderError.OrderErrorModel> Get(Core.Identity.Models.UserModel user)
		{
			try
			{
				var response = new List<Models.OrderError.OrderErrorModel>();

				var orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.Get();

				if(user != null)
				{
					var verificationCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(user.Id)
						.Select(e => e.CustomerNumber)
						.ToList();
					var verificationCustomersIds = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(verificationCustomersNumbers)
						.Select(e => e.Nr)
						.ToList();

					orderErrorsDb = orderErrorsDb.FindAll(e => verificationCustomersIds.Contains(e.CustomerId));
				}

				var validationUsersIds = orderErrorsDb
					.Where(e => e.ValidationUserId.HasValue)
					.Select(e => e.ValidationUserId.Value)
					.ToList();
				var validationUsersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validationUsersIds);

				var customersIds = orderErrorsDb.Select(e => e.CustomerId).ToList();
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customersIds);

				var customersNumbers = customersDb.Where(e => e.Nummer.HasValue).Select(e => e.Nummer.Value).ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNumbers);

				foreach(var orderErrorDb in orderErrorsDb)
				{
					var validationUserDb = orderErrorDb.ValidationUserId.HasValue
						? validationUsersDb.Find(e => e.Id == orderErrorDb.ValidationUserId.Value)
						: null;

					var customerName = "";
					var customerDb = customersDb.Find(e => e.Nr == orderErrorDb.CustomerId);
					if(customerDb != null && customerDb.Nummer.HasValue)
					{
						customerName = adressesDb.Find(e => e.Nr == customerDb.Nummer.Value)?.Name1;
					}

					response.Add(Helpers.ToOrderError(orderErrorDb, validationUserDb, customerName));
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static List<Models.OrderError.OrderErrorModel> GetValidated(Core.Identity.Models.UserModel user)
		{
			try
			{
				var result = new List<Models.OrderError.OrderErrorModel>();

				var orderErrorsDb = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

				if(user != null)
				{
					var verificationCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(user.Id)
						.Select(e => e.CustomerNumber)
						.ToList();
					var verificationCustomersIds = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(verificationCustomersNumbers)
						.Select(e => e.Nr)
						.ToList();

					orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(verificationCustomersIds, true);
				}
				else
				{
					orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(null,true);
				}

				var validationUsersIds = orderErrorsDb
					.Where(e => e.ValidationUserId.HasValue)
					.Select(e => e.ValidationUserId.Value)
					.ToList();
				var validationUsersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validationUsersIds);

				var customersIds = orderErrorsDb.Select(e => e.CustomerId).ToList();
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customersIds);

				var customersNumbers = customersDb.Where(e => e.Nummer.HasValue).Select(e => e.Nummer.Value).ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNumbers);

				foreach(var orderErrorDb in orderErrorsDb)
				{
					var validationUserDb = orderErrorDb.ValidationUserId.HasValue
						   ? validationUsersDb.Find(e => e.Id == orderErrorDb.ValidationUserId.Value)
						   : null;

					var customerName = "";
					var customerDb = customersDb.Find(e => e.Nr == orderErrorDb.CustomerId);
					if(customerDb != null && customerDb.Nummer.HasValue)
					{
						customerName = adressesDb.Find(e => e.Nr == customerDb.Nummer.Value)?.Name1;
					}

					result.Add(Helpers.ToOrderError(orderErrorDb, validationUserDb, customerName));
				}

				return result;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static List<Models.OrderError.OrderErrorModel> GetNotValidated(Core.Identity.Models.UserModel user)
		{
			try
			{
				var result = new List<Models.OrderError.OrderErrorModel>();

				var orderErrorsDb = new List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity>();

				if(user != null)
				{
					var verificationCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(user.Id)
						.Select(e => e.CustomerNumber)
						.ToList();
					var verificationCustomersIds = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(verificationCustomersNumbers)
						.Select(e => e.Nr)
						.ToList();

					orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(verificationCustomersIds, false);
				}
				else
				{
					orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(null, false);
				}
				var validationUsersIds = orderErrorsDb
						.Where(e => e.ValidationUserId.HasValue)
						.Select(e => e.ValidationUserId.Value)
						.ToList();
				var validationUsersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validationUsersIds);

				var customersIds = orderErrorsDb.Select(e => e.CustomerId).ToList();
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customersIds);

				var customersNumbers = customersDb.Where(e => e.Nummer.HasValue).Select(e => e.Nummer.Value).ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNumbers);

				foreach(var orderErrorDb in orderErrorsDb)
				{
					var validationUserDb = orderErrorDb.ValidationUserId.HasValue
						? validationUsersDb.Find(e => e.Id == orderErrorDb.ValidationUserId.Value)
						: null;

					var customerName = "";
					var customerDb = customersDb.Find(e => e.Nr == orderErrorDb.CustomerId);
					if(customerDb != null && customerDb.Nummer.HasValue)
					{
						customerName = adressesDb.Find(e => e.Nr == customerDb.Nummer.Value)?.Name1;
					}

					result.Add(Helpers.ToOrderError(orderErrorDb, validationUserDb, customerName));
				}

				return result;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static int CountNotValidated(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null)
				{
					return Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetCountByIsValidated(false);
				}
				else
				{
					var customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(user.Id)
						.Select(e => e.CustomerNumber)
						.ToList();
					var customersIds = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers)
						.Select(e => e.Nr)
						.ToList();

					return Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(customersIds, false)?.Count ?? 0;
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		#region Customer stuff
		public static Core.Models.ResponseModel<List<Models.Customers.CustomerOrderErrorsCountModel>> GetCountByCustomers(List<int> customersIds, bool? validated = null, Core.Identity.Models.UserModel user = null)
		{
			try
			{
				if(!validated.HasValue)
				{
					return Core.Models.ResponseModel<List<Models.Customers.CustomerOrderErrorsCountModel>>.SuccessResponse(GetCountByCustomers(customersIds));
				}
				else if(validated.Value)
				{
					return Core.Models.ResponseModel<List<Models.Customers.CustomerOrderErrorsCountModel>>.SuccessResponse(GetValidatedCountByCustomers(customersIds));
				}
				else
				{
					return Core.Models.ResponseModel<List<Models.Customers.CustomerOrderErrorsCountModel>>.SuccessResponse(GetUnvalidatedCountByCustomers(customersIds));
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Customers.CustomerOrderErrorsCountModel> GetCountByCustomers(List<int> customersIds)
		{
			try
			{
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customersIds);
				var orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.Get()?.FindAll(a => customersIds.Exists(i => i == a.CustomerId) || a.CustomerId < 0)?.ToList();

				return GetCountByCustomer(orderErrorsDb, customersDb);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Customers.CustomerOrderErrorsCountModel> GetValidatedCountByCustomers(List<int> customersIds)
		{
			try
			{
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customersIds);
				var orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(customersIds,  true);

				return GetCountByCustomer(orderErrorsDb, customersDb);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Customers.CustomerOrderErrorsCountModel> GetUnvalidatedCountByCustomers(List<int> customersIds)
		{
			try
			{
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customersIds);
				var orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(customersIds, false);

				return GetCountByCustomer(orderErrorsDb, customersDb);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.Customers.CustomerOrderErrorsCountModel> GetCountByCustomer(List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> ordersDb, List<Infrastructure.Data.Entities.Tables.PRS.KundenEntity> customersDb)
		{
			try
			{
				var response = new List<Models.Customers.CustomerOrderErrorsCountModel>();
				if(ordersDb == null || customersDb == null)
					return response;

				var customersNumbers = customersDb
					.Where(e => e.Nummer.HasValue)
					.Select(e => e.Nummer.Value).ToList();
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(customersNumbers);

				foreach(var customerDb in customersDb)
				{
					var customerName = "";
					int? customerNumber = -1;
					if(customerDb != null && customerDb.Nummer.HasValue)
					{
						var adressDb = adressesDb == null ? null : adressesDb.Find(e => e.Nr == customerDb.Nummer.Value);
						if(adressDb != null)
						{
							customerName = adressDb.Name1;
							customerNumber = adressDb?.Kundennummer;
						}
					}
					var _orderErrors = ordersDb.FindAll(e => e.CustomerId == customerDb.Nr)?.ToList();
					response.Add(new Models.Customers.CustomerOrderErrorsCountModel()
					{
						ClientId = customerDb.Nr,
						ClientName = customerName,
						CustomerNumber = customerDb.Nummer.HasValue ? (int)customerDb.Nummer : -1,
						AdressCustomerNumber = customerNumber,
						Count = _orderErrors == null ? 0 : _orderErrors.Count
					});
				}

				// Add Missing customer
				var missingCustomerErrors = ordersDb.FindAll(e => e.CustomerId < 0).ToList();
				if(missingCustomerErrors != null && missingCustomerErrors.Count > 0)
				{
					response.Add(new Models.Customers.CustomerOrderErrorsCountModel()
					{
						ClientId = -1,
						ClientName = "",
						CustomerNumber = -1,
						AdressCustomerNumber = (int?)null,
						Count = missingCustomerErrors.Count
					});
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static Core.Models.ResponseModel<List<Models.OrderError.OrderErrorModel>> GetByCustomer(int customerId, bool? validated = false, Core.Identity.Models.UserModel user = null, Core.Apps.EDI.Models.OrderError.GetRequestModel data=null)
		{
			try
			{
				return Core.Models.ResponseModel<List<Models.OrderError.OrderErrorModel>>.SuccessResponse(GetByCustomer(customerId, validated, data));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.OrderError.OrderErrorModel> GetByCustomer(int customerId, bool? validated, Core.Apps.EDI.Models.OrderError.GetRequestModel data)
		{
			try
			{
				Infrastructure.Data.Entities.Tables.PRS.KundenEntity customerDb;
				if(customerId < 0)
				{
					customerDb = new Infrastructure.Data.Entities.Tables.PRS.KundenEntity
					{
						Nr = -1
					};
				}
				else
				{
					customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customerId);
				}
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data?.PageSize > 0 ? ((data?.RequestedPage ?? 0) * (data?.PageSize ?? 0)) : 0,
					RequestRows = data?.PageSize ?? 10
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data?.SortField))
				{
					var sortFieldName = "[CreationTime]";
					switch(data?.SortField?.ToLower())
					{
						default:
						case "creationtime":
							sortFieldName = "[CreationTime]";
							break;
						case "CustomerNumber":
							sortFieldName = "CustomerNumber";
							break;
						case "clientname":
							sortFieldName = "[ClientName]";
							break;
						case "error":
							sortFieldName = "[Error]";
							break;
						case "filepath":
							sortFieldName = "[FileName]";
							break;
						case "validationuser":
							sortFieldName = "[ValidationUserId]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data?.SortDesc ?? false,
					};
				}
				#endregion

				if(!validated.HasValue)
				{
					var orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByCustomer(customerId,  data?.SearchText, data is null ? null : dataSorting, data is null ? null : dataPaging);
					return GetByCustomer(orderErrorsDb, customerDb);
				}
				else if(validated.Value)
				{
					var orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(customerId, true, data?.SearchText, data is null ? null : dataSorting, data is null ? null : dataPaging);
					return GetByCustomer(orderErrorsDb, customerDb);
				}
				else
				{
					var orderErrorsDb = Infrastructure.Data.Access.Tables.PRS.OrderErrorAccess.GetByIsValidated(customerId, false, data?.SearchText, data is null ? null : dataSorting, data is null ? null : dataPaging);
					return GetByCustomer(orderErrorsDb, customerDb);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.OrderError.OrderErrorModel> GetByCustomer(List<Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity> orderErrorsDb, Infrastructure.Data.Entities.Tables.PRS.KundenEntity customerDb)
		{
			try
			{
				var response = new List<Models.OrderError.OrderErrorModel>();
				if(orderErrorsDb == null || customerDb == null)
					return response;

				var ordersIds = orderErrorsDb.Select(e => e.Id).ToList();
				var ordersExtensionsDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrdersIds(ordersIds);

				var validationUsersIds = orderErrorsDb.Select(e => e.ValidationUserId == null ? -1 : (int)e.ValidationUserId).ToList();
				var validationUsersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validationUsersIds);




				foreach(var orderErrorDb in orderErrorsDb)
				{
					var orderExtensionDb = ordersExtensionsDb.Find(e => e.OrderId == orderErrorDb.Id);
					var validationUserDb = orderErrorDb.ValidationUserId.HasValue
						? validationUsersDb.Find(e => e.Id == orderErrorDb.ValidationUserId.Value)
						: null;

					var addressDb = int.TryParse(orderErrorDb.CustomerNumber, out var custNumber) ? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(custNumber) : null;

					var order = new Models.OrderError.OrderErrorModel()
					{
						Id = orderErrorDb.Id,

						Error = orderErrorDb.Error,
						FilePath = System.IO.Path.GetFileName(orderErrorDb.FileName),
						ClientId = orderErrorDb.CustomerId,
						ClientName = orderErrorDb.CustomerName,

						Validated = orderErrorDb.Validated,
						ValidationTime = orderErrorDb.ValidationTime,
						ValidationUser = validationUserDb == null ? "" : validationUserDb.Name,
						ValidationUserId = orderErrorDb.ValidationUserId,

						CustomerNumber = orderErrorDb.CustomerNumber,
						CreationTime = orderErrorDb.CreationTime,

						AdressCustomerNumber = addressDb?.Kundennummer,

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
		#endregion
	}
}
