using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class GetGutschriftItems : IHandle<Identity.Models.UserModel , ResponseModel<List<GutschriftItemModel>>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetGutschriftItems(Identity.Models.UserModel user , int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<GutschriftItemModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if (!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<GutschriftItemModel>();
				var PositionsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data);
				if (PositionsEntities != null && PositionsEntities.Count > 0)
				{
					foreach (var item in PositionsEntities)
					{
						var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.ArtikelNr ?? -1);
						var availableQty = CalculateRechnungAvailableQty(item.REPoszuGSPos ?? -1 , item.Nr);
						response.Add(new GutschriftItemModel
						{
							Nr = item.Nr ,
							Position = item.Position ,
							ItemNumber = item.ArtikelNr ,
							ItemNummer = article?.ArtikelNummer ,
							KundenIndex = item.Index_Kunde ,
							Designation = item.Bezeichnung1 ,
							OriginalOrderQuantity = item.OriginalAnzahl ,
							DesiredDate = item.Wunschtermin ,
							DelivredQuantity = item.Geliefert ,
							OpenQuantity = availableQty ,
							DeliveryDate = item.Liefertermin ,
							Done = item.erledigt_pos ,
							UnitPriceBasis = item.Preiseinheit ,
							OpenQuantity_UnitPrice = item.Einzelpreis ,
							OpenQuantity_TotalPrice = item.Gesamtpreis ,
							VAT = item.USt ,
							Discount = item.Rabatt ,
						});
					}
				}

				return ResponseModel<List<GutschriftItemModel>>.SuccessResponse(response);
			}
			catch (Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<GutschriftItemModel>> Validate()
		{
			if (this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<GutschriftItemModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GutschriftItemModel>>.SuccessResponse();
		}

		public static decimal CalculateRechnungAvailableQty(int NrRechnungPos , int NrGutschriftPos)
		{

			var rechnungPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrRechnungPos);
			var gsPosRechnungList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRechnungPositions(new List<int> { NrRechnungPos }) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();

			var rechnungOriginalQty = rechnungPosEntity?.Anzahl ?? 0;
			var gutschriftLinksTakenQty = gsPosRechnungList.Sum(x => x.Anzahl) ?? 0;
			var _avaialable = (rechnungOriginalQty - gutschriftLinksTakenQty);
			return _avaialable;
		}
	}
}
