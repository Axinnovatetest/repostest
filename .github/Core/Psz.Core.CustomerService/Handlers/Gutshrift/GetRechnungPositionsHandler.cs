using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class GetRechnungPositionsHandler : IHandle<Identity.Models.UserModel , ResponseModel<RechnungPositionsModel>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetRechnungPositionsHandler(Identity.Models.UserModel user , int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<RechnungPositionsModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if (!validationResponse.Success)
				{
					return validationResponse;
				}

				var gutschriftEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				var rechnungEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get((int) gutschriftEntity.Nr_rec);
				var rechnungItemsEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(rechnungEntity.Nr);

				var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get((int) gutschriftEntity.Kunden_Nr);
				var response = new RechnungPositionsModel
				{
					Nr = gutschriftEntity.Nr ,
					AngebotNr = gutschriftEntity.Angebot_Nr ?? 0 ,
					DocumentNr = gutschriftEntity.Bezug ,
					CustomerName = adressenEntity.Name1 ,
					Customernumber = adressenEntity.Kundennummer ?? 0 ,
					Items = new List<GutschriftItemModel>() ,
				};
				if (rechnungItemsEntities != null && rechnungItemsEntities.Count > 0)
				{
					foreach (var item in rechnungItemsEntities)
					{
						var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(item.ArtikelNr ?? -1);
						var availableQty = CalculateRechnungAvailableQty(item.Nr);
						response.Items.Add(new GutschriftItemModel
						{
							Nr = item.Nr ,
							Position = item.Position ,
							ItemNumber = item.ArtikelNr ,
							ItemNummer = article?.ArtikelNummer ,
							KundenIndex = item.Index_Kunde ,
							Designation = item.Bezeichnung1 ,
							OriginalOrderQuantity = item.Anzahl ,
							DesiredDate = item.Wunschtermin ,
							DelivredQuantity = item.Geliefert ,
							OpenQuantity = availableQty ,
							DeliveryDate = item.Liefertermin ,
							Done = item.erledigt_pos ,
							UnitPriceBasis = item.Preiseinheit ,
							OpenQuantity_UnitPrice = item.Einzelpreis ,
							OpenQuantity_TotalPrice = item.Gesamtpreis ,
							VAT = Math.Round(Convert.ToDecimal(item.USt ?? 0) * 100 , 2) ,
							Discount = Math.Round(Convert.ToDecimal(item.Rabatt ?? 0) * 100 , 2) ,
						});
					}
				}

				return ResponseModel<RechnungPositionsModel>.SuccessResponse(response);
			}
			catch (Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<RechnungPositionsModel> Validate()
		{
			if (this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<RechnungPositionsModel>.AccessDeniedResponse();
			}
			var gutschriftEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if (gutschriftEntity == null)
				return ResponseModel<RechnungPositionsModel>.FailureResponse("gutschrift not found .");
			var rechnungEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get((int) gutschriftEntity.Nr_rec);
			if (rechnungEntity == null)
				return ResponseModel<RechnungPositionsModel>.FailureResponse("rechnung not found .");
			return ResponseModel<RechnungPositionsModel>.SuccessResponse();
		}

		public static decimal CalculateRechnungAvailableQty(int NrRechnungPos)
		{

			var rechnungPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(NrRechnungPos);
			var gsPosRechnungList = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyRechnungPositions(new List<int> { NrRechnungPos }) ?? new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();

			var rechnungOriginalQty = rechnungPosEntity.Anzahl ?? 0;
			var gutschriftLinksTakenQty = gsPosRechnungList?.Sum(x => x.Anzahl) ?? 0;
			var _avaialable = (rechnungOriginalQty - gutschriftLinksTakenQty);
			return _avaialable;
		}
	}
}
