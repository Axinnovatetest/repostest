using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class OpenHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public OpenHandler(Identity.Models.UserModel user, int id)
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

				/// 
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data);
				projectEntity.Closed = false;
				projectEntity.ClosedTime = null;
				projectEntity.ClosedUserId = null;
				projectEntity.ClosedUserName = null;
				projectEntity.ClosedUserEmail = null;
				projectEntity.ProjectStatus = (int)Enums.BudgetEnums.ProjectStatuses.Active;
				projectEntity.ProjectStatusName = Enums.BudgetEnums.ProjectStatuses.Active.GetDescription();


				var emailAddresses = new List<string> { projectEntity.ResponsableEmail };
				if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External)
				{
					var globalDirectors = Infrastructure.Data.Access.Tables.COR.UserAccess.GetGlobalDirectors();
					if(globalDirectors != null && globalDirectors.Count > 0)
					{
						emailAddresses.AddRange(globalDirectors.Where(x => x.Id > 0)?.Select(x => x.Email));
					}
				}
				var emailBody = Helpers.Notifications.Email.ExternalProjectTemplate_StatusChange(this._user, projectEntity, "Open");
				Helpers.Notifications.Email.SendEmail($"[Budget] {((Enums.BudgetEnums.ProjectTypes)projectEntity.Id_Type).GetDescription()} project status change", emailBody, emailAddresses, null, this._user, null);

				// - History
				Infrastructure.Data.Access.Tables.FNC.ProjectHistoryAccess.Insert(Models.Budget.Project.HistoryModel.GetHistory(projectEntity, this._user));
				Helpers.Processings.Budget.Project.SaveProjectHistory(projectEntity, Enums.BudgetEnums.ProjectWorkflowActions.Open, this._user);

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

			// if closing project, check in-progress orders
			var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(this._data)
				?.Where(x => x.Level > 0 && x.ApprovalUserId == null)?.ToList();
			if(orderEntities != null && orderEntities.Count > 0)
				return ResponseModel<int>.FailureResponse("Cannot open project with in-progress orders");

			var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(user?.IsGlobalDirector != true && user?.SuperAdministrator != true
				&& this._user.Id != projectEntity.ResponsableId)
				return ResponseModel<int>.FailureResponse("User cannot change project status");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
