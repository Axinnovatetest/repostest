using Psz.Core.FinanceControl.Enums;
using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetProjectsMonthlyOverviewHandler: IHandle<UserModel, ResponseModel<PorjectsMonthlyViewStats>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;

		public GetProjectsMonthlyOverviewHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<PorjectsMonthlyViewStats> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var monthlyAll = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsmonthlyViewAll(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var monthlyPending = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByStatus((int)BudgetEnums.ProjectApprovalStatuses.Inactive, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var monthlyApproved = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByStatus((int)BudgetEnums.ProjectApprovalStatuses.Active, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var monthlyRejected = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByStatus((int)BudgetEnums.ProjectApprovalStatuses.Reject, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

			var monthlySuspended = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByApprovalStatus((int)BudgetEnums.ProjectStatuses.Suspended, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var monthlyActive = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByApprovalStatus((int)BudgetEnums.ProjectStatuses.Active, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			List<Tuple<string, string, int>> monthlyClosed = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByApprovalStatus((int)BudgetEnums.ProjectStatuses.Closed, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

			List<Tuple<string, string, int>> monthlyInternal = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByType(BudgetEnums.ProjectTypes.Internal.GetDescription(), _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			List<Tuple<string, string, int>> monthlyExternal = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByType(BudgetEnums.ProjectTypes.External.GetDescription(), _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			List<Tuple<string, string, int>> monthlyFinance = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsMonthlyViewByType(BudgetEnums.ProjectTypes.Finance.GetDescription(), _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

			var response = new PorjectsMonthlyViewStats
			{
				ProjectsMonthlyAll = Helpers.CalculationsHelper.FillEmptyMonthsV3(monthlyAll?.Select(x => new PorjectsMonthlyStats(x)).ToList()),
				ProjectsMonthlyPending = Helpers.CalculationsHelper.FillEmptyMonthsV4(monthlyPending?.Select(x => new PorjectsMonthlyStatsByStatus(x)).ToList()),
				ProjectsMonthlyApproved = Helpers.CalculationsHelper.FillEmptyMonthsV4(monthlyApproved?.Select(x => new PorjectsMonthlyStatsByStatus(x)).ToList()),
				ProjectsMonthlyRejected = Helpers.CalculationsHelper.FillEmptyMonthsV4(monthlyRejected?.Select(x => new PorjectsMonthlyStatsByStatus(x)).ToList()),
				ProjectsMonthlySuspended = Helpers.CalculationsHelper.FillEmptyMonthsV5(monthlySuspended?.Select(x => new PorjectsMonthlyStatsByApprovalStatus(x)).ToList()),
				ProjectsMonthlyActive = Helpers.CalculationsHelper.FillEmptyMonthsV5(monthlyActive?.Select(x => new PorjectsMonthlyStatsByApprovalStatus(x)).ToList()),
				ProjectsMonthlyClosed = Helpers.CalculationsHelper.FillEmptyMonthsV5(monthlyClosed?.Select(x => new PorjectsMonthlyStatsByApprovalStatus(x)).ToList()),
				ProjectsMonthlyInternal = Helpers.CalculationsHelper.FillEmptyMonthsV6(monthlyInternal?.Select(x => new PorjectsMonthlyStatsByType(x)).ToList()),
				ProjectsMonthlyExternal = Helpers.CalculationsHelper.FillEmptyMonthsV6(monthlyExternal?.Select(x => new PorjectsMonthlyStatsByType(x)).ToList()),
				ProjectsMonthlyFinance = Helpers.CalculationsHelper.FillEmptyMonthsV6(monthlyFinance?.Select(x => new PorjectsMonthlyStatsByType(x)).ToList()),
			};
			return ResponseModel<PorjectsMonthlyViewStats>.SuccessResponse(response);
		}
		public ResponseModel<PorjectsMonthlyViewStats> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<PorjectsMonthlyViewStats>.AccessDeniedResponse();
			}
			return ResponseModel<PorjectsMonthlyViewStats>.SuccessResponse();
		}
	}
}