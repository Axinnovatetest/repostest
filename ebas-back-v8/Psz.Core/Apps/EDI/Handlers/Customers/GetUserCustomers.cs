using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> GetUserCustomers_2(Core.Identity.Models.UserModel user, string search,
			bool? isEDI = null)
		{
			try
			{
				if(search?.Trim() == "10146") // PSZ - as Kunde
				{
					user.Access.Purchase.AllCustomers = true;
				}

				List<Models.Customers.CustomerModel> filtred = new List<Models.Customers.CustomerModel>();
				var customers = GetUserCustomersInternal(user, isEDI);
				if(customers != null && customers.Count > 0)
					filtred = customers.Where(x => x.AdressCustomerNumber.HasValue).ToList();

				if(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search))
					return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.SuccessResponse(filtred);

				var _serchText = search.Trim().ToLower();
				var final = filtred?.Where(x => x.Name.ToLower().Contains(_serchText) || x.AdressCustomerNumber.ToString().Contains(_serchText)).ToList();
				return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.SuccessResponse(final);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> GetUserCustomers_2ForCreate(Core.Identity.Models.UserModel user, string search,
			bool? isEDI = null)
		{
			try
			{
				if(search?.Trim() == "10146") // PSZ - as Kunde
				{
					user.Access.Purchase.AllCustomers = true;
				}

				List<Models.Customers.CustomerModel> filtred = new List<Models.Customers.CustomerModel>();
				var data = new CustomerService.Handlers.OrderProcessing.GetMyCustomersForCreateHandler(isEDI, user).Handle().Body ?? new List<CustomerService.Models.OrderProcessing.CustomerModel>();
				var customers = data.Select(x => new Models.Customers.CustomerModel(x)).ToList();
				if(customers != null && customers.Count > 0)
					filtred = customers.Where(x => x.AdressCustomerNumber.HasValue).ToList();

				if(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search))
					return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.SuccessResponse(filtred);

				var _serchText = search.Trim().ToLower();
				var final = filtred?.Where(x => x.Name.ToLower().Contains(_serchText) || x.AdressCustomerNumber.ToString().Contains(_serchText)).ToList();
				return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.SuccessResponse(final);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> GetUserCustomersForCreate(Core.Identity.Models.UserModel user,
			bool? isEDI = null)
		{
			try
			{
				List<Models.Customers.CustomerModel> filtred = new List<Models.Customers.CustomerModel>();

				var data = new CustomerService.Handlers.OrderProcessing.GetMyCustomersForCreateHandler(isEDI, user).Handle().Body ?? new List<CustomerService.Models.OrderProcessing.CustomerModel>();
				var customers = data.Select(x => new Models.Customers.CustomerModel(x)).ToList();
				// -
				if(customers != null && customers.Count > 0)
				{
					filtred = customers.Where(x => x.AdressCustomerNumber.HasValue).ToList();
				}
				return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.SuccessResponse(filtred);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> GetUserCustomers(Core.Identity.Models.UserModel user,
			bool? isEDI = null)
		{
			try
			{
				List<Models.Customers.CustomerModel> filtred = new List<Models.Customers.CustomerModel>();
				var customers = GetUserCustomersInternal(user, isEDI);
				if(customers != null && customers.Count > 0)
				{
					filtred = customers.Where(x => x.AdressCustomerNumber.HasValue).ToList();
				}
				//return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.SuccessResponse(GetUserCustomersInternal(user, isEDI));
				return Core.Models.ResponseModel<List<Models.Customers.CustomerModel>>.SuccessResponse(filtred);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public static List<Models.Customers.CustomerModel> GetUserCustomersInternal(Core.Identity.Models.UserModel user,
			bool? isEDI = null)
		{
			try
			{
				if(user == null/* || !user.Access.Purchase.ModuleActivated*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				//if(user.Access.Purchase.AllCustomers)
				//{
				//	return Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(), isEDI).Body
				//		?? new List<Models.Customers.CustomerModel>();
				//}

				//var customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(true, user.Id)
				//	?.Select(e => e.CustomerNumber)
				//	?.ToList() ?? new List<int>();

				//var notPrimaryCustomersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(false, user.Id)
				//	?.Where(e => DateTime.Now >= e.ValidFromTime.Date && DateTime.Now <= e.ValidIntoTime.Date.AddDays(1))
				//	?.Select(e => e.CustomerNumber)
				//	?.ToList() ?? new List<int>();

				//// - 2022-05-17 - Sabine - open Customers to CS
				//if(user.Access.CustomerService.ModuleActivated &&
				//	(user.Access.CustomerService.ConfirmationCreate ||
				//	user.Access.CustomerService.DeliveryNoteCreate ||
				//	user.Access.CustomerService.FaCreate ||
				//	user.Access.CustomerService.ConfirmationEdit ||
				//	user.Access.CustomerService.DeliveryNoteEdit ||
				//	user.Access.CustomerService.FaEdit
				//	))
				//{
				//	var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummers(Program.CTS.OpenCsCustomers?.Select(x => x.Number)?.ToList() ?? new List<int>());
				//	customersNumbers.AddRange(addressEntities?.Select(x => x.Nr) ?? new List<int>());
				//}

				//customersNumbers.AddRange(notPrimaryCustomersNumbers);

				//customersNumbers = customersNumbers.Distinct().ToList();

				//return Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers), isEDI).Body
				//	?? new List<Models.Customers.CustomerModel>();

				// - 2022-12-14 - redirect to CTS module
				var data = new CustomerService.Handlers.OrderProcessing.GetMyCustomersHandler(isEDI, user).Handle().Body ?? new List<CustomerService.Models.OrderProcessing.CustomerModel>();
				return data.Select(x => new Models.Customers.CustomerModel(x)).ToList();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
