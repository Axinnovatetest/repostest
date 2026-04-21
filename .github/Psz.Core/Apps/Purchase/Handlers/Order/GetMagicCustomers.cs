using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<List<KeyValuePair<int, string>>> GetMagicCustomers(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null /*|| !user.Access.EDI.Order*/)
				{
					return Core.Models.ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
				}

				return Core.Models.ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(GetMagicCustomersInternal(user));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static List<KeyValuePair<int, string>> GetMagicCustomersInternal(Core.Identity.Models.UserModel user)
		{
			try
			{
				var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);

				var userCustomersNumbers = userCustomers.Select(e => e.CustomerNumber).ToList();

				var dataTimeMinus6Months = DateTime.Now.AddMonths(-6);
				var ordersDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByTypsAndKundenNrsDate(null, userCustomersNumbers, dataTimeMinus6Months);

				var magicCustomersNumbers = ordersDb
					.Where(e => e.Kunden_Nr.HasValue && e.Datum.HasValue && e.Datum > dataTimeMinus6Months) // get last 6 months orders
					.GroupBy(e => e.Kunden_Nr.Value)
					.Select(e => new { CustomerNumber = e.Key, OrdersCount = e.Count() }) // count orders bu customer
					.OrderByDescending(e => e.OrdersCount)
					.Take(5) // top 5 are what we need
					.Select(e => e.CustomerNumber)
					.ToList();

				var response = new List<KeyValuePair<int, string>>();
				foreach(var magicCustomerNumber in magicCustomersNumbers)
				{
					var customer = userCustomers.Find(e => e.CustomerNumber == magicCustomerNumber);
					if(customer != null)
					{
						response.Add(new KeyValuePair<int, string>(customer.CustomerNumber, customer.Name));
					}
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
