using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class ResetAllDepartementsHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;

		public ResetAllDepartementsHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<int> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;

			try
			{
				var userCompanies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { _user.Id });
				var deptsUser = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompanies(userCompanies?.Select(x => (long)x.Id)?.ToList());

				deptsUser?.ForEach(x =>
				{
					new ResetBudgetDepartmentHandler(_user,(int)x.Id).Handle();
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