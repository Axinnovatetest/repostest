using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Project.UpdateStatusModel _data { get; set; }

		public UpdateStatusHandler(Identity.Models.UserModel user, Models.Budget.Project.UpdateStatusModel model)
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
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId);

				// - History
				Helpers.Processings.Budget.Project.SaveProjectHistory(projectEntity, Enums.BudgetEnums.ProjectWorkflowActions.UpdateStatus, this._user,
					$"Status from: {((Enums.BudgetEnums.ProjectApprovalStatuses)projectEntity.Id_State)} to: {((Enums.BudgetEnums.ProjectApprovalStatuses)this._data.Status).GetDescription()}");

				projectEntity.Id_State = this._data.Status;
				projectEntity.ApprovalTime = DateTime.Now;
				projectEntity.ApprovalUserId = this._user.Id;
				projectEntity.ApprovalUserName = this._user.Name;
				projectEntity.ApprovalUserEmail = this._user.Email;

				var emailBody = "";
				switch((Enums.BudgetEnums.ProjectApprovalStatuses)this._data.Status)
				{
					case Enums.BudgetEnums.ProjectApprovalStatuses.Inactive:
						emailBody = Helpers.Notifications.Email.ExternalProjectTemplate_Approval(this._user, projectEntity, this._data.Status);
						break;
					case Enums.BudgetEnums.ProjectApprovalStatuses.Active:
						emailBody = Helpers.Notifications.Email.ExternalProjectTemplate_Approval(this._user, projectEntity, this._data.Status);
						break;
					case Enums.BudgetEnums.ProjectApprovalStatuses.Reject:
						emailBody = Helpers.Notifications.Email.ExternalProjectTemplate_Approval(this._user, projectEntity, this._data.Status, this._data.Notes);
						break;
					default:
						break;
				}
				if(!string.IsNullOrEmpty(emailBody))
				{
					Helpers.Notifications.Email.SendEmail($"[Budget] {((Enums.BudgetEnums.ProjectTypes)projectEntity.Id_Type).GetDescription()} project status", emailBody, new List<string> { projectEntity.ResponsableEmail }, null, this._user, null);
				}

				// - History
				Infrastructure.Data.Access.Tables.FNC.ProjectHistoryAccess.Insert(Models.Budget.Project.HistoryModel.GetHistory(projectEntity, this._user));

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

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId);
			if(projectEntity == null)
				return ResponseModel<int>.FailureResponse("Project not found");

			// if deactivating project, check in-progress orders
			if(this._data.Status == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
			{
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(this._data.ProjectId)
					?.Where(x => x.Level > 0 && x.ApprovalUserId == null)?.ToList();
				if(orderEntities != null && orderEntities.Count > 0)
					return ResponseModel<int>.FailureResponse("Cannot deactivate project with in-progress orders");
			}

			var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(user?.IsGlobalDirector != true && user?.SuperAdministrator != true
				&& (projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External || projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.Internal && this._user.Id != projectEntity.ResponsableId))
				return ResponseModel<int>.FailureResponse("User cannot change project status");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
