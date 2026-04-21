using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetLSCustomerByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<OrderProcessingLSCustomerResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }

		public GetLSCustomerByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<OrderProcessingLSCustomerResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var statEntityAB = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats8(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);

				List<OrderProcessingLSCustomerResponseModel> response = new List<OrderProcessingLSCustomerResponseModel>();
				if(statEntityAB != null && statEntityAB.Count > 0)
					response = statEntityAB.Select(x => new OrderProcessingLSCustomerResponseModel(x)).ToList();
				return ResponseModel<List<OrderProcessingLSCustomerResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<OrderProcessingLSCustomerResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<OrderProcessingLSCustomerResponseModel>>.AccessDeniedResponse();
			}
			if(string.IsNullOrWhiteSpace(this._data.Typ))
				return ResponseModel<List<OrderProcessingLSCustomerResponseModel>>.FailureResponse("Data cannot be empty");
			return ResponseModel<List<OrderProcessingLSCustomerResponseModel>>.SuccessResponse();
		}
	}
}
