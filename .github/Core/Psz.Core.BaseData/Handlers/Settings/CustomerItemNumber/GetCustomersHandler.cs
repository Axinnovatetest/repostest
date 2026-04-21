using Psz.Core.BaseData.Models.Settings.CustomerItemNumber;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.CustomerItemNumber
{
	public class GetCustomersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CustomerNumbersRequestModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetCustomersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<CustomerNumbersRequestModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<List<CustomerNumbersRequestModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetKundenAddresses()
					?.Select(x => new CustomerNumbersRequestModel
					{
						Key = $"{x.Kundennummer ?? 0}",
						Value = $"{x.Name1}".Trim(),
						Analyse = x.PendingValidation ?? false
					})?.Distinct()?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<CustomerNumbersRequestModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<CustomerNumbersRequestModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<CustomerNumbersRequestModel>>.SuccessResponse();
		}
	}
}
