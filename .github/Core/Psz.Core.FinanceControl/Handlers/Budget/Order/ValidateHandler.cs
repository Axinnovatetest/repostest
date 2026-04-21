using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.FinanceControl.Helpers;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class ValidateHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Budget.Order.ValidateModel _data { get; set; }
		public ValidateHandler(Models.Budget.Order.ValidateModel validateData, UserModel user)
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
					validateOrder();
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
			if(orderEntity.Level.HasValue && orderEntity.Level.Value > this._data.CurrentStep)
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

			// - External order
			if(orderEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower())
			{
				if(orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing)
					return ResponseModel<int>.FailureResponse(key: "1", value: "External order cannot be leased");

				if(projectEntity == null)
					return ResponseModel<int>.FailureResponse(key: "1", value: "External order should have a project");

				if(projectEntity.Closed.HasValue && projectEntity.Closed.Value == true)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been closed");

				if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is archived");

				// - Global Director Approval
				if(projectEntity.Id_State != (int)Enums.BudgetEnums.ProjectApprovalStatuses.Active)
				{
					if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Reject)
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] has been rejected");
					}

					if(projectEntity.ApprovalUserId > 0)
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] approval has been withdrawn");
					}
					else
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is pending approval");
					}
				}

				// - PM Approval
				if(projectEntity.ProjectStatus != (int)Enums.BudgetEnums.ProjectStatuses.Active)
				{
					if(projectEntity.ApprovalUserId > 0)
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is not active");
					}
					else
					{
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity?.ProjectName}] is on-hold");
					}
				}

				// - update external project
				if(this._data.CurrentStep == 0 && projectEntity != null && projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External)
				{
					var orderEntites = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(projectEntity.Id)
						?.Where(x => x.Level > 0)?.ToList();

					if(orderEntites == null)
						orderEntites = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> { orderEntity };
					else
						orderEntites.Add(orderEntity);

					var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntites?.Select(x => x.OrderId)?.ToList());
					var prjectOrderAmount = articleEntites?.Select(x => x.TotalCostDefaultCurrency ?? 0).Sum()
						- orderEntites?.Sum(x => (1m - (x.Discount ?? 0) / 100) * articleEntites?.Where(y => y.OrderId == x.OrderId)?.Select(x => x.TotalCostDefaultCurrency ?? 0).Sum());
					if(prjectOrderAmount > projectEntity.ProjectBudget)
					{
						var globalDirectors = Infrastructure.Data.Access.Tables.COR.UserAccess.GetGlobalDirectors();
						var emailAddresses = new List<string> { projectEntity.ResponsableEmail };
						if(globalDirectors != null && globalDirectors.Count > 0)
							emailAddresses.AddRange(globalDirectors?.Where(x => x.Id > 0)?.Select(x => x.Email));

						var emailBody = Helpers.Notifications.Email.ExternalProjectTemplate_Profit(this._user, projectEntity, orderEntites, articleEntites, orderAmount);

						//Helpers.Notifications.Email.SendEmail("[Budget] External project exceeding budget", emailBody, emailAddresses, null, this._user, null);

						//Module.EmailingService.SendEmailSendGridWithStaticTemplate(
						//	emailBody,
						//	"[Budget] External project exceeding budget",
						//	emailAddresses, null, null,
						//   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);


						return ResponseModel<int>.FailureResponse(key: "1", value: $"External project [{projectEntity?.ProjectName}] exceeding budget");
					}
				}


				// search creator user - or validators
				if(orderEntity.IssuerId != this._user.Id || (orderEntity.ProjectId == null || orderEntity.ProjectId < 1))
				{
					var validatorEntities = Validators.getByOrderId(orderEntity.OrderId, out var errs);
					var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);
					if(validatorEntities == null || validatorEntities.Count <= 0)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Empty validator list");
					if(orderEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.SuperValidator)
						validatorEntities.Add(new Infrastructure.Data.Entities.Tables.FNC.ProjectValidatorsEntity
						{
							Id_Validator = _user.Id,
							Level = (int)Enums.BudgetEnums.ValidationLevels.SuperValidator
						});

					if(this._data.CurrentStep == validatorEntities.Count - 1) // Purchase
					{
						if(Helpers.Processings.Budget.HasPurchaseProfile(this._user.Id, companyExtensionEntity) != true) // profileId
						{
							return ResponseModel<int>.FailureResponse(key: "1", value: "User not found as validator");
						}
					}
					else
					{
						var validatorEntity = validatorEntities.Find(x => x.Id_Validator == this._user.Id);
						if(validatorEntity == null)
							return ResponseModel<int>.FailureResponse(key: "1", value: "User not found as validator");

						if(!validatorEntity.Level.HasValue || validatorEntity.Level.Value != this._data.CurrentStep)
							return ResponseModel<int>.FailureResponse(key: "1", value: "User not found as validator in current step");
					}

				}
			}
			else // internal project
			{
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
				else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector) // department' s budget
				{
					var siteDirector = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
					var deptDirectors = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderEntity.DepartmentId ?? -1);
					if((deptDirectors == null || deptDirectors.HeadUserId != this._user.Id)
						&& (siteDirector == null || siteDirector.DirectorId != this._user.Id))
						return ResponseModel<int>.FailureResponse(key: "1", value: "User not a director of department");

					var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderEntity.DepartmentId ?? -1, orderEntity.ValidationRequestTime?.Year ?? -1);
					if(deptBudgetEntity == null)
						return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to department");

					// check dept's budget
					if(deptBudgetEntity.AmountInitial - Helpers.Processings.Budget.Order.GetAmountSpent_Department(orderEntity.DepartmentId ?? -1, true) < orderAmount)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient department Budget");

					// - 2022-11-18 - validate only Orders of current year
					if(orderEntity.ValidationRequestTime == null || orderEntity.ValidationRequestTime.Value.Year != DateTime.Today.Year)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Validation for orders from year {(orderEntity.ValidationRequestTime.HasValue ? orderEntity.ValidationRequestTime.Value.Year : 0000)} is not allowed");
				}
				else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.SiteDirector) // site' s budget
				{
					var siteDirector = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
					if(siteDirector == null || siteDirector.DirectorId != this._user.Id)
						return ResponseModel<int>.FailureResponse(key: "1", value: "User not a director of site");

					var siteBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(orderEntity.CompanyId ?? -1, orderEntity.ValidationRequestTime?.Year ?? -1);
					if(siteBudgetEntity == null)
						return ResponseModel<int>.FailureResponse(key: "1", value: "No budget allocated to site");

					if(siteBudgetEntity.AmountInitial - Helpers.Processings.Budget.Order.GetAmountSpent_Company(orderEntity.DepartmentId ?? -1, true) < orderAmount)
						return ResponseModel<int>.FailureResponse(key: "1", value: "Insufficient site Budget");

					// - 2022-11-18 - validate only Orders of current year
					if(orderEntity.ValidationRequestTime == null || orderEntity.ValidationRequestTime.Value.Year != DateTime.Today.Year)
						return ResponseModel<int>.FailureResponse(key: "1", value: $"Validation for orders from year {(orderEntity.ValidationRequestTime.HasValue ? orderEntity.ValidationRequestTime.Value.Year : 0000)} is not allowed");
				}
				else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.Purchase) // purchase dept
				{

				}
			}

			if(this._data.CurrentStep > 0 && orderEntity.MandantId != this._data.MandantId)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot change Mandant at this step");

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
		internal void validateOrder()
		{
			var sendOutEmail = true;
			var isInterimValidation = false;
			int orderLevel = this._data.CurrentStep + 1;
			var headOfEmail = "";

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data.OrderId);
			var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			var orderArticleExtensionEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);
			var orderArticleEntites = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(this._data.OrderId);
			var siteBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(orderExtensionEntity.CompanyId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? DateTime.Now.Year);

			var supplierEntity = orderExtensionEntity.SupplierId.HasValue ?
				Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get(orderExtensionEntity.SupplierId.Value) : null;


			#region >>>> Budget DB <<<<

			//
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

			// - Internal Project
			if(orderExtensionEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower())
			{
				var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderExtensionEntity.DepartmentId ?? -1);

				// -- Decrease Budget
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
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

					sendOutEmail = orderAmount > userBudgetEntity.AmountNotificationThreshold || orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing;
					if(sendOutEmail == true)
					{
						nextValidatorEmail = new List<string> { department?.HeadUserEmail };
					}
					else
					{
						this._data.CurrentStep += 1;

						// - validate from HeadOf
						// FIXME: where ProjectExists
						var deptBudgetEntity = projectEntity == null
							? Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderExtensionEntity.DepartmentId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1) // - should exist, as per validate() condition
							: Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderExtensionEntity.DepartmentId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1 /*projectEntity.BudgetYear*/);

						deptBudgetEntity.AmountSpent = deptBudgetEntity.AmountSpent + orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptBudgetEntity);

						sendOutEmail = orderAmount > deptBudgetEntity.AmountNotificationThreshold || orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing;
						if(sendOutEmail)
						{
							nextValidatorEmail = new List<string> { company?.DirectorEmail };
						}
						else
						{
							// - 
							this._data.CurrentStep += 1;
						}

						// - Validation by Site director before HeadOf
						if(department.HeadUserId != this._user.Id && company.DirectorId == this._user.Id)
						{
							siteBudgetEntity.AmountSpent = siteBudgetEntity.AmountSpent + orderAmount;
							Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(siteBudgetEntity);

							// Send mail to Purchase
							nextValidatorEmail = new List<string> { companyExtension?.PurchaseGroupEmail };
							nextValidatorEmail.AddRange(getPurchaseUserEmails(companyExtension?.PurchaseProfileId ?? -1, companyExtension.CompanyId));

							isInterimValidation = true;
							headOfEmail = department?.HeadUserEmail;
						}
					}
				}
				else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)
				{
					// - Workflow history
					Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Validate, this._user, Enums.BudgetEnums.ValidationLevels.DepartmentDirector.GetDescription());

					//FIXME: where project exists
					var deptBudgetEntity = projectEntity == null
						? Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderExtensionEntity.DepartmentId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1)
						: Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderExtensionEntity.DepartmentId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1/*projectEntity.BudgetYear*/);

					deptBudgetEntity.AmountSpent = deptBudgetEntity.AmountSpent + orderAmount;
					Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptBudgetEntity);

					sendOutEmail = orderAmount > deptBudgetEntity.AmountNotificationThreshold || orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing;
					if(sendOutEmail == true)
					{
						nextValidatorEmail = new List<string> { company?.DirectorEmail };
					}
					else
					{
						// - set as validate from Site dir
						siteBudgetEntity.AmountSpent = siteBudgetEntity.AmountSpent + orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(siteBudgetEntity);
						// Send mail to Purchase
						nextValidatorEmail = new List<string> { companyExtension?.PurchaseGroupEmail };
						nextValidatorEmail.AddRange(getPurchaseUserEmails(companyExtension?.PurchaseProfileId ?? -1, companyExtension.CompanyId));
						// - 
						this._data.CurrentStep += 1;
					}

					// - Validation by Site director before HeadOf
					if(department.HeadUserId != this._user.Id && company.DirectorId == this._user.Id)
					{
						siteBudgetEntity.AmountSpent = siteBudgetEntity.AmountSpent + orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(siteBudgetEntity);

						// Send mail to Purchase
						nextValidatorEmail = new List<string> { companyExtension?.PurchaseGroupEmail };
						nextValidatorEmail.AddRange(getPurchaseUserEmails(companyExtension?.PurchaseProfileId ?? -1, companyExtension.CompanyId));

						isInterimValidation = true;
						headOfEmail = department?.HeadUserEmail;
					}
				}
				else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)
				{

					// - Workflow history
					Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Validate, this._user, Enums.BudgetEnums.ValidationLevels.SiteDirector.GetDescription());

					//
					if(orderAmount > siteBudgetEntity.AmountNotificationSuperValidationThreshold)
					{
						nextValidatorEmail = new List<string> { };
						if(!companyExtension.SuperValidatorOneEmail.IsnullOrEmptyOrWhiteSpaces())
							nextValidatorEmail.Add(companyExtension.SuperValidatorOneEmail);

						if(!companyExtension.SuperValidatorTowEmail.IsnullOrEmptyOrWhiteSpaces())
							nextValidatorEmail.Add(companyExtension.SuperValidatorTowEmail);
					}
					else
					{
						siteBudgetEntity.AmountSpent = siteBudgetEntity.AmountSpent + orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(siteBudgetEntity);
						// Send mail to Purchase
						nextValidatorEmail = new List<string> { companyExtension?.PurchaseGroupEmail };
						nextValidatorEmail.AddRange(getPurchaseUserEmails(companyExtension?.PurchaseProfileId ?? -1, companyExtension.CompanyId));
					}
				}
				else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.SuperValidator)
				{
					// - Workflow history
					Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Validate, this._user, Enums.BudgetEnums.ValidationLevels.SuperValidator.GetDescription());
					siteBudgetEntity.AmountSpent = siteBudgetEntity.AmountSpent + orderAmount;
					Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(siteBudgetEntity);
					// Send mail to Purchase
					nextValidatorEmail = new List<string> { companyExtension?.PurchaseGroupEmail };
					nextValidatorEmail.AddRange(getPurchaseUserEmails(companyExtension?.PurchaseProfileId ?? -1, companyExtension.CompanyId));
				}
				else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.Purchase)
				{
					// - Workflow history
					Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Validate, this._user, Enums.BudgetEnums.ValidationLevels.Purchase.GetDescription());

					// Set Approval flags
					orderExtensionEntity.ApprovalTime = DateTime.Now;
					orderExtensionEntity.ApprovalUserId = this._user.Id;

					// Send mail to Original User
					var originalUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity.IssuerId);
					nextValidatorEmail = new List<string> { originalUser?.Email };
					getEmailBody(orderExtensionEntity, projectEntity, out emailTitle, out emailContent, true);
				}
			}
			else // External Project
			{

				// - workflow history
				Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Validate, this._user, ((Enums.BudgetEnums.ValidationLevels)this._data.CurrentStep).GetDescription());

				if(this._data.CurrentStep == 0)
				{
					projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) + orderAmount;
					projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) + 1;
				}
				Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);

				var validators = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectId(orderExtensionEntity.ProjectId ?? -1);

				company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(projectEntity.CompanyId ?? -1);
				companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(projectEntity.CompanyId ?? -1);
				if(validators != null)
				{
					if(this._data.CurrentStep < validators.Count)
					{
						nextValidatorEmail = new List<string> { Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetNextByProjectId(orderExtensionEntity.ProjectId ?? -1, this._data.CurrentStep + 1)?.email };
					}
					else if(this._data.CurrentStep == validators.Count) // Purchase 
					{
						// Send mail to Purchase
						nextValidatorEmail = new List<string> { companyExtension.PurchaseGroupEmail };
						nextValidatorEmail.AddRange(getPurchaseUserEmails(companyExtension.PurchaseProfileId ?? -1, companyExtension.CompanyId));
					}
					else // Email from Purchase - Last step
					{
						// Set Approval flags
						orderExtensionEntity.ApprovalTime = DateTime.Now;
						orderExtensionEntity.ApprovalUserId = this._user.Id;

						var originalUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity.IssuerId);
						nextValidatorEmail = new List<string> { originalUser?.Email };
						getEmailBody(orderExtensionEntity, projectEntity, out emailTitle, out emailContent, true);
					}
				}
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


			if(isInterimValidation)
			{
				var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderExtensionEntity.DepartmentId ?? -1);
				// - Validation by Site director before HeadOf
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
					ValidationLevel = this._data.CurrentStep + 1,
					ValidationNotes = this._data.Notes,
					ValidationTime = DateTime.Now,
					Username = this._user.Username,
					UserEmail = this._user.Email
				});
			}

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

			orderExtensionEntity.Level = isInterimValidation ? this._data.CurrentStep + 2 : this._data.CurrentStep + 1;
			orderExtensionEntity.Status = isInterimValidation ? this._data.CurrentStep + 2 : this._data.CurrentStep + 1;
			if(this._data.CurrentStep == 0)
			{
				orderExtensionEntity.ValidationRequestTime = DateTime.Now;
				orderExtensionEntity.BudgetYear = orderExtensionEntity.ValidationRequestTime?.Year ?? -1;
			}
			//check for super validation
			var oldOrderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			if(oldOrderExtensionEntity.Level != (int)Enums.BudgetEnums.ValidationLevels.SuperValidator
			&& orderExtensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.Purchase)
			{
				if(orderAmount > siteBudgetEntity.AmountNotificationSuperValidationThreshold)
				{
					orderExtensionEntity.Level = (int)Enums.BudgetEnums.ValidationLevels.SuperValidator;
					orderExtensionEntity.Status = (int)Enums.BudgetEnums.ValidationLevels.SuperValidator;
				}
			}

			if(oldOrderExtensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.SuperValidator && orderExtensionEntity.Level > 6)
			{
				orderExtensionEntity.Level = (int)Enums.BudgetEnums.ValidationLevels.Purchase;
				orderExtensionEntity.Status = (int)Enums.BudgetEnums.ValidationLevels.Purchase;
			}
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

					/*Module.EmailingService.SendEmailAsync(emailTitle, emailContent, nextValidatorEmail, attachments,
						saveHistory: true, senderEmail: this._user.Email, senderName: this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: attachmentIds);*/
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]"));
					Infrastructure.Services.Logging.Logger.Log(ex);
					//throw new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]");
				}

				if(isInterimValidation && !string.IsNullOrWhiteSpace(headOfEmail))
				{
					try
					{
						var directorUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(company?.DirectorId ?? -1);
						// Send email notification
						var content = "";
						getHeadOfEmailBody(directorUser?.Name, orderExtensionEntity, projectEntity, out emailTitle, out content);

						Module.EmailingService.SendEmailSendGridWithStaticTemplate(
							emailContent,
							emailTitle,
							new List<string> { headOfEmail }, null, null,
						   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);
						/*Module.EmailingService.SendEmailAsync(emailTitle, content, new List<string> { headOfEmail }, null,
						saveHistory: true, senderEmail: this._user.Email, senderName: this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: attachmentIds);*/
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]"));
						Infrastructure.Services.Logging.Logger.Log(ex);
						//throw new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]");
					}
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

		private void getHeadOfEmailBody(string siteDirName, Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity orderEntity, Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity projectEntity, out string emailTitle, out string emailContent, bool isLastValidation = false)
		{
			emailTitle = "[Budget] Order Validation";
			emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";


			emailContent += $"<br/><span style='font-size:1.15em;'><strong>{siteDirName?.ToUpper()}</strong> has validated the budget validation request sent by <strong>{orderEntity.IssuerName?.ToUpper()}</strong> for order <strong>{orderEntity.OrderNumber?.ToUpper()}</strong>";
			if(projectEntity != null)
				emailContent += $" from project <strong>{projectEntity?.ProjectName?.ToUpper()}</strong>";

			if(!string.IsNullOrWhiteSpace(this._data.Notes))
				emailContent += $"<br/><br/><strong>Notes from {this._user.Name?.ToUpper()}:</strong> {this._data.Notes}";

			emailContent += $"</span>";
			emailContent += "<br/><br/>Regards, <br/>IT Department </div>";
		}

		private List<string> getPurchaseUserEmails(int purchaseProfileId, int companyId)
		{
			var fncAccessProfileEntities = Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.GetByMainAccessProfilesIds(new List<int> { purchaseProfileId });
			var userEntities = Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByAccessProfileIds(fncAccessProfileEntities?.Select(x => x.Id).ToList());
			return Infrastructure.Data.Access.Tables.COR.UserAccess.Get(userEntities?.Select(x => x.UserId)?.ToList())?.Where(x => x.CompanyId == companyId)?.Select(x => x.Email)?.ToList();
		}
	}
}


