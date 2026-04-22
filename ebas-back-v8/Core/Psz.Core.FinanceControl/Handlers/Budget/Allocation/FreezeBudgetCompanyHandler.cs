using Infrastructure.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static Psz.Core.FinanceControl.Helpers.Processings.Budget;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class FreezeBudgetCompanyHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly int _companyId;
		private readonly int _type;

		public FreezeBudgetCompanyHandler(UserModel user, int companyId, int type)
		{
			_user = user;
			_companyId = companyId;
			_type = type;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{

				var companyAlloc = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(_companyId, DateTime.Now.Year);
				if(companyAlloc != null)
				{
					companyAlloc.LastFreezeTime = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Freeze)
					? DateTime.Now
					: null;
					companyAlloc.LastFreezeUserId = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Freeze)
					? _user.Id
						: null;
					companyAlloc.LastUnFreezeTime = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Unfreeze)
					? DateTime.Now
						: null;
					companyAlloc.LastUnFreezeUserId = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Unfreeze)
						? _user.Id
						: null;
					Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(companyAlloc);
				}

				var depts = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompany(_companyId);

				depts?.ForEach(x =>
				{
					new FreezeDepartementBudgetHandler(_user, (int)x.Id, _type).Handle();
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
			if(_user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			return ResponseModel<int>.SuccessResponse();
		}
	}
}