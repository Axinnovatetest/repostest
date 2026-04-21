using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class GetPosABLinkBlanketHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<PosRahmenABModel>>>
	{
		private int _data { get; set; }

		private Identity.Models.UserModel _user { get; set; }
		public GetPosABLinkBlanketHandler(Identity.Models.UserModel user, int Nr)
		{
			this._user = user;
			this._data = Nr;


		}
		public ResponseModel<List<PosRahmenABModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<PosRahmenABModel>();

				var abEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				var abItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data, false);

				if(abItemsEntities != null && abItemsEntities.Count > 0)
				{
					foreach(var item in abItemsEntities)
					{
						if(item.ABPoszuRAPos.HasValue && item.ABPoszuRAPos.Value != -1 && item.ABPoszuRAPos.Value != 0)
						{
							var abArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.ArtikelNr ?? -1);
							var raItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(item.ABPoszuRAPos.Value);
							var raItemExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(raItemEntity.Nr);
							response.Add(new PosRahmenABModel
							{
								Nr = item.Nr,
								Bezeichnung1 = item.Bezeichnung1,
								Bezeichnung2 = item.Bezeichnung2,
								Bezeichnung3 = item.Bezeichnung3,
								Position = item.Position,
								ArtikelNummer = abArticleEntity.ArtikelNummer,
								ArtieklNr = item.ArtikelNr,
								RefWert = item.Gesamtpreis,
								Einheit = item.Einheit,
								Geliefert = item.Geliefert,
								Menge = item.OriginalAnzahl,
								ValidFrom = raItemExtensionEntity?.GultigAb,
								DateOfExpiry = raItemExtensionEntity?.GultigBis,
								WarungName = raItemExtensionEntity?.WahrungName
							});
						}
					}
				}

				response = response.OrderBy(r => r.Position).ToList();

				return ResponseModel<List<PosRahmenABModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<PosRahmenABModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<PosRahmenABModel>>.AccessDeniedResponse();
			}

			var ABEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
			if(ABEntity == null)
				return ResponseModel<List<PosRahmenABModel>>.FailureResponse("AB not found");
			return ResponseModel<List<PosRahmenABModel>>.SuccessResponse();
		}

	}
}
