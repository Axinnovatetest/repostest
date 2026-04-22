using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class UnvalidateHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Budget.Order.UnvalidateModel _data { get; set; }
		public UnvalidateHandler(Models.Budget.Order.UnvalidateModel validateData, UserModel user)
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
				unvalidateOrder();

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

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not found");

			if(orderEntity.Archived.HasValue && orderEntity.Archived.Value)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order is archived");

			if(this._user.Id != orderEntity.IssuerId && !this._user.IsGlobalDirector)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User cannot unvalidate order");

			// --
			if(orderEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower()) // External
			{
				var validatorEntities = Validators.getByOrderId(orderEntity.OrderId, out List<string> errors);
				if(validatorEntities == null || validatorEntities.Count <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Empty validator list");

				if(orderEntity.Level < validatorEntities.Count)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot unvalidate in-progress order");
			}
			else // Internal order
			{
				if(orderEntity.Level < (int)Enums.BudgetEnums.ValidationLevels.Purchase)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot unvalidate in-progress order");
			}

			// --
			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);
			if(projectEntity != null)
			{
				if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Project is archived");
			}

			// - 2024-01-05 - allow unvalidate always
			//if(orderEntity.ValidationRequestTime == null || orderEntity.ValidationRequestTime.Value.Year != DateTime.Today.Year)
			//	return ResponseModel<int>.FailureResponse(key: "1", value: $"Unvalidation for orders from year {(orderEntity.ValidationRequestTime.HasValue ? orderEntity.ValidationRequestTime.Value.Year : 0000)} is not allowed");

			return ResponseModel<int>.SuccessResponse();
		}
		internal void unvalidateOrder()
		{
			var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			var orderArticleExtensionEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);

			#region >>>>>> Budget DB
			var emailAddresses = new List<string> { };

			// -- Increase User Budget
			//var orderAmount = Helpers.Processings.Budget.Order.getItemsAmount(orderArticleExtensionEntites, false, true);
			#region >>>> Order Amount <<<<
			var orderAmount = 0m;
			if(orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
			{
				orderAmount = Helpers.Processings.Budget.Order.getItemsAmount(orderArticleExtensionEntites, false, true, orderExtensionEntity.Discount ?? 0);
			}
			else
			{
				var nbMonths = 0;
				if(orderExtensionEntity.LeasingStartYear == DateTime.Today.Year)
					nbMonths = Helpers.Processings.Budget.Order.getOrderLeasingFirstYearNbMonths(orderExtensionEntity);

				var leasingStartDate = new DateTime((int)orderExtensionEntity.LeasingStartYear, (int)orderExtensionEntity.LeasingStartMonth, 1);
				var leasingEndDate = leasingStartDate.AddMonths((int)orderExtensionEntity.LeasingStartYear);
				if(orderExtensionEntity.LeasingStartYear < DateTime.Today.Year && DateTime.Today.Year < leasingEndDate.Year)
				{
					nbMonths += 12 * (DateTime.Today.Year - (int)orderExtensionEntity.LeasingStartYear);
				}

				if(orderExtensionEntity.LeasingStartYear != DateTime.Today.Year && DateTime.Today.Year == leasingEndDate.Year)
					nbMonths += Helpers.Processings.Budget.Order.getOrderLeasingLastYearNbMonths(orderExtensionEntity);

				// - Will get the total First Year Amount
				orderAmount = Helpers.Processings.Budget.Order.getLeasingUserAmount(
					this._data.OrderId,
					orderExtensionEntity.ValidationRequestTime.Value.Year,
					orderExtensionEntity.ValidationRequestTime.Value.Month)
					* nbMonths;
			}
			#endregion Order Amount

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderExtensionEntity.ProjectId ?? -1);

			if(orderExtensionEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower()) // Internal Project
			{
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity.IssuerId);
				if(projectEntity != null)
				{
					projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) - (decimal?)orderAmount ?? 0;
					projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) - 1;
					Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);
				}

				//user
				var userBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
				userBudgetEntity.AmountSpent = userBudgetEntity.AmountSpent - orderAmount;
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userBudgetEntity);
				//dept
				var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderExtensionEntity.DepartmentId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
				deptBudgetEntity.AmountSpent = deptBudgetEntity.AmountSpent - orderAmount;
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptBudgetEntity);
				//site
				var siteBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(orderExtensionEntity.CompanyId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
				siteBudgetEntity.AmountSpent = siteBudgetEntity.AmountSpent - orderAmount;
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(siteBudgetEntity);

				// - 
				var headOfEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderExtensionEntity.DepartmentId ?? -1);
				emailAddresses.Add(headOfEntity?.HeadUserEmail ?? "");
			}
			else // External Project
			{
				projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) - orderAmount;
				projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) - 1;
				Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);

				// - 
				var validatorEntities = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByProjectId(orderExtensionEntity.ProjectId ?? -1);
				if(validatorEntities != null && validatorEntities.Count > 0)
					emailAddresses.Add(validatorEntities[validatorEntities.Count - 1].email ?? "");
			}

			// - workflow history
			Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Unvalidate, this._user, $"");


			#region // - Unvalidation History 
			Infrastructure.Data.Access.Tables.FNC.OrderUnvalidationAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.OrderUnvalidationEntity
			{
				Id = -1,
				OrderId = orderExtensionEntity?.OrderId ?? -1,
				OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
				OrderProjectId = orderExtensionEntity?.ProjectId ?? -1,
				OrderTotalAmount = (decimal)orderAmount,
				OrderType = orderExtensionEntity.OrderType,
				OrderUserId = orderExtensionEntity.IssuerId,
				UserId = this._user.Id,
				UnvalidationLevel = orderExtensionEntity.Level ?? -1,
				UnvalidationNotes = this._data.Notes,
				UnvalidationTime = DateTime.Now
			});
			#endregion // - Unvalidation History 

			// Unvalidation
			orderExtensionEntity.Level = 0; // reset workflow
			orderExtensionEntity.Status = 0;
			orderExtensionEntity.ApprovalTime = null;
			orderExtensionEntity.ApprovalUserId = null;
			orderExtensionEntity.ValidationRequestTime = null;
			orderExtensionEntity.BudgetYear = -1;
			Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(orderExtensionEntity);
			#endregion // >>>>>> Budget DB

			#region >>> // Send email notification
			var firstValidator = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity.IssuerId);
			if(firstValidator != null && !string.IsNullOrEmpty(firstValidator.Email) && !string.IsNullOrWhiteSpace(firstValidator.Email))
			{
				if(emailAddresses != null)
					emailAddresses.Add(firstValidator.Email);
				else
					emailAddresses = new List<string> { firstValidator.Email };


				var emailTitle = "[Budget] Order unvalidation";
				var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")} {firstValidator.Username},</span><br/>"
				+ $"<br/><span style='font-size:1.15em;'>Your order <strong>{orderExtensionEntity.OrderNumber?.ToUpper()}</strong>";
				if(projectEntity != null)
					emailContent += $" from project <strong>{projectEntity?.ProjectName?.ToUpper()}</strong>";

				emailContent += $" has been unvalidated by <strong>{this._user.Name?.ToUpper()}</strong> on {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}.";
				if(!string.IsNullOrWhiteSpace(this._data.Notes))
					emailContent += $"<br/><br/><strong>Notes from {this._user.Name?.ToUpper()}:</strong> {this._data.Notes}";

				emailContent += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/budget-rebuild/orders/edit/{orderExtensionEntity.OrderId}'>here</a>"
					+ "<br/><br/>Regards, <br/>IT Department</div>";

				try
				{

					Module.EmailingService.SendEmailSendGridWithStaticTemplate(
						emailContent
						, emailTitle,
						   emailAddresses, null, null,
					   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);
					// Send email notification
					/*Module.EmailingService.SendEmailAsync(emailTitle, emailContent, emailAddresses,
						saveHistory: true, senderEmail: this._user.Email, senderName: this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);*/
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{firstValidator.Email}]"));
					Infrastructure.Services.Logging.Logger.Log(e);
				}
			}

			#endregion >>>> Notifications
		}
	}
}
