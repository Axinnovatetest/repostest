using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetOrdersMonthlyOverviewHandler: IHandle<UserModel, ResponseModel<List<OrdersOverviewModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModelOrdersOverviewMonthly _data;

		public GetOrdersMonthlyOverviewHandler(UserModel user, StatsRequestModelOrdersOverviewMonthly data)
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

				var orderTypes = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectTypes)).Cast<Enums.BudgetEnums.ProjectTypes>()
					.Select(x => x.GetDescription()).ToList();
				var orderPoPayements = Enum.GetValues(typeof(Enums.BudgetEnums.OrderTypes)).Cast<Enums.BudgetEnums.OrderTypes>()
					.Select(x => x.GetDescription()).ToList();

				var orders = new List<Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersOverviewEntity>();
				if(orderTypes.Contains(_data.Text))
					orders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersOverview(_data.Text, 1, null, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, false, _data.Month);

				if(orderPoPayements.Contains(_data.Text))
					orders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersOverview(_data.Text, 2, null, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, false, _data.Month);

				return ResponseModel<List<OrdersOverviewModel>>.SuccessResponse(orders?.Select(x => new OrdersOverviewModel(x)).ToList());
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