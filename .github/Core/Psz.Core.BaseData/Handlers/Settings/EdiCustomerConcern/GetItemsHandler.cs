using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class GetItemsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetItemsHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>();
				var items = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByConcernId(this._data);
				if(items?.Count > 0)
				{
					var customers = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummers(items.Select(x => x.CustomerNumber ?? 0).ToList())
						?.Where(x => (x.Sperren ?? false) == false && x.Adresstyp == 1 /* CUSTOMER STD ADDRESS = 1 */);
					foreach(var item in items)
					{
						var customer = customers.FirstOrDefault(x => x.Kundennummer == item.CustomerNumber);
						responseBody.Add(new Models.Settings.EdiCustomerConcern.ConcernItemResponseModel(item, customer?.Name1));
					}
				}
				// -
				return ResponseModel<List<Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get(this._data) == null)
			{
				return ResponseModel<List<Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>>.FailureResponse("Concern Item not found");
			}

			return ResponseModel<List<Models.Settings.EdiCustomerConcern.ConcernItemResponseModel>>.SuccessResponse();
		}
	}
}
