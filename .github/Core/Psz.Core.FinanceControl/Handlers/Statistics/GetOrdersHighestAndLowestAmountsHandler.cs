using Psz.Core.FinanceControl.Models.Statistics;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetOrdersHighestAndLowestAmountsHandler: IHandle<UserModel, ResponseModel<OrdersHighestOrLowestAmountModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;

		public GetOrdersHighestAndLowestAmountsHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<OrdersHighestOrLowestAmountModel> Handle()
		{
			lock(Locks.StatsLock)
			{
				try
				{
					var validationResponse = Validate();
					if(!validationResponse.Success)
						return validationResponse;

					var highest = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersHighestOrLowestAmount(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, true);
					var lowest = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersHighestOrLowestAmount(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
					var response = new OrdersHighestOrLowestAmountModel
					{
						HighestAmounts = highest?.Select(x => new OrdersAmountsModel(x)),
						LowestAmounts = lowest?.Select(x => new OrdersAmountsModel(x))
					};
					return ResponseModel<OrdersHighestOrLowestAmountModel>.SuccessResponse(response);

				} catch(System.Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
		public ResponseModel<OrdersHighestOrLowestAmountModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<OrdersHighestOrLowestAmountModel>.AccessDeniedResponse();
			}
			return ResponseModel<OrdersHighestOrLowestAmountModel>.SuccessResponse();
		}
	}
}