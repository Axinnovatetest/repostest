using System;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> Confirm(Models.Order.UpdateGlobalDataModel data,
		  Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null/* || !user.Access.EDI.Order*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return UpdateGlobalData(data, true, user);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
