using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class ResetBudgetDepartmentHandler: IHandle<UserModel, ResponseModel<bool>>
	{
		private readonly UserModel _user;
		private readonly int _departmentId;

		public ResetBudgetDepartmentHandler(UserModel user, int depId)
		{
			this._user = user;
			this._departmentId = depId;
		}
		public ResponseModel<bool> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;

			try
			{
				var response = -1;
				var departmentAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(_departmentId, DateTime.Now.Year);
				if(departmentAllocation != null)
				{
					var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(departmentAllocation.DepartmentId);

					var orderEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
					var orderInternalEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartementAndLevelAndPayementTypeAndYear(
						(int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector,
						 (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
						DateTime.Now.Year, _departmentId, Enums.BudgetEnums.ProjectTypes.Internal.GetDescription(), null);
					if(orderInternalEntities != null && orderInternalEntities.Count>0)
					{
						orderEntities.AddRange(orderInternalEntities);
					}

					var orderFinanceEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartementAndLevelAndPayementTypeAndYear(
						(int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector,
						 (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
						DateTime.Now.Year, _departmentId, Enums.BudgetEnums.ProjectTypes.Finance.GetDescription(), null);
					if(orderFinanceEntities != null && orderFinanceEntities.Count > 0)
					{
						orderEntities.AddRange(orderFinanceEntities);
					}

					var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities.Select(x => x.OrderId)?.ToList())
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();

					var orderLeasingEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByDepartemantAndYear(DateTime.Now.Year, _departmentId);

					var spent = Helpers.Processings.Budget.Order.getItemsAmount(
									articleEntities.Where(x => orderEntities? // currentAllocation's department orders
									.Any(z => z.OrderId == x.OrderId) == true).ToList(), false, true)
									+
									(orderLeasingEntities?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, DateTime.Now.Year))?.Sum() ?? 0m);
					var diff = departmentAllocation.AmountInitial - spent;
					departmentAllocation.AmountInitial = spent;


					departmentAllocation.LastResetUserId = _user.Id;
					departmentAllocation.LastResetTime = DateTime.Now;

					var companyAlloc = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear((int)department.CompanyId, DateTime.Now.Year);
					companyAlloc.AmountAllocatedToDepartments -= diff;
					response = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(departmentAllocation);
					Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(companyAlloc);
				}
				return ResponseModel<bool>.SuccessResponse(response > 0);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<bool> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<bool>.AccessDeniedResponse();
			}
			return ResponseModel<bool>.SuccessResponse();
		}
	}
}
