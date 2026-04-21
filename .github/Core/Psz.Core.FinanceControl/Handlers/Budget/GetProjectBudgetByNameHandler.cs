using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetProjectBudgetByNameHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.AllDataProjectModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		public GetProjectBudgetByNameHandler(Identity.Models.UserModel user, string value)
		{
			this._user = user;
			this._data = value;
		}

		public ResponseModel<List<Models.Budget.AllDataProjectModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// Logics
				var responseBody = new List<Models.Budget.AllDataProjectModel>();
				//var projectName_tableEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetPrjName(this._data);
				//responseBody.Add(new Models.Budget.AllDataProjectModel(projectName_tableEntities));



				/* foreach (var projectName_tableEntity in projectName_tableEntities)
                 {
                     responseBody.Add(new Models.Budget.GetProjectsModel(projectName_tableEntity));
                 }*/

				return ResponseModel<List<Models.Budget.AllDataProjectModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.AllDataProjectModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.AllDataProjectModel>>.SuccessResponse();
		}
	}
}
