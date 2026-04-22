using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetTimeFAByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<TimeFAResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }
		public GetTimeFAByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<TimeFAResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statEntityTimeFAEntity = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA11(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				List<TimeFAResponseModel> response = new List<TimeFAResponseModel>();

				if(statEntityTimeFAEntity != null && statEntityTimeFAEntity.Count > 0)
					response = statEntityTimeFAEntity.Select(x => new TimeFAResponseModel(x)).ToList();

				return ResponseModel<List<TimeFAResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<TimeFAResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<TimeFAResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<TimeFAResponseModel>>.SuccessResponse();
		}
	}
}
