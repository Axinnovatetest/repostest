using System;
using System.Collections.Generic;
using System.Linq;
using static Psz.Core.FinanceControl.Helpers.Processings.Budget;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class FreezeDepartementBudgetHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly int _departementId;
		private readonly int _type;

		public FreezeDepartementBudgetHandler(UserModel user, int departementId, int type)
		{
			this._user = user;
			this._departementId = departementId;
			this._type = type;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var response = -1;

			var deptAlloc = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(_departementId, DateTime.Now.Year);
			if(deptAlloc != null)
			{
				deptAlloc.LastFreezeTime = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Freeze)
					? DateTime.Now
					: null;
				deptAlloc.LastFreezeUserId = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Freeze)
				? _user.Id
					: null;
				deptAlloc.LastUnFreezeTime = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Unfreeze)
				? DateTime.Now
					: null;
				deptAlloc.LastUnFreezeUserId = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Unfreeze)
					? _user.Id
					: null;
				response = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptAlloc);
			}

			var deptUsers = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(new List<int> { _departementId });

			deptUsers?.ForEach(x =>
			{
				new FreezeBudgetUserHandler(_user, x.Id, _type).Handle();
			});
			return ResponseModel<int>.SuccessResponse(response);

		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userCompanies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { _user.Id });
			if(!_user.IsGlobalDirector && !_user.IsCorporateDirector && (userCompanies == null || userCompanies.Count <= 0))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
