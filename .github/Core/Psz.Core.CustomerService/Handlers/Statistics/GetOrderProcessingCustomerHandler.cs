using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;
using static Psz.Core.CustomerService.Models.Statistics.OrderProcessingCustomerResponseModel;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetOrderProcessingCustomerHandler: IHandle<Identity.Models.UserModel, ResponseModel<OrderProcessingCustomerResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetOrderProcessingCustomerHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				var statEntityAll = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats1("");
				var statEntityAB = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats1("Auftragsbestätigung");
				var statEntityLS = Infrastructure.Data.Access.Joins.CTS.StatisticProjectAccess.GetStats1("Lieferschein");
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
			return ResponseModel<OrderProcessingCustomerResponseModel>.SuccessResponse();
		}
	}
}
