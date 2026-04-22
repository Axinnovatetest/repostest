using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CheckProjectDeptBDGHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.checkProjectDeptBdgModel>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }
		private int _data2 { get; set; }
		private string _data4 { get; set; }
		private float _data5 { get; set; }
		public CheckProjectDeptBDGHandler(Identity.Models.UserModel user, string land_name, int year, string Dept_name, float current)
		{
			this._user = user;
			this._data = land_name;
			this._data2 = year;
			this._data4 = Dept_name;
			this._data5 = current;
		}
		public ResponseModel<Models.Budget.checkProjectDeptBdgModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.checkProjectDeptBdgModel();
				//var chk_dept = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.CheckProjectBudgetDept(this._data, this._data2,this._data4);


				//responseBody.Budget_DEPT = chk_dept.Budget_DEPT;
				//responseBody.DPT = chk_dept.DPT;
				return ResponseModel<Models.Budget.checkProjectDeptBdgModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.checkProjectDeptBdgModel> Validate()
		{

			//var CHKDept = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.CheckProjectBudgetDept(this._data, this._data2, this._data4);
			////var errors = new List<ResponseModel<int>.ResponseError>();

			//var Budget_DEPT = CHKDept.Budget_DEPT;
			//var DeptName = CHKDept.DPT;
			//float EntredValue = this._data5;
			//var reste = /*Budget_DEPT*/0 - EntredValue;
			//if (reste < 0)
			//{ reste = 0; }




			//if (DeptName == "")
			//{
			//    return new ResponseModel<Models.Budget.checkProjectDeptBdgModel>()
			//    {
			//        Errors = new List<ResponseModel<Models.Budget.checkProjectDeptBdgModel>.ResponseError>() {
			//            new ResponseModel<Models.Budget.checkProjectDeptBdgModel>.ResponseError {Key = "0", Value = "Departement is not assigned in this land at this year "}
			//        }
			//    };
			//}
			//else
			//{
			//    if (Budget_DEPT == 0)
			//    {
			//        return new ResponseModel<Models.Budget.checkProjectDeptBdgModel>()
			//        {
			//            Errors = new List<ResponseModel<Models.Budget.checkProjectDeptBdgModel>.ResponseError>() {
			//            new ResponseModel<Models.Budget.checkProjectDeptBdgModel>.ResponseError {Key = "1", Value = "Departement Have no Budget"}
			//        }
			//        };
			//    }

			//   if (EntredValue > Budget_DEPT)
			//    {
			//        return new ResponseModel<Models.Budget.checkProjectDeptBdgModel>()
			//        {
			//            Errors = new List<ResponseModel<Models.Budget.checkProjectDeptBdgModel>.ResponseError>() {
			//            new ResponseModel<Models.Budget.checkProjectDeptBdgModel>.ResponseError {Key = "3", Value = "Project Budget Exceeded, the departement still have "+Budget_DEPT}
			//        }
			//        };
			//    }
			/* else if (LandBudget != null && sum != null)
				 { return ResponseModel<Models.Budget.checkProjectDeptBdgModel>.SuccessResponse(); }*/
			//}

			return ResponseModel<Models.Budget.checkProjectDeptBdgModel>.SuccessResponse();

		}

	}
}
