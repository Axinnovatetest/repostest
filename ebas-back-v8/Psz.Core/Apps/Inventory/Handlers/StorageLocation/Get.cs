using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Inventory.Handlers
{
	public partial class StorageLocation
	{
		internal static List<Models.StorageLocation.StorageLocationModel> Get()
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.INV.LagerorteAccess.Get());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static List<Models.StorageLocation.StorageLocationModel> GetSpecific()
		{
			try
			{
				return Get(Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetLagortes());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		internal static List<Models.StorageLocation.StorageLocationModel> Get(List<Infrastructure.Data.Entities.Tables.INV.LagerorteEntity> storageLocationsDb)
		{
			try
			{
				var response = new List<Models.StorageLocation.StorageLocationModel>();

				foreach(var storageLocationDb in storageLocationsDb)
				{
					if(storageLocationDb.Lagerort.ToLower().StartsWith("haupt"))
					{
						response.Add(new Models.StorageLocation.StorageLocationModel()
						{
							Id = storageLocationDb.LagerortId,
							Name = storageLocationDb.Lagerort,
							IsStandard = storageLocationDb.Standard ?? false
						});
					}
				}

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
