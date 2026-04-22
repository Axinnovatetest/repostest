using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CheckUserBDGHandlerXXXX: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.checkUserBdgModel>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data1 { get; set; }//landname
		private string _data2 { get; set; }//deptname
		private string _data3 { get; set; }//username
		private int _data4 { get; set; }//year
		private float _data5 { get; set; }//entred year
		private float _data6 { get; set; }//entred month
		private float _data7 { get; set; }//entred order

		public CheckUserBDGHandlerXXXX(Identity.Models.UserModel user, string land_name, string deptname, string username, int year, float entred_byear, float entred_bmonth, float entred_border)
		{
			this._user = _user;
			this._data1 = land_name;
			this._data2 = deptname;
			this._data3 = username;
			this._data4 = year;
			this._data5 = entred_byear;
			this._data6 = entred_bmonth;
			this._data7 = entred_border;
		}
		public ResponseModel<Models.Budget.checkUserBdgModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Budget.checkUserBdgModel();
				//var chk_user = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.CheckBudgetUser(this._data1, this._data2, this._data3, this._data4);

				//responseBody.DeptBudget = chk_user.DeptBudget;
				//responseBody.SOMME_USERS = chk_user.SOMME_USERS;
				//responseBody.USR = chk_user.USR;
				return ResponseModel<Models.Budget.checkUserBdgModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.checkUserBdgModel> Validate()
		{


			//var CHK_USR = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.CheckBudgetUser(this._data1, this._data2, this._data3, this._data4);
			//var DEPTBudget = CHK_USR.DeptBudget;
			//var SUMUsersBudget = CHK_USR.SOMME_USERS;
			//var UserName = CHK_USR.USR;
			//float EntredBYear = this._data5;
			//float EntredBmonth = this._data6;
			//float EntredBOrder = this._data7;


			////***do sum
			//var sum = SUMUsersBudget + EntredBYear;
			//var reste = DEPTBudget - SUMUsersBudget;

			//if (reste < 0)
			//{ reste = 0; }

			//if (UserName != "")
			//{
			//    return new ResponseModel<Models.Budget.checkUserBdgModel>()
			//    {
			//        Errors = new List<ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError>() {
			//            new ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError {Key = "0", Value = "User already assigned in this year at the same land and the same Departement"}
			//        }
			//    };
			//}
			//else
			//{
			//    //if (DEPTBudget == null || DEPTBudget == 0)
			//    if (DEPTBudget == 0 && SUMUsersBudget == 0)
			//    {
			//        return new ResponseModel<Models.Budget.checkUserBdgModel>()
			//        {
			//            Errors = new List<ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError>() {
			//            new ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError {Key = "1", Value = "Departement Have No Budget"}
			//        }
			//        };
			//    }
			//    //else if (DEPTBudget != null && sum > DEPTBudget && DEPTBudget != 0)
			//    else if (sum > DEPTBudget)
			//            {
			//        return new ResponseModel<Models.Budget.checkUserBdgModel>()
			//        {
			//            Errors = new List<ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError>() {
			//            new ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError {Key = "3", Value = "Departement Budget Exceeded, this Departement still have "+reste}
			//        }
			//        };
			//    }
			//    else if (DEPTBudget != 0 && EntredBmonth > EntredBYear)
			//    {
			//        return new ResponseModel<Models.Budget.checkUserBdgModel>()
			//        {
			//            Errors = new List<ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError>() {
			//            new ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError {Key = "4", Value = "Budget month must not exceed budget year"}
			//        }
			//        };
			//    }
			//    else if (DEPTBudget != 0 && EntredBOrder > EntredBmonth)
			//    {
			//        return new ResponseModel<Models.Budget.checkUserBdgModel>()
			//        {
			//            Errors = new List<ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError>() {
			//            new ResponseModel<Models.Budget.checkUserBdgModel>.ResponseError {Key = "5", Value = "Budget Order must not exceed budget Month"}
			//        }
			//        };
			//    }
			//}

			return ResponseModel<Models.Budget.checkUserBdgModel>.SuccessResponse();

		}
	}
}
