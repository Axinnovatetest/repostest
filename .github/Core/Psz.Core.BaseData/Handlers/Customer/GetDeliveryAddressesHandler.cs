using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Customer
{

	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetDeliveryAddressesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetDeliveryAddressesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<KeyValuePair<int, string>>();
				var adressenEntities = (Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetCustomerDeliveryAddresses()
					?? new List<Infrastructure.Data.Entities.Tables.PRS.AdressenEntity>()).Distinct().OrderBy(x => x.Nr).ToList();

				if(adressenEntities != null && adressenEntities.Count > 0)
				{
					foreach(var adressenEntity in adressenEntities)
					{
						responseBody.Add(new KeyValuePair<int, string>(adressenEntity.Nr, $"{adressenEntity.Nr} || {adressenEntity.Name1.Trim()} {adressenEntity.Name2.Trim()} {adressenEntity.Name3.Trim()} {adressenEntity.StraBe.Trim()} {adressenEntity.Ort.Trim()}".Trim()));
					}
					responseBody = responseBody.Distinct().ToList();
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
