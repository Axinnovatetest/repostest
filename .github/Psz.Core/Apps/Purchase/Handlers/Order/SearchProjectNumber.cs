using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<List<KeyValuePair<string, string>>> SearchProjectNumber(string searchText,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null /*|| !user.Access.EDI.ModuleActivated*/)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				return new Core.Models.ResponseModel<List<KeyValuePair<string, string>>>()
				{
					Success = true,
					Body = SearchProjectNumberInternal(searchText, user)
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static List<KeyValuePair<string, string>> SearchProjectNumberInternal(string searchText,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				if(searchText == null || string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText) || searchText.Trim().Length < 2)
				{
					return new List<KeyValuePair<string, string>>();
				}

				searchText = searchText.Trim();

				var response = new List<KeyValuePair<string, string>>();

				var searchTextLowerCase = searchText.ToLower();

				var listTupleCustomerProjektNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get_TupleKundenNrProjektNr_Like_ProjektNr(searchText)
					.OrderBy(e => e.Item2)
					.ToList();

				// > Filter orders by user's customers
				if(!user.Access.Purchase.AllCustomers)
				{
					var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);
					var userCustomersNumbers = userCustomers.Select(e => e.CustomerNumber).ToList();

					listTupleCustomerProjektNr = listTupleCustomerProjektNr.FindAll(e => userCustomersNumbers.Contains(e.Item1));
				}

				var usedList = new List<string>();
				foreach(var tupleCustomerProjektNr in listTupleCustomerProjektNr)
				{
					if(!usedList.Contains(tupleCustomerProjektNr.Item2))
					{
						response.Add(new KeyValuePair<string, string>(tupleCustomerProjektNr.Item2, tupleCustomerProjektNr.Item2));
					}
				}

				response = response.Distinct().ToList();

				return response;
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
