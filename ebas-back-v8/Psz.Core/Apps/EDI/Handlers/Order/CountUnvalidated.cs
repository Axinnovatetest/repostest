using Infrastructure.Data.Entities.Tables.Logistics;
using System;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static int CountUnvalidated(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null)
				{
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CountCustomerOrdersByIsNew(true);
				}
				else
				{
					if(user.IsAdministrator == true || user.IsGlobalDirector == true || user.Access.Purchase.AllCustomers == true)
					{
						return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrderCount(true);
					}

					var customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(user.Id)
					.Select(e => e.CustomerNumber);

					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCustomerOrdersByNeuOrderCount(true, customersNumbers);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static int CountUnvalidated(Core.Identity.Models.UserModel user, Infrastructure.Services.Utils.TransactionsManager botransaction)
		{
			try
			{
				if(user == null)
				{
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.CountCustomerOrdersByIsNew(true, botransaction.connection, botransaction.transaction);
				}
				else
				{
					var customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByUserId(user.Id, botransaction.connection, botransaction.transaction)
					.Select(e => e.CustomerNumber);

					//FIXME!: transactionize this
					var orders = Get(false).FindAll(e => e.CustomerNumber.HasValue
						|| !customersNumbers.Contains(e.CustomerNumber.Value));

					return orders.Where(e => e.NewOrder).Count();
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
