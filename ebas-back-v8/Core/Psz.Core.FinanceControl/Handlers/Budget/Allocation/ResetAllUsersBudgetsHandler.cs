using Infrastructure.Data.Entities.Tables.FNC;
using Infrastructure.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class ResetAllUsersBudgetsHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;

		public ResetAllUsersBudgetsHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var userDepts = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(_user.Id);

				var users = new List<Infrastructure.Data.Entities.Tables.COR.UserEntity>();

				users.AddRange(Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(userDepts?.Select(x => (int)x.Id).ToList() ?? new List<int> { }));

				users = users?.DistinctBy(x => x.Id).ToList();
				users?.ForEach(x =>
				{
					new ResetUserBudgetAllocationHandler(_user, x.Id).Handle();
				});

				return ResponseModel<int>.SuccessResponse(1);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}