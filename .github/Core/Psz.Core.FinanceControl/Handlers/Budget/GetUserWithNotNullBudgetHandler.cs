using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetUserWithNotNullBudgetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetBudgetUsersModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		private string _data2 { get; set; }
		private string _data3 { get; set; }
		private int _data4 { get; set; }
		public GetUserWithNotNullBudgetHandler(Identity.Models.UserModel user, string username, string land_name, string dept_name, int year)
		{
			this._user = user;
			this._data = username;
			this._data2 = land_name;
			this._data3 = dept_name;
			this._data4 = year;
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
				//var users_tableEntities = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.Get(this._data, this._data2,this._data3,this._data4);

				//if (users_tableEntities!=null)
				//{
				//    foreach (var dep_tableEntity in users_tableEntities)
				//    {
				//        responseBody.Add(new Models.Budget.GetBudgetUsersModel(dep_tableEntity));
				//    }
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
