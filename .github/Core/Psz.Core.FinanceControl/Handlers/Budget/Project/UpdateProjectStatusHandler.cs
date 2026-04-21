using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateProjectStatusHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Project.UpdateStatusModel _data { get; set; }

		public UpdateProjectStatusHandler(Identity.Models.UserModel user, Models.Budget.Project.UpdateStatusModel model)
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
					$"Status from: {projectEntity?.ProjectStatusName} to: {((Enums.BudgetEnums.ProjectStatuses)this._data.Status).GetDescription()}");

				projectEntity.ProjectStatus = this._data.Status;
				projectEntity.ProjectStatusChangeTime = DateTime.Now;
				projectEntity.ProjectStatusChangeUserId = this._user.Id;
				projectEntity.ProjectStatusChangeUserName = this._user.Name;
				projectEntity.ProjectStatusName = ((Enums.BudgetEnums.ProjectStatuses)this._data.Status).GetDescription();

				if(this._data.Status == (int)Enums.BudgetEnums.ProjectStatuses.Suspended)
				{
					var emailAddresses = new List<string> { projectEntity.ResponsableEmail };
					if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External)
					{
						var globalDirectors = Infrastructure.Data.Access.Tables.COR.UserAccess.GetGlobalDirectors();
						if(globalDirectors != null && globalDirectors.Count > 0)
						{
							emailAddresses.AddRange(globalDirectors.Where(x => x.Id > 0)?.Select(x => x.Email));
						}
					}
					var emailBody = Helpers.Notifications.Email.ExternalProjectTemplate_StatusChange(this._user, projectEntity, this._data.Status);
					Helpers.Notifications.Email.SendEmail($"[Budget] {((Enums.BudgetEnums.ProjectTypes)projectEntity.Id_Type).GetDescription()} project status change", emailBody, emailAddresses, null, this._user, null);
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
			if(this._data.Status == (int)Enums.BudgetEnums.ProjectStatuses.Suspended)
			{
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(this._data.ProjectId)
					?.Where(x => x.Level > 0 && x.ApprovalUserId == null)?.ToList();
				if(orderEntities != null && orderEntities.Count > 0)
					return ResponseModel<int>.FailureResponse("Cannot suspend project with in-progress orders");
			}

			var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(user?.IsGlobalDirector != true && user?.SuperAdministrator != true
				&& this._user.Id != projectEntity.ResponsableId)
				return ResponseModel<int>.FailureResponse("User cannot change project status");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
