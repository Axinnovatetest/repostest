using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetPosBestellLinkBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<PosBestellLinkBlanketModel>>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetPosBestellLinkBlanketHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<PosBestellLinkBlanketModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<PosBestellLinkBlanketModel>();
				var bestellPositions = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetByOrderId(_data);

				if(bestellPositions?.Count > 0)
				{
					var artikelEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(bestellPositions.Select(x => x.Artikel_Nr ?? -1).ToList());
					var raPosEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(bestellPositions.Select(x => x.RA_Pos_zu_Bestellposition ?? -1).ToList());
					var raEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(raPosEntities?.Select(x => x.AngebotNr ?? -1)?.ToList());

					foreach(var item in bestellPositions)
					{
						var artikelEntity = artikelEntities.FirstOrDefault(x => x.ArtikelNr == item.Artikel_Nr);
						var raPosEntity = raPosEntities.FirstOrDefault(x => x.Nr == item.RA_Pos_zu_Bestellposition);
						var raEntity = raEntities.FirstOrDefault(x => x.Nr == raPosEntity?.AngebotNr);

						response.Add(new PosBestellLinkBlanketModel(item, artikelEntity, raEntity, raPosEntity));
					}
				}

				return ResponseModel<List<PosBestellLinkBlanketModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<PosBestellLinkBlanketModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<PosBestellLinkBlanketModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<PosBestellLinkBlanketModel>>.SuccessResponse();
		}

	}
}
