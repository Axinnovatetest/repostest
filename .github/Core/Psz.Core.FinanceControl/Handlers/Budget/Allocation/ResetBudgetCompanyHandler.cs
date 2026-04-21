using System;
using System.Collections.Generic;
using System.Linq;


namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	public class ResetBudgetCompanyHandler: IHandle<UserModel, ResponseModel<bool>>
	{
		private readonly UserModel _user;
		private readonly int _companyId;

		public ResetBudgetCompanyHandler(UserModel user, int compId)
		{
			this._user = user;
			this._companyId = compId;
		}

		public ResponseModel<bool> Handle()
		{
			var validationResult = this.Validate();
			if(!validationResult.Success)
				return validationResult;
			try
			{
				var response = -1;
				var companyAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(_companyId, DateTime.Now.Year);
				if(companyAllocation != null)
				{
					var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(companyAllocation.CompanyId);

					var orderEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
					var orderInternalEntities =
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyAndLevelAndPayementTypeAndYear((int)Enums.BudgetEnums.ValidationLevels.SiteDirector,
						(int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
						 DateTime.Now.Year, _companyId, Enums.BudgetEnums.ProjectTypes.Internal.GetDescription(), null);
					if(orderInternalEntities != null && orderInternalEntities.Count > 0)
					{
						orderEntities.AddRange(orderInternalEntities);
					}

					var orderFinanceEntities =
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyAndLevelAndPayementTypeAndYear((int)Enums.BudgetEnums.ValidationLevels.SiteDirector,
						(int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
						 DateTime.Now.Year, _companyId, Enums.BudgetEnums.ProjectTypes.Finance.GetDescription(), null);
					if(orderFinanceEntities != null && orderFinanceEntities.Count > 0)
					{
						orderEntities.AddRange(orderFinanceEntities);
					}

					var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities.Select(x => x.OrderId)?.ToList())
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();

					var orderLeasingEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByCompanyAndYear(DateTime.Now.Year, _companyId, null);



					var spent = Helpers.Processings.Budget.Order.getItemsAmount(
								articleEntities.Where(x => orderEntities?
								.Any(z => z.OrderId == x.OrderId) == true).ToList(), false, true)
								+
								(orderLeasingEntities?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, DateTime.Now.Year))?.Sum() ?? 0m);

					companyAllocation.AmountInitial = spent;
					companyAllocation.LastResetTime = DateTime.Now;
					companyAllocation.LastResetUserId = _user.Id;

					//companyAllocation.AmountAllocatedToDepartments -= spent;

					response = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(companyAllocation);
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
