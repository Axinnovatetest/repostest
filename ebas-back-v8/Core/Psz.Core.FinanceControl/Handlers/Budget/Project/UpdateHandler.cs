using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	using static Psz.Core.FinanceControl.Helpers.Processings.Budget;

	public class UpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Project.ProjectModel _data { get; set; }

		public UpdateHandler(Identity.Models.UserModel user, Models.Budget.Project.ProjectModel model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.Id);

				if(this._data.StateId == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject)
				{
					if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External)
					{
						this._data.StateId = (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive;
						this._data.ApprovalTime = null;
						this._data.ApprovalUserEmail = null;
						this._data.ApprovalUserId = -1;
						this._data.ApprovalUserName = null;
					}
					else
					{
						var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
						this._data.StateId = (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active;
						this._data.ApprovalTime = DateTime.Now;
						this._data.ApprovalUserEmail = user?.Email;
						this._data.ApprovalUserId = this._user.Id;
						this._data.ApprovalUserName = user?.Username;
					}
				}

				// update Validators - External
				if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External)
				{
					if(this._data.Validators != null && this._data.Validators.Count > 0)
					{
						for(int i = 0; i < this._data.Validators.Count; i++)
						{
							this._data.Validators[i].Id_Project = projectEntity.Id;
							this._data.Validators[i].Level = i + 1;
						}

						// New Validators
						if(Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.DeletebyProject(projectEntity.Id) > 0)
						{
							Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.Insert(this._data.Validators?.Select(x => x.ToValidator_ProjectEntity())?.ToList());
						}
					}
				}
				// update visibilty users - Finance
				if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.Finance)
				{
					if(this._data.Validators != null && this._data.Validators.Count > 0)
					{
						// New visibilty users
						if(Infrastructure.Data.Access.Tables.FNC.FinanceProjectsVisibiltyUsersAccess.DeletebyProject(projectEntity.Id) > 0)
						{
							Infrastructure.Data.Access.Tables.FNC.FinanceProjectsVisibiltyUsersAccess.Insert(this._data.Validators?.Select(x => new Infrastructure.Data.Entities.Tables.FNC.FinanceProjectsVisibiltyUsersEntity
							{
								DateTimeAdd = DateTime.Now,
								UserAddId = _user.Id,
								ProjectId = projectEntity.Id,
								UserId = x.Id_Validator
							})?.ToList());
						}
					}
				}

				var updatedId = -1;
				// Update Project
				if(projectEntity.DepartmentId == null || projectEntity.DepartmentId == -1)
				{
					var projectEntityAlt = this._data.ToBudgetProjectExceptDept();
					projectEntityAlt.Archived = false;
					projectEntityAlt.Deleted = false;
					updatedId = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.UpdateExceptDept(projectEntityAlt);
				}
				else
				{
					var projectEntityAlt = this._data.ToBudgetProject();
					projectEntityAlt.Archived = false;
					projectEntityAlt.Deleted = false;
					updatedId = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntityAlt);
				}

				projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.Id);
				if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External)
				{
					Helpers.Notifications.Email.SendProjectCreationNotification(projectEntity.Id, this._user, false);
				}


				// - History
				Infrastructure.Data.Access.Tables.FNC.ProjectHistoryAccess.Insert(Models.Budget.Project.HistoryModel.GetHistory(projectEntity, this._user));

				return ResponseModel<int>.SuccessResponse(updatedId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.Id);
			if(projectEntity == null)
				return ResponseModel<int>.FailureResponse("Project not found");

			if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active && projectEntity.Id_Type != (int)Enums.BudgetEnums.ProjectTypes.Finance)
			{ return ResponseModel<int>.FailureResponse("Cannot update approved project"); }

			// Internal
			if(_data.TypeId == (int)Enums.BudgetEnums.ProjectTypes.Internal)
			{
				var dirs = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data.DepartmentId ?? -1);

				if(dirs == null)
					return ResponseModel<int>.FailureResponse("Department not found");

				//var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(this._data.DepartmentId ?? -1, DateTime.Today.Year);
				//if(deptBudgetEntity == null)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to department");

				//if (projectEntity.ProjectBudget + deptBudgetEntity.AmountInitial - (deptBudgetEntity.AmountAllocatedToProjects + deptBudgetEntity.AmountAllocatedToUsers) < this._data.ProjectBudget)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient department budget [{String.Format("{0:N}", projectEntity.ProjectBudget + deptBudgetEntity.AmountInitial - (deptBudgetEntity.AmountAllocatedToProjects + deptBudgetEntity.AmountAllocatedToUsers))}]");

				var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId ?? -1);
				if(companyEntity == null)
					return ResponseModel<int>.FailureResponse("Company not found");
			}
			else
			{
				if(_data.Validators == null || _data.Validators.Count <= 0)
					return ResponseModel<int>.FailureResponse("Validator list can not be empty");

				var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId ?? -1);
				if(companyEntity == null)
					return ResponseModel<int>.FailureResponse("Company not found");

				if(string.IsNullOrEmpty(_data.Description) || string.IsNullOrWhiteSpace(_data.Description))
					return ResponseModel<int>.FailureResponse("Project description can not be empty");

				// - Remove link to Company budget see Issue #127
				//var companyBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(this._data.CompanyId ?? -1, projectEntity.BudgetYear);
				//if (companyBudgetEntity == null)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to company");

				//var supplementsAmount = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { this._data.CompanyId ?? -1 }, projectEntity.BudgetYear)
				//    ?.Select(x => x.AmountInitial)?.Sum() ?? 0;
				//if (projectEntity.ProjectBudget + companyBudgetEntity.AmountInitial + supplementsAmount - (companyBudgetEntity.AmountAllocatedToDepartments + companyBudgetEntity.AmountAllocatedToProjects) < this._data.ProjectBudget)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient company budget [{String.Format("{0:N}", projectEntity.ProjectBudget + companyBudgetEntity.AmountInitial + supplementsAmount - (companyBudgetEntity.AmountAllocatedToDepartments + companyBudgetEntity.AmountAllocatedToProjects))}]");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
