using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;


	public class GetWorkflowHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Project.WorkflowHistoryModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetWorkflowHistoryHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Budget.Project.WorkflowHistoryModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var historyEntities = Infrastructure.Data.Access.Tables.FNC.ProjectWorkflowHistoryAccess.GetByProjectId(this._data);
				var response = new List<Models.Budget.Project.WorkflowHistoryModel>();
				if(historyEntities != null && historyEntities.Count > 0)
				{
					foreach(var historyItem in historyEntities)
					{
						response.Add(new Models.Budget.Project.WorkflowHistoryModel(historyItem));
					}
				}

				return ResponseModel<List<Models.Budget.Project.WorkflowHistoryModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Project.WorkflowHistoryModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Project.WorkflowHistoryModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data) == null)
			{
				return ResponseModel<List<Models.Budget.Project.WorkflowHistoryModel>>.FailureResponse("Project not found");
			}

			return ResponseModel<List<Models.Budget.Project.WorkflowHistoryModel>>.SuccessResponse();
		}
	}
}
