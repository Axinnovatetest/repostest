using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{

	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetDeliveryAddressesFullHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Address.AddressModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetDeliveryAddressesFullHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Address.AddressModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Address.AddressModel>();
				var adressenEntities = (Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetCustomerDeliveryAddresses(true)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>()).Distinct().OrderBy(x => x.Nr).ToList();

				if(adressenEntities != null && adressenEntities.Count > 0)
				{
					foreach(var adressenEntity in adressenEntities)
					{
						responseBody.Add(new Models.Address.AddressModel(adressenEntity));
					}
					responseBody = responseBody.Distinct().ToList();
				}

				return ResponseModel<List<Models.Address.AddressModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Address.AddressModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Address.AddressModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Address.AddressModel>>.SuccessResponse();
		}
	}
}
