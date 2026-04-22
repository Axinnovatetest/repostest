using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CheckDeptBDGSupplementbyIdsHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.checkDeptBdgModel>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		private int _data2 { get; set; }
		private int _data3 { get; set; }
		private int _data4 { get; set; }
		private float _data5 { get; set; }
		public CheckDeptBDGSupplementbyIdsHandler(Identity.Models.UserModel user, int land, int year, int Dept, float current)
		{
			this._user = user;
			this._data = land;
			this._data2 = year;
			this._data4 = Dept;
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
				var chk_dept = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.CheckBudgetDeptbyId(this._data, this._data2, this._data3, this._data4);
				responseBody.LandBudget = chk_dept.LandBudget;
				responseBody.SOMME_DEPT = chk_dept.SOMME_DEPT;
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
			var IDLand = Infrastructure.Data.Access.Tables.FNC.Assign_budget_landAccess.GetIDbyIdLand(this._data, this._data2);
			var CHKDept = Infrastructure.Data.Access.Tables.FNC.Assign_budget_departementAccess.CheckBudgetDeptbyId(this._data, this._data2, IDLand.ID, this._data4);
			var LandBudget = CHKDept.LandBudget;
			var SUMDeptBudget = CHKDept.SOMME_DEPT;
			var previousDeptBudget = CHKDept.SOMME_DEPT;
			var DeptName = CHKDept.DPT;
			var DeptId = CHKDept.Id_DPT;
			float EntredValue = this._data5;
			var SUMLandBudgetSupplement = CHKDept.LandBudgetSupplement + LandBudget;

			//***do sum
			var sum = SUMDeptBudget + EntredValue;
			var reste = SUMLandBudgetSupplement - SUMDeptBudget;
			if(reste < 0)
			{ reste = 0; }


			if(IDLand.ID == 0)
			{
				return new ResponseModel<Models.Budget.checkDeptBdgModel>()
				{
					Errors = new List<ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError>() {
						 new ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError {Key = "0", Value = "Land Have no Budget"}
					 }
				};
			}
			if(DeptName != "")
			{
				return new ResponseModel<Models.Budget.checkDeptBdgModel>()
				{
					Errors = new List<ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError>() {
						 new ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError {Key = "1", Value = "Departement already assigned in this year at the same land"}
					 }
				};
			}
			else
			{
				if(SUMLandBudgetSupplement == 0 && SUMDeptBudget == 0)
				{
					return new ResponseModel<Models.Budget.checkDeptBdgModel>()
					{
						Errors = new List<ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError>() {
						new ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError {Key = "2", Value = "Land Have no Budget"}
					}
					};
				}

				if(sum > SUMLandBudgetSupplement)
				{
					return new ResponseModel<Models.Budget.checkDeptBdgModel>()
					{
						Errors = new List<ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError>() {
						new ResponseModel<Models.Budget.checkDeptBdgModel>.ResponseError {Key = "3", Value = "Land Budget Exceeded, this land still have "+ reste}
					}
					};
				}

			}

			return ResponseModel<Models.Budget.checkDeptBdgModel>.SuccessResponse();

		}

	}
}
