using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.Purchase.Handlers.Production
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetPositionManufacturingFacilityHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetPositionManufacturingFacilityHandler(Identity.Models.UserModel user, int positionStorageLocation)
		{
			this._user = user;
			this._data = positionStorageLocation;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = getManucaturingFacilityId(Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data));


				return ResponseModel<int>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var errors = new List<ResponseModel<int>.ResponseError>();
			var storageLocation = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data);
			if(storageLocation == null)
			{
				errors.Add(new ResponseModel<int>.ResponseError() { Key = "", Value = "Position not found" });
			}

			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}

			return ResponseModel<int>.SuccessResponse();
		}

		internal int getManucaturingFacilityId(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angeboteneArtikelEntity)
		{
			var errors = new List<string> { };
			if(!angeboteneArtikelEntity.ArtikelNr.HasValue || !int.TryParse(angeboteneArtikelEntity.ArtikelNr.ToString(), out var articleNr))
			{
				errors.Add("Invalid Article nummer");
				return -1;
			}

			var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleNr);
			if(article == null)
			{
				errors.Add("Article not found");
				return -1;
			}

			if(string.IsNullOrEmpty(article.ArtikelNummer) || string.IsNullOrWhiteSpace(article.ArtikelNummer)
				|| article.ArtikelNummer.Length < 3)
			{
				errors.Add("Invalid article nummer");
				return -1;
			}

			var article_ganz = article.ArtikelNummer;                                   // Artikel_ganz = DLookup("Artikelnummer", "Artikel", "[Artikel-Nr]=" & Forms![angebote - artikel]![Artikel-Nr])
			var land = article_ganz.Substring(Math.Max(0, article_ganz.Length - 2));    // Land = Right(Artikel_ganz, 2)
			var laenge = article_ganz.Length;                                           // Laenge = Len(Artikel_ganz)
			var artikelWolf = article_ganz.Substring(0, Math.Max(0, 3));                 // ArtikelWolf = Left(Artikel_ganz, 3)

			if(!angeboteneArtikelEntity.Lagerort_id.HasValue || !int.TryParse(angeboteneArtikelEntity.Lagerort_id.ToString(), out var storageLocationId))
			{
				errors.Add("Invalid Storage location");
				return -1;
			}

			if(angeboteneArtikelEntity.Lagerort_id == 4)
			{
				if(artikelWolf == "825" || artikelWolf == "985" || artikelWolf == "984" || artikelWolf == "884" || artikelWolf == "887")
				{
					return 42;
				}
				else if(artikelWolf == "948" || artikelWolf == "928" || artikelWolf == "929" || artikelWolf == "957")
				{
					return 60;
				}
				else
				{
					return 7;
				}
			}
			else if(angeboteneArtikelEntity.Lagerort_id == 3)
			{
				return 6;
			}
			else if(angeboteneArtikelEntity.Lagerort_id == 24)
			{
				return 26;
			}
			else if(angeboteneArtikelEntity.Lagerort_id == 15)
			{
				return 15;
			}
			else
			{
				return 6;
			}
		}
	}
}
