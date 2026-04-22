using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{

	public class GetResponsableDepartementHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.GetDeptJointLandResponsableModel>>

	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetResponsableDepartementHandler(Identity.Models.UserModel user, int dept)
		{
			this._user = user;
			this._data = dept;


		}
		public ResponseModel<Models.Budget.GetDeptJointLandResponsableModel> Handle()

		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.GetDeptJointLandResponsableModel();

				//var budgetLand = Infrastructure.Data.Access.Tables.FNC.Land_Department_JointAccess.GetDeptResponsable(this._data);
				//responseBody.ID = budgetLand.ID;
				//responseBody.ID_Land = budgetLand.ID_Land;
				//responseBody.ID_Department = budgetLand.ID_Department;
				//responseBody.ID_user = budgetLand.ID_user;
				//responseBody.Username = budgetLand.Username;
				//responseBody.Name = budgetLand.Name;

				return ResponseModel<Models.Budget.GetDeptJointLandResponsableModel>.SuccessResponse(responseBody);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.GetDeptJointLandResponsableModel> Validate()
		{

			return ResponseModel<Models.Budget.GetDeptJointLandResponsableModel>.SuccessResponse();


		}

	}
}
