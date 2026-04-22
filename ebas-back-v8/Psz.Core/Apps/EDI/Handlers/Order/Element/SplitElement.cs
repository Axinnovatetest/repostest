using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<int> SplitElement(int id)
		{
			try
			{
				var elementDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(new List<int> { id }).FirstOrDefault();
				if(elementDb == null)
				{
					return new Core.Models.ResponseModel<int>()
					{
						Errors = new List<string> { "Element is not found" }
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
				// link the two positions
				elementDbCopy.PositionZUEDI = elementDb.Nr;

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
