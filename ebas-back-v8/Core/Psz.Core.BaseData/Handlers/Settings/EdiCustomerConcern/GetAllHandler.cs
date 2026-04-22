using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				var concerns = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get();
				var responseBody = new List<Models.Settings.EdiCustomerConcern.EdiConcernResponseModel>();
				if(concerns?.Count > 0)
				{
					var counts = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetCustomersCount();
					foreach(var item in concerns)
					{
						var count = counts.FirstOrDefault(x => x.Key == item.Id);
						responseBody.Add(new Models.Settings.EdiCustomerConcern.EdiConcernResponseModel(item, count.Value));
					}
				}

				// -
				return ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernResponseModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Settings.EdiCustomerConcern.EdiConcernResponseModel>>.SuccessResponse();
		}
	}
}
