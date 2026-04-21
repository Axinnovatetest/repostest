using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class StorageLocation
	{
		public static Core.Models.ResponseModel<List<Apps.Inventory.Models.StorageLocation.StorageLocationModel>> Get(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null
					|| (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return new Core.Models.ResponseModel<List<Inventory.Models.StorageLocation.StorageLocationModel>>()
				{
					Success = true,
					Body = Apps.Inventory.Handlers.StorageLocation.Get()
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static Core.Models.ResponseModel<List<Apps.Inventory.Models.StorageLocation.StorageLocationModel>> GetSpecific(Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return new Core.Models.ResponseModel<List<Inventory.Models.StorageLocation.StorageLocationModel>>()
				{
					Success = true,
					Body = Apps.Inventory.Handlers.StorageLocation.GetSpecific()
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
