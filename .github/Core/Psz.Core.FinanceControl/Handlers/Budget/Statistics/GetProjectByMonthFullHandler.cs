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
	// -- To do 11/10/2023  
	public class GetProjectByMonthFullHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<ProjectByMonthAllViewResponseModel>>
	{
		public Psz.Core.Identity.Models.UserModel _user { get; set; }
		public int? _data { get; set; }
		public GetProjectByMonthFullHandler(UserModel user, int? year)
		{
			_user = user;
			this._data = year;
		}

		public ResponseModel<ProjectByMonthAllViewResponseModel> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();
				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}



				//if enumeration, then several option and each option stand for KeyValuePair<key, description>; 
				var project_Type = new List<KeyValuePair<int, string>>();

				var getProjectType = Enum.GetValues(typeof(Enums.BudgetEnums.ProjectTypes)).Cast<Enums.BudgetEnums.ProjectTypes>().ToList();

				project_Type.AddRange(getProjectType.Select(enumOption => new KeyValuePair<int, string>((int)enumOption, enumOption.GetDescription())));

				var enumkeysOfProjectType = project_Type.Select(x => x.Key).ToList();

				//Get All Available Year
				List<int> years = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetAvailableYear();


				//-- get keyValuePaire<string, int> ==> transorform it into a more suitable model 
				var projectByMonth = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectPerMonth(_data);


				List<ProjectByMonth> projectByMonths = projectByMonth.Select(keyValuePaire => new ProjectByMonth
				{
					MonthNumber = keyValuePaire.Key,
					Count = keyValuePaire.Value
				}).ToList();


				// Get Tuple list  ==> transform it into a more suitable models 
				var projectTypeByMonthsEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByMonth(_data);


				//Transformation ==> Methode Link (Select)
				//List<ProjectTypeByMonth> projectTypeByMonths = ProjectTypeByMonth.Select(tuple => new ProjectTypeByMonth
				//{
				//	Month = tuple.Item1,
				//	ProjectTypeName = tuple.Item2,
				//	Count = tuple.Item3
				//}).ToList();

				//separation of Project types

				var resultByProjectTypeDto = new List<List<ProjectTypeByMonthEntity>>();

				if(projectTypeByMonthsEntity.Count > 0)
				{
					foreach(var item in project_Type)
					{
						//filter ==> methode Link (Where)
						var filteredProjects = projectTypeByMonthsEntity.Where(x => x.ProjectTypeName == item.Value).ToList();

						resultByProjectTypeDto.Add(filteredProjects);
					}
				}



				//Approbation status 
				//inside Database you haven't  Status Approbation Description, you have only Status Approbation Id << Id_Status >>.
				// if you want to get  Status Approbation Description, you should use an Enumeration 
				List<int> projectApprobatioStatus = new List<int>
				{
					(int)Enums.BudgetEnums.ProjectApprovalStatuses.Active,
					(int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive,
					(int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject
				};

				var projectStatusByApprobation = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusApprobationAndMonth(enumkeysOfProjectType, projectApprobatioStatus, _data);

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
				var projectStatus = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusAndMonth(enumkeysOfProjectType, project_status, _data);

				//var projectStatus_Suspended = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusAndMonth(Project_Type, (int)Enums.BudgetEnums.ProjectStatuses.Suspended, _data);
				//var projectStatus_pending = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectTypeByStatusAndMonth(Project_Type, (int)Enums.BudgetEnums.ProjectStatuses.Closed, _data);

				// Tomorrow 31-01-2024 : Perform a modification and call methode --> GetProjectsMonthlyByStatus ***

				var projectsByStatuses = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetProjectsMonthlyByStatus(project_status, _data);

				// Declaration
				var projectMonthlyActive = new List<Tuple<int, int, int>>();
				var projectMonthlySuspended = new List<Tuple<int, int, int>>();
				var projectMonthlyClosed = new List<Tuple<int, int, int>>();

				var transformedProjectMonthlyActive = new List<ProjectMonthlyStatus>();
				var transformedProjectMonthlySuspended = new List<ProjectMonthlyStatus>();
				var transformedProjectMonthlyClosed = new List<ProjectMonthlyStatus>();

				if(projectsByStatuses.Count > 0)
				{
					for(int i = 0; i < 12; i++)
					{
						projectMonthlyActive.AddRange(projectsByStatuses.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectStatuses.Active));
						projectMonthlySuspended.AddRange(projectsByStatuses.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectStatuses.Suspended));
						projectMonthlyClosed.AddRange(projectsByStatuses.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectStatuses.Closed));

					}

					transformedProjectMonthlyActive = projectMonthlyActive.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();

					transformedProjectMonthlySuspended = projectMonthlySuspended.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();


					transformedProjectMonthlyClosed = projectMonthlyClosed.Select(x => new ProjectMonthlyStatus
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

				var transformedProjectMonthlyApprovalStatusActive = new List<ProjectMonthlyStatus>();
				var transformedProjectMonthlyApprovalStatusInactive = new List<ProjectMonthlyStatus>();
				var transformedProjectMonthlyApprovalStatusReject = new List<ProjectMonthlyStatus>();

				if(projectsByApprovalStatus?.Count > 0)
				{
					for(int i = 0; i < 12; i++)
					{
						projectMonthlyApprovalStatusActive.AddRange(projectsByApprovalStatus.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active));
						projectMonthlyApprovalStatusInactive.AddRange(projectsByApprovalStatus.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive));
						projectMonthlyApprovalStatusReject.AddRange(projectsByApprovalStatus.Where(x => x.Item1 == i && x.Item2 == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject));
					}

					// Transform it into DTO
					transformedProjectMonthlyApprovalStatusActive = projectMonthlyApprovalStatusActive.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();

					transformedProjectMonthlyApprovalStatusInactive = projectMonthlyApprovalStatusInactive.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();


					transformedProjectMonthlyApprovalStatusReject = projectMonthlyApprovalStatusReject.Select(x => new ProjectMonthlyStatus
					{
						MonthNumber = x.Item1,
						//ProjectStatus = x.Item2,
						Count = x.Item3,
					}).ToList();

				}


				var countOrderPerProjectType = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.CountOrderByProjectType(enumkeysOfProjectType, _data);


				// Filter my projects by type.  07-02-2024

				//-- apply principe : divide and conquer
				var internalProjects = countOrderPerProjectType.Where(x => x.ProjectTypeName == "Internal"); // FIXME 07-02-2024 : Don't use a hard code  review after
				var externalProject = countOrderPerProjectType.Where(x => x.ProjectTypeName == "External"); // FIXME 07-02-2024 : Don't use a hard code  review after


				var projectTypeDescription = project_Type.Select(x => x.Value).ToList();

				var countOrderPerProjectType2 = new List<KeyValuePair<string, List<ProjectTypeOrderCountEntity>>>();

				;
				foreach(string description in projectTypeDescription)
				{
					var filteredProject = countOrderPerProjectType.Where(x => x.ProjectTypeName == description).ToList();

					var kvp = new KeyValuePair<string, List<ProjectTypeOrderCountEntity>>(description, filteredProject);

					countOrderPerProjectType2.Add(kvp); // i will get a tab of list

				}





				/*curiosity test*/

				//// --Get All Project List
				//var Allproject = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				//// --Apply link expression to filter
				//var query = Allproject
				//	.Where(bp => bp.CreationDate.Value.Year == _data && project_status.Contains(bp.Id_State) && Project_Type.Contains(bp.Id_Type)) //filter
				//	.GroupBy(bp => new { bp.CreationDate.Value.Month, bp.Id_State, bp.Type, bp.Id_Type })
				//		.Select(g => new
				//		{
				//			_month = g.Key.Month,
				//			projectId_Type = g.Key.Id_Type,
				//			project_type = g.Key.Type,
				//			status = g.Key.Id_State,
				//			_count = g.Count()
				//		})
				//		.OrderBy(g => g._month);  //Trie

				//var result = query.ToList();


				// --Get  All Month
				//int currentYear = DateTime.Now.Year;

				//List<string> months = new List<string>();

				//for(int i = 12; i >= 1; i--)
				//{
				//	DateTime date = new DateTime(currentYear, i, 1);
				//	string monthName = date.ToString("MMMM",CultureInfo.CreateSpecificCulture("en"));
				//	months.Add(monthName);
				//}				//int currentYear = DateTime.Now.Year;

				//List<string> months = new List<string>();

				//for(int i = 12; i >= 1; i--)
				//{
				//	DateTime date = new DateTime(currentYear, i, 1);
				//	string monthName = date.ToString("MMMM",CultureInfo.CreateSpecificCulture("en"));
				//	months.Add(monthName);
				//}



				if(projectByMonth != null || projectByMonth.Count <= 0) // check list
				{
					////////var response = new ProjectByMonthAllView
					////////{

					////////	//ProjectApprobation = projectStatusByApprobation,

					////////	ProjectMonthlyApprovalStatusActive = transformedProjectMonthlyApprovalStatusInactive, ///transformedProjectMonthlyApprovalStatusActive,
					////////	ProjectMonthlyApprovalStatusInactive = transformedProjectMonthlyApprovalStatusInactive,
					////////	ProjectMonthlyApprovalStatusReject = transformedProjectMonthlyApprovalStatusReject,

					////////	ProjectMonthlyActive = transformedProjectMonthlyActive,
					////////	ProjectMonthlyClosed = transformedProjectMonthlyClosed,
					////////	ProjectMonthlySuspended = transformedProjectMonthlySuspended,

					////////	CountOrdersPerProjectType = countOrderPerProjectType2,
					////////	TotalProjectByMonths = projectByMonths,
					////////	ProjectTypeByMonths = resultByProjectTypeDto,



					////////	//ProjectStatues = projectStatus
					////////};

					////////return ResponseModel<ProjectByMonthAllViewResponseModel>.SuccessResponse((ProjectByMonthAllViewResponseModel)response);
				}

				return null;

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<ProjectByMonthAllViewResponseModel> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<ProjectByMonthAllViewResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<ProjectByMonthAllViewResponseModel>.SuccessResponse();
		}
	}
}
