using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Customers
	{
		public static Core.Models.ResponseModel<List<Models.Customers.CustomerModel>> Search(string searchText)
		{
			try
			{
				var adressesDb = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetLikeNames(searchText);
				var customersNumbers = adressesDb.Select(e => e.Nr).ToList();

				return Get(Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummers(customersNumbers));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
