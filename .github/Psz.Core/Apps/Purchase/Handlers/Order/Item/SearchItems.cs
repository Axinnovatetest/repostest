using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<List<KeyValuePair<int, string>>> SearchItems(string searchText,
			Core.Identity.Models.UserModel user)
		{
			try
			{
				if(user == null || !user.Access.Purchase.ModuleActivated
					|| !user.Access.CustomerService.ModuleActivated)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				if(searchText.Trim().Length < 2)
				{
					return new Core.Models.ResponseModel<List<KeyValuePair<int, string>>>()
					{
						Success = true,
						Body = new List<KeyValuePair<int, string>>()
					};
				}

				searchText = searchText.Trim();

				var responseBody = new List<KeyValuePair<int, string>>();

				var itemsDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumber(searchText)
					.FindAll(e => e.Freigabestatus != "O");

				var searchTextLowerCase = searchText.ToLower();

				itemsDb = itemsDb.OrderBy(e => e.ArtikelNummer.ToLower()
					.StartsWith(searchTextLowerCase) ? 0 : 1)
					.ToList();

				foreach(var itemDb in itemsDb)
				{
					responseBody.Add(new KeyValuePair<int, string>(itemDb.ArtikelNr, itemDb.ArtikelNummer));
				}

				return new Core.Models.ResponseModel<List<KeyValuePair<int, string>>>()
				{
					Success = true,
					Body = responseBody
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public static Core.Models.ResponseModel<List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>> SearchArtikelsItems(string searchText,
			Core.Identity.Models.UserModel currentUser)
		{
			try
			{
				if(currentUser == null
					|| !currentUser.Access.Purchase.ModuleActivated)
				{
					throw new Core.Exceptions.UnauthorizedException();
				}

				if(searchText.Trim().Length < 2)
				{
					return new Core.Models.ResponseModel<List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>>()
					{
						Success = true,
						Body = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>()
					};
				}

				searchText = searchText.Trim();

				var responseBody = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();

				var itemsDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetLikeNumber(searchText);

				var searchTextLowwer = searchText.ToLower();

				itemsDb = itemsDb.OrderBy(m => m.ArtikelNummer.ToLower()
					.StartsWith(searchTextLowwer) ? 0 : 1)
					.ToList();

				foreach(var itemDb in itemsDb)
				{
					responseBody.Add(itemDb);
				}

				return new Core.Models.ResponseModel<List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>>()
				{
					Success = true,
					Body = responseBody
				};
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
