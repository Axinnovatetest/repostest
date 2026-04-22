using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetAllDataProjectHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.AllDataProjectModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetAllDataProjectHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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

				var responseBody = new List<Models.Budget.AllDataProjectModel>();
				//var alldataprojects_tableEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetAllData();


				//foreach (var alldataproject_tableEntity in alldataprojects_tableEntities)
				//{
				//    responseBody.Add(new Models.Budget.AllDataProjectModel(alldataproject_tableEntity));
				//}

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
