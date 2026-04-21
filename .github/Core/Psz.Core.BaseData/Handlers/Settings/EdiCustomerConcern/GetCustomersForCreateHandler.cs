using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class GetCustomersForCreateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private bool _data { get; set; }

		public GetCustomersForCreateHandler(Identity.Models.UserModel user, bool data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var results = new List<Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel>();
				var customerEntities = Infrastructure.Data.Access.Tables.PRS.KundenAccess.Get();
				var items = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.Get();
				var adressenEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetCustomersExceptKundennummers(customerEntities?.Select(x => x.Nummer ?? -1),
					 items?.Select(x => x.CustomerNumber ?? 0));
				if(this._data)
				{
					adressenEntities = adressenEntities.Where(x => x.EDI_Aktiv == true)?.ToList();
				}
				foreach(var customer in customerEntities)
				{
					var adressen = adressenEntities?.FirstOrDefault(x => x.Nr == customer.Nummer);
					if(adressen != null)
					{
						results.Add(new Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel
						{
							CustomerDUNS = adressen.Duns,
							CustomerId = customer.Nr,
							CustomerName = adressen.Name1,
							CustomerNumber = adressen.Kundennummer ?? -1
						});
					}
				}
				// -
				return ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernCustomerForCreateResponseModel>>.SuccessResponse();
		}
	}
}
