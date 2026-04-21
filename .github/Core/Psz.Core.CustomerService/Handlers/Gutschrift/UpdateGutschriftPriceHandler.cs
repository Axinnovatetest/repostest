using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Gutshrift;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.Gutshrift
{
	public class UpdateGutschriftPriceHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private UpdateGutschriftPriceRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateGutschriftPriceHandler(Identity.Models.UserModel user, UpdateGutschriftPriceRequestModel data)
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

				gutschriftPositionEntity.erledigt_pos = false;
				//gutschriftPositionEntity.VKFestpreis = _data.PriceFixed;
				gutschriftPositionEntity.EKPreise_Fix = _data.PriceFixed; // - 2022-09-14 save manual change

				// - 2022-08-04 - update if Price is not FIXED
				if(_data.PriceFixed != true)
				{
					var rechnungPosEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(gutschriftPositionEntity.REPoszuGSPos ?? -1);
					_data.UnitPrice = rechnungPosEntity?.VKEinzelpreis ?? 0;
				}

				// - update calcul
				updatePrice(gutschriftPositionEntity, _data.UnitPrice, _data.PriceFixed, this._data.WithoutCopper);

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
			if(_data.PriceFixed && _data.UnitPrice <= 0)
			{
				return ResponseModel<int>.FailureResponse($"Invalid price [{_data.UnitPrice}].");
			}
			var gutschriftEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data.GutschriftNr);
			if(gutschriftEntity == null)
				return ResponseModel<int>.FailureResponse("Gutschrift not found.");
			var gutschriftPositionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(_data.PositionNr);
			if(gutschriftPositionEntity == null)
				return ResponseModel<int>.FailureResponse("Gutschrift item not found.");

			return ResponseModel<int>.SuccessResponse();
		}

		internal void updatePrice(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity itemDb, decimal newUnitPrice, bool isNewPrice, bool withoutCopper = false, bool withoutVAT = false)
		{
			var artikelEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(itemDb.ArtikelNr ?? -1);
			var rechnungItemEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(itemDb.REPoszuGSPos ?? -1);
			var itemPricingGroupsDb = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNr(artikelEntity.ArtikelNr);
			var discount = itemDb.Rabatt ?? 0;
			var fixedPrice = isNewPrice; // - 2023-03-22 - rechnungItemEntity.VKFestpreis ?? false;
			var unitPriceBasis = itemDb.Preiseinheit ?? 1;
			var cuWeight = Convert.ToDecimal(artikelEntity.CuGewicht ?? 0);
			var del = (itemDb.DEL ?? 0);

			#region >>> pricing <<<<
			var me1 = 0m;
			var me2 = 0m;
			var me3 = 0m;
			var me4 = 0m;
			var pm1 = 0m;
			var pm2 = 0m;
			var pm3 = 0m;
			var pm4 = 0m;
			var verkaufspreis = 0m;
			if(itemPricingGroupsDb != null)
			{
				me1 = Convert.ToDecimal(itemPricingGroupsDb.ME1 ?? 0m);
				me2 = Convert.ToDecimal(itemPricingGroupsDb.ME2 ?? 0m);
				me3 = Convert.ToDecimal(itemPricingGroupsDb.ME3 ?? 0m);
				me4 = Convert.ToDecimal(itemPricingGroupsDb.ME4 ?? 0m);
				pm1 = Convert.ToDecimal(itemPricingGroupsDb.PM1 ?? 0m);
				pm2 = Convert.ToDecimal(itemPricingGroupsDb.PM2 ?? 0m);
				pm3 = Convert.ToDecimal(itemPricingGroupsDb.PM3 ?? 0m);
				pm4 = Convert.ToDecimal(itemPricingGroupsDb.PM4 ?? 0m);
			}
			verkaufspreis = isNewPrice ? newUnitPrice : rechnungItemEntity.VKEinzelpreis ?? 0; // - 2022-08-04 - allow fixed Price // rechnungItemEntity.VKEinzelpreis ?? 0;//Convert.ToDecimal(itemPricingGroupsDb.Verkaufspreis ?? 0m);

			var singleCopperSurcharge = withoutCopper == true ? 0 : Common.Helpers.CTS.BlanketHelpers.CalculateSingleCopperSurcharge(fixedPrice,
					del,
					cuWeight);
			var totalCopperSurcharge = Common.Helpers.CTS.BlanketHelpers.CalculateTotalCopperSurcharge(fixedPrice,
				itemDb.Anzahl ?? 0,
				singleCopperSurcharge);
			var vkUnitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkUnitPrice(fixedPrice,
				verkaufspreis,
				 itemDb.Anzahl ?? 0,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);
			var unitPrice = Common.Helpers.CTS.BlanketHelpers.CalculateUnitPrice(fixedPrice,
				unitPriceBasis,
				itemDb.Anzahl ?? 0,
				vkUnitPrice,
				verkaufspreis,
				singleCopperSurcharge,
				me1,
				me2,
				me3,
				me4,
				pm2,
				pm3,
				pm4);
			var totalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateTotalPrice(unitPriceBasis,
				unitPrice,
				itemDb.Anzahl ?? 0,
				discount);
			var vKTotalPrice = Common.Helpers.CTS.BlanketHelpers.CalculateVkTotalPrice(unitPriceBasis,
				vkUnitPrice,
				itemDb.Anzahl ?? 0);
			var totalCuWeight = Common.Helpers.CTS.BlanketHelpers.CalculateTotalWeight(itemDb.Anzahl ?? 0,
				cuWeight);
			#endregion pricing


			// - 
			itemDb.Kupferbasis = 150;
			itemDb.Preiseinheit = unitPriceBasis == 0 ? 1 : unitPriceBasis;
			itemDb.VKFestpreis = fixedPrice;
			itemDb.Einzelkupferzuschlag = (decimal)singleCopperSurcharge;
			itemDb.GesamtCuGewicht = (decimal)totalCuWeight;
			itemDb.Einzelpreis = (decimal)unitPrice;
			itemDb.VKEinzelpreis = (decimal)vkUnitPrice;
			itemDb.Gesamtpreis = (decimal)totalPrice;
			itemDb.Gesamtkupferzuschlag = (decimal)totalCopperSurcharge;
			itemDb.VKGesamtpreis = (decimal)vKTotalPrice;
			// -
			itemDb.GSWithoutCopper = withoutCopper;
		}
	}
}
