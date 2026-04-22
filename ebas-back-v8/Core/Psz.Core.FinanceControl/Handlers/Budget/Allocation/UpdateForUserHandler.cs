using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;
	using System.ComponentModel.Design;
	using System.Linq;

	public class UpdateForUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Allocation.User.UpdateModel _data { get; set; }

		public UpdateForUserHandler(Identity.Models.UserModel user, Models.Budget.Allocation.User.UpdateModel model)
		{
			this._user = user;
			this._data = model;
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

				/// 
				this._data.CreationTime = DateTime.Now;
				this._data.CreationUserId = this._user.Id;

				var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.UserId);
				var oldAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Get(this._data.Id);
				var departmentAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(user.DepartmentId ?? -1, this._data.Year);
				departmentAllocation.LastEditTime = DateTime.Now;
				departmentAllocation.LastEditUserId = this._user.Id;
				departmentAllocation.AmountAllocatedToUsers = departmentAllocation.AmountAllocatedToUsers + this._data.AmountYear - oldAllocation.AmountYear;
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(departmentAllocation);

				oldAllocation.AmountYear = this._data.AmountYear;
				oldAllocation.AmountMonth = this._data.AmountMonth;
				oldAllocation.AmountOrder = this._data.AmountOrder;
				oldAllocation.AmountNotificationThreshold = this._data.AmountNotificationThreshold;
				var updatedCount = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Update(oldAllocation);

				// - check check leasings to update
				Helpers.Processings.Budget.Order.applyLeasingFees(false);

				// - 2024-01-16 - send email notifs
				var leasingOrdersResponse = new Handlers.Budget.Order.GetLeasingHandler(this._user, new Models.Budget.Order.OrderLeasingRequestModel { CompanyId = user.CompanyId, DepartmentId = user.DepartmentId, EmployeeId = user.Id, Year = this._data.Year })
					.Handle();
				if(leasingOrdersResponse.Success == true && leasingOrdersResponse.Body?.Count > 0)
				{
					#region Email
					var emailTitle = "";
					var emailContent = "";

					emailTitle = "[Budget] Leasing Orders summary";
					emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

					emailContent += $"<br/><span style='font-size:1.15em;'>This is a summary of the leasing orders for current year issued by <strong>{user.Name?.ToUpper()}</strong>.<br/>";

					emailContent += $@"
				  <h2 class=''> Leasing items (<strong>{leasingOrdersResponse.Body?.Count ?? 0}</strong>)
				  </h2> <table border='collapse'>
								<tr>
								  <th class='p-2 border-bottom'>Order Number</th>
								  <th class='p-2 border-bottom'>Type</th>
								  <th class='p-2 border-bottom'>Project</th>
								  <th class='p-2 border-bottom'>Supplier</th>
								  <th class='p-2 border-bottom'>Employee</th>
								  <th class='p-2 border-bottom text-right'>Month Amount</th>
								  <th class='p-2 border-bottom text-right'>Year Amount</th>
								</tr>
{string.Join("", leasingOrdersResponse.Body.Select(x => $@"
								<tr>
								  <td class='p-2'>{x.Number}</td>
								  <td class='p-2'>{x.Type}</td>
								  <td class='p-2'>{x.ProjectName}</td>
								  <td class='p-2'>{x.SupplierName}</td>
								  <td class='p-2'>{x.ResponsableName}</td>
								  <td class='p-2 text-right nobr'>{(x.LeasingMonthAmount ?? 0).ToString("n3")}</td>
								  <td class='p-2 text-right nobr'>{x.LeasingCurrentYearAmount.ToString("n3")}</td>
								</tr>
"))}
								
								<tr>
								  <td colspan='5' ><strong>Total</strong></td>
								  <td >{leasingOrdersResponse.Body.Sum(x => x.LeasingMonthAmount ?? 0).ToString("n3")}</td>
								  <td >{leasingOrdersResponse.Body.Sum(x => x.LeasingCurrentYearAmount).ToString("n3")}</td>
								</tr>
				  </table>
							";

					emailContent += $"</span><br/><br/>You can login to check it <a href='{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/budgets/budget-rebuild/orders'>here</a>";
					emailContent += "<br/><br/>Regards, <br/>IT Department </div>";


					try
					{
						// Send email notification
						Module.EmailingService.SendEmailAsync(emailTitle, emailContent, new List<string> { user?.Email ?? "" }, null, null,
							saveHistory: true, senderEmail: user.Email, senderName: user.Username, senderId: user.Id, senderCC: false, attachmentIds: null, emailHead: "", IsTable: true);
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", user?.Email ?? "")}]"));
						Infrastructure.Services.Logging.Logger.Log(ex);
						//throw new Exception($"Unable to send email to [{string.Join(", ", nextValidatorEmail)}]");
					}
					#endregion
				}

				return ResponseModel<int>.SuccessResponse(updatedCount);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null || this._user.Access?.Financial?.Budget?.AssignEditUser != true)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			//if (this._data.AmountInvest + this._data.AmountFix != this._data.AmountYear)
			//    return ResponseModel<int>.FailureResponse($"Amount total [{String.Format("{0:N}", this._data.AmountYear)}] not equal to Fix [{String.Format("{0:N}", this._data.AmountFix)}] + Invest [{String.Format("{0:N}", this._data.AmountInvest)}].");

			var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._data.UserId);
			if(user == null)
				return ResponseModel<int>.FailureResponse("User not found");

			var oldAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.Get(this._data.Id);
			if(oldAllocation == null)
				return ResponseModel<int>.FailureResponse("Allocation not found");

			// - 2024-03-22 use Discount
			var articleEntities = Helpers.Processings.Budget.Order.getPurchaseItems(this._data.UserId, DateTime.Today.Year, null);
			var orders = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderIds(articleEntities.Select(x => x.OrderId)?.ToList());
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
				Helpers.Processings.Budget.Order.GetLeasingAmount(this._data.UserId, DateTime.Today.Year, null);
			if(this._data.AmountYear < yearExpenses)
				return ResponseModel<int>.FailureResponse($"Year amount smaller than already spent");

			if(this._data.AmountYear < this._data.AmountMonth)
			{
				return ResponseModel<int>.FailureResponse($"Month amount cannot be bigger than year amount");
			}

			if(this._data.AmountYear < this._data.AmountOrder)
			{
				return ResponseModel<int>.FailureResponse($"Order amount cannot be bigger than year amount");
			}

			if(this._data.AmountMonth < this._data.AmountOrder)
			{
				return ResponseModel<int>.FailureResponse($"Order amount cannot be bigger than month amount");
			}

			var departmentAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(user.DepartmentId ?? -1, this._data.Year);
			if(departmentAllocation == null)
			{
				return ResponseModel<int>.FailureResponse($"Department not allocated for year {this._data.Year}");
			}

			if(departmentAllocation.AmountNotificationThreshold > 0 && this._data.AmountNotificationThreshold > 0 &&
				departmentAllocation.AmountNotificationThreshold < this._data.AmountNotificationThreshold)
			{
				return ResponseModel<int>.FailureResponse($"User notification min amount cannot be bigger than department [{departmentAllocation.AmountNotificationThreshold?.ToString("0.###")}]");
			}

			if(this._data.AmountYear > oldAllocation.AmountYear)
			{
				if(this._data.AmountYear > oldAllocation.AmountYear + (departmentAllocation.AmountInitial - departmentAllocation.AmountAllocatedToUsers))
				{
					return ResponseModel<int>.FailureResponse($"Department allocation not enough {String.Format("{0:N}", oldAllocation.AmountYear + (departmentAllocation.AmountInitial - departmentAllocation.AmountAllocatedToUsers))}");
				}
			}
			//// - return old value, b4 comparison
			//if (departmentAllocation.AmountInitial - departmentAllocation.AmountAllocatedToUsers + oldAllocation.AmountYear < this._data.AmountOrder)
			//{
			//    return ResponseModel<int>.FailureResponse($"Department user allocation not enough for order amount [{String.Format("{0:N}", departmentAllocation.AmountInitial - (departmentAllocation.AmountAllocatedToUsers + departmentAllocation.AmountAllocatedToUsers) + oldAllocation.AmountYear)}]");
			//}

			//if (departmentAllocation.AmountInitial - (departmentAllocation.AmountAllocatedToUsers + departmentAllocation.AmountAllocatedToUsers) + oldAllocation.AmountYear < this._data.AmountMonth)
			//{
			//    return ResponseModel<int>.FailureResponse($"Department user allocation not enough for month amount [{String.Format("{0:N}", (departmentAllocation.AmountInitial - (departmentAllocation.AmountAllocatedToUsers + departmentAllocation.AmountAllocatedToUsers) + oldAllocation.AmountYear))}]");
			//}

			//if (departmentAllocation.AmountInitial - (departmentAllocation.AmountAllocatedToUsers + departmentAllocation.AmountAllocatedToProjects) + oldAllocation.AmountYear < this._data.AmountYear)
			//{
			//    return ResponseModel<int>.FailureResponse($"Department user allocation not enough for year amount [{String.Format("{0:N}", departmentAllocation.AmountInitial - (departmentAllocation.AmountAllocatedToUsers + departmentAllocation.AmountAllocatedToUsers) + oldAllocation.AmountYear)}]");
			//}


			// - User's Leasings
			var yearLeasingAmount = Helpers.Processings.Budget.Order.GetLeasingAmount(this._data.UserId, this._data.Year, null);
			var purchaseAmount = Helpers.Processings.Budget.Order.getAmountSpent_User(this._data.UserId, includeLeasing: false);
			if(this._data.AmountYear < yearLeasingAmount + purchaseAmount)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Allocation not enough for current leasings [{String.Format("{0:N}", yearLeasingAmount)}] and purchases [{String.Format("{0:N}", purchaseAmount)}]");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
