using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.FinanceControl.Helpers;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class ValidateFinanceHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Budget.Order.ValidateModel _data { get; set; }
		public ValidateFinanceHandler(Models.Budget.Order.ValidateModel validateData, UserModel user)
		{
			_user = user;
			_data = validateData;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//
				if(saveVersionHistory())
				{
					validateOrderFinance();
				}

				return ResponseModel<int>.SuccessResponse(0);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			#region >>>> Order Status <<<<
			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not found");

			if(orderEntity.Archived.HasValue && orderEntity.Archived.Value)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order is archived");

			var orderArticleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderArticleEntities == null || orderArticleEntities.Count <= 0)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order articles list empty");

			if(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
			{
				// - Multiple currencies - need to update Prices
				Helpers.Processings.Budget.Order.updateArticlePrices(new List<int> { this._data.OrderId });
			}

			//REM: >>> Network latency issues causes endpoint to execute twice and this shows wrong error
			if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.Draft && orderEntity.Level.HasValue && orderEntity.Level.Value > this._data.CurrentStep)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order already validated in current step");

			if(orderEntity.Level.HasValue && orderEntity.Level.Value < this._data.CurrentStep)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order is not reviewed yet by previous validator");
			#endregion Order Status

			#region >>>> Leasing Date <<<<
			if(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
			{
				if(orderEntity.LeasingStartMonth == null || orderEntity.LeasingStartMonth <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Start Month invalid");

				if(orderEntity.LeasingStartYear == null || orderEntity.LeasingStartYear <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Start Year invalid");

				if(orderEntity.LeasingMonthAmount == null || orderEntity.LeasingMonthAmount <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Month Amount invalid");

				if(orderEntity.LeasingNbMonths == null || orderEntity.LeasingNbMonths <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing Nb months invalid");

				if(orderEntity.LeasingStartYear < DateTime.Today.Year
					|| orderEntity.LeasingStartYear == DateTime.Today.Year && orderEntity.LeasingStartMonth < DateTime.Today.Month)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Leasing cannot start in the past");

				if(new DateTime(orderEntity.LeasingStartYear.Value, orderEntity.LeasingStartMonth.Value, 1).AddMonths(orderEntity.LeasingNbMonths.Value) < DateTime.Today)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot validate Leasing that has already ended");

				//if (this._data.CurrentStep == 0)
				{
					if(orderEntity.LeasingStartYear < DateTime.Today.Year
						|| orderEntity.LeasingStartYear == DateTime.Today.Year && orderEntity.LeasingStartMonth < DateTime.Today.Month)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot validate Leasing that has already started");
				}
			}
			#endregion Leasing Date

			#region >>>> Order Amount <<<<
			var orderAmount = 0m;
			if(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
			{
				orderAmount = Helpers.Processings.Budget.Order.getItemsAmount(orderArticleEntities, false, true, orderEntity.Discount ?? 0);
			}
			else
			{
				// - Will be * by Nb Months of current Year
				orderAmount = Helpers.Processings.Budget.Order.getOrderLeasingMonthAmount(orderEntity, false, true);
			}
			#endregion Order Amount

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);
			// - project
				if(projectEntity != null)
				{
					if(projectEntity.Closed.HasValue && projectEntity.Closed.Value == true)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been closed");

					if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Project is archived");

					if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been deactivated");
				}

				// User's budget
				if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.Draft)
				{
					var budgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, DateTime.Today.Year); // - work with validation day's Budget
					if(budgetEntity == null)
						return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to user");

					if((budgetEntity.AmountOrder /*- budgetEntity.AmountSpent*/) < orderAmount)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient user Order Budget");

					// - 2024-03-22 use Discount
					var articleEntities = Helpers.Processings.Budget.Order.getPurchaseItems(this._user.Id, DateTime.Today.Year, DateTime.Today.Month);
					var orders = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderIds(articleEntities?.Select(x => x.OrderId)?.ToList());
					foreach(var item in articleEntities)
					{
						var order = orders?.FirstOrDefault(x => x.OrderId == item.OrderId);
						if(order != null && order.Discount.HasValue && order.Discount.Value > 0)
						{
							item.TotalCost = (1m - order.Discount / 100) * item.TotalCost;
							item.TotalCostDefaultCurrency = (1m - order.Discount / 100) * item.TotalCostDefaultCurrency;
							item.UnitPrice = (1m - order.Discount / 100) * item.UnitPrice;
							item.UnitPriceDefaultCurrency = (1m - order.Discount / 100) * item.UnitPriceDefaultCurrency;
						}
					}
					var monthExpenses =
						Helpers.Processings.Budget.Order.getItemsAmount(articleEntities, false, true, 0) +
						Helpers.Processings.Budget.Order.GetLeasingAmount(this._user.Id, DateTime.Today.Year, DateTime.Today.Month);
					if(budgetEntity.AmountMonth - monthExpenses < orderAmount)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient user Month Budget");

					// - 2024-03-22 use Discount
					articleEntities = Helpers.Processings.Budget.Order.getPurchaseItems(this._user.Id, DateTime.Today.Year, null);
					orders = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderIds(articleEntities.Select(x => x.OrderId)?.ToList());
					foreach(var item in articleEntities)
					{
						var order = orders?.FirstOrDefault(x => x.OrderId == item.OrderId);
						if(order != null && order.Discount.HasValue && order.Discount.Value > 0)
						{
							item.TotalCost = (1m - order.Discount / 100) * item.TotalCost;
							item.TotalCostDefaultCurrency = (1m - order.Discount / 100) * item.TotalCostDefaultCurrency;
							item.UnitPrice = (1m - order.Discount / 100) * item.UnitPrice;
							item.UnitPriceDefaultCurrency = (1m - order.Discount / 100) * item.UnitPriceDefaultCurrency;
						}
					}
					var yearExpenses =
						Helpers.Processings.Budget.Order.getItemsAmount(articleEntities, false, true, 0) +
						Helpers.Processings.Budget.Order.GetLeasingAmount(this._user.Id, DateTime.Today.Year, null);
					// - Take full Year Amount
					if(budgetEntity.AmountYear - yearExpenses < orderAmount *
						(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing
						? Helpers.Processings.Budget.Order.getOrderLeasingFirstYearNbMonths(orderEntity)
						: 1))
						return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient user Year Budget");
				}
				else if(this._data.CurrentStep >= (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector) // department' s budget
				{
					if(!Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.IsUserFinanceValidator(orderEntity.CompanyId ?? -1,this._user.Id))
						return ResponseModel<int>.FailureResponse(key: "1", value: "User not a validator");

					var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderEntity.DepartmentId ?? -1, orderEntity.ValidationRequestTime?.Year ?? -1);
					if(deptBudgetEntity == null)
						return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to department");

					// check dept's budget
					if(deptBudgetEntity.AmountInitial - Helpers.Processings.Budget.Order.GetAmountSpent_Department(orderEntity.DepartmentId ?? -1, true) < orderAmount)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient department Budget");
				
					// - site' s budget
					var siteBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(orderEntity.CompanyId ?? -1, orderEntity.ValidationRequestTime?.Year ?? -1);
					if(siteBudgetEntity == null)
						return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to site");

					if(siteBudgetEntity.AmountInitial - Helpers.Processings.Budget.Order.GetAmountSpent_Company(orderEntity.DepartmentId ?? -1, true) < orderAmount)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient site Budget");

					// - 2022-11-18 - validate only Orders of current year
					if(orderEntity.ValidationRequestTime == null || orderEntity.ValidationRequestTime.Value.Year != DateTime.Today.Year)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Validation for orders from year {(orderEntity.ValidationRequestTime.HasValue ? orderEntity.ValidationRequestTime.Value.Year : 0000)} is not allowed");
			}
			else
			{
				return ResponseModel<int>.FailureResponse( $"Invalid validation step: [{this._data.CurrentStep}]");
			}

			if(this._data.CurrentStep > 0 && orderEntity.MandantId != this._data.MandantId)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot change Mandant at this step");

			//var totalOrders = Infrastructure.Data.Access.Tables.FNC.
			//if(projectEntity.ProjectBudget <)
			//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been closed");

			return ResponseModel<int>.SuccessResponse();
		}
		internal bool saveVersionHistory()
		{
			try
			{
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
				var orderArticleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);
				var isLastValidation = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetLastByProjectId(orderEntity.ProjectId ?? -1)?.Level == this._data.CurrentStep;

				// Save Order
				Infrastructure.Data.Access.Tables.FNC.Budget_Order_VersionAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity
				{
					Id_Order = orderEntity.OrderId,
					Dept_name = orderEntity.DepartmentName,
					Id_Currency_Order = orderEntity.CurrencyId,
					Id_Dept = orderEntity.DepartmentId,
					Id_Land = orderEntity.CompanyId,
					Land_name = orderEntity.CompanyName,
					Id_Level = this._data.CurrentStep,
					Id_Project = orderEntity.ProjectId,
					Id_Status = isLastValidation ? (int)Enums.BudgetEnums.ValidationStatuses.Completion : (int)Enums.BudgetEnums.ValidationStatuses.Validation,
					Id_Supplier_VersionOrder = orderEntity.SupplierId,
					Id_User = this._user.Id,
					Id_VO = -1,
					Nr_version_Order = -1, // >>>>>>>>>>>>>>>
					Step_Order = Enums.BudgetEnums.GetOrderValidationStatus(this._data.CurrentStep), // >>>>>>>>>>
					TotalCost_Order = (double?)orderArticleEntites.Select(x => x.TotalCost.GetValueOrDefault())?.Sum(),
					Version_Order_date = DateTime.Now
				});

				// Save articles
				Infrastructure.Data.Access.Tables.FNC.Budget_Article_VersionAccess.Insert(
					orderArticleEntites?.Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity
					{
						Action_Version_Article = Enums.BudgetEnums.ValidationStatuses.Validation.GetDescription(),
						Currency_Version_Article = x.CurrencyName,
						Dept_name_VersionArticle = orderEntity.DepartmentName,
						Id_AOV = -1,
						Id_Article = x.ArticleId,
						Id_Currency_Version_Article = x.CurrencyId,
						Id_Dept_VersionArticle = orderEntity.DepartmentId,
						Id_Land_VersionArticle = orderEntity.CompanyId,
						Id_Level_VersionArticle = this._data.CurrentStep,
						Id_Order_Version = orderEntity.OrderId,
						Id_Project_VersionArticle = orderEntity.ProjectId,
						Id_Status_VersionArticle = isLastValidation ? (int)Enums.BudgetEnums.ValidationStatuses.Completion : (int)Enums.BudgetEnums.ValidationStatuses.Validation,
						Id_Supplier_VersionArticle = orderEntity.SupplierId,
						Id_User_VersionArticle = this._user.Id,
						Land_name_VersionArticle = orderEntity.CompanyName,
						Quantity_VersionArticle = x.Quantity,
						TotalCost__VersionArticle = (double?)x.TotalCost,
						Unit_Price_VersionArticle = (double?)x.UnitPrice,
						Version_Article_date = DateTime.Now
					})?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}

			return true;
		}
		internal void validateOrderFinance()
		{
			int FINCANCE_LEVEL = 99999999;
			var sendOutEmail = true;
			int orderLevel = this._data.CurrentStep + 1;

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data.OrderId);
			var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			var orderArticleExtensionEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);
			var orderArticleEntites = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(this._data.OrderId);

			var supplierEntity = orderExtensionEntity.SupplierId.HasValue ?
				Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get(orderExtensionEntity.SupplierId.Value) : null;


			#region >>>> Budget DB <<<<

			// -
			if(orderArticleEntites != null && orderArticleEntites.Count > 0)
			{
				for(int i = 0; i < orderArticleEntites.Count; i++)
				{
					if(orderArticleEntites[i].Start_Anzahl == 0)
					{
						orderArticleEntites[i].Start_Anzahl = orderArticleExtensionEntites[i].Quantity;
					}
				}
			}

			if(supplierEntity != null)
			{
				switch(orderExtensionEntity.CompanyName?.ToLower())
				{
					case "psz albania gmbh":
						orderEntity.Ihr_Zeichen = supplierEntity.Kundennummer_PSZ_AL_Lieferanten;
						break;
					case "psz czech republic":
						orderEntity.Ihr_Zeichen = supplierEntity.Kundennummer_PSZ_CZ_Lieferanten;
						break;
					case "psz tunisie":
					case "ws tunisie":
						orderEntity.Ihr_Zeichen = supplierEntity.Kundennummer_PSZ_AL_Lieferanten;
						break;
					case "psz electronic gmbh":
						orderEntity.Ihr_Zeichen = supplierEntity.Kundennummer_Lieferanten;
						break;
					default:
						orderEntity.Ihr_Zeichen = supplierEntity.Kundennummer_Lieferanten;
						break;
				}
			}
			orderEntity.gebucht = true;
			orderEntity.erledigt = false;

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderExtensionEntity.CompanyId ?? -1);
			var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderExtensionEntity.CompanyId ?? -1);

			List<string> nextValidatorEmail = null;
			string emailTitle, emailContent;
			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderExtensionEntity.ProjectId ?? -1);
			getEmailBody(orderExtensionEntity, projectEntity, out emailTitle, out emailContent);


			#region >>>> Order Amount <<<<
			var orderAmount = 0m;
			if(orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
			{
				orderAmount = Helpers.Processings.Budget.Order.getItemsAmount(orderArticleExtensionEntites, false, true, orderExtensionEntity.Discount ?? 0);
			}
			else
			{
				// - Force same amount over the Validation Levels for same month
				if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.Draft)
				{
					orderAmount = orderExtensionEntity.LeasingStartYear == DateTime.Today.Year
						? Helpers.Processings.Budget.Order.getOrderLeasingMonthAmount(orderExtensionEntity, false, true)
						: 0; // - leasing will start in a futur year
				}
				else
				{
					// - Will get the total First Year Amount
					orderAmount = Helpers.Processings.Budget.Order.getLeasingUserAmount(
						this._data.OrderId,
						orderExtensionEntity.ValidationRequestTime.Value.Year,
						orderExtensionEntity.ValidationRequestTime.Value.Month)
						* Helpers.Processings.Budget.Order.getOrderLeasingFirstYearNbMonths(orderExtensionEntity);
				}
			}
			#endregion Order Amount


			// -- Decrease Budget
			if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.Draft)
			{
				// - workflow history
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.ValidationRequest, this._user, Enums.BudgetEnums.ValidationLevels.Draft.GetDescription());

				if(orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
				{
					// - For first validation take whole year Amount. This will also be the same validated at the other levels, at first validation and Rejection.
					// - For the other years, Amount will computed by Cron Job Agent
					orderAmount = orderAmount * Helpers.Processings.Budget.Order.getOrderLeasingFirstYearNbMonths(orderExtensionEntity);
				}

				if(projectEntity != null)
				{
					projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) + (decimal?)orderAmount ?? 0;
					projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) + 1;
					Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);
				}

				orderExtensionEntity.ValidationRequestTime = DateTime.Now;
				orderExtensionEntity.BudgetYear = orderExtensionEntity.ValidationRequestTime?.Year ?? -1;

				// - Always remove from User 
				var userBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, DateTime.Today.Year); // - work with validation day's Budget
				userBudgetEntity.AmountSpent = userBudgetEntity.AmountSpent + ((decimal?)orderAmount ?? 0);
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userBudgetEntity);

				// - 
				orderExtensionEntity.Level = FINCANCE_LEVEL;
				orderExtensionEntity.Status = 1;

				// - Send mail to validators User
				nextValidatorEmail = new List<string> { companyExtension?.FinanceValidatorOneEmail, companyExtension?.FinanceValidatorTowEmail };
				getEmailBody(orderExtensionEntity, projectEntity, out emailTitle, out emailContent, false);
			}
			else
			{
				// - Workflow history
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Validate, this._user, Enums.BudgetEnums.ValidationLevels.Purchase.GetDescription());

				// Set Approval flags
				orderExtensionEntity.ApprovalTime = DateTime.Now;
				orderExtensionEntity.ApprovalUserId = this._user.Id;

				// - validate from HeadOf
				var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderExtensionEntity.DepartmentId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
				deptBudgetEntity.AmountSpent = deptBudgetEntity.AmountSpent + orderAmount;
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptBudgetEntity);

				// - Validation by Site director before HeadOf
				var siteBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(orderExtensionEntity.CompanyId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
				siteBudgetEntity.AmountSpent = siteBudgetEntity.AmountSpent + orderAmount;
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(siteBudgetEntity);

				// - 
				orderExtensionEntity.Level = FINCANCE_LEVEL;
				orderExtensionEntity.Status = 2;

				// - Send mail to Original User
				var originalUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity.IssuerId);
				nextValidatorEmail = new List<string> { originalUser?.Email };
				getEmailBody(orderExtensionEntity, projectEntity, out emailTitle, out emailContent, true);
			}

			#region /// - Validation History - ///
			Infrastructure.Data.Access.Tables.FNC.OrderValidationAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.OrderValidationEntity
			{
				Id = -1,
				OrderId = orderExtensionEntity?.OrderId ?? -1,
				OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
				OrderProjectId = orderExtensionEntity?.ProjectId ?? -1,
				OrderTotalAmount = (decimal)orderAmount,
				OrderType = orderExtensionEntity.OrderType,
				OrderUserId = orderExtensionEntity.IssuerId,
				UserId = this._user.Id,
				ValidationLevel = this._data.CurrentStep,
				ValidationNotes = this._data.Notes,
				ValidationTime = DateTime.Now,
				Username = this._user.Username,
				UserEmail = this._user.Email
			});

			// - Leasing history
			if(orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
			{
				var orderAmountMonth = orderAmount / Helpers.Processings.Budget.Order.getOrderLeasingFirstYearNbMonths(orderExtensionEntity);
				var defaultCurrency = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(orderExtensionEntity.CurrencyId ?? -1)
					?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
				Infrastructure.Data.Access.Tables.FNC.OrderValidationLeasingAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.OrderValidationLeasingEntity
				{
					Id = -1,
					OrderId = orderExtensionEntity?.OrderId ?? -1,
					OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
					OrderProjectId = orderExtensionEntity?.ProjectId ?? -1,
					OrderType = orderExtensionEntity.OrderType,
					OrderIssuerId = orderExtensionEntity.IssuerId,
					UserId = this._user.Id,
					ValidationLevel = this._data.CurrentStep,
					ValidationNotes = this._data.Notes,
					ValidationTime = DateTime.Now,
					DefaultCurrencyId = defaultCurrency.Nr,
					DefaultCurrencyDecimals = defaultCurrency.Dezimalstellen,
					DefaultCurrencyName = defaultCurrency.Symbol,
					DefaultCurrencyRate = (decimal?)defaultCurrency.entspricht_DM,
					OrderTotalAmount = orderExtensionEntity.LeasingTotalAmount ?? 0,
					OrderTotalAmountDefaultCurrency = (orderExtensionEntity.LeasingTotalAmount ?? 0) * (decimal)(defaultCurrency.entspricht_DM ?? 0),
					OrderLeasingMonthAmount = orderAmountMonth,
					OrderLeasingMonthAmountDefaultCurrency = orderAmountMonth * (decimal)(defaultCurrency.entspricht_DM ?? 0),
					OrderLeasingYear = DateTime.Today.Year,
					OrderLeasingYearTotalAmount = orderAmount,
					OrderLeasingYearTotalAmountDefaultCurrency = orderAmount * (decimal)(defaultCurrency.entspricht_DM ?? 0),
					OrderLeasingYearTotalMonths = Helpers.Processings.Budget.Order.getOrderLeasingFirstYearNbMonths(orderExtensionEntity)
				});
			}
			#endregion  Validation History 

			// check for finance orders
			Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Update(orderEntity);
			Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(orderExtensionEntity);
			#endregion // >>>>>> Budget DB

			#region >>>> Email Notification <<<<
			if(sendOutEmail)
			{
				var attachments = new List<KeyValuePair<string, System.IO.Stream>>();
				var attachmentIds = new List<int>();
				try
				{
					var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdOrder(orderExtensionEntity.OrderId);
					if(fileEntities != null && fileEntities.Count > 0)
					{
						var n = 0;
						foreach(var fileItem in fileEntities)
						{
							if(fileItem.Id_File > 0)
							{
								var data = Psz.Core.Common.Program.FilesManager.GetFile(fileItem.FileId);
								if(data != null)
								{
									attachments.Add(new KeyValuePair<string, System.IO.Stream>($"AttachedFile{n++}{data.FileExtension}", new System.IO.MemoryStream(data.FileBytes)));
									attachmentIds.Add(fileItem.FileId);
								}
							}
						}
					}
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
				}

				try
				{
					var reportData = ReportHandler.generateReportData(orderExtensionEntity.OrderId, companyExtension.ReportDefaultLanguageId);
					if(reportData != null)
					{
						attachments.Add(new KeyValuePair<string, System.IO.Stream>($"{orderExtensionEntity.OrderNumber}.pdf", new System.IO.MemoryStream(reportData)));
					}
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
				}

				try
				{
					// Send email notification
					Module.EmailingService.SendEmailSendGridWithStaticTemplate(
						emailContent,
						emailTitle,
						  nextValidatorEmail, null, null,
					   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}

			}
			#endregion Email Notification
		}
		private void getEmailBody(Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity, Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity, out string emailTitle, out string emailContent, bool isLastValidation = false)
		{
			emailTitle = $"[Budget] {(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing ? "Leasing " : "")}Order Validation";
			emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

			if(isLastValidation)
			{
				emailContent += $"<br/><span style='font-size:1.15em;'>Your budget validation request for order <strong>{orderEntity.OrderNumber?.ToUpper()}</strong>";
				if(projectEntity != null)
					emailContent += $" from project <strong>{projectEntity?.ProjectName?.ToUpper()}</strong>";
				emailContent += $" has been <strong>Approved</strong> by <strong>{this._user.Name?.ToUpper()}</strong> on {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}.";
			}
			else
			{
				emailContent += $"<br/><span style='font-size:1.15em;'><strong>{this._user.Name?.ToUpper()}</strong> has send a budget validation request for {(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing ? "Leasing " : "")}order <strong>{orderEntity.OrderNumber?.ToUpper()}</strong>";
				if(projectEntity != null)
					emailContent += $" from project <strong>{projectEntity?.ProjectName?.ToUpper()}</strong>";
			}

			if(!string.IsNullOrWhiteSpace(this._data.Notes))
				emailContent += $"<br/><br/><strong>Notes from {this._user.Name?.ToUpper()}:</strong> {this._data.Notes}";

			emailContent += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/budget-rebuild/orders/edit/{orderEntity.OrderId}'>here</a>";
			emailContent += "<br/><br/>Regards, <br/>IT Department </div>";
		}
	}
}