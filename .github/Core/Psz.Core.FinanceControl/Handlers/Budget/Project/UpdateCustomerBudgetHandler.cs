using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateCustomerBudgetHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Project.UpdateBudgetModel _data { get; set; }

		public UpdateCustomerBudgetHandler(Identity.Models.UserModel user, Models.Budget.Project.UpdateBudgetModel model)
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
				Helpers.Processings.Budget.Project.SaveProjectHistory(projectEntity, Enums.BudgetEnums.ProjectWorkflowActions.UpdateCustomerBudget, this._user, $"Customer budget old amount: {projectEntity?.PSZOffer} | new amount: {this._data.BudgetAmount}");
				projectEntity.PSZOffer = this._data.BudgetAmount;

				// - Notifs
				var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(projectEntity.Id)
					?.Where(x => x.Level > 0)?.ToList();
				var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities?.Select(x => x.OrderId)?.ToList());
				var emailBody = Helpers.Notifications.Email.ExternalProjectTemplate_Budget(this._user, projectEntity, orderEntities, articleEntities);
				if(!string.IsNullOrEmpty(emailBody))
				{
					Helpers.Notifications.Email.SendEmail("[Budget] External project", emailBody, new List<string> { projectEntity.ResponsableEmail }, null, this._user, null);
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

			// -
			if(this._data.BudgetAmount < projectEntity.PSZOffer)
			{
				return ResponseModel<int>.FailureResponse("Cannot decrease project budget");
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id)?.IsGlobalDirector != true)
				return ResponseModel<int>.FailureResponse("User cannot change project budget");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
