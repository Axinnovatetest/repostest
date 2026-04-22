using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.PricingGroup
{
	public class AddPriceGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.PricingGroup.PricingGroupModel _data { get; set; }
		public AddPriceGroupHandler(Identity.Models.UserModel user, Models.PricingGroup.PricingGroupModel data)
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
				var MaxGroup = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.GetMaxGroup();
				var _entity = this._data.ToEntity(MaxGroup + 1);
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Insert(_entity);
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
			if(this._data.PriceGroup == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Price Group should not be empty" }
						}
				};
			}
			if(string.IsNullOrEmpty(this._data.Comment))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Comment should not be empty" }
						}
				};
			}
			var pricingGroupEntities = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.Get();
			var check = pricingGroupEntities.Where(x => x.Bemerkung == this._data.Comment);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A pricing group with the same comment already exsists" }
						}
				};
			}

			var checkId = pricingGroupEntities.Where(x => x.Preisgruppe == this._data.PriceGroup);
			if(checkId != null)
			{
				return ResponseModel<int>.FailureResponse("A Pricing Group with the same value exists");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
