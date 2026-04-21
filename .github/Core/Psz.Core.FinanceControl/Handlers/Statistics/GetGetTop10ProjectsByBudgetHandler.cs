using Psz.Core.FinanceControl.Models.Statistics;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetGetTop10ProjectsByBudgetHandler: IHandle<UserModel, ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModelMonth _data;

		public GetGetTop10ProjectsByBudgetHandler(UserModel user, StatsRequestModelMonth data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var entities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetTop10ProjectsByAmountOrBudget(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, false, _data.Month);
			var response = entities?.Select(x => new AllocationsVsOrdersAmountModel(x)).ToList();
			return ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>>.SuccessResponse(response);
		}
		public ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>>.SuccessResponse();
		}
	}
}