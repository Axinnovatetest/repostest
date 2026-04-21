using Psz.Core.FinanceControl.Enums;
using Psz.Core.FinanceControl.Models.Statistics;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetProjectsStatisticsHandler: IHandle<UserModel, ResponseModel<ProjectsStatsModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;

		public GetProjectsStatisticsHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<ProjectsStatsModel> Handle()
		{
			var validationresponse = Validate();
			if(!validationresponse.Success)
				return validationresponse;


			var projectsByStatus = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsCountByApprovalStatus(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var countsByApprovalStatus = projectsByStatus?.Select(x =>
			new ProjectsCountsModel
			{
				Count = x.Key,
				Status = ((Enums.BudgetEnums.ProjectApprovalStatuses)x.Value).GetDescription()
			})
				.ToList();
			var countsByStatus = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsCountByStatus(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId)?
				.Select(x => new ProjectsCountsModel { Count = x.Key, Status = x.Value }).ToList();
			var projectsByType = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsCountByType(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var response = new ProjectsStatsModel
			{
				Counts = countsByStatus?.Union(countsByApprovalStatus ?? new System.Collections.Generic.List<ProjectsCountsModel> { }).ToList(),
				External = projectsByType?.Where(x => x.Value == BudgetEnums.ProjectTypes.External.GetDescription()).Sum(x => x.Key) ?? 0,
				Internal = projectsByType?.Where(x => x.Value == BudgetEnums.ProjectTypes.Internal.GetDescription()).Sum(x => x.Key) ?? 0,
				Finance = projectsByType?.Where(x => x.Value == BudgetEnums.ProjectTypes.Finance.GetDescription()).Sum(x => x.Key) ?? 0,
			};
			return ResponseModel<ProjectsStatsModel>.SuccessResponse(response);
		}
		public ResponseModel<ProjectsStatsModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<ProjectsStatsModel>.AccessDeniedResponse();
			}
			return ResponseModel<ProjectsStatsModel>.SuccessResponse();
		}
	}
}