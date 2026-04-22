using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetBudgetUsersModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetUsersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.GetBudgetUsersModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetBudgetUsersModel>();
				//var users_tableEntities = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.Get();


				//foreach (var user_tableEntity in users_tableEntities)
				//{
				//    responseBody.Add(new Models.Budget.GetBudgetUsersModel(user_tableEntity));
				//}

				return ResponseModel<List<Models.Budget.GetBudgetUsersModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.GetBudgetUsersModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.GetBudgetUsersModel>>.SuccessResponse();
		}

	}
}
