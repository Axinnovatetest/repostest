using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CheckDeptBDGHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.checkDeptBdgModel>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		private int _data2 { get; set; }
		private int _data3 { get; set; }
		private string _data4 { get; set; }
		private float _data5 { get; set; }
		public CheckDeptBDGHandler(Identity.Models.UserModel user, string land_name, int year, int ID_assign, string Dept_name, float current)
		{
			this._user = user;
			this._data = land_name;
			this._data2 = year;
			this._data3 = ID_assign;
			this._data4 = Dept_name;
			this._data5 = current;
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
				var chk_dept = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.CheckBudgetDept(this._data, this._data2, this._data3, this._data4);
				responseBody.LandBudget = chk_dept.LandBudget;
				responseBody.SOMME_DEPT = chk_dept.SOMME_DEPT;
				responseBody.DPT = chk_dept.DPT;
				var sum_budget_LandSupplements = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetSumBudgetLandSupplements(this._data3);
				responseBody.LandBudgetSupplement = sum_budget_LandSupplements;
				return ResponseModel<Models.Budget.checkDeptBdgModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.checkDeptBdgModel> Validate()
		{

			var CHKDept = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.CheckBudgetDept(this._data, this._data2, this._data3, this._data4);
			//var errors = new List<ResponseModel<int>.ResponseError>();
			var LandBudget = CHKDept.LandBudget;
			var SUMDeptBudget = CHKDept.SOMME_DEPT;
			var previousDeptBudget = CHKDept.SOMME_DEPT;
			var DeptName = CHKDept.DPT;
			float EntredValue = this._data5;
			var SUMLandBudgetSupplement = CHKDept.LandBudgetSupplement + LandBudget;

			//***do sum
			//var sum = (SUMDeptBudget - previousDeptBudget) + EntredValue;
			var sum = SUMDeptBudget + EntredValue;
			//var reste = LandBudget - SUMDeptBudget;
			var reste = SUMLandBudgetSupplement - SUMDeptBudget;
			if(reste < 0)
			{ reste = 0; }

			if(DeptName != "")
			{
				return new ResponseModel<Models.Budget.checkDeptBdgModel>()
				{
					Errors = new List<ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError>() {
						 new ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError {Key = "0", Value = "Departement already assigned in this year at the same land"}
					 }
				};
			}
			else
			{
				// if (LandBudget == 0 && SUMDeptBudget == 0)
				if(SUMLandBudgetSupplement == 0 && SUMDeptBudget == 0)
				{
					return new ResponseModel<Models.Budget.checkDeptBdgModel>()
					{
						Errors = new List<ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError>() {
						new ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError {Key = "1", Value = "Land Have no Budget"}
					}
					};
				}

				//if ( sum > LandBudget)
				if(sum > SUMLandBudgetSupplement)
				{
					return new ResponseModel<Models.Budget.checkDeptBdgModel>()
					{
						Errors = new List<ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError>() {
						new ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError {Key = "3", Value = "Land Budget Exceeded, this land still have "+ reste}
					}
					};
				}
				/* else if (LandBudget != null && sum != null)
					 { return ResponseModel<Models.Budget.checkDeptBdgModel>.SuccessResponse(); }*/
			}

			return ResponseModel<Models.Budget.checkDeptBdgModel>.SuccessResponse();

		}

	}
}
