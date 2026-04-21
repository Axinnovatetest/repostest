using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteUserHandler: IHandle<Models.Budget.GetBudgetUsersModel, ResponseModel<int>>
	{
		private int _UserID { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteUserHandler(int userid, Identity.Models.UserModel user)
		{
			this._UserID = userid;
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

				//var userentity = Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.Get(_UserID);
				//if (userentity == null)
				//{
				//    return ResponseModel<int>.SuccessResponse();
				//}

				return ResponseModel<int>.SuccessResponse(/*Infrastructure.Data.Access.Tables.FNC.Budget_usersAccessXXX.Delete(userentity.ID)*/-1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.AssignDeleteUser)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
