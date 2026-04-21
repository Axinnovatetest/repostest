using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class ResetAllCompaniesHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;

		public ResetAllCompaniesHandler(UserModel user)
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
				// - user is GlobalDirector or CorporateDirector
				var userCompanies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();

				userCompanies?.ForEach(x =>
				{
					new ResetBudgetCompanyHandler(_user,x.Id).Handle();
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
			if(_user == null || (!_user.IsGlobalDirector && !_user.IsCorporateDirector))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}