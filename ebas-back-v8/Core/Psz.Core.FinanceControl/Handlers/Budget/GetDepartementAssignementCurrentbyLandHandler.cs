using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetDepartementAssignementCurrentbyLandHandler
	{
		public Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }
		public string _data2 { get; set; }
		//***
		public GetDepartementAssignementCurrentbyLandHandler(Identity.Models.UserModel user, int id_user, string land)
		{
			this._user = user;
			this._data = id_user;
			this._data2 = land;
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
				var depts_tableEntities = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.GetByUserLand(this._data, this._data2);

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

