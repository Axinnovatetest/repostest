using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Statistics
{
	public class GetTopCustomerFAHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CustomerFAResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetTopCustomerFAHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
				var statEntityCustomFAEntity = Infrastructure.Data.Access.Joins.CTS.StatisticFAAccess.GetFA14();
				List<CustomerFAResponseModel> response = new List<CustomerFAResponseModel>();
				if(statEntityCustomFAEntity != null && statEntityCustomFAEntity.Count > 0)
					response = statEntityCustomFAEntity.Select(x => new CustomerFAResponseModel(x)).ToList();
				return ResponseModel<List<CustomerFAResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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
