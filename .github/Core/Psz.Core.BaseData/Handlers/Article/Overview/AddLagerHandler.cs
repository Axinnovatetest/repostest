using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class AddLagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public AddLagerHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				//lock ()
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					// -
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
					var lagerorteEntities = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetCreationLagers();
					var standardLagerEntities = Infrastructure.Data.Access.Tables.PRS.LagerAccess.GetStandardByArticleAndId(articleEntity.ArtikelNr, lagerorteEntities.Select(x => x.Lagerort_id)?.ToList());

					// -
					var newLagers = new List<Infrastructure.Data.Entities.Tables.PRS.LagerEntity>();
					foreach(var lagerortItem in lagerorteEntities)
					{
						// - not existing ones
						if(standardLagerEntities.FindIndex(x => x.Lagerort_id == lagerortItem.Lagerort_id) < 0)
						{
							newLagers.Add(
								new Infrastructure.Data.Entities.Tables.PRS.LagerEntity
								{
									Artikel_Nr = articleEntity.ArtikelNr,
									Lagerort_id = lagerortItem.Lagerort_id,
									Bestand = 0,
									letzte_Bewegung = DateTime.Now,
									Durchschnittspreis = 0,
									CCID = false
								});
						}
					}
					// -
					var id = Infrastructure.Data.Access.Tables.PRS.LagerAccess.Insert(newLagers);
					Infrastructure.Data.Access.Tables.BSD.PSZ_Eingangskontrolle_PrufungenAccess.Insert(
						new Infrastructure.Data.Entities.Tables.BSD.PSZ_Eingangskontrolle_PrufungenEntity
						{
							Artikelnummer = articleEntity.ArtikelNummer,
							Hilfsmittel = "Visuell",
							Prufung = "Viz příchozí složky"
						});

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data);

					// -
					return ResponseModel<int>.SuccessResponse(id);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			// -
			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Article not found"}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
