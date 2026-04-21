using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class SommeDeptBDGHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.checkDeptBdgModel>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		private int _data2 { get; set; }

		private float _data5 { get; set; }
		public SommeDeptBDGHandler(Identity.Models.UserModel user, string land_name, int year)
		{
			this._user = user;
			this._data = land_name;
			this._data2 = year;

		}
		public ResponseModel<Models.Budget.checkDeptBdgModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.checkDeptBdgModel();
				var chk_dept = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.SumBudgetDept(this._data, this._data2);

				responseBody.LandBudget = chk_dept.LandBudget;
				responseBody.SOMME_DEPT = chk_dept.SOMME_DEPT;

				return ResponseModel<Models.Budget.checkDeptBdgModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.checkDeptBdgModel> Validate()
		{

			var SumCHKDept = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.SumBudgetDept(this._data, this._data2);
			//var errors = new List<ResponseModel<int>.ResponseError>();
			var LandBudget = SumCHKDept.LandBudget;
			var SUMDeptBudget = SumCHKDept.SOMME_DEPT;

			float EntredValue = this._data5;

			//***do sum
			var sum = SUMDeptBudget;
			var reste = LandBudget - SUMDeptBudget;
			if(reste < 0)
			{ reste = 0; }

			return ResponseModel<Models.Budget.checkDeptBdgModel>.SuccessResponse();

		}

	}
}
