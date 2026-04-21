using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<List<Models.Customers.ReplacementModel>> GetUserReplacements(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				var responseBody = new List<Models.Customers.ReplacementModel>();

				var customersNumbers = new List<int> { };
				if(user.Access.Purchase.AllCustomers == true)
				{
					customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.Get()
						.Select(e => e.CustomerNumber)
						.ToList();
				}
				else
				{
					customersNumbers = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetBy_IsPrimary_UserId(true, user.Id)
						.Select(e => e.CustomerNumber)
						.ToList();
				}
				var customersDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers);
				var customers = Apps.EDI.Handlers.Customers.Get(customersDb).Body ?? new List<Models.Customers.CustomerModel>();

				var customerReplacementsDb = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByCustomersNumbers(customersNumbers)
					.FindAll(e => !e.IsPrimary);

				var replacementsUsersIds = customerReplacementsDb.Select(e => e.UserId).ToList();
				var usersDb = Apps.Settings.Handlers.Users.Get(replacementsUsersIds);

				foreach(var customerReplacementDb in customerReplacementsDb)
				{
					var userDb = usersDb.Find(e => e.Id == customerReplacementDb.UserId);
					if(userDb == null)
					{
						continue;
					}

					var customer = customers.Find(e => e.CustomerNumber == customerReplacementDb.CustomerNumber);
					Infrastructure.Services.Logging.Logger.Log($"{customersNumbers}: {customers?.Count}: {customer?.Id}, {customer?.CustomerNumber}, {customer?.Name}");

					responseBody.Add(new Models.Customers.ReplacementModel()
					{
						Id = userDb.Id,
						AccessProfileId = userDb.AccessProfileId,
						AccessProfileName = userDb.AccessProfileName,
						Name = userDb.Name,
						ValidFromTime = customerReplacementDb.ValidFromTime,
						ValidIntoTime = customerReplacementDb.ValidIntoTime,
						CustomerId = customerReplacementDb?.CustomerNumber ?? -1,
						CustomerName = customer?.Name
					});
				}

				return new Core.Models.ResponseModel<List<Models.Customers.ReplacementModel>>()
				{
					Success = true,
					Body = responseBody
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
	}
}
