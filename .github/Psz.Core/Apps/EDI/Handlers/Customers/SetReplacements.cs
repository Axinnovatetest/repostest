using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<object> SetReplacements(Models.Customers.SetReplacementsModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.CustomersLock)
			{
				try
				{
					#region > Check User Access
					if(user == null
						|| (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
					{
						throw new Core.Exceptions.UnauthorizedException();
					}
					#endregion

					#region > Validation
					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(data.CustomerId);
					if(customerDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Customer not found" }
						};
					}

					var userCustomers = Psz.Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);
					if(!userCustomers.Exists(e => e.Id == customerDb.Nr))
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Customer not authorized" }
						};
					}

					var errors = new List<string>();

					if(data.Replacements == null || data.Replacements.Count == 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "No replacement" }
						};
					}

					foreach(var replacement in data.Replacements)
					{
						if(replacement.ValidFromTime.Date > replacement.ValidIntoTime.Date)
						{
							errors.Add("Invalid dates");
						}
						else if(replacement.ValidFromTime.Date < DateTime.Today)
						{
							errors.Add("Start date is not correct");
						}
					}

					var secondaryUsersIds = data.Replacements.Select(e => e.Id).Distinct().ToList();
					var secondaryUsersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(secondaryUsersIds);
					if(secondaryUsersDb.Count != secondaryUsersIds.Count)
					{
						errors.Add("Replacement not found");
					}

					if(errors.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = errors
						};
					}
					#endregion

					// > Remove old users
					Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.DeleteBy_IsPrimary_CustomerNumber(false, customerDb.Nummer ?? -1);

					// > Insert Secondary Users
					foreach(var replacement in data.Replacements)
					{
						Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.CustomerUserEntity()
						{
							Id = -1,
							CustomerNumber = customerDb.Nummer ?? -1,
							UserId = replacement.Id,
							IsPrimary = false,
							ValidIntoTime = replacement.ValidIntoTime.Date,
							ValidFromTime = replacement.ValidFromTime.Date
						});
					}

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
