using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<object> SetPrimaryUser(Models.Customers.SetPrimaryUserModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.CustomersLock)
			{
				try
				{
					#region > Check User Access
					if(user == null
						|| (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated)
						/*|| !user.Access.Purchase.CustomerUpdate*/)
					{
						throw new Core.Exceptions.UnauthorizedException();
					}
					#endregion

					#region > Validation
					var errors = new List<string>();

					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(data.CustomerId);
					if(customerDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Customer not found" }
						};
					}

					var primaryUserDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.PrimaryUserId);
					if(primaryUserDb == null || primaryUserDb.IsArchived)
					{
						errors.Add("Primary User not found");
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Primary User not found" }
						};
					}
					#endregion

					// > Remove old users
					Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.DeleteBy_IsPrimary_CustomerNumber(true, customerDb.Nummer ?? -1);

					// > Insert Primary User
					Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.CustomerUserEntity()
					{
						Id = -1,
						CustomerNumber = customerDb.Nummer ?? -1,
						UserId = primaryUserDb.Id,
						IsPrimary = true,
						ValidIntoTime = DateTime.Now,
						ValidFromTime = DateTime.Now,
					});

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
