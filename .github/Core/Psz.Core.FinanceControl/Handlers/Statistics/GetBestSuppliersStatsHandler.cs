using Psz.Core.FinanceControl.Models.Statistics;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetBestSuppliersStatsHandler: IHandle<UserModel, ResponseModel<BestSuppliersStatsModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;
		public GetBestSuppliersStatsHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<BestSuppliersStatsModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var bestOrdersCount = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetBestSuppliersOrdersCount(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
			var bestOrdersAmount = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetBestSuppliersOrdersAmount(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

			var response = new BestSuppliersStatsModel
			{
				BestSuppliersOrdersAmount = bestOrdersAmount?.Select(x => new OrdersAmountModel(x)).ToList(),
				BestSuppliersOrdersCount = bestOrdersCount?.Select(x => new OrdersCountModel(x)).ToList()
			};
			return ResponseModel<BestSuppliersStatsModel>.SuccessResponse(response);
		}
		public ResponseModel<BestSuppliersStatsModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<BestSuppliersStatsModel>.AccessDeniedResponse();
			}
			return ResponseModel<BestSuppliersStatsModel>.SuccessResponse();
		}
	}
}