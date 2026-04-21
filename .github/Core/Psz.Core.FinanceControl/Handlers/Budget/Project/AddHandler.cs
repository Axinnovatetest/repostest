using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using iText.Layout.Element;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Project.ProjectModel _data { get; set; }

		public AddHandler(Identity.Models.UserModel user, Models.Budget.Project.ProjectModel model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				lock(Locks.ProjectLock)
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					/// 
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(this._data.CompanyId ?? -1);
					this._data.CurrencyId = companyExtensionEntity?.DefaultCurrencyId;
					this._data.CurrencyName = companyExtensionEntity?.DefaultCurrencyName;
					this._data.CreationDate = DateTime.Now;
					this._data.BudgetYear = DateTime.Now.Year;
					var projectEntity = this._data.ToBudgetProject();
					projectEntity.Archived = false;
					projectEntity.Deleted = false;
					projectEntity.Id_State = (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive;
					projectEntity.ProjectStatusName = Enums.BudgetEnums.ProjectStatuses.Active.GetDescription();
					projectEntity.ProjectStatus = (int)Enums.BudgetEnums.ProjectStatuses.Active;
					projectEntity.CreationUserId = this._user.Id;
					projectEntity.CreationUserName = this._user.Username;
					projectEntity.CreationUserEmail = this._user.Email;
					if(this._data.TypeId == (int)Enums.BudgetEnums.ProjectTypes.Internal || this._data.TypeId == (int)Enums.BudgetEnums.ProjectTypes.Finance)
					{
						projectEntity.Id_State = (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active;
						projectEntity.ProjectStatusName = Enums.BudgetEnums.ProjectStatuses.Active.GetDescription();
						projectEntity.ProjectStatus = (int)Enums.BudgetEnums.ProjectStatuses.Active;
						projectEntity.Closed = false;
					}
					var projectId = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Insert(projectEntity);

					// Internal Project
					if(this._data.TypeId == (int)Enums.BudgetEnums.ProjectTypes.External)
					{
						if(this._data.Validators != null && this._data.Validators.Count > 0)
						{
							for(int i = 0; i < this._data.Validators.Count; i++)
							{
								this._data.Validators[i].Id_Project = projectId;
								this._data.Validators[i].Level = i + 1;
							}

							// New Validators
							Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.Insert(this._data.Validators?.Select(x => x.ToValidator_ProjectEntity())?.ToList());
						}
					}
					if(this._data.TypeId == (int)Enums.BudgetEnums.ProjectTypes.Finance)
					{
						if(this._data.Validators != null && this._data.Validators.Count > 0)
						{
							var visibilityUsers = this._data.Validators.Select(v => new Infrastructure.Data.Entities.Tables.FNC.FinanceProjectsVisibiltyUsersEntity
							{
								DateTimeAdd = DateTime.Now,
								UserAddId = _user.Id,
								ProjectId = projectId,
								UserId = v.Id_Validator
							}).ToList();

							// visibilty users
							Infrastructure.Data.Access.Tables.FNC.FinanceProjectsVisibiltyUsersAccess.Insert(visibilityUsers);
						}
					}


					// - History
					projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(projectId);
					Infrastructure.Data.Access.Tables.FNC.ProjectHistoryAccess.Insert(Models.Budget.Project.HistoryModel.GetHistory(projectEntity, this._user));
					Helpers.Processings.Budget.Project.SaveProjectHistory(projectEntity, Enums.BudgetEnums.ProjectWorkflowActions.Create, this._user);

					// Insert Project
					return ResponseModel<int>.SuccessResponse(projectId);
				}
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

			// Internal
			if(_data.TypeId == (int)Enums.BudgetEnums.ProjectTypes.Internal)
			{
				var dirs = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data.DepartmentId ?? -1);

				if(dirs == null)
					return ResponseModel<int>.FailureResponse("Department not found");

				//var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(this._data.DepartmentId ?? -1, DateTime.Today.Year);
				//if (deptBudgetEntity == null)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to department");

				//if (deptBudgetEntity.AmountInitial - (deptBudgetEntity.AmountAllocatedToUsers + deptBudgetEntity.AmountAllocatedToProjects) < this._data.ProjectBudget)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient department budget [{String.Format("{0:N}", deptBudgetEntity.AmountInitial - (deptBudgetEntity.AmountAllocatedToUsers + deptBudgetEntity.AmountAllocatedToProjects))}]");

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
				//var companyBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(this._data.CompanyId ?? -1, DateTime.Today.Year);
				//if (companyBudgetEntity == null)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to company");

				//var supplementsAmount = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { this._data.CompanyId ?? -1 }, DateTime.Today.Year)
				//    ?.Select(x => x.AmountInitial)?.Sum() ?? 0;
				//if (companyBudgetEntity.AmountInitial + supplementsAmount - (companyBudgetEntity.AmountAllocatedToDepartments + companyBudgetEntity.AmountAllocatedToProjects) < this._data.ProjectBudget)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient company budget [{String.Format("{0:N}", companyBudgetEntity.AmountInitial + supplementsAmount - (companyBudgetEntity.AmountAllocatedToDepartments + companyBudgetEntity.AmountAllocatedToProjects))}]");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
