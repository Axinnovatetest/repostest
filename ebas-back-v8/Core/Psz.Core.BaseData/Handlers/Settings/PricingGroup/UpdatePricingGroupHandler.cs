using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.PricingGroup
{
	public class UpdatePricingGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.PricingGroup.PricingGroupModel _data { get; set; }
		public UpdatePricingGroupHandler(Identity.Models.UserModel user, Models.PricingGroup.PricingGroupModel data)
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
				var priceGroupEntity = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Get(this._data.Id);
				var _entity = this._data.ToEntity((int)priceGroupEntity.Preisgruppe);
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Update(_entity);
				return ResponseModel<int>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var priceGroupEntity = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Get(this._data.Id);
			if(priceGroupEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Price Group not found" }
						}
				};
			}
			if(this._data.PriceGroup == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Price Group should not be empty" }
						}
				};
			}
			var pricingGroupEntities = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Get();
			var theRest = pricingGroupEntities.Where(x => x.ID != this._data.Id);
			var check = theRest.Where(x => x.Bemerkung == this._data.Comment);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A pricing group with the same comment already exsists" }
						}
				};
			}
			var checkId = theRest.Where(x => x.Preisgruppe == this._data.PriceGroup);
			if(checkId != null && check.Count() > 0)
			{
				return ResponseModel<int>.FailureResponse("A Pricing Group with the same value exists");
			}

			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByPricingGroup(priceGroupEntity.Preisgruppe ?? -1);
			if(exsist1 != null && exsist1.Count > 0)
			{
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(exsist1.Select(x => x.Nummer ?? -1)?.Distinct().ToList());
				return ResponseModel<int>.FailureResponse(
					$"Cannot update Pricing Group. The following customer(s) use this Pricing [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Kundennummer} - {x.Name1}")?.ToList())}]");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
