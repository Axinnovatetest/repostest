using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateForDepartmentHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Allocation.Department.UpdateModel _data { get; set; }

		public UpdateForDepartmentHandler(Identity.Models.UserModel user, Models.Budget.Allocation.Department.UpdateModel model)
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
				this._data.LastEditTime = DateTime.Now;
				this._data.LastEditUserId = this._user.Id;

				var allocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Get(this._data.Id);
				var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data.DepartmentId);
				var companyAlloaction = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear((int)department.CompanyId, this._data.Year);
				companyAlloaction.AmountAllocatedToDepartments = companyAlloaction.AmountAllocatedToDepartments + this._data.AmountInitial - allocation.AmountInitial;
				companyAlloaction.LastEditTime = DateTime.Now;
				companyAlloaction.LastEditUserId = this._user.Id;
				// -
				Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(companyAlloaction);
				var updatedCount = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Update(this._data.ToEntity());

				// - check check leasings to update
				Helpers.Processings.Budget.Order.applyLeasingFees(false);

				// - 2024-01-16 - send email notifs
				var leasingOrdersResponse = new Handlers.Budget.Order.GetLeasingHandler(this._user, new Models.Budget.Order.OrderLeasingRequestModel { CompanyId = (int)(department?.CompanyId ?? -1), DepartmentId = (int)(department?.Id ?? -1), EmployeeId = null, Year = this._data.Year })
				.Handle();
				if(leasingOrdersResponse.Success == true && leasingOrdersResponse.Body?.Count > 0)
				{
					var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get((int)(department?.CompanyId ?? -1));
					#region Email
					var emailTitle = "";
					var emailContent = "";

					emailTitle = "[Budget] Leasing Orders summary";
					emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

					emailContent += $"<br/><span style='font-size:1.15em;'>This is a summary of the leasing orders for <strong>{department?.Name}  | {companyExtension?.CompanyName}</strong>, current year.<br/>";

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
						Module.EmailingService.SendEmailAsync(emailTitle, emailContent, new List<string> { department?.HeadUserEmail ?? "" }, null, null,
						saveHistory: true, senderEmail: "", senderName: "", senderId: -1, senderCC: false, attachmentIds: null, emailHead: "", IsTable: true);
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", department?.HeadUserEmail ?? "")}]"));
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
			if(this._user == null || this._user.Access?.Financial?.Budget?.AssignEditDept != true)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(this._data.AmountInvest + this._data.AmountFix != this._data.AmountInitial)
				return ResponseModel<int>.FailureResponse($"Amount total[{String.Format("{0:N}", this._data.AmountInitial)}] not equal to Fix[{String.Format("{0:N}", this._data.AmountFix)}] + Invest[{String.Format("{0:N}", this._data.AmountInvest)}].");

			var department = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(this._data.DepartmentId);
			if(department == null)
				return ResponseModel<int>.FailureResponse("Department not found");

			var oldAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.Get(this._data.Id);
			if(oldAllocation == null)
			{
				return ResponseModel<int>.FailureResponse($"Allocation not found");
			}

			if(/*oldAllocation.AmountAllocatedToProjects + */oldAllocation.AmountAllocatedToUsers > this._data.AmountInitial)
			{
				return ResponseModel<int>.FailureResponse($"New amount [{String.Format("{0:N}", this._data.AmountInitial)}] smaller than already allocated amount [{String.Format("{0:N}", /*oldAllocation.AmountAllocatedToUsers + */oldAllocation.AmountAllocatedToUsers)}]");
			}

			var allocations = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentAndYear(this._data.DepartmentId, this._data.Year);
			if(allocations?.DepartmentId == this._data.DepartmentId && allocations?.Year == this._data.Year && allocations?.Id != this._data.Id)
			{
				return ResponseModel<int>.FailureResponse($"Department already allocated for year {this._data.Year}");
			}

			var employeeIds = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(new List<int> { this._data.DepartmentId })
				?.Select(x => x.Id)?.ToList();
			var employeesAllocationEntities = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUsersAndYear(employeeIds, this._data.Year);
			var maxEmployeeNotifAmount = employeesAllocationEntities?.Max(x => x.AmountNotificationThreshold) ?? 0;
			if(this._data.AmountNotificationThreshold < maxEmployeeNotifAmount)
			{
				return ResponseModel<int>.FailureResponse($"New notification amount [{String.Format("{0:N}", this._data.AmountNotificationThreshold)}] smaller than employee amount [{maxEmployeeNotifAmount.ToString("0.##")}]");
			}

			// - Site
			var companyAlloaction = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear((int)department.CompanyId, this._data.Year);
			if(companyAlloaction == null)
			{
				return ResponseModel<int>.FailureResponse($"Company not allocated for year {this._data.Year}");
			}

			//var supplements = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { companyAlloaction.Id }, this._data.Year)?.Sum(x => x.AmountInitial) ?? 0m;
			//// - return old department Amount b4 comparison
			//if (companyAlloaction.AmountInitial + supplements + oldAllocation.AmountInitial < companyAlloaction.AmountAllocatedToDepartments + companyAlloaction.AmountAllocatedToProjects + this._data.AmountInitial)
			//{
			//    return ResponseModel<int>.FailureResponse($"Company amount for year {this._data.Year} not enough");
			//}

			var supplements = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { companyAlloaction.CompanyId }, this._data.Year)?.Sum(x => x.AmountInitial) ?? 0m;
			if(allocations.AmountInitial + companyAlloaction.AmountInitial + supplements - (companyAlloaction.AmountAllocatedToDepartments + companyAlloaction.AmountAllocatedToProjects) < this._data.AmountInitial)
			{
				return ResponseModel<int>.FailureResponse($"Company amount for year {this._data.Year} not enough [{String.Format("{0:N}", (allocations.AmountInitial + companyAlloaction.AmountInitial + supplements - (companyAlloaction.AmountAllocatedToDepartments + companyAlloaction.AmountAllocatedToProjects)))}]");
			}

			var departmentUserIds = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(new List<int> { this._data.DepartmentId })?.Select(x => x.Id)?.ToList();
			var departmentUserAllocations = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUsersAndYear(departmentUserIds, this._data.Year);
			var userWAllocationIds = departmentUserAllocations.Select(x => x.UserId)?.ToList();
			var userAllocationAmount = departmentUserAllocations.Select(x => x.AmountYear)?.Sum() ?? 0m;

			// - Department's Leasings
			var leasingEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByYearAndDepartments(this._data.Year, new List<int> { this._data.DepartmentId }, null)
				?.Where(x => userWAllocationIds?.Contains(x.IssuerId) != true) // - remove allocatedUser Leasing as it is in their AmountYear
				?.Where(x => x.Level > (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)?.ToList();
			var yearLeasingAmount = leasingEntities?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, this._data.Year))?.Sum() ?? 0;
			var purchaseAmount = Helpers.Processings.Budget.Order.GetPurchaseAmount_Department(this._data.DepartmentId, this._data.Year);

			if(this._data.AmountInitial < yearLeasingAmount + userAllocationAmount)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Allocation not enough for current detached leasings [{String.Format("{0:N}", yearLeasingAmount)}] and user allocations [{String.Format("{0:N}", userAllocationAmount)}]");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
