using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetUserAssignementCurrentHandler
	{
		public Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }
		//***
		public GetUserAssignementCurrentHandler(Identity.Models.UserModel user, int id_user)
		{
			this._user = user;
			this._data = id_user;
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
				var usersAssign_tableEntities = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(this._data, DateTime.Today.Year);

				if(usersAssign_tableEntities != null)
				{
					//foreach (var userAssign_tableEntity in usersAssign_tableEntities)
					{
						responseBody.Add(new Models.Budget.GetBudgetUsersModel(usersAssign_tableEntities));
					}
				}

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

