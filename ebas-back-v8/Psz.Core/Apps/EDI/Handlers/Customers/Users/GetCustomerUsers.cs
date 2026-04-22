using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public partial class Users
		{
			public static Core.Models.ResponseModel<Models.Customers.CustomerUsersModel> GetCustomerUsers(int customerId)
			{
				try
				{
					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(customerId);
					if(customerDb == null)
					{
						return new Core.Models.ResponseModel<Models.Customers.CustomerUsersModel>()
						{
							Errors = new List<string>()
							{
								"Customer not found"
							}
						};
					}

					var customerUsersDb = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.GetByCustomerNumber(customerDb.Nummer ?? -1);

					var usersIds = customerUsersDb.Where(e => e.IsPrimary).Select(e => e.UserId).ToList();
					var usersDb = Core.Apps.Settings.Handlers.Users.Get(usersIds);

					var customerName = Apps.EDI.Handlers.Customers.Get(new List<int> { customerDb.Nr })
						.Body
						?.FirstOrDefault()
						?.Name;

					var responseBody = new Models.Customers.CustomerUsersModel();

					foreach(var customerUserDb in customerUsersDb)
					{
						var userDb = usersDb.Find(e => e.Id == customerUserDb.UserId);
						if(userDb == null)
						{
							continue;
						}

						if(responseBody.PrimaryUser == null && customerUserDb.IsPrimary)
						{
							responseBody.PrimaryUser = new Models.Customers.PrimaryUserModel()
							{
								Id = userDb.Id,
								AccessProfileId = userDb.AccessProfileId,
								AccessProfileName = userDb.AccessProfileName,
								Name = userDb.Name,
								CustomerId = customerId,
								CustomerName = customerName
							};
						}
						else if(!customerUserDb.IsPrimary)
						{
							responseBody.Replacements.Add(new Models.Customers.ReplacementModel()
							{
								Id = userDb.Id,
								AccessProfileId = userDb.AccessProfileId,
								AccessProfileName = userDb.AccessProfileName,
								Name = userDb.Name,
								ValidFromTime = customerUserDb.ValidFromTime,
								ValidIntoTime = customerUserDb.ValidIntoTime,
								CustomerId = customerId,
								CustomerName = customerName
							});
						}
					}

					return new Core.Models.ResponseModel<Models.Customers.CustomerUsersModel>()
					{
						Success = true,
						Body = responseBody
					};
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					return Core.Models.ResponseModel<Models.Customers.CustomerUsersModel>.UnexpectedErrorResponse();
				}
			}
		}
	}
}
