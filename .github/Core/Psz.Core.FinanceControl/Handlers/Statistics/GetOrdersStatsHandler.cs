using System.Linq;
using Psz.Core.FinanceControl.Models.Statistics;


namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetOrdersStatsHandler: IHandle<UserModel, ResponseModel<OrdersCountsModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;

		public GetOrdersStatsHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<OrdersCountsModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{

				var byPoPaymentType = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersCountByPoPayementType(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var byOrderType = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersCountByType(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var byOrderStatus = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersCountByStatus(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId);
				var response = new OrdersCountsModel
				{
					ByPoPaymentType = byPoPaymentType?.Select(x => new OrdersCountByPoPaymentType { Count = x.Key, PoPaymentTypeName = x.Value }),
					ByType = byOrderType?.Select(x => new OrdersCountByType { Count = x.Key, OrderType = x.Value }),
					ByStatus = byOrderStatus?.Select(x => new OrdersCountByStatus { Count = x.Key, Status = ((Enums.BudgetEnums.ValidationLevels)x.Value).GetDescription() }),
					Placed = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersCountPlacedOrBooked(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId, true),
					Booked = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetOrdersCountPlacedOrBooked(_data.Year, _data.CompanyId, _data.DepartmentId, _data.EmployeeId)
				};
				return ResponseModel<OrdersCountsModel>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<OrdersCountsModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<OrdersCountsModel>.AccessDeniedResponse();
			}
			return ResponseModel<OrdersCountsModel>.SuccessResponse();
		}
	}
}