using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Supplier.Overview
{
	public class GetSupplierOverviewHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Supplier.OverviewModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetSupplierOverviewHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Supplier.OverviewModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				lock(Locks.SupplierEditLock.GetOrAdd(this._data, new object()))
				{
					var industryEntity = Infrastructure.Data.Access.Tables.BSD.IndustryAccess.Get();
					var pszLieferantengruppenEntities = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get();
					//
					var lieferentenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
					var adressEntity = int.TryParse(lieferentenEntity.Nummer.ToString(), out int lieferentenNummer)
						? Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(lieferentenNummer)
						: null;
					var lieferentenEntensionEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(lieferentenEntity.Nr);
					var finalgeolocationEntiy = new Infrastructure.Data.Entities.Tables.PRS.AdressenGeocodingExtensionEntity();

					var geolocationEntiy = Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Get((int)lieferentenEntity.Nummer);

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
									Nr = (int)lieferentenEntity.Nummer
								});

							finalgeolocationEntiy = Infrastructure.Data.Access.Tables.PRS.AdressenGeocodingExtensionAccess.Get((int)lieferentenEntity.Nummer);
						}
					}

					// - Add if not exists
					if(lieferentenEntensionEntity == null)
					{
						Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.Insert(new Infrastructure.Data.Entities.Tables.BSD.LieferantenExtensionEntity
						{
							Nr = lieferentenEntity.Nr,
							IsArchived = false,
							UpdateUserId = this._user.Id,
							UpdateTime = DateTime.Now,
						});
						lieferentenEntensionEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenExtensionAccess.GetByLieferantenNr(lieferentenEntity.Nr);
					}

					var kundenEntity = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(adressEntity?.Nr ?? -1);
					var respone = new Models.Supplier.OverviewModel(lieferentenEntity, adressEntity, lieferentenEntensionEntity, finalgeolocationEntiy, kundenEntity);
					//
					var actualIndustry = int.TryParse(respone.Industry, out var val) ? val : 0;
					var replacedIndustry = industryEntity.FirstOrDefault(x => x.Id == actualIndustry)?.Name;
					respone.Industry = (respone.Industry != null && respone.Industry != "" && replacedIndustry != null) ? replacedIndustry : respone.Industry;

					var actualGroup = int.TryParse(respone.Supplier_group, out var val2) ? val2 : 0;
					var relapcedGroup = pszLieferantengruppenEntities.FirstOrDefault(x => x.ID == actualGroup)?.Lieferantengruppe;
					respone.Supplier_group = (respone.Supplier_group != null && respone.Supplier_group != "" && relapcedGroup != null) ? relapcedGroup : respone.Supplier_group;

					respone.PreviousNr = Infrastructure.Data.Access.Joins.BSD.KundenAccess.GetPrevSupplier(respone.Number);
					respone.NextNr = Infrastructure.Data.Access.Joins.BSD.KundenAccess.GetNextSupplier(respone.Number);
					//
					respone.Deals = Infrastructure.Data.Access.Tables.BSD.BestellungenAccess.GetCountBySupplier(respone.AdressId, onlyValidated: true);
					return ResponseModel<Models.Supplier.OverviewModel>.SuccessResponse(respone);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Supplier.OverviewModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Supplier.OverviewModel>.AccessDeniedResponse();
			}

			var kundenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data);
			if(kundenEntity == null)
			{
				return new ResponseModel<Models.Supplier.OverviewModel>()
				{
					Errors = new List<ResponseModel<Models.Supplier.OverviewModel>.ResponseError>() {
						new ResponseModel<Models.Supplier.OverviewModel>.ResponseError {Key = "1", Value = "Supplier not found"}
					}
				};
			}

			return ResponseModel<Models.Supplier.OverviewModel>.SuccessResponse();
		}
	}
}
