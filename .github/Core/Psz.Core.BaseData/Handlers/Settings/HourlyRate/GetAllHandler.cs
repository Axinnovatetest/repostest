using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.HourlyRate
{
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Settings.HourlyRate.HourlyRateResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Settings.HourlyRate.HourlyRateResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<List<Models.Settings.HourlyRate.HourlyRateResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.HourlyRateAccess.Get()?.Select(x => new Models.Settings.HourlyRate.HourlyRateResponseModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Settings.HourlyRate.HourlyRateResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Settings.HourlyRate.HourlyRateResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Settings.HourlyRate.HourlyRateResponseModel>>.SuccessResponse();
		}
	}
}
