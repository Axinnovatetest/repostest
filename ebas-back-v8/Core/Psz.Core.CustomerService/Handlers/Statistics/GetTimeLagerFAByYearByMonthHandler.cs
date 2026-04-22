using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetTimeLagerFAByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<TimeLagerResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		private Models.Statistics.StatRequestModel _data { get; set; }
		public GetTimeLagerFAByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<TimeLagerResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var statEntityTimeLEntity = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA13(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				List<TimeLagerResponseModel> response = new List<TimeLagerResponseModel>();

				if(statEntityTimeLEntity != null && statEntityTimeLEntity.Count > 0)
					response = statEntityTimeLEntity.Select(x => new TimeLagerResponseModel(x)).ToList();

				return ResponseModel<List<TimeLagerResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<TimeLagerResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<TimeLagerResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<TimeLagerResponseModel>>.SuccessResponse();
		}
	}
}
