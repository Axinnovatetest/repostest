using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetOrdersOverviewHandler: IHandle<UserModel, ResponseModel<List<OrdersOverviewModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModelOrdersOverview _data;

		public GetOrdersOverviewHandler(UserModel user, StatsRequestModelOrdersOverview data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<OrdersOverviewModel>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var orderTypes = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectTypes)).Cast<Enums.BudgetEnums.ProjectTypes>()
					.Select(x => x.GetDescription()).ToList();
				var orderPoPayements = Enum.GetValues(typeof(Enums.BudgetEnums.OrderTypes)).Cast<Enums.BudgetEnums.OrderTypes>()
					.Select(x => x.GetDescription()).ToList();
				var orderValidationStatuses = Enum.GetValues(typeof(Enums.BudgetEnums.ValidationLevels)).Cast<Enums.BudgetEnums.ValidationLevels>()
					.Select(x => x.GetDescription()).ToList();
				var orders = new List<Infrastructure.Data.Entities.Joins.FNC.Statistics.OrdersOverviewEntity>();
				if(orderTypes.Contains(_data.Text))
					orders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersOverview(_data.Text, 1, null, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

				if(orderPoPayements.Contains(_data.Text))
					orders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersOverview(_data.Text, 2, null, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

				if(orderValidationStatuses.Contains(_data.Text))
				{
					var status = (int)Enum.GetValues(typeof(Enums.BudgetEnums.ValidationLevels))
											  .Cast<Enums.BudgetEnums.ValidationLevels>()
											  .FirstOrDefault(v => v.GetDescription() == _data.Text);
					orders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersOverview(null, 3, status, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				}

				if(_data.Text == "Booked" || _data.Text == "Placed")
					orders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersOverview(null, 4, null, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, _data.Text == "Placed");
				var response = orders?.Select(x => new OrdersOverviewModel(x)).ToList();

				return ResponseModel<List<OrdersOverviewModel>>.SuccessResponse(response);
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
