using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetBestSuppliersOverviwOrdersCountHandler: IHandle<UserModel, ResponseModel<List<OrdersOverviewModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestBestSuppliersOverviewOrdersCount _data;

		public GetBestSuppliersOverviwOrdersCountHandler(UserModel user, StatsRequestBestSuppliersOverviewOrdersCount data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<OrdersOverviewModel>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;

				var entities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetBestSuppliersOverviwOrdersCount(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, _data.Supplier);

				return ResponseModel<List<OrdersOverviewModel>>.SuccessResponse(entities?.Select(x => new OrdersOverviewModel(x)).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<OrdersOverviewModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<OrdersOverviewModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<OrdersOverviewModel>>.SuccessResponse();
		}
	}
}