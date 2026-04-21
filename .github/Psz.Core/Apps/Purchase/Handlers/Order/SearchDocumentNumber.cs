using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<List<KeyValuePair<string, string>>> SearchDocumentNumber(string searchText,
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
					Body = SearchDocumentNumberInternal(searchText, user)
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		internal static List<KeyValuePair<string, string>> SearchDocumentNumberInternal(string searchText,
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

				var listTupleCustomerBezug =
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get_TupleKundenNrBezug_Like_Bezug(searchText)
					.OrderBy(e => e.Item2.StartsWith(searchTextLowerCase) ? 0 : 1)
					.ToList();

				// > Filter orders by user's customers
				if(!user.Access.Purchase.AllCustomers)
				{
					var userCustomers = Core.Apps.EDI.Handlers.Customers.GetUserCustomersInternal(user);
					var userCustomersNumbers = userCustomers.Select(e => e.CustomerNumber).ToList();

					listTupleCustomerBezug = listTupleCustomerBezug.FindAll(e => userCustomersNumbers.Contains(e.Item1));
				}

				var usedList = new List<string>();
				foreach(var tupleCustomerBezug in listTupleCustomerBezug)
				{
					if(!usedList.Contains(tupleCustomerBezug.Item2))
					{
						response.Add(new KeyValuePair<string, string>(tupleCustomerBezug.Item2, tupleCustomerBezug.Item2));
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
