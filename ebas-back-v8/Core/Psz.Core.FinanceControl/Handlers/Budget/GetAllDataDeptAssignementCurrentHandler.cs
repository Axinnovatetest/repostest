using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetAllDataDeptAssignementCurrentHandler
	{
		public Identity.Models.UserModel _user { get; set; }
		public int _data { get; set; }
		//***
		public GetAllDataDeptAssignementCurrentHandler(Identity.Models.UserModel user, int id_user)
		{
			this._user = user;
			this._data = id_user;
		}

		public ResponseModel<List<Models.Budget.AllDataDeptAsignementModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.AllDataDeptAsignementModel>();
				var lands_tableEntities = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.GetByUserAllDataDept(this._data);

				if(lands_tableEntities != null)
				{
					foreach(var land_tableEntity in lands_tableEntities)
					{
						responseBody.Add(new Models.Budget.AllDataDeptAsignementModel(land_tableEntity));
					}
				}

				return ResponseModel<List<Models.Budget.AllDataDeptAsignementModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.AllDataDeptAsignementModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}

			return ResponseModel<List<Models.Budget.AllDataDeptAsignementModel>>.SuccessResponse();
		}
	}
}

