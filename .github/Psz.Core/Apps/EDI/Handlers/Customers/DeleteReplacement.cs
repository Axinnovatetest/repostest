using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<object> DeleteReplacement(Models.Customers.DeleteReplacementModel data,
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

					var replacementDb = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(data.ReplacementId);
					if(replacementDb == null || replacementDb.IsArchived)
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

					Infrastructure.Data.Access.Tables.PRS.CustomerUserAccess
						.DeleteBy_CustomerNumber_UserId(data.CustomerId, replacementDb.Id);

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
