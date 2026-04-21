using Psz.Core.FinanceControl.Models.Statistics;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetBestAndWorstCustomersHandler: IHandle<UserModel, ResponseModel<BestAndWorstCustomersStatsModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;
		public GetBestAndWorstCustomersHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<BestAndWorstCustomersStatsModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var bestprofit = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetBestCustomersBestProfit(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, true);
			var worstprofit = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetBestCustomersBestProfit(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

			var bestOffer = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetBestCustomersBestPszOffer(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, true);
			var WorstOffer = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetBestCustomersBestPszOffer(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

			var response = new BestAndWorstCustomersStatsModel
			{
				BestProfits = bestprofit?.Select(x => new BestProfitModel(x)),
				WorstProfits = worstprofit?.Select(x => new BestProfitModel(x)),
				BestPszOffer = bestOffer?.Select(x => new BestPszOffermodel(x)),
				WorstPszOffer = WorstOffer?.Select(x => new BestPszOffermodel(x))
			};
			return ResponseModel<BestAndWorstCustomersStatsModel>.SuccessResponse(response);
		}
		public ResponseModel<BestAndWorstCustomersStatsModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<BestAndWorstCustomersStatsModel>.AccessDeniedResponse();
			}
			return ResponseModel<BestAndWorstCustomersStatsModel>.SuccessResponse();
		}
	}
}