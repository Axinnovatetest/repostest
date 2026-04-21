using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetProjectHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetProjectsModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetProjectHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.GetProjectsModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetProjectsModel>();
				var projects_tableEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();


				foreach(var project_tableEntity in projects_tableEntities)
				{
					responseBody.Add(new Models.Budget.GetProjectsModel(project_tableEntity));
				}

				return ResponseModel<List<Models.Budget.GetProjectsModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.GetProjectsModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.GetProjectsModel>>.SuccessResponse();


		}
	}
}
