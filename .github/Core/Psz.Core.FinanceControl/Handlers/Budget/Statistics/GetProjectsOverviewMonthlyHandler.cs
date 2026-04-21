using Infrastructure.Data.Entities.Joins.FNC.Statistics;
using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetProjectsOverviewMonthlyHandler: IHandle<UserModel, ResponseModel<List<ProjectsOverviewModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModelOrdersOverviewMonthly _data;

		public GetProjectsOverviewMonthlyHandler(UserModel user, StatsRequestModelOrdersOverviewMonthly data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<ProjectsOverviewModel>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;

				var prjectOverviewEntities = new List<ProjectsOverviewEntity>();
				var projectApprovals = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectApprovalStatuses)).Cast<Enums.BudgetEnums.ProjectApprovalStatuses>()
					.Select(a => a.GetDescription()).ToList();
				var projectStatuses = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectStatuses)).Cast<Enums.BudgetEnums.ProjectStatuses>()
					.Select(a => a.GetDescription()).ToList();
				var projectTypes = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectTypes)).Cast<Enums.BudgetEnums.ProjectTypes>()
					.Select(a => a.GetDescription()).ToList();
				if(_data.Text.ToLower() == "all")
					prjectOverviewEntities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsOverviewAll(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, _data.Month);
				if(projectStatuses.Contains(_data.Text))
					prjectOverviewEntities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsOverviewByStatus(_data.Text, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, _data.Month);
				if(projectApprovals.Contains(_data.Text))
				{
					var status = (int)Enum.GetValues(typeof(Enums.BudgetEnums.ProjectApprovalStatuses))
				.Cast<Enums.BudgetEnums.ProjectApprovalStatuses>()
				.FirstOrDefault(v => v.GetDescription() == _data.Text);
					prjectOverviewEntities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsOverviewByApprovalStatus(status, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, _data.Month);
				}
				if(projectTypes.Contains(_data.Text))
					prjectOverviewEntities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsOverviewByType(_data.Text, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, _data.Month);
				var response = prjectOverviewEntities?.Select(x => new ProjectsOverviewModel(x)).ToList();

				return ResponseModel<List<ProjectsOverviewModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<ProjectsOverviewModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<ProjectsOverviewModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<ProjectsOverviewModel>>.SuccessResponse();
		}
	}
}
