using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetTypeProjectHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetTypeProjectModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetTypeProjectHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.GetTypeProjectModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetTypeProjectModel>();
				var type_project_tableEntities = Infrastructure.Data.Access.Tables.FNC.Type_Project_BudgetAccess.Get();


				foreach(var type_project_tableEntity in type_project_tableEntities)
				{
					responseBody.Add(new Models.Budget.GetTypeProjectModel(type_project_tableEntity));
				}

				return ResponseModel<List<Models.Budget.GetTypeProjectModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.GetTypeProjectModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.GetTypeProjectModel>>.SuccessResponse();
		}
	}
}
