using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Org.BouncyCastle.Asn1.Crmf;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class RejectHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Budget.Order.RejectModel _data { get; set; }
		public RejectHandler(Models.Budget.Order.RejectModel validateData, UserModel user)
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
					rejectOrder();
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

			// do nothing
			if(this._data.CurrentStep == 0)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Draft Order can not be rejected");

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "User not found");

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order not found");

			if(orderEntity.Archived.HasValue && orderEntity.Archived.Value)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order is archived");

			var orderArticleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderArticleEntities == null || orderArticleEntities.Count <= 0)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Order articles list empty");


			// - Multiple currencies - need to update Prices
			Helpers.Processings.Budget.Order.updateArticlePrices(new List<int> { this._data.OrderId });

			// --
			if(orderEntity.OrderType == Enums.BudgetEnums.ProjectTypes.Finance.GetDescription())
			{ // Finance
				if(orderEntity.Status.HasValue && orderEntity.Status.Value > this._data.CurrentStep)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot reject order that is already validated");

				if(orderEntity.Status.HasValue && orderEntity.Status.Value < this._data.CurrentStep)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Order is not reviewed yet by previous validator");
			}
			else
			{
				if(orderEntity.Level.HasValue && orderEntity.Level.Value > this._data.CurrentStep)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Cannot reject order that is already validated");

				if(orderEntity.Level.HasValue && orderEntity.Level.Value < this._data.CurrentStep)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Order is not reviewed yet by previous validator");
			}

			// --
			if(orderEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower()) // External
			{
				var validatorEntities = Validators.getByOrderId(orderEntity.OrderId, out List<string> errors);
				if(validatorEntities == null || validatorEntities.Count <= 0)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Empty validator list");

				if(orderEntity.IssuerId != this._user.Id)
				{
					if(this._data.CurrentStep == validatorEntities.Count - 1) // Purchase
					{
						var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);
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
			else // Internal order
			{
				if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector) // department' s budget
				{
					var siteDirector = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
					var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderEntity.DepartmentId ?? -1);
					if((department == null || department.HeadUserId != this._user.Id)
						&& (siteDirector == null || siteDirector.DirectorId != this._user.Id))
						return ResponseModel<int>.FailureResponse(key: "1", value: "User not a director of department");
				}
				else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.SiteDirector) // site' s budget
				{
					var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderEntity.CompanyId ?? -1);
					if(company == null || company.DirectorId != this._user.Id)
						return ResponseModel<int>.FailureResponse(key: "1", value: "User not a director of site");
				}
			}

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderEntity.ProjectId ?? -1);
			if(projectEntity != null)
			{
				if(projectEntity.Closed.HasValue && projectEntity.Closed.Value == true)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been closed");

				if(projectEntity.Archived.HasValue && projectEntity.Archived.Value)
					return ResponseModel<int>.FailureResponse(key: "1", value: "Project is archived");

				if(projectEntity.Id_State == (int)Enums.BudgetEnums.ProjectApprovalStatuses.Inactive)
					return ResponseModel<int>.FailureResponse(key: "1", value: $"Project [{projectEntity.ProjectName}] has been deactivated");

			}

			return ResponseModel<int>.SuccessResponse();
		}

		internal bool saveVersionHistory()
		{
			try
			{
				//var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data.OrderId);
				var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
				var orderArticleEntites = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderId(this._data.OrderId);
				var orderArticleExtensionEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);

				// Save Order
				Infrastructure.Data.Access.Tables.FNC.Budget_Order_VersionAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Budget_Order_VersionEntity
				{
					Id_Order = orderExtensionEntity.OrderId,
					Dept_name = orderExtensionEntity.DepartmentName,
					Id_Currency_Order = orderExtensionEntity.CurrencyId,
					Id_Dept = orderExtensionEntity.DepartmentId,
					Id_Land = orderExtensionEntity.CompanyId,
					Land_name = orderExtensionEntity.CompanyName,
					Id_Level = this._data.CurrentStep,
					Id_Project = orderExtensionEntity.ProjectId,
					Id_Status = (int)Enums.BudgetEnums.ValidationStatuses.Rejection,
					Id_Supplier_VersionOrder = orderExtensionEntity.SupplierId,
					Id_User = this._user.Id,
					Id_VO = -1,
					Nr_version_Order = -1, // >>>>>>>>>>>>>>>
					Step_Order = Enums.BudgetEnums.GetOrderValidationStatus(this._data.CurrentStep), // >>>>>>>>>>
					TotalCost_Order = (double?)orderArticleExtensionEntites.Select(x => x.TotalCost.GetValueOrDefault())?.Sum(),
					Version_Order_date = DateTime.Now
				});

				// Save articles
				Infrastructure.Data.Access.Tables.FNC.Budget_Article_VersionAccess.Insert(
					orderArticleExtensionEntites?.Select(x => new Infrastructure.Data.Entities.Tables.FNC.Budget_Article_VersionEntity
					{
						Action_Version_Article = Enums.BudgetEnums.ValidationStatuses.Validation.GetDescription(),
						Currency_Version_Article = x.CurrencyName,
						Dept_name_VersionArticle = orderExtensionEntity.DepartmentName,
						Id_AOV = -1,
						Id_Article = x.ArticleId,
						Id_Currency_Version_Article = x.CurrencyId,
						Id_Dept_VersionArticle = orderExtensionEntity.DepartmentId,
						Id_Land_VersionArticle = orderExtensionEntity.CompanyId,
						Id_Level_VersionArticle = this._data.CurrentStep,
						Id_Order_Version = orderExtensionEntity.OrderId,
						Id_Project_VersionArticle = orderExtensionEntity.ProjectId,
						Id_Status_VersionArticle = (int)Enums.BudgetEnums.ValidationStatuses.Rejection,
						Id_Supplier_VersionArticle = orderExtensionEntity.SupplierId,
						Id_User_VersionArticle = this._user.Id,
						Land_name_VersionArticle = orderExtensionEntity.CompanyName,
						Quantity_VersionArticle = x.Quantity,
						TotalCost__VersionArticle = (double?)x.TotalCost,
						Unit_Price_VersionArticle = (double?)x.UnitPrice,
						Version_Article_date = DateTime.Now
					})?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				return false;
			}

			return true;
		}
		internal void rejectOrder()
		{
			var orderExtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			var orderArticleExtensionEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data.OrderId);

			#region >>>>>> Budget DB
			var isInterimValidation = false;
			var headOfEmail = "";
			var directorUserName = "";

			// -- Increase User Budget
			#region >>>> Order Amount <<<<
			var orderAmount = 0m;
			if(orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
			{
				orderAmount = Helpers.Processings.Budget.Order.getItemsAmount(orderArticleExtensionEntites, false, true, orderExtensionEntity.Discount ?? 0);
			}
			else
			{
				// - This will send back th Amount of first validation
				orderAmount = Helpers.Processings.Budget.Order.getLeasingUserAmount(
					this._data.OrderId,
					orderExtensionEntity.ValidationRequestTime.Value.Year,
					orderExtensionEntity.ValidationRequestTime.Value.Month)
					* Helpers.Processings.Budget.Order.getOrderLeasingFirstYearNbMonths(orderExtensionEntity);
			}
			#endregion Order Amount

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(orderExtensionEntity.ProjectId ?? -1);

			if(orderExtensionEntity.OrderType?.Trim().ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower()) // Internal Project
			{
				if(orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
				{
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity.IssuerId);
					if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)
					{
						if(projectEntity != null)
						{
							projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) - (decimal?)orderAmount ?? 0;
							projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) - 1;
							Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);
						}

						var userBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
						userBudgetEntity.AmountSpent = userBudgetEntity.AmountSpent - orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userBudgetEntity);


						var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderExtensionEntity.DepartmentId ?? -1);
						var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderExtensionEntity.CompanyId ?? -1);
						// - Rejection by Site Director before HeadOf
						if(department.HeadUserId != this._user.Id && company.DirectorId == this._user.Id)
						{
							var directorUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(company?.DirectorId ?? -1);
							isInterimValidation = true;
							headOfEmail = department?.HeadUserEmail;
							directorUserName = directorUser?.Name;
						}

					}
					else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)
					{
						if(projectEntity != null)
						{
							projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) - (decimal?)orderAmount ?? 0;
							projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) - 1;
							Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);
						}

						var userBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
						userBudgetEntity.AmountSpent = userBudgetEntity.AmountSpent - orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userBudgetEntity);

						var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderExtensionEntity.DepartmentId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
						deptBudgetEntity.AmountSpent = deptBudgetEntity.AmountSpent - orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptBudgetEntity);
					}
					else if(this._data.CurrentStep > (int)Enums.BudgetEnums.ValidationLevels.SiteDirector) // This includes Purchase rejection
					{
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
					}
				}
				else // - Leasing
				{
					var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity.IssuerId);
					if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)
					{
						if(projectEntity != null)
						{
							projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) - (decimal?)orderAmount ?? 0;
							projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) - 1;
							Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);
						}

						var userBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
						userBudgetEntity.AmountSpent = userBudgetEntity.AmountSpent - orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userBudgetEntity);


						var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(orderExtensionEntity.DepartmentId ?? -1);
						var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderExtensionEntity.CompanyId ?? -1);
						// - Rejection by Site Director before HeadOf
						if(department.HeadUserId != this._user.Id && company.DirectorId == this._user.Id)
						{
							var directorUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(company?.DirectorId ?? -1);
							isInterimValidation = true;
							headOfEmail = department?.HeadUserEmail;
							directorUserName = directorUser?.Name;
						}

					}
					else if(this._data.CurrentStep == (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)
					{
						if(projectEntity != null)
						{
							projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) - (decimal?)orderAmount ?? 0;
							projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) - 1;
							Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);
						}

						var userBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUserAndYear(userEntity.Id, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
						userBudgetEntity.AmountSpent = userBudgetEntity.AmountSpent - orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(userBudgetEntity);

						var deptBudgetEntity = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(orderExtensionEntity.DepartmentId ?? -1, orderExtensionEntity.ValidationRequestTime?.Year ?? -1);
						deptBudgetEntity.AmountSpent = deptBudgetEntity.AmountSpent - orderAmount;
						Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(deptBudgetEntity);
					}
					else if(this._data.CurrentStep > (int)Enums.BudgetEnums.ValidationLevels.SiteDirector) // This includes Purchase rejection
					{
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
					}

					//- Remove validation History
					Infrastructure.Data.Access.Tables.FNC.OrderValidationLeasingAccess.DeleteByOrderId(this._data.OrderId);
				}
			}
			else // External Project
			{
				if(orderExtensionEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase)
				{
					projectEntity.TotalSpent = (projectEntity.TotalSpent ?? 0) - orderAmount;
					projectEntity.OrderCount = (projectEntity?.OrderCount ?? 0) - 1;
					Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity);
				}
			}


			// - workflow history
			Helpers.Processings.Budget.Order.SaveOrderHistory(orderExtensionEntity, Enums.BudgetEnums.OrderWorkflowActions.Reject, this._user, ((Enums.BudgetEnums.ValidationLevels)this._data.CurrentStep).GetDescription());

			// - Rejection History 
			Infrastructure.Data.Access.Tables.FNC.OrderRejectionAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.OrderRejectionEntity
			{
				Id = -1,
				OrderId = orderExtensionEntity?.OrderId ?? -1,
				OrderArticleCount = orderArticleExtensionEntites?.Count ?? 0,
				OrderProjectId = orderExtensionEntity?.ProjectId ?? -1,
				OrderTotalAmount = (decimal)orderAmount,
				OrderType = orderExtensionEntity.OrderType,
				OrderUserId = orderExtensionEntity.IssuerId,
				UserId = this._user.Id,
				RejectionLevel = this._data.CurrentStep,
				RejectionNotes = this._data.Notes,
				RejectionTime = DateTime.Now
			});
			// - Rejection History 

			// Rejection
			orderExtensionEntity.Level = 0; // reset workflow
			orderExtensionEntity.Status = 0;
			orderExtensionEntity.ApprovalTime = null;
			orderExtensionEntity.ApprovalUserId = null;
			// Set Rejection flags
			orderExtensionEntity.LastRejectionLevel = this._data.CurrentStep;
			orderExtensionEntity.LastRejectionTime = DateTime.Now;
			orderExtensionEntity.LastRejectionUserId = this._user.Id;
			orderExtensionEntity.ValidationRequestTime = null;
			orderExtensionEntity.BudgetYear = -1;
			// check if super validator rejection
			var companyDirectorEmail = "";
			var oldOrderEnxtensionEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			if(oldOrderEnxtensionEntity.Level == (int)Enums.BudgetEnums.ValidationLevels.SuperValidator)
			{
				var companyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(orderExtensionEntity.CompanyId ?? -1);
				var companyUserEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(companyEntity.DirectorId ?? -1);
				companyDirectorEmail = companyUserEntity?.Email;
			}
			Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(orderExtensionEntity);
			#endregion // >>>>>> Budget DB

			// Send email notification
			var firstValidator = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(orderExtensionEntity.IssuerId);
			if(firstValidator != null && !string.IsNullOrEmpty(firstValidator.Email) && !string.IsNullOrWhiteSpace(firstValidator.Email))
			{
				var emailTitle = "[Budget] Order Rejection";
				var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
				+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")} {firstValidator.Username},</span><br/>"
				+ $"<br/><span style='font-size:1.15em;'>Your budget validation request for order <strong>{orderExtensionEntity.OrderNumber?.ToUpper()}</strong>";
				if(projectEntity != null)
					emailContent += $" from project <strong>{projectEntity?.ProjectName?.ToUpper()}</strong>";

				emailContent += $" has been rejeted by <strong>{this._user.Name?.ToUpper()}</strong> on {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}.";
				if(!string.IsNullOrWhiteSpace(this._data.Notes))
					emailContent += $"<br/><br/><strong>Notes from {this._user.Name?.ToUpper()}:</strong> {this._data.Notes}";

				emailContent += $"</span><br/><br/>Please, login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/budget-rebuild/orders/edit/{orderExtensionEntity.OrderId}'>here</a>"
					+ "<br/><br/>Regards, <br/>IT Department</div>";

				try
				{
					Module.EmailingService.SendEmailSendGridWithStaticTemplate(
						emailContent,
						emailTitle,
						new List<string> { firstValidator.Email }, null,
						!string.IsNullOrEmpty(companyDirectorEmail) && !string.IsNullOrWhiteSpace(companyDirectorEmail) ? new List<string> { companyDirectorEmail } : null,
					   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);

					// Send email notification
					/*Module.EmailingService.SendEmailAsync(emailTitle, emailContent, new List<string> { firstValidator.Email },
						saveHistory: true, senderEmail: this._user.Email, senderName: this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);*/
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{firstValidator.Email}]"));
					Infrastructure.Services.Logging.Logger.Log(e);
				}
			}

			if(isInterimValidation && !string.IsNullOrWhiteSpace(headOfEmail))
			{
				try
				{
					var emailTitle = "[Budget] Order Rejection";
					var emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
					+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>"
					+ $"<br/><span style='font-size:1.15em;'><strong>{directorUserName}</strong> has rejected the budget validation request sent by <strong>{firstValidator.Username}</strong> for order <strong>{orderExtensionEntity.OrderNumber?.ToUpper()}</strong>";
					if(projectEntity != null)
						emailContent += $" from project <strong>{projectEntity?.ProjectName?.ToUpper()}</strong>";

					emailContent += $" on {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}.";
					if(!string.IsNullOrWhiteSpace(this._data.Notes))
						emailContent += $"<br/><br/><strong>Notes from {this._user.Name?.ToUpper()}:</strong> {this._data.Notes}";

					emailContent += $"</span>"
						+ "<br/><br/>Regards, <br/>IT Department</div>";

					Module.EmailingService.SendEmailSendGridWithStaticTemplate(
						emailContent
						, emailTitle,
						new List<string> { headOfEmail }, null, null,
					   true, this._user.Email, this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);


					/*Module.EmailingService.SendEmailAsync(emailTitle, emailContent, new List<string> { headOfEmail }, null,
						saveHistory: true, senderEmail: this._user.Email, senderName: this._user.Username, senderId: this._user.Id, senderCC: false, attachmentIds: null);*/
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", headOfEmail)}]"));
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
			}
		}
	}
}
