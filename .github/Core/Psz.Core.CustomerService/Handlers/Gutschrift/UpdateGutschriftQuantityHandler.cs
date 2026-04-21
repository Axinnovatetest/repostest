using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class UpdateGutschriftQuantityHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private UpdateGutschriftPositionRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateGutschriftQuantityHandler(Identity.Models.UserModel user, UpdateGutschriftPositionRequestModel data)
		{
			this._user = user;
			this._data = data;
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

				var gutschriftPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.PositionNr);


				gutschriftPositionEntity.OriginalAnzahl = _data.Quantity;
				gutschriftPositionEntity.Anzahl = _data.Quantity;
				gutschriftPositionEntity.erledigt_pos = false;
				gutschriftPositionEntity.GSWithoutCopper = this._data.WithoutCopper;

				// - 2022-08-04 - update if Price is not FIXED
				// - ???? if (gutschriftPositionEntity.VKFestpreis == true)
				{
					//update calcul
					gutschriftPositionEntity.Gesamtpreis = (gutschriftPositionEntity.Anzahl) / gutschriftPositionEntity.Preiseinheit * gutschriftPositionEntity.Einzelpreis * (1 - gutschriftPositionEntity.Rabatt);

					// 1.5
					gutschriftPositionEntity.Einzelkupferzuschlag = this._data.WithoutCopper == true ? 0 : Math.Round((decimal)(((gutschriftPositionEntity.DEL * 1.01m) - gutschriftPositionEntity.Kupferbasis)
																			  / 100
																			  * (decimal?)gutschriftPositionEntity.EinzelCuGewicht), 2);
					// 1.6 
					gutschriftPositionEntity.GesamtCuGewicht = gutschriftPositionEntity.Anzahl * gutschriftPositionEntity.EinzelCuGewicht;
					gutschriftPositionEntity.Einzelpreis = gutschriftPositionEntity.VKFestpreis.HasValue && gutschriftPositionEntity.VKFestpreis.Value == true
						? gutschriftPositionEntity.VKEinzelpreis
						: gutschriftPositionEntity.Einzelkupferzuschlag * gutschriftPositionEntity.Preiseinheit + gutschriftPositionEntity.VKEinzelpreis;

					// 1.7
					gutschriftPositionEntity.Gesamtpreis = gutschriftPositionEntity.Einzelpreis / gutschriftPositionEntity.Preiseinheit * gutschriftPositionEntity.Anzahl * (1 - gutschriftPositionEntity.Rabatt);
					gutschriftPositionEntity.Gesamtkupferzuschlag = gutschriftPositionEntity.VKFestpreis.HasValue && gutschriftPositionEntity.VKFestpreis.Value == true
						? 0
						: gutschriftPositionEntity.Anzahl * gutschriftPositionEntity.Einzelkupferzuschlag;
					gutschriftPositionEntity.VKGesamtpreis = gutschriftPositionEntity.OriginalAnzahl * gutschriftPositionEntity.VKEinzelpreis / gutschriftPositionEntity.Preiseinheit;
				}

				// - 
				var response = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(gutschriftPositionEntity);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			if(_data.Quantity <= 0)
			{
				return ResponseModel<int>.FailureResponse($"Invalid quantity [{_data.Quantity}].");
			}
			var gutschriftEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.GutschriftNr);
			if(gutschriftEntity == null)
				return ResponseModel<int>.FailureResponse("Gutschrift not found.");
			var gutschriftPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.PositionNr);
			if(gutschriftPositionEntity == null)
				return ResponseModel<int>.FailureResponse("Gutschrift item not found.");

			var availableQty = CalculateRechnungAvailableQty(gutschriftPositionEntity.REPoszuGSPos ?? -1, _data.PositionNr);
			if(_data.Quantity > availableQty)
				return ResponseModel<int>.FailureResponse($"Requested quantity [{_data.Quantity}] is bigger then Rechnung available quantity [{availableQty}]");

			return ResponseModel<int>.SuccessResponse();
		}
		public static decimal CalculateRechnungAvailableQty(int NrRechnungPos, int NrGutschriftPos)
		{

			var rechnungPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrRechnungPos);
			var gsPosRechnungList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRechnungPositions(new List<int> { NrRechnungPos }) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();

			var rechnungOriginalQty = rechnungPosEntity.Anzahl ?? 0;
			var gutschriftLinksTakenQty = gsPosRechnungList.Where(x => x.Nr != NrGutschriftPos)?.Sum(x => x.Anzahl) ?? 0;
			var _avaialable = (rechnungOriginalQty - gutschriftLinksTakenQty);
			return _avaialable;
		}
	}
}
