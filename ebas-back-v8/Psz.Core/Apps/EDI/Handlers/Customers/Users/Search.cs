using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public partial class Users
		{
			public static Core.Models.ResponseModel<List<Models.Customers.Users.UserModel>> Search(string searchText,
				Core.Identity.Models.UserModel user)
			{
				try
				{
					if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var usersDb = Infrastructure.Data.Access.Tables.COR.UserAccess.GetLikeName(searchText);

					return Core.Models.ResponseModel<List<Models.Customers.Users.UserModel>>.SuccessResponse(Get(usersDb));
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					return Core.Models.ResponseModel<List<Models.Customers.Users.UserModel>>.UnexpectedErrorResponse();
				}
			}
		}
	}
}
