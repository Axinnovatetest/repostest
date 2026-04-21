using Psz.Core.FinanceControl.Models.Statistics;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetTop5OrdersStatsHandler: IHandle<UserModel, ResponseModel<OrdersTop5StatsModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;
		public GetTop5OrdersStatsHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<OrdersTop5StatsModel> Handle()
		{
			lock(Locks.StatsLock)
			{
				try
				{
					var validationResponse = Validate();
					if(!validationResponse.Success)
						return validationResponse;

					var highestAmount = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersHighestOrLowestAmount(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, true);
					var lowestAmount = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersHighestOrLowestAmount(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

					var fastest = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetSlowestOrFastestOrders(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
					var slowest = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetSlowestOrFastestOrders(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, true);

					var highestDelay = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersHighestDelay(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
					var slowestBooking = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersSlowestBookings(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);

					var response = new OrdersTop5StatsModel
					{
						HighestAmounts = highestAmount?.Select(x => new OrdersHigestOrSlowestAmountModel(x)),
						LowestAmounts = lowestAmount?.Select(x => new OrdersHigestOrSlowestAmountModel(x)),

						FastestOrders = fastest?.Select(x => new FastestOrSlowestOrdersModel(x)),
						SlowestOrders = slowest?.Select(x => new FastestOrSlowestOrdersModel(x)),

						HighestDelay = highestDelay?.Select(x => new OrdersHigestDelayModel(x)),
						SlowestBooking = slowestBooking?.Select(x => new OrdersSlowestBookingModel(x))
					};

					return ResponseModel<OrdersTop5StatsModel>.SuccessResponse(response);
				} catch(System.Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
		public ResponseModel<OrdersTop5StatsModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<OrdersTop5StatsModel>.AccessDeniedResponse();
			}
			return ResponseModel<OrdersTop5StatsModel>.SuccessResponse();
		}
	}
}