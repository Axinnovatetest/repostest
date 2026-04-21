using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class FreezeBudgetUserHandler: IHandle<UserModel, ResponseModel<bool>>
	{
		private readonly UserModel _user;
		private readonly int _userId;
		private readonly int _type;

		public FreezeBudgetUserHandler(UserModel user, int _userId, int type)
		{
			this._user = user;
			this._userId = _userId;
			this._type = type;
		}
		public ResponseModel<bool> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;
			var response = -1;
			var userAlloc = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(_userId, DateTime.Now.Year);
			if(userAlloc != null)
			{
				userAlloc.AmountMonth = userAlloc.AmountMonth * -1;
				userAlloc.AmountOrder = userAlloc.AmountOrder * -1;
				userAlloc.LastFreezeTime = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Freeze)
					? DateTime.Now
					: null;
				userAlloc.LastFreezeUserId = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Freeze)
					? _user.Id
					: null;
				userAlloc.LastUnFreezeTime = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Unfreeze)
					? DateTime.Now
					: null;
				userAlloc.LastUnFreezeUserId = (_type == (int)Enums.BudgetEnums.freezeOrUnfreeze.Unfreeze)
					? _user.Id
					: null;
				response = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userAlloc);
			}

			return ResponseModel<bool>.SuccessResponse(response > 0);
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
