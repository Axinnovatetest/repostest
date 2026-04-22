using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<object> AddReplacement(Models.Customers.AddReplacementModel data,
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
					var errors = new List<string>();

					var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(data.CustomerId);
					if(customerDb == null)
					{
						errors.Add("Customer not found");
					}

					var replacementDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.ReplacementId);
					if(replacementDb == null || replacementDb.IsArchived)
					{
						errors.Add("Replacement not found");
					}

					var customerUserDb = Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess
						.GetBy_CustomerNumber_UserId(customerDb.Nummer ?? -1, replacementDb.Id);
					if(customerUserDb != null)
					{
						errors.Add("Replacement exists");
					}

					if(errors.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = errors
						};
					}
					#endregion

					#region > Check Current User Is Primary
					//var customerUserAccessDb = customerDb.Nummer.HasValue
					//    ? Infrastructure.Data.Access.Tables.EDI.CustomerUserAccess.GetBy_IsPrimary_CustomerNumber(true, customerDb.Nummer.Value)
					//    : null;
					//if (customerUserAccessDb == null || customerUserAccessDb.Id != user.Id)
					//{
					//    throw new Core.Exceptions.UnauthorizedException();
					//}
					#endregion

					Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess.Insert(new Infrastructure.Data.Entities.Tables.PRS.CustomerUserEntity()
					{
						Id = -1,
						CustomerNumber = customerDb.Nummer ?? -1,
						UserId = replacementDb.Id,
						IsPrimary = false,
						ValidFromTime = data.ValidFromTime,
						ValidIntoTime = data.ValidIntoTime
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
