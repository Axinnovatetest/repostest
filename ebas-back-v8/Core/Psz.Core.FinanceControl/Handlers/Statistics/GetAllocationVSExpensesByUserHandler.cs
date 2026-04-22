using Psz.Core.FinanceControl.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Statistics
{
	public class GetAllocationVSExpensesByUserHandler: IHandle<UserModel, ResponseModel<AmountVsExpensesModel>>
	{
		private readonly UserModel _user;
		private readonly StatsRequestModel _data;

		public GetAllocationVSExpensesByUserHandler(UserModel user, StatsRequestModel data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<AmountVsExpensesModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;
			if(_data.EmployeeId == null)
				return ResponseModel<AmountVsExpensesModel>.SuccessResponse(new AmountVsExpensesModel
				{
					Allocation = 0,
					Expenses = 0
				});
			var allocation = Infrastructure.Data.Access.Joins.FNC.Statistics.StatsticsAccess.GetAmountTotal(Infrastructure.Data.Access.Enums.FNCEnums.Amountname.AmountYear,
				Infrastructure.Data.Access.Enums.FNCEnums.AmountTable.__FNC_BudgetAllocationUser, _data.EmployeeId ?? -1, _data.Year ?? -1);

			var orderEntities = (Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByLevel((int)Enums.BudgetEnums.ValidationLevels.SiteDirector, "internal", null)?
			.Where(x => x.PoPaymentType != (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
			.Where(x => x.CreationDate.HasValue && x.CreationDate.Value.Year == _data.Year)
			)?.ToList()
			?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities.Select(x => x.OrderId)?.ToList())
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
			var orderLeasingEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByYear(_data.Year ?? -1)
						?.Where(x => x.Level > (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)?.ToList() ??
						 new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			if(_data.EmployeeId != null)
			{
				orderEntities = orderEntities.Where(y => (y.IssuerId == _data.EmployeeId))?.ToList();
				orderLeasingEntities = orderLeasingEntities.Where(x => x.IssuerId == _data.EmployeeId)?.ToList();
			}
			// - 2024-03-22 use Discount
			foreach(var item in articleEntities)
			{
				var order = orderEntities?.FirstOrDefault(x => x.OrderId == item.OrderId);
				if(order != null && order.Discount.HasValue && order.Discount.Value > 0)
				{
					item.TotalCost = (1m - order.Discount / 100) * item.TotalCost;
					item.TotalCostDefaultCurrency = (1m - order.Discount / 100) * item.TotalCostDefaultCurrency;
					item.UnitPrice = (1m - order.Discount / 100) * item.UnitPrice;
					item.UnitPriceDefaultCurrency = (1m - order.Discount / 100) * item.UnitPriceDefaultCurrency;
				}
			}
			var expenses = Helpers.Processings.Budget.Order.getItemsAmount(
							articleEntities.Where(x => orderEntities?
							.Any(z => z.OrderId == x.OrderId) == true).ToList(), false, true, orderEntities.Sum(x => x.Discount ?? 0))
							+
							(orderLeasingEntities?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, _data.Year ?? -1))?.Sum() ?? 0m);

			var response = new AmountVsExpensesModel { Allocation = allocation, Expenses = expenses };
			return ResponseModel<AmountVsExpensesModel>.SuccessResponse(response);
		}
		public ResponseModel<AmountVsExpensesModel> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<AmountVsExpensesModel>.AccessDeniedResponse();
			}

			return ResponseModel<AmountVsExpensesModel>.SuccessResponse();
		}
	}
}