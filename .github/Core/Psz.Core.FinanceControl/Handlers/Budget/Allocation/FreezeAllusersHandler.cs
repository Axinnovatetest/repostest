using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class FreezeAllUsersHandler: IHandle<UserModel, ResponseModel<bool>>
	{
		private readonly UserModel _user;

		public FreezeAllUsersHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<bool> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;

			try
			{
				var userDepts = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(_user.Id);
				if(userDepts != null && userDepts.Count > 0)
				{
					var users = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(userDepts.Select(x => (int)x.Id).ToList());
					users?.ForEach(x =>
					{
						new FreezeBudgetUserHandler(_user, x.Id, (int)Enums.BudgetEnums.freezeOrUnfreeze.Freeze).Handle();
					});
				}
				return ResponseModel<bool>.SuccessResponse(true);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<bool> Validate()
		{

			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}
			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
