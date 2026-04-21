using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetDepartementsWithNotNullBudgetUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetDepartementAssignementModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		private int _data2 { get; set; }
		private int _data3 { get; set; }
		public GetDepartementsWithNotNullBudgetUserHandler(Identity.Models.UserModel user, string land_name, int year, int id_user)
		{
			this._user = user;
			this._data = land_name;
			this._data2 = year;
			this._data3 = id_user;
		}
		public ResponseModel<List<Models.Budget.GetDepartementAssignementModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetDepartementAssignementModel>();
				var depts_tableEntities = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.Get(this._data, this._data2, this._data3);

				if(depts_tableEntities != null)
				{
					foreach(var dep_tableEntity in depts_tableEntities)
					{
						responseBody.Add(new Models.Budget.GetDepartementAssignementModel(dep_tableEntity));
					}
				}

				return ResponseModel<List<Models.Budget.GetDepartementAssignementModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.GetDepartementAssignementModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}

			return ResponseModel<List<Models.Budget.GetDepartementAssignementModel>>.SuccessResponse();
		}
	}
}
