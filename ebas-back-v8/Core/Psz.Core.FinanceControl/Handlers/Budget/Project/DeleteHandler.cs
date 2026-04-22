using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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
				/// delete validators
				var validatorEntity = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectId(this._data)?.ToList();

				if(validatorEntity != null && validatorEntity.Count > 0)
				{
					Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.DeletebyProject(this._data);

				}

				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data);
				// Internal Project
				if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.Internal)
				{
					// - Allocation of budget
					//var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(projectEntity.DepartmentId ?? -1, projectEntity.BudgetYear);
					//deptBudgetEntity.AmountAllocatedToProjects = (deptBudgetEntity.AmountAllocatedToProjects ?? 0) - projectEntity.ProjectBudget;
					//deptBudgetEntity.LastEditTime = DateTime.Now;
					//deptBudgetEntity.LastEditUserId = this._user.Id;
					//Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptBudgetEntity);
				}
				else
				{
					// - Remove link to Company budget see Issue #127
					//var companyBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(projectEntity.CompanyId ?? -1, projectEntity.BudgetYear);
					//companyBudgetEntity.AmountAllocatedToProjects = (companyBudgetEntity.AmountAllocatedToProjects ?? 0) - projectEntity.ProjectBudget;
					//companyBudgetEntity.LastEditTime = DateTime.Now;
					//companyBudgetEntity.LastEditUserId = this._user.Id;
					//Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(companyBudgetEntity);
				}

				/// delete project
				projectEntity.Deleted = true;
				projectEntity.DeleteTime = DateTime.Now;
				projectEntity.DeleteUserId = this._user.Id;

				// - History
				Helpers.Processings.Budget.Project.SaveProjectHistory(projectEntity, Enums.BudgetEnums.ProjectWorkflowActions.Delete, this._user);
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity));


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

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data);
			if(projectEntity == null)
				return ResponseModel<int>.FailureResponse("Project not found");

			if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External && projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active)
				return ResponseModel<int>.FailureResponse("Cannot delete approved project");

			if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External && projectEntity.ProjectStatus == (int)Enums.BudgetEnums.ProjectStatuses.Active)
				return ResponseModel<int>.FailureResponse("Cannot delete active project");

			var orderEntites = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(projectEntity.Id);
			if(orderEntites != null && orderEntites.Count > 0)
				return ResponseModel<int>.FailureResponse("Cannot delete project with orders");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
