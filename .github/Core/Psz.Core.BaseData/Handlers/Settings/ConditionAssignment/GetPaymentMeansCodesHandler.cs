using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.ConditionAssignment
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetPaymentMeansCodesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<string, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetPaymentMeansCodesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
					return validationResponse;

				var paymentMeansCodes = Infrastructure.Data.Access.Tables.PRS.PaymentMeansCodesAccess.Get();

				var response = paymentMeansCodes
					?.OrderBy(p => p.DescriptionEnglish)
					?.Select(p => new KeyValuePair<string, string>(
						p.Code,
						$"{p.DescriptionEnglish}"
					))
					?.ToList();

				return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<string, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<string, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<string, string>>>.SuccessResponse();
		}
	}
}
