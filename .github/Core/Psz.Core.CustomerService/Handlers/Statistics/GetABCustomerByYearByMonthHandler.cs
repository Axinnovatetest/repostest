using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetABCustomerByYearByMonthHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<OrderProcessingABCustomerResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Statistics.StatRequestModel _data { get; set; }
		public GetABCustomerByYearByMonthHandler(Identity.Models.UserModel user, Models.Statistics.StatRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<OrderProcessingABCustomerResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var statEntityAB = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats8(this._data.Typ, this._data.CurentYear, this._data.CurentMonth, this._data.Year, this._data.Month);
				List<OrderProcessingABCustomerResponseModel> response = new List<OrderProcessingABCustomerResponseModel>();
				if(statEntityAB != null && statEntityAB.Count > 0)
					response = statEntityAB.Select(x => new OrderProcessingABCustomerResponseModel(x)).ToList();
				return ResponseModel<List<OrderProcessingABCustomerResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<OrderProcessingABCustomerResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<OrderProcessingABCustomerResponseModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<OrderProcessingABCustomerResponseModel>>.SuccessResponse();
		}
	}
}
