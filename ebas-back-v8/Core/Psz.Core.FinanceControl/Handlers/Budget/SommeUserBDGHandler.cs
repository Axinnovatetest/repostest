using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class SommeUserBDGHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.checkUserBdgModel>>
	{
		public Identity.Models.UserModel _user { get; set; }
		private string _data1 { get; set; }//landname
		private string _data2 { get; set; }//deptname
		private int _data4 { get; set; }//year


		public SommeUserBDGHandler(Identity.Models.UserModel user, string land_name, string deptname, int year)
		{
			this._user = _user;
			this._data1 = land_name;
			this._data2 = deptname;

			this._data4 = year;

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
				//var chk_user = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.SumBudgetUser(this._data1, this._data2, this._data4);

				//responseBody.DeptBudget = chk_user.DeptBudget;
				//responseBody.SOMME_USERS = chk_user.SOMME_USERS;

				return ResponseModel<Models.Budget.checkUserBdgModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.checkUserBdgModel> Validate()
		{


			//var Sum_USR = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.SumBudgetUser(this._data1, this._data2, this._data4);
			//var DEPTBudget = Sum_USR.DeptBudget;
			//var SUMUsersBudget = Sum_USR.SOMME_USERS;



			////***do sum
			//var sum = SUMUsersBudget;
			//var reste = DEPTBudget - SUMUsersBudget;

			//if (reste < 0)
			//{ reste = 0; }




			return ResponseModel<Models.Budget.checkUserBdgModel>.SuccessResponse();

		}
	}
}
