using Psz.Core.FinanceControl.Enums;
using Psz.Core.FinanceControl.Models.Statistics;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetOrdersMonthlyOverviewHandler: IHandle<UserModel, ResponseModel<OrdersMonthlyViewStats>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;

		public GetOrdersMonthlyOverviewHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<OrdersMonthlyViewStats> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;

				var monthlyInternal = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersMonthlyViewByType(BudgetEnums.ProjectTypes.Internal.GetDescription(), _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var monthlyExternal = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersMonthlyViewByType(BudgetEnums.ProjectTypes.External.GetDescription(), _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

				var monthlyPurchase = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersMonthlyViewByPoPaymentType(BudgetEnums.OrderTypes.Purchase.GetDescription(), _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var monthlyLeasing = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersMonthlyViewByPoPaymentType(BudgetEnums.OrderTypes.Leasing.GetDescription(), _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

				var response = new OrdersMonthlyViewStats
				{
					OverviewExternal = Helpers.CalculationsHelper.FillEmptyMonthsV1(Helpers.CalculationsHelper.GetAmountsForOrdersByType(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, monthlyExternal, BudgetEnums.ProjectTypes.External.GetDescription())),
					OverviewInternal = Helpers.CalculationsHelper.FillEmptyMonthsV1(Helpers.CalculationsHelper.GetAmountsForOrdersByType(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, monthlyInternal, BudgetEnums.ProjectTypes.Internal.GetDescription())),
					OverviewLeasing = Helpers.CalculationsHelper.FillEmptyMonthsV2(Helpers.CalculationsHelper.GetAmountsForOrdersByPoPayement(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, monthlyLeasing, BudgetEnums.OrderTypes.Leasing.GetDescription())),
					OverviewPurchase = Helpers.CalculationsHelper.FillEmptyMonthsV2(Helpers.CalculationsHelper.GetAmountsForOrdersByPoPayement(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, monthlyPurchase, BudgetEnums.OrderTypes.Purchase.GetDescription()))
				};
				return ResponseModel<OrdersMonthlyViewStats>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<OrdersMonthlyViewStats> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<OrdersMonthlyViewStats>.AccessDeniedResponse();
			}
			return ResponseModel<OrdersMonthlyViewStats>.SuccessResponse();
		}
	}
}