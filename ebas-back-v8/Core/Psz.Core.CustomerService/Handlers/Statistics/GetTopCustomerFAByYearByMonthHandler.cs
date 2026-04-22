using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetTopCustomerFAByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CustomerFAResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }

		public GetTopCustomerFAByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<CustomerFAResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var statEntityCustomFAEntity = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA10(this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);

				List<CustomerFAResponseModel> response = new List<CustomerFAResponseModel>();
				if(statEntityCustomFAEntity != null && statEntityCustomFAEntity.Count > 0)
					response = statEntityCustomFAEntity.Select(x => new CustomerFAResponseModel(x)).ToList();
				return ResponseModel<List<CustomerFAResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<CustomerFAResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<CustomerFAResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<CustomerFAResponseModel>>.SuccessResponse();
		}
	}
}
