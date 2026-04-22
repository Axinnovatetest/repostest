using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.ShippingMethods
{
	public class GetShippingMethodsForSettingsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.ShippingMethod.ShippingMethodModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetShippingMethodsForSettingsHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.ShippingMethod.ShippingMethodModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.ShippingMethod.ShippingMethodModel>();
				var versandartenEntities = Infrastructure.Data.Access.Tables.BSD.Versandarten_AuswahlAccess.GetShipppingMethods();

				if(versandartenEntities != null && versandartenEntities.Count > 0)
				{
					foreach(var versandartenEntity in versandartenEntities)
					{
						responseBody.Add(new Models.ShippingMethod.ShippingMethodModel(versandartenEntity));
					}
				}

				return ResponseModel<List<Models.ShippingMethod.ShippingMethodModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.ShippingMethod.ShippingMethodModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.ShippingMethod.ShippingMethodModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.ShippingMethod.ShippingMethodModel>>.SuccessResponse();
		}
	}
}
