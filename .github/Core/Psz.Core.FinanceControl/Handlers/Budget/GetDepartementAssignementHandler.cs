using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetDepartementAssignementHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetDepartementAssignementModel>>>
	{

		public Identity.Models.UserModel _user { get; set; }

		public GetDepartementAssignementHandler(Identity.Models.UserModel user)
		{
			this._user = user;

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
				var lands_tableEntities = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.Get();


				foreach(var land_tableEntity in lands_tableEntities)
				{
					responseBody.Add(new Models.Budget.GetDepartementAssignementModel(land_tableEntity));
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
