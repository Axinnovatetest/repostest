using Infrastructure.Data.Access.Tables.FNC;
using Infrastructure.Data.Entities.Tables.FNC;
using iText.StyledXmlParser.Jsoup.Select;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Psz.Core.FinanceControl.Enums.BudgetEnums;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetAllProjectStatusesDistinctPerMonthHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<AllProjectStatusesDistinctResponseModel>>
	{
		public Psz.Core.Identity.Models.UserModel _user { get; set; }
		public int? _data { get; set; }
		public GetAllProjectStatusesDistinctPerMonthHandler(UserModel user, int? year)
		{
			_user = user;
			this._data = year;
		}

		public ResponseModel<AllProjectStatusesDistinctResponseModel> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();
				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}

				List<int> projectApprobatioStatus = new List<int>
				{
					(int)Enums.BudgetEnums.ProjectApprovalStatuses.Active,
					(int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive,
					(int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject
				};

				//var projectStatusByApprobation = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusApprobationAndMonth(enumkeysOfProjectType, projectApprobatioStatus, _data);

				//var projectStatusByApprobation_Inactive = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusApprobationAndMonth((int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive,_data);
				//var projectStatusByApprobation_Reject = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusApprobationAndMonth((int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject, _data);



				// Status Project 

				List<int> project_status = new List<int>
				{
					(int)Enums.BudgetEnums.ProjectStatuses.Active,
					(int)Enums.BudgetEnums.ProjectStatuses.Suspended,
					(int)Enums.BudgetEnums.ProjectStatuses.Closed
				};

				// avoid hard values(string) and opt for variables, enumerations or struct.
				//var projectStatus = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusAndMonth(enumkeysOfProjectType, project_status, _data);

				//var projectStatus_Suspended = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusAndMonth(Project_Type, (int)Enums.BudgetEnums.ProjectStatuses.Suspended, _data);
				//var projectStatus_pending = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusAndMonth(Project_Type, (int)Enums.BudgetEnums.ProjectStatuses.Closed, _data);

				// Tomorrow 31-01-2024 : Perform a modification and call methode --> GetProjectsMonthlyByStatus ***

				var projectsByStatuses = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectsMonthlyByStatus(project_status, _data);

				// Declaration
				var projectMonthlyActive = new List<Tuple<int, int, int>>();
				var projectMonthlySuspended = new List<Tuple<int, int, int>>();
				var projectMonthlyClosed = new List<Tuple<int, int, int>>();

				var ProjectMonthlyActiveResponseModel = new List<ProjectMonthlyStatus>();
				var ProjectMonthlySuspendedResponseModel = new List<ProjectMonthlyStatus>();
				var ProjectMonthlyClosedResponseModel = new List<ProjectMonthlyStatus>();

				if(projectsByStatuses.Count > 0)
				{
					for(int i = 0; i < 12; i++)
					{
						projectMonthlyActive.AddRange(projectsByStatuses.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectStatuses.Active));
						projectMonthlySuspended.AddRange(projectsByStatuses.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectStatuses.Suspended));
						projectMonthlyClosed.AddRange(projectsByStatuses.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectStatuses.Closed));

					}

					ProjectMonthlyActiveResponseModel = projectMonthlyActive.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();

					ProjectMonthlySuspendedResponseModel = projectMonthlySuspended.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();


					ProjectMonthlyClosedResponseModel = projectMonthlyClosed.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();

				}



				// adding 31-01-2024
				var projectsByApprovalStatus = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectByApprovalStatus(projectApprobatioStatus, _data);

				var projectMonthlyApprovalStatusActive = new List<Tuple<int, int, int>>();
				var projectMonthlyApprovalStatusInactive = new List<Tuple<int, int, int>>();
				var projectMonthlyApprovalStatusReject = new List<Tuple<int, int, int>>();

				// DTO 
				var ApprovalStatusActiveResponseModel = new List<ProjectMonthlyStatus>();
				var ApprovalStatusInactiveResponseModel = new List<ProjectMonthlyStatus>();
				var ApprovalStatusRejectResponseModel = new List<ProjectMonthlyStatus>();

				if(projectsByApprovalStatus?.Count > 0)
				{
					for(int i = 0; i < 12; i++)
					{
						projectMonthlyApprovalStatusActive.AddRange(projectsByApprovalStatus.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active));
						projectMonthlyApprovalStatusInactive.AddRange(projectsByApprovalStatus.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive));
						projectMonthlyApprovalStatusReject.AddRange(projectsByApprovalStatus.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject));
					}

					// Transform it into DTO
					ApprovalStatusActiveResponseModel = projectMonthlyApprovalStatusActive.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();

					// we can encapsulate this logic, by creating a constructor for your ProjectMonthlyStatus class 
					ApprovalStatusInactiveResponseModel = projectMonthlyApprovalStatusInactive.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();


					ApprovalStatusRejectResponseModel = projectMonthlyApprovalStatusReject.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();

				}



				return ResponseModel<AllProjectStatusesDistinctResponseModel>.SuccessResponse(new AllProjectStatusesDistinctResponseModel
				{

					projectMonthlyApprovalStatusActiveResponseModel = ApprovalStatusActiveResponseModel,
					projectMonthlyApprovalStatusInactiveResponseModel = ApprovalStatusInactiveResponseModel,
					projectMonthlyApprovalStatusRejectResponseModel = ApprovalStatusRejectResponseModel,
					ProjectMonthlyActiveResponseModel = ProjectMonthlyActiveResponseModel,
					ProjectMonthlySuspendedResponseModel = ProjectMonthlySuspendedResponseModel,
					ProjectMonthlyClosedResponseModel = ProjectMonthlyClosedResponseModel,
				});


			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<AllProjectStatusesDistinctResponseModel> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<AllProjectStatusesDistinctResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<AllProjectStatusesDistinctResponseModel>.SuccessResponse();
		}
	}
}
