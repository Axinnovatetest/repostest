using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateUserHandler
	{
		private Models.Budget.GetBudgetUsersModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateUserHandler(Models.Budget.GetBudgetUsersModel data, Identity.Models.UserModel user)
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
				//var usersEntity = new Infrastructure.Data.Entities.Tables.FNC.Budget_usersEntity()
				//{
				//    username = _data.username,
				//    land = _data.land,
				//    departement_user=_data.departement_user,
				//    budget_year=_data.budget_year,
				//    budget_month=_data.budget_month,
				//    budget_order=_data.budget_order,
				//    U_year=_data.U_year,
				//    LandId = _data.LandId,
				//    DepartmentId=_data.DepartmentId,
				//    UserId=_data.UserId,
				//    TotalSpent=_data.TotalSpent
				//};
				//var InsertedLand = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.Insert(usersEntity);
				return ResponseModel<int>.SuccessResponse(-1);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.AssignCreateUser)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
