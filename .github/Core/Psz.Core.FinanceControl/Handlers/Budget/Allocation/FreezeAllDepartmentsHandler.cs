using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class FreezeAllDepartmentsHandler: IHandle<UserModel, ResponseModel<bool>>
	{
		private readonly UserModel _user;
		private readonly int _data;

		public FreezeAllDepartmentsHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<bool> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;

			try
			{

				var userCompanies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { _user.Id });
				var depts = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompanies(userCompanies?.Select(x=> (long)x.Id)?.ToList());
				if(depts != null && depts.Count > 0)
				{
					depts?.ForEach(x =>
					{
						new FreezeDepartementBudgetHandler(_user, (int)x.Id, (int)Enums.BudgetEnums.freezeOrUnfreeze.Freeze).Handle();
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
