using Infrastructure.Services.Utils;
using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class FreezeAllCompaniesHandler: IHandle<UserModel, ResponseModel<bool>>
	{
		private readonly UserModel _user;

		public FreezeAllCompaniesHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<bool> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;

			var transaction = new TransactionsManager();
			try
			{
				// - user is GlobalDirector or CorporateDirector
				var companies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get();
				companies?.ForEach(x =>
				{
					new FreezeBudgetCompanyHandler(_user, x.Id, 1).Handle();
				});

				return ResponseModel<bool>.SuccessResponse(true);

			} catch(Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<bool> Validate()
		{
			if(_user == null || (!_user.IsGlobalDirector && !_user.IsCorporateDirector))
				return ResponseModel<bool>.AccessDeniedResponse();
			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
