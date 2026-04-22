using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateUserHandler: IHandle<Models.Budget.GetBudgetUsersModel, ResponseModel<int>>
	{
		private Models.Budget.GetBudgetUsersModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateUserHandler(Models.Budget.GetBudgetUsersModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//var Userentity = new Infrastructure.Data.Entities.Tables.FNC.Budget_usersEntity()
				//{
				//    ID = _data.ID,
				//    username = _data.username,
				//    land = _data.land,
				//    departement_user = _data.departement_user,
				//    budget_year = _data.budget_year,
				//    budget_month = _data.budget_month,
				//    budget_order = _data.budget_order,
				//    U_year=_data.U_year,
				//    LandId = _data.LandId,
				//    DepartmentId = _data.DepartmentId,
				//    UserId = _data.UserId,
				//    TotalSpent = _data.TotalSpent
				//};
				//var updatedLand = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.Update(Userentity);
				return ResponseModel<int>.SuccessResponse(/*updatedLand*/-1);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.AssignEditUser)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			//var UserID = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.Get(this._data.ID);
			//var CHK_USR = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.CheckBudgetUser(this._data.land, this._data.departement_user, this._data.username, this._data.U_year);
			//var DEPTBudget = CHK_USR.DeptBudget;
			//var SUMUsersBudget = CHK_USR.SOMME_USERS;
			//var UserName = CHK_USR.USR;
			//float? EntredBYear = this._data.budget_year;
			//float? EntredBmonth = this._data.budget_month;
			//float? EntredBOrder = this._data.budget_order;


			////***do sum
			//var sum = (SUMUsersBudget - UserID.budget_year) + EntredBYear;
			//var reste = DEPTBudget - SUMUsersBudget;
			//var errors = new List<ResponseModel<int>.ResponseError>();
			//if (UserID == null)
			//{
			//    return new ResponseModel<int>()
			//    {
			//        Errors = new List<ResponseModel<int>.ResponseError>() {
			//            new ResponseModel<int>.ResponseError {Key = "", Value = "User not found"}
			//        }
			//    };
			//}
			//if (sum > DEPTBudget)
			//{
			//    return new ResponseModel<int>()
			//    {
			//        Errors = new List<ResponseModel<int>.ResponseError>() {
			//            new ResponseModel<int>.ResponseError {Key = "", Value = "Departement Budget Exceeded Departement Still Have "+reste}
			//        }
			//    };
			//}
			// if (EntredBmonth > EntredBYear)
			//{
			//    return new ResponseModel<int>()
			//    {
			//        Errors = new List<ResponseModel<int>.ResponseError>() {
			//            new ResponseModel<int>.ResponseError {Key = "", Value = "Budget month must not exceed budget year"}
			//        }
			//    };
			//}
			// if (EntredBOrder > EntredBmonth)
			//{
			//    return new ResponseModel<int>()
			//    {
			//        Errors = new List<ResponseModel<int>.ResponseError>() {
			//            new ResponseModel<int>.ResponseError {Key = "", Value = "Budget Order must not exceed budget Month"}
			//        }
			//    };
			//}
			//if (errors.Count > 0)
			//{
			//    return new ResponseModel<int>() { Errors = errors };
			//}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
