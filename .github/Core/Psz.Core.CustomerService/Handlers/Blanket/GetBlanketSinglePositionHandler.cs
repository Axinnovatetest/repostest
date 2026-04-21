using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetBlanketSinglePositionHandler: IHandle<Identity.Models.UserModel, ResponseModel<BlanketItem>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetBlanketSinglePositionHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<BlanketItem> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new BlanketItem();
				var blanketItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data);
				var blanketItemExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(_data);
				var wahrungEntity = Infrastructure.Data.Access.Tables.BSD.WahrungenAccess.GetBySymbole("€");


				if(blanketItemExtension == null)
				{
					var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(blanketItem?.ArtikelNr ?? -1);
					Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity()
					{
						AngeboteArtikelNr = blanketItem.Nr,
						Bezeichnung = blanketItem.Bezeichnung1,
						Gesamtpreis = blanketItem.Gesamtpreis,
						GultigAb = DateTime.MinValue,
						GultigBis = DateTime.MaxValue,
						ExtensionDate = DateTime.MaxValue,
						Id = -1,
						KundenMatNummer = null,
						Material = articleEntity?.ArtikelNummer ?? "",
						ME = articleEntity?.Einheit ?? "",
						Preis = null, // item.VKEinzelpreis // item.Einzelpreis // depending on ontract Direction
						WahrungName = wahrungEntity?.Wahrung ?? "",
						WahrungSymbole = wahrungEntity?.Symbol ?? "",
						WahrungId = wahrungEntity?.Nr ?? -1,
						Zielmenge = blanketItem.OriginalAnzahl,
						RahmenNr = blanketItem.AngebotNr ?? -1,
					});
				}

				response = new BlanketItem(blanketItem, blanketItemExtension);
				return ResponseModel<BlanketItem>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<BlanketItem> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<BlanketItem>.AccessDeniedResponse();
			}
			var blanketItem = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data);
			if(blanketItem == null)
				return ResponseModel<BlanketItem>.FailureResponse("Blanket position Not Found");

			return ResponseModel<BlanketItem>.SuccessResponse();
		}
	}
}
