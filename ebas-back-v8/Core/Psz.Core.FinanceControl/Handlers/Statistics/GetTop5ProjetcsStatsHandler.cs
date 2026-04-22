using Psz.Core.FinanceControl.Models.Statistics;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetTop5ProjetcsStatsHandler: IHandle<UserModel, ResponseModel<ProjectsTop5StatsModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;

		public GetTop5ProjetcsStatsHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<ProjectsTop5StatsModel> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;

				var biggestAllocations = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsWithBiggestBudget(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var biggestAmountOfOrders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsWithBiggestOrdersAmounts(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var oldetApprovals = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOldestProjects(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var mostProfitable = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetMostProfitableProjects(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

				var overbudgeted = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOverbudgetedProjects(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var budgetLeaks = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetBudgetLeakProjects(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				budgetLeaks?.ForEach(x =>
				{
					x.Diffrence = x.ProjectBudget - x.OrdersAmount;
				});
				overbudgeted?.ForEach(x =>
				{
					x.Diffrence = x.OrdersAmount - x.ProjectBudget;
				});
				var response = new ProjectsTop5StatsModel
				{
					BiggectAllocations = biggestAllocations?.Select(x => new Top5Model(x)),
					BiggectOrdersAmount = biggestAmountOfOrders?.Select(x => new Top5Model(x)),
					OldestApproval = oldetApprovals?.Select(x => new Top5Model2(x)),
					MostProfitable = mostProfitable?.Select(x => new Top5Model(x)),

					Overbudgeted = overbudgeted?.Select(x => new OverOrUnderBudgetModel(x)),
					BudgetLeaks = budgetLeaks?.Select(x => new OverOrUnderBudgetModel(x))
				};
				return ResponseModel<ProjectsTop5StatsModel>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<ProjectsTop5StatsModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<ProjectsTop5StatsModel>.AccessDeniedResponse();
			}
			return ResponseModel<ProjectsTop5StatsModel>.SuccessResponse();
		}
	}
}