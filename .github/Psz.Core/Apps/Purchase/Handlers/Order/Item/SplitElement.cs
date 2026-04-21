using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<int> SplitElement(int id)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					var elementDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(id);
					if(elementDb == null)
					{
						return new Core.Models.ResponseModel<int>()
						{
							Errors = new List<string> { "Position is not found" }
						};
					}
					var itemDb = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(elementDb.ArtikelNr.HasValue ? elementDb.ArtikelNr.Value : -1);
					if(itemDb == null)
					{
						return new Core.Models.ResponseModel<int>()
						{
							Errors = new List<string>() { "Item not found" }
						};
					}
					if(itemDb.Freigabestatus.ToUpper() == "O")
					{
						return new Core.Models.ResponseModel<int>()
						{
							Errors = new List<string>() { "Item is 'Obsolete'" }
						};
					}
					if(elementDb.Fertigungsnummer.HasValue && elementDb.Fertigungsnummer.Value > 0)
					{
						return new Core.Models.ResponseModel<int>()
						{
							Errors = new List<string> { "Element command already validated" }
						};
					}

					var elementDbCopy = elementDb;

					elementDbCopy.PositionZUEDI = elementDb.Nr; // link the two positions
					elementDbCopy.Nr = -1;
					elementDbCopy.Position = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetNextPositionNumberForOrder((int)elementDbCopy.AngebotNr);

					var copyId = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Insert(elementDbCopy);

					return Core.Models.ResponseModel<int>.SuccessResponse(copyId);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
					return new Core.Models.ResponseModel<int>()
					{
						Errors = new List<string> { "Something went wrong" }
					};
				}
			}
		}
	}
}
