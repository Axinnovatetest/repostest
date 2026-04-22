using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<List<KeyValuePair<string, string>>> SearchPrefailNumber(string searchText,
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
					Body = SearchPrefailNumberInternal(searchText, user)
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static List<KeyValuePair<string, string>> SearchPrefailNumberInternal(string searchText,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				if(searchText == null || string.IsNullOrEmpty(searchText) || string.IsNullOrWhiteSpace(searchText) || searchText.Trim().Length < 2)
				{
					return new List<KeyValuePair<string, string>>();
				}

				searchText = (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText)) ? searchText.Trim() : searchText;

				var response = new List<KeyValuePair<string, string>>();

				var searchTextLowerCase = (!string.IsNullOrEmpty(searchText) && !string.IsNullOrWhiteSpace(searchText)) ? searchText.ToLower() : searchText;

				var listTupleCustomerAngebotNr = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get_TupleKundenNrAngebotNr_Like_AngebotNr(searchText)
					.OrderBy(e => e.Item2.StartsWith(searchTextLowerCase) ? 0 : 1)
					.ToList();

				// > Filter orders by user's customers
				if(!user.Access.Purchase.AllCustomers)
				{
					var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);
					var userCustomersNumbers = userCustomers.Select(e => e.CustomerNumber).ToList();

					listTupleCustomerAngebotNr = listTupleCustomerAngebotNr.FindAll(e => userCustomersNumbers.Contains(e.Item1));
				}

				var usedList = new List<string>();
				foreach(var tupleCustomerAngebotNr in listTupleCustomerAngebotNr)
				{
					if(!usedList.Contains(tupleCustomerAngebotNr.Item2))
					{
						response.Add(new KeyValuePair<string, string>(tupleCustomerAngebotNr.Item2, tupleCustomerAngebotNr.Item2));
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
