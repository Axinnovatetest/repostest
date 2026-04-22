using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<object> UpdateReplacement(Models.Customers.UpdateReplacementModel data,
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

					var replacementDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.ReplacementId);
					if(replacementDb == null || replacementDb.IsArchived)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Replacement not found" }
						};
					}

					var customerUserDb = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess
						.GetBy_CustomerNumber_UserId(customerDb.Nummer ?? -1, replacementDb.Id);
					if(customerUserDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "User is not a replacement" }
						};
					}

					if(data.ValidFromTime.Date > data.ValidIntoTime.Date)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Invalid dates" }
						};
					}
					else if(data.ValidFromTime.Date < DateTime.Today)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Start date is not correct" }
						};
					}
					#endregion

					customerUserDb.ValidFromTime = data.ValidFromTime.Date;
					customerUserDb.ValidIntoTime = data.ValidIntoTime.Date;
					Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.Update(customerUserDb);

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
