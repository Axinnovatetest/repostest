using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{
	public class GetInternalOrdersByDepartmentHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, int>>>>
	{
		private readonly UserModel _user;
		private readonly int _id;

		public GetInternalOrdersByDepartmentHandler(UserModel user, int id)
		{
			this._user = user;
			this._id = id;
		}
		public ResponseModel<List<KeyValuePair<int, int>>> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;

			var response = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetInternalOrders(_id, false);
			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(response);
		}

		public ResponseModel<List<KeyValuePair<int, int>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, int>>>.AccessDeniedResponse();
			}
			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse();
		}
	}
}
