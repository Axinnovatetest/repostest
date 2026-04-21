using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetBestSuppliersOverviewBiggetCountOfOverdueOrdersHandler: IHandle<UserModel, ResponseModel<List<OrdersOverviewModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestBestSuppliersOverviewOrdersCount _data;

		public GetBestSuppliersOverviewBiggetCountOfOverdueOrdersHandler(UserModel user, StatsRequestBestSuppliersOverviewOrdersCount data)
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

				var ids = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.SupplementForBestSuppliers(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, _data.Supplier);
				var entities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetWorstSuppliersOverviewBiggetCountOfOverdueOrders(ids);
				return ResponseModel<List<OrdersOverviewModel>>.SuccessResponse(entities?.Select(x => new OrdersOverviewModel(x)).ToList());
			} catch(Exception e)
			{

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