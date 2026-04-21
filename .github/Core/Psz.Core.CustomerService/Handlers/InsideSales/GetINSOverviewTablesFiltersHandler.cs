using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.CustomerService.Handlers.InsideSales
{
	public partial class InsideSalesOveview
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetUsersForOverview(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			try
			{
				var users = Infrastructure.Data.Access.Joins.CTS.INSOverviewAccess.GetUsersForOverview();
				var response = new List<KeyValuePair<int, string>>();
				if(!user.Access.Purchase.AllCustomers)
					response = users?.Where(u => u.Key == user.Id).ToList();
				else
					response = users;
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> GetCustomersForOverview(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			try
			{
				var data = new CustomerService.Handlers.OrderProcessing.GetMyCustomersHandler(true, user).Handle().Body ?? new List<CustomerService.Models.OrderProcessing.CustomerModel>();
				var response = data?.Select(d => new KeyValuePair<int, string>(d.AdressCustomerNumber ?? -1, $"{d.AdressCustomerNumber} || {d.Name}")).ToList();
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> GetWarehousesForOverview(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			try
			{
				var warehouses = Enum.GetValues(typeof(Enums.InsideSalesEnums.Warehouses)).Cast<Enums.InsideSalesEnums.Warehouses>().ToList();
				var response = warehouses
										.Select(x => new KeyValuePair<int, string>((int)x, $"{x.GetDescription()}".Trim())).Distinct().ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}