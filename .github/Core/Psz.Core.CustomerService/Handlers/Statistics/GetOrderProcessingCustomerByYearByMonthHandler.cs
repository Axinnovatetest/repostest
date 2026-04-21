using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;
using static Psz.Core.CustomerService.Models.Statistics.OrderProcessingCustomerResponseModel;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetOrderProcessingCustomerByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<OrderProcessingCustomerResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }

		public GetOrderProcessingCustomerByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<OrderProcessingCustomerResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var statEntityAll = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats8(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityAB = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats8(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				var statEntityLS = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats8(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				OrderProcessingCustomerResponseModel response = new OrderProcessingCustomerResponseModel();
				if(statEntityAll != null && statEntityAll.Count > 0)
					response.ByProject = statEntityAll.Select(x => new Item(x)).ToList();
				if(statEntityAB != null && statEntityAB.Count > 0)
					response.ByAB = statEntityAB.Select(x => new Item(x)).ToList();
				if(statEntityLS != null && statEntityLS.Count > 0)
					response.ByLS = statEntityLS.Select(x => new Item(x)).ToList();
				return ResponseModel<OrderProcessingCustomerResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<OrderProcessingCustomerResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<OrderProcessingCustomerResponseModel>.AccessDeniedResponse();
			}
			if(string.IsNullOrWhiteSpace(this._data.Typ))
				return ResponseModel<OrderProcessingCustomerResponseModel>.FailureResponse("Data cannot be empty");
			return ResponseModel<OrderProcessingCustomerResponseModel>.SuccessResponse();
		}
	}
}
