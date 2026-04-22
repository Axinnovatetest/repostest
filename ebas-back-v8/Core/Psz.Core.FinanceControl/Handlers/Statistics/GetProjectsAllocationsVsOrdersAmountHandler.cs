using Psz.Core.FinanceControl.Models.Statistics;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetProjectsAllocationsVsOrdersAmountHandler: IHandle<UserModel, ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>>>
	{
		private readonly UserModel _user;
		public GetProjectsAllocationsVsOrdersAmountHandler(UserModel user)
		{
			_user = user;
		}
		public ResponseModel<IEnumerable<AllocationsVsOrdersAmountModel>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var entities = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetProjectsVsOrdersAmount();
			var response = entities?.Select(x => new AllocationsVsOrdersAmountModel(x));
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