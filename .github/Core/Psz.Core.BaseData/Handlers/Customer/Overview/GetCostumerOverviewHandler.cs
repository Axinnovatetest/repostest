using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Customer.Overview
{
	public class GetCostumerOverviewHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Customer.OverviewModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetCostumerOverviewHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Customer.OverviewModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				lock(Locks.CostumerEditLock.GetOrAdd(this._data, new object()))
				{
					var industryEntity = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get();
					var pszKundengruppenEntities = Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Get();
					//
					var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
					var adressEntity = int.TryParse(kundenEntity.Nummer.ToString(), out int knudenNummer)
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(knudenNummer)
						: null;
					var KundenEntensionEntity = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(kundenEntity.Nr);
					var finalgeolocationEntiy = new Infrastructure.Data.Entities.Tables.PRS.AdressenGeocodingExtensionEntity();

					var geolocationEntiy = Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Get((int)kundenEntity.Nummer);

					if(geolocationEntiy != null)
						finalgeolocationEntiy = geolocationEntiy;
					else
					{
						var newLoc = Infrastructure.Services.Geocoding.Converter.LocationFromAddress($"{adressEntity.StraBe} {adressEntity.Postfach} {adressEntity.Ort} {(adressEntity.Land.ToLower() == "d" ? "Deutschland" : adressEntity.Land)}");
						if(newLoc != null)
						{
							Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Insert(
								new Infrastructure.Data.Entities.Tables.PRS.AdressenGeocodingExtensionEntity
								{
									Longitude = float.TryParse(newLoc.Longitude.ToString(), out var val3) ? val3 : 0,
									Latitude = float.TryParse(newLoc.Latitude.ToString(), out var val4) ? val4 : 0,
									Confidence = newLoc.Confidence,
									Nr = (int)kundenEntity.Nummer,
									UpdateDate = null
								});
							finalgeolocationEntiy = Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Get((int)kundenEntity.Nummer);
						}
					}

					// - Add if not exists
					if(KundenEntensionEntity == null)
					{
						Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.KundenExtensionEntity
						{
							Nr = kundenEntity.Nr,
							IsArchived = false,
							UpdateUserId = this._user.Id,
							UpdateTime = DateTime.Now,
						});
						KundenEntensionEntity = Infrastructure.Data.Access.Tables.BSD.KundenExtensionAccess.GetByKundenNr(kundenEntity.Nr);
					}

					var supplierEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(adressEntity?.Nr ?? -1);
					var respone = new Models.Customer.OverviewModel(kundenEntity, adressEntity, KundenEntensionEntity, finalgeolocationEntiy, supplierEntity);
					//
					var actualIndustry = int.TryParse(respone.Industry, out var val) ? val : 0;
					var replacedIndustry = industryEntity.FirstOrDefault(x => x.Id == actualIndustry)?.Name;
					respone.Industry = (respone.Industry != null && respone.Industry != "" && replacedIndustry != null) ? replacedIndustry : respone.Industry;

					var actualGroup = int.TryParse(respone.Costumer_group, out var val2) ? val2 : 0;
					var ReplacedGroup = pszKundengruppenEntities?.FirstOrDefault(x => x.ID == actualGroup)?.Kundengruppe;
					respone.Costumer_group = (respone.Costumer_group != null && respone.Costumer_group != "" && ReplacedGroup != null) ? ReplacedGroup : respone.Costumer_group;
					//
					respone.Deals = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetCountByCustomer(respone.AdressId, onlyValidated: true, closed:false);
					respone.PreviousNr = Infrastructure.Data.Access.Joins.BSD.KundenAccess.GetPrevCustomer(respone.Number);
					respone.NextNr = Infrastructure.Data.Access.Joins.BSD.KundenAccess.GetNextCustomer(respone.Number);

					return ResponseModel<Models.Customer.OverviewModel>.SuccessResponse(respone);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Customer.OverviewModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Customer.OverviewModel>.AccessDeniedResponse();
			}

			var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get(this._data);
			if(kundenEntity == null)
			{
				return new ResponseModel<Models.Customer.OverviewModel>()
				{
					Errors = new List<ResponseModel<Models.Customer.OverviewModel>.ResponseError>() {
						new ResponseModel<Models.Customer.OverviewModel>.ResponseError {Key = "1", Value = "Customer not found"}
					}
				};
			}

			return ResponseModel<Models.Customer.OverviewModel>.SuccessResponse();
		}
	}
}
