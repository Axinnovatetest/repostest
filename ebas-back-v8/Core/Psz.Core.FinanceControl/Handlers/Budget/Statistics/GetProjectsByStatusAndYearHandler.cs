using Infrastructure.Data.Entities.Joins.FNC.Statistics;
using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetProjectsByStatusAndYearHandler: IHandle<UserModel, ResponseModel<List<ProjectsOverviewModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModelStatus _data;

		public GetProjectsByStatusAndYearHandler(UserModel user, StatsRequestModelStatus data)
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
				var projectApprovals = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectApprovalStatuses)).Cast<Enums.BudgetEnums.ProjectApprovalStatuses>().ToList();
				var projectStatuses = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectStatuses)).Cast<Enums.BudgetEnums.ProjectStatuses>().ToList();

				var approvalStatuses = projectApprovals.Select(x => x.GetDescription()).ToList();
				var ProjectStatuses = projectStatuses.Select(x => x.GetDescription()).ToList();
				bool useStatusId = false;
				if(ProjectStatuses.Contains(_data.Status))
					prjectOverviewEntities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsOverviewByStatus(_data.Status, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				if(approvalStatuses.Contains(_data.Status))
				{
					var status = (int)Enum.GetValues(typeof(Enums.BudgetEnums.ProjectApprovalStatuses))
				.Cast<Enums.BudgetEnums.ProjectApprovalStatuses>()
				.FirstOrDefault(v => v.GetDescription() == _data.Status);
					prjectOverviewEntities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsOverviewByApprovalStatus(status, _data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
					useStatusId = true;
				}
				var response = prjectOverviewEntities?.Select(x => new ProjectsOverviewModel(x)).ToList();

				return ResponseModel<List<ProjectsOverviewModel>>.SuccessResponse(response);
			} catch(System.Exception e)
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