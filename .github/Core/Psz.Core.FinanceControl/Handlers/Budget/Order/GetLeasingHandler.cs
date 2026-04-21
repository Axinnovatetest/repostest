using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetLeasingHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.OrderLeasingRequestModel _data { get; set; }

		public GetLeasingHandler(Identity.Models.UserModel user, Models.Budget.Order.OrderLeasingRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Budget.Order.OrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var leasingOrderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByYearCompanyDepartmentEmployee(this._data.Year, this._data.CompanyId, this._data.DepartmentId, this._data.EmployeeId);
				leasingOrderEntities = leasingOrderEntities?.DistinctBy(x => x.Id)?.ToList();

				var responseBody = Helpers.Processings.Budget.Order.GetOrderLeasingModels(leasingOrderEntities, this._user, out var errors, this._data.Year);
				if(errors != null && errors.Count > 0)
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse(errors);

				return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.OrderModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.AccessDeniedResponse();
			}

			if(this._data.Year <= 0)
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse("1", "Year invalid");

			//if (this._data.CompanyId <= 0)
			//    return ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse("1", "Company invalid");

			return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
		}
	}

}
