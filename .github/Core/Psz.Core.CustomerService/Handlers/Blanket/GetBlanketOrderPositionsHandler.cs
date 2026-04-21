using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetBlanketOrderPositionsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<BlanketItem>>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBlanketOrderPositionsHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}
		public ResponseModel<List<BlanketItem>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<BlanketItem>();
				var rahmenExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(_data);
				var BlanketDetailsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data, false).OrderBy(x => x.Position).ToList();
				;
				var BlanketDetailsExtensionEntities = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenNr(new List<int> { _data });
				var wahrungEntity = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.GetBySymbole("€");

				// - for Old Add extensions
				var newExtensions = new List<Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity>();
				//var newExtensionsArticleIds = new List<int>();
				// - add missing items
				if(BlanketDetailsEntities != null && BlanketDetailsEntities.Count > 0)
				{
					foreach(var item in BlanketDetailsEntities)
					{
						var extension = BlanketDetailsExtensionEntities.Find(x => x.AngeboteArtikelNr == item.Nr);
						if(extension == null)
						{
							var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get((int)item.ArtikelNr);
							newExtensions.Add(new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity
							{
								AngeboteArtikelNr = item.Nr,
								RahmenNr = _data,
								Bezeichnung = item.Bezeichnung2,
								Gesamtpreis = item.Gesamtpreis,
								GultigAb = new DateTime(1900, 1, 1),
								GultigBis = new DateTime(2199, 1, 1),
								ExtensionDate = new DateTime(2199, 1, 1),
								Id = -1,
								KundenMatNummer = item.Bezeichnung1,
								Material = article.ArtikelNummer,
								MaterialNr = item.ArtikelNr ?? -1,
								ME = item.Einheit,
								WahrungName = wahrungEntity?.Wahrung ?? "",
								WahrungSymbole = wahrungEntity?.Symbol ?? "",
								WahrungId = wahrungEntity?.Nr ?? -1,
								Zielmenge = item.OriginalAnzahl,
								GesamtpreisDefault = item.Gesamtpreis,
								Preis = item.Einzelpreis,
								PreisDefault = item.Einzelpreis, // - 2022 11 30 - init for P3000 Legacy RA
								BasePrice = item.VKEinzelpreis, // - 2022 11 30 - init for P3000 Legacy RA
							});
						}
					}
					// save new extensions
					Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.Insert(newExtensions);
				}
				var __BlanketDetailsExtensionEntities = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenNr(new List<int> { _data });
				foreach(var item in BlanketDetailsEntities)
				{
					var extension = __BlanketDetailsExtensionEntities.Find(x => x.AngeboteArtikelNr == item.Nr);
					response.Add(new BlanketItem(item, extension));

				}
				if(response != null && response.Count > 0 && rahmenExtensionEntity != null && rahmenExtensionEntity.StatusId == (int)Enums.BlanketEnums.RAStatus.Validated)
					response = SetPrice(response);
				return ResponseModel<List<BlanketItem>>.SuccessResponse(response);


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<BlanketItem>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<BlanketItem>>.AccessDeniedResponse();
			}

			var blanketEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
			if(blanketEntity == null)
				return ResponseModel<List<BlanketItem>>.FailureResponse("Order not found");




			// -
			return ResponseModel<List<BlanketItem>>.SuccessResponse();
		}

		public static List<BlanketItem> SetPrice(List<BlanketItem> data)
		{
			foreach(var item in data)
			{
				var _priceHistory = Infrastructure.Data.Access.Tables.CTS.RahmenPriceHistoryAccess.GetByMaxPriceAndDate(item.AngeboteArtiklNr, DateTime.Now);
				if(_priceHistory != null && _priceHistory.Count > 0)
				{
					var right_price = _priceHistory[0];
					item.Preis = right_price.Price;
					item.PreisDefault = right_price.PriceDefault;
					item.ValidFrom = right_price.ValidFrom;
					item.WahrungSymbole = right_price.WarungSymbol;
				}
			}
			return data;
		}

	}
}
