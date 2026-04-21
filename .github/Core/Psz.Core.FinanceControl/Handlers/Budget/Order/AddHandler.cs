using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	using System.Threading.Tasks;

	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.OrderModel _data { get; set; }

		public AddHandler(Identity.Models.UserModel user, Models.Budget.Order.OrderModel model)
		{
			this._user = user;
			this._data = model;
		}

		public async Task<ResponseModel<int>> Handleasync()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(userEntity.CompanyId ?? -1);
				var departmentEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(userEntity.DepartmentId ?? -1);
				var companyFNCExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity?.CompanyId ?? -1);
				var orderCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(this._data.CurrencyId ?? -1)
					?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };

				this._data.ResponsableId = userEntity.Id;
				this._data.ResponsableName = userEntity.Name;

				this._data.CompanyId = companyEntity?.Id ?? -1;
				this._data.CompanyName = companyEntity?.Name;
				this._data.DepartmentId = ((int?)departmentEntity?.Id) ?? -1;
				this._data.DepartmentName = departmentEntity?.Name;


				this._data.BillingCompanyId = this._data.BillingCompanyId <= 0
					? companyEntity?.Id ?? -1
					: this._data.BillingCompanyId;
				this._data.BillingCompanyName = string.IsNullOrWhiteSpace(this._data.BillingCompanyName)
					? companyEntity?.Name
					: this._data.BillingCompanyName;

				this._data.OrderDate = DateTime.Now;
				this._data.Deleted = false;
				this._data.Archived = false;
				this._data.TotalAmount = this._data.Articles?.Select(x => (decimal)(x.TotalCost_Article ?? 0) * (1 + (x.VAT ?? 0)))?.Sum() ?? 0m;
				this._data.TotalAmountDefaultCurrency = this._data.Articles?.Select(x => (decimal)(x.TotalCost_Article ?? 0) * (decimal)(orderCurrency?.entspricht_DM ?? 1) * (1 + (x.VAT ?? 0)))?.Sum() ?? 0m;

				// - 2022-01-25 - Allocation type
				if(this._data.AllocationType.HasValue && this._data.AllocationType == (int)Enums.BudgetEnums.AllocationTypes.Invest)
				{
					this._data.AllocationTypeName = Enums.BudgetEnums.AllocationTypes.Invest.GetDescription();
				}
				else
				{ // - default as Fix
					this._data.AllocationType = (int)Enums.BudgetEnums.AllocationTypes.Fix;
					this._data.AllocationTypeName = Enums.BudgetEnums.AllocationTypes.Fix.GetDescription();
				}

				//var maxBestellungNr = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetByMaxBestellungNr();
				// - -- -
				var supplierEntity = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get(this._data.SupplierId ?? -1);
				if(supplierEntity != null && supplierEntity.Nummer == 89) // - generic supplier
				{
					this._data.SupplierId = 0;
				}
				var bestellungEntity = this._data.ToBestellungenEntity();
				//bestellungEntity.Bestellung_Nr = maxBestellungNr + 1;
				var orderId = 0;
				try
				{
					orderId = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.InsertWoBestllungNr(bestellungEntity);
					// - -- -
					this._data.Number = $"PO-{companyFNCExtension?.OrderPrefix?.Trim().ToUpper()}{this._data.Type?.Trim().Substring(0, 3).ToUpper()}{(this._data.ProjectId > 0 ? "-" + this._data.ProjectId.ToString("D4") : "")}{orderId.ToString("D4")}"; // internal order may not have project

					var bestellExt = this._data.ToOrderExtension();
					bestellExt.OrderId = orderId;
					var defaultCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(companyFNCExtension?.DefaultCurrencyId ?? -1)
						?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
					bestellExt.DefaultCurrencyId = defaultCurrency?.Nr;
					bestellExt.DefaultCurrencyName = defaultCurrency?.Symbol;
					bestellExt.DefaultCurrencyRate = (decimal?)defaultCurrency?.entspricht_DM;
					bestellExt.DefaultCurrencyDecimals = defaultCurrency?.Dezimalstellen;
					try
					{
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Insert(bestellExt);
					} catch(Exception exc)
					{
						Infrastructure.Services.Logging.Logger.Log(exc.StackTrace);
						Infrastructure.Services.Logging.Logger.Log(exc.InnerException?.StackTrace);
					}

					if(this._data.File != null && this._data.File.Count > 0)
					{
						foreach(var fileItem in this._data.File)
						{
							fileItem.idOrder = orderId;
							fileItem.userId = this._user.Id;
							Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.Insert(await fileItem.ToFile_OrderEntity(_user.Id));
						}
					}

					// -
					this._data.Id_Order = orderId;
					Helpers.Processings.Budget.Order.HandleArticles(this._data, this._user);

					// - history
					var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(orderId);
					Helpers.Processings.Budget.Order.SaveOrderHistory(orderEntity, Enums.BudgetEnums.OrderWorkflowActions.Create, this._user);
				} catch(Exception exb)
				{
					Infrastructure.Services.Logging.Logger.Log(exb.StackTrace);
					Infrastructure.Services.Logging.Logger.Log(exb.InnerException?.StackTrace);
				}


				// Insert Order
				return ResponseModel<int>.SuccessResponse(orderId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			#region >>>> Leasing Date <<<<
			if(this._data.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
			{
				var leasingAllowedDepartments = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetAllowedLeasing();
				if(leasingAllowedDepartments?.Exists(x => x.Id == userEntity.DepartmentId) != true)
					return ResponseModel<int>.FailureResponse(key: "1", value: "user cannot add Leasing PO");

				if(this._data.LeasingStartMonth == null || this._data.LeasingStartMonth <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Start Month invalid");

				if(this._data.LeasingStartYear == null || this._data.LeasingStartYear <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Start Year invalid");

				if(this._data.LeasingMonthAmount == null || this._data.LeasingMonthAmount <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Month Amount invalid");

				if(this._data.LeasingNbMonths == null || this._data.LeasingNbMonths <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Nb months invalid");

				if(this._data.LeasingStartYear < DateTime.Today.Year
					|| this._data.LeasingStartYear == DateTime.Today.Year && this._data.LeasingStartMonth < DateTime.Today.Month)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing cannot start in the past");

				var companyFncExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity.CompanyId ?? -1);
				if(this._data.CurrencyId != null && this._data.CurrencyId != companyFncExtension?.DefaultCurrencyId)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Leasing accepts only [{companyFncExtension?.DefaultCurrencyName}] as currency.");
			}
			#endregion Leasing Date

			#region >>>> Order Amount <<<<
			var orderCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(this._data.CurrencyId ?? -1)
				?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
			var orderAmount = 0m;
			if(this._data.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
			{
				orderAmount = this._data.Articles.Select(x => (decimal)(x.TotalCost_Article ?? 0) * (decimal)(orderCurrency?.entspricht_DM ?? 1))?.Sum() ?? 0; // - ignore VAT
			}
			else
			{
				if(this._data.LeasingStartYear == DateTime.Today.Year && this._data.LeasingStartMonth == DateTime.Today.Month)
				{
					orderAmount = (this._data.LeasingMonthAmount ?? 0) * (decimal)(orderCurrency?.entspricht_DM ?? 1) / (this._data.LeasingNbMonths ?? 1);
					//orderAmount = (this._data.LeasingTotalAmount ?? 0)* (decimal)(orderCurrency?.entspricht_DM ?? 1) / (this._data.LeasingNbMonths ?? 1);
				}
				// - if Leasing does not start current year and month leave Amount to Zero!
			}
			#endregion Order Amount

			#region >>>> Budget Checks <<<<
			// External Order/Project
			if(this._data.Type?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower().ToLower())
			{
				// - No leasing in External
				if(this._data.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
					return ResponseModel<int>.FailureResponse(key: "1", value: "External order cannot be leased");

				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId);
				if(projectEntity == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: "External order should have a project");

				//if ((projectEntity.ProjectBudget - projectEntity.TotalSpent) < orderAmount)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient project budget [{String.Format("{0:N}", projectEntity.ProjectBudget - projectEntity.TotalSpent)}]");

				if(projectEntity.Closed.HasValue && projectEntity.Closed.Value == true)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been closed");

				if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Project is archived");

				if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been deactivated");

			}
			else // Internal order
			{

				// - 2022-12-12 - end-of-year closing - 2KW prior to 12.31 - given date
				// - 2022-12-19 - External Orders should be allowed
				if(DateTime.Today > Module.ModuleSettings.LastDayOfOrder && DateTime.Today.Year <= Module.ModuleSettings.LastDayOfOrder.Year && !this._user.SuperAdministrator)
					return ResponseModel<int>.FailureResponse("Internal Order creation is closed for this year!");

				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId);
				if(projectEntity != null)
				{
					if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Project is archived");

					if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been deactivated");

					if(projectEntity.Closed.HasValue && projectEntity.Closed.Value == true)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been closed");
				}

				var budgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, DateTime.Today.Year);
				if(budgetEntity == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to user");

				// - 2024-02-06 - allow save w/o enough budget
				//REM: Leasing Amount is added only if it starts on current Month and Year
				//if(budgetEntity.AmountOrder /*- budgetEntity.AmountSpent*/ < orderAmount)
				//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient user Order Budget");

				// - 2024-02-06 - allow save w/o enough budget
				//var monthExpenses =
				//	Helpers.Processings.Budget.Order.getItemsAmount(Helpers.Processings.Budget.Order.getPurchaseItems(this._user.Id, DateTime.Today.Year, DateTime.Today.Month), false, true, this._data.Discount ?? 0) +
				//	Helpers.Processings.Budget.Order.GetLeasingAmount(this._user.Id, DateTime.Today.Year, DateTime.Today.Month);
				//REM: Leasing Amount is added only if it starts on current Month and Year
				//if(budgetEntity.AmountMonth - monthExpenses < orderAmount)
				//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient user Month Budget [{String.Format("{0:N}", budgetEntity.AmountMonth - monthExpenses)}]");

				// - 2024-02-06 - allow save w/o enough budget
				//var yearExpenses =
				//		Helpers.Processings.Budget.Order.getItemsAmount(Helpers.Processings.Budget.Order.getPurchaseItems(this._user.Id, DateTime.Today.Year, null), false, true, this._data.Discount ?? 0) +
				//		Helpers.Processings.Budget.Order.GetLeasingAmount(this._user.Id, DateTime.Today.Year, null);
				//REM: Leasing Amount is added only if it starts on current Month and Year
				//if(budgetEntity.AmountYear - yearExpenses < orderAmount)
				//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient user Year Budget [{String.Format("{0:N}", budgetEntity.AmountYear - yearExpenses)}]");
			}
			#endregion Budget Checks 

			return ResponseModel<int>.SuccessResponse();
		}

		internal ResponseModel<int> ValidatePurchase(Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity)
		{
			if(this._data.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
			{
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order is a leasing");
			}

			var orderCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(this._data.CurrencyId ?? -1)
				?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
			var orderAmount = this._data.Articles.Select(x => (decimal)(x.TotalCost_Article ?? 0) * (decimal)(orderCurrency?.entspricht_DM ?? 1))?.Sum() ?? 0; // - ignore VAT

			// - External Order/Project
			if(this._data.Type?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower().ToLower())
			{
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId);
				if(projectEntity == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: "External order should have a project");

				//if ((projectEntity.ProjectBudget - projectEntity.TotalSpent) < orderAmount)
				//    return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient project budget [{String.Format("{0:N}", projectEntity.ProjectBudget - projectEntity.TotalSpent)}]");

				if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been deactivated");
			}
			else // Internal order
			{
				var budgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id,
					this._data.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
					? DateTime.Today.Year
					: this._data.LeasingStartYear ?? -1);

				if(budgetEntity == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to user");

				// - 2024-02-06 - allow save w/o enough budget
				//if(budgetEntity.AmountOrder /*- budgetEntity.AmountSpent*/ < orderAmount)
				//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient user Order Budget");

				var monthExpenses =
					Helpers.Processings.Budget.Order.getPurchaseAmount(userEntity.Id, DateTime.Today.Year, DateTime.Today.Month) +
					Helpers.Processings.Budget.Order.GetLeasingAmount(userEntity.Id, DateTime.Today.Year, DateTime.Today.Month);
				// - 2024-02-06 - allow save w/o enough budget
				//if(budgetEntity.AmountMonth - monthExpenses < orderAmount)
				//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient user Month Budget [{String.Format("{0:N}", budgetEntity.AmountMonth - monthExpenses)}]");

				var yearExpenses =
					Helpers.Processings.Budget.Order.getPurchaseAmount(userEntity.Id, DateTime.Today.Year, null) +
					Helpers.Processings.Budget.Order.GetLeasingAmount(userEntity.Id, DateTime.Today.Year, null);
				// - compute leased months in curr year 
				// - 2024-02-06 - allow save w/o enough budget
				//if(budgetEntity.AmountYear - yearExpenses < orderAmount)
				//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Insufficient user Year Budget [{String.Format("{0:N}", budgetEntity.AmountYear - yearExpenses)}]");
			}

			return ResponseModel<int>.SuccessResponse();
		}

		public ResponseModel<int> Handle()
		{
			throw new NotImplementedException();
		}
	}
}
