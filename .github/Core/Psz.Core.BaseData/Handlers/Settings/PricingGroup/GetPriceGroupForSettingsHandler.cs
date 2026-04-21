using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.PricingGroup
{
	public class GetPriceGroupForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.PricingGroup.PricingGroupModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetPriceGroupForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.PricingGroup.PricingGroupModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.PricingGroup.PricingGroupModel>();
				var preisgruppenEntities = Infrastructure.Data.Access.Tables.BSD.Preisgruppen_VorgabenAccess.GetPricingGroups();

				if(preisgruppenEntities != null && preisgruppenEntities.Count > 0)
				{
					foreach(var preisgruppenEntity in preisgruppenEntities)
					{
						responseBody.Add(new Models.PricingGroup.PricingGroupModel(preisgruppenEntity));
					}
				}

				return ResponseModel<List<Models.PricingGroup.PricingGroupModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.PricingGroup.PricingGroupModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.PricingGroup.PricingGroupModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.PricingGroup.PricingGroupModel>>.SuccessResponse();
		}
	}
}
