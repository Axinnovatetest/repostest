using Psz.Core.FinanceControl.Models.Statistics;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetWorstSupplierStatsHandler: IHandle<UserModel, ResponseModel<WorstSuppliersStatsModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;

		public GetWorstSupplierStatsHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<WorstSuppliersStatsModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var biggestOverdueOrders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetWorstSuppliersBiggestOverdueOrders(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var biggestCountOfOverdueOrders = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetWorstSuppliersBiggestCountOfOverdueOrders(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var biggestBookingCountForOneOrder = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetWorstSuppliersBiggestBookingCountForOneOrder(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

			var response = new WorstSuppliersStatsModel
			{
				BiggestBookingCountForOneOrder = biggestBookingCountForOneOrder?.Select(x => new BookingCountForOneOrderModel(x)),
				BiggestCountOfOverdueOrders = biggestCountOfOverdueOrders?.Select(x => new BiggestCountOfOverdueOrders(x)),
				BiggestOverdueOrders = biggestOverdueOrders?.Select(x => new OverdueOrdersModel(x)),
			};
			return ResponseModel<WorstSuppliersStatsModel>.SuccessResponse(response);
		}
		public ResponseModel<WorstSuppliersStatsModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<WorstSuppliersStatsModel>.AccessDeniedResponse();
			}
			return ResponseModel<WorstSuppliersStatsModel>.SuccessResponse();
		}
	}
}