using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class ResetUserBudgetAllocationHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly int _data;

		public ResetUserBudgetAllocationHandler(UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				var response = -1;
				var currentYear = DateTime.Now.Year;
				var userAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(_data, currentYear);
				if(userAllocation != null)
				{
					var orderEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
					var orderInternalEntities =
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUserAndLevelAndPayementTypeAndYear((int)Enums.BudgetEnums.ValidationLevels.Draft,
						(int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
					currentYear, _data, Enums.BudgetEnums.ProjectTypes.Internal.GetDescription(), null);
					if(orderInternalEntities != null && orderInternalEntities.Count > 0)
					{
						orderEntities.AddRange(orderInternalEntities);
					}

					var orderFinanceEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUserAndLevelAndPayementTypeAndYear((int)Enums.BudgetEnums.ValidationLevels.Draft,
						(int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
					currentYear, _data, Enums.BudgetEnums.ProjectTypes.Finance.GetDescription(), null);
					if(orderFinanceEntities != null && orderFinanceEntities.Count > 0)
					{
						orderEntities.AddRange(orderFinanceEntities);
					}

					var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities.Select(x => x.OrderId)?.ToList())
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();

					var orderLeasingEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByUserAndYear(currentYear, _data);

					var spent = Helpers.Processings.Budget.Order.getItemsAmount(
									articleEntities.Where(x => orderEntities?
									.Any(z => z.OrderId == x.OrderId) == true).ToList(), false, true)
									+
									(orderLeasingEntities?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, currentYear))?.Sum() ?? 0m);
					var diff = userAllocation.AmountYear - spent;
					userAllocation.AmountYear = spent;

					userAllocation.LastResetTime = DateTime.Now;
					userAllocation.LastResetUserId = _user.Id;
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(_data);
					var deptAllloc = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(userEntity.DepartmentId ?? -1, DateTime.Now.Year);
					deptAllloc.AmountAllocatedToUsers -= diff;
					response = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userAllocation);
					Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptAllloc);
				}
				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
