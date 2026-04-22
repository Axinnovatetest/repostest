using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class UpdateForCompanyHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Allocation.Company.UpdateModel _data { get; set; }

		public UpdateForCompanyHandler(Identity.Models.UserModel user, Models.Budget.Allocation.Company.UpdateModel model)
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
				var updatedCount = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Update(this._data.ToEntity());

				// - check check leasings to update
				Helpers.Processings.Budget.Order.applyLeasingFees(false);

				// - 2024-01-16 - send email notifs
				var leasingOrdersResponse = new Handlers.Budget.Order.GetLeasingHandler(this._user, new Models.Budget.Order.OrderLeasingRequestModel { CompanyId = this._data.CompanyId, DepartmentId = null, EmployeeId = null, Year = this._data.Year })
				.Handle();
				if(leasingOrdersResponse.Success == true && leasingOrdersResponse.Body?.Count > 0)
				{
					var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get(this._data.CompanyId);
					var company = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(companyExtension?.CompanyId ?? -1);
					#region Email
					var emailTitle = "";
					var emailContent = "";

					emailTitle = "[Budget] Leasing Orders summary";
					emailContent = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>"
							+ $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";

					emailContent += $"<br/><span style='font-size:1.15em;'>This is a summary of the leasing orders for <strong>{company?.Name}</strong>, current year.<br/>";

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
						Module.EmailingService.SendEmailAsync(emailTitle, emailContent, new List<string> { company?.DirectorEmail ?? "" }, null, null,
						saveHistory: true, senderEmail: "", senderName: "", senderId: -1, senderCC: false, attachmentIds: null, emailHead: "", IsTable: true);
					} catch(Exception ex)
					{
						Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(", ", company?.DirectorEmail ?? "")}]"));
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
			if(this._user == null || !(this._user.IsGlobalDirector || this._user.IsCorporateDirector))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(this._data.AmountInvest + this._data.AmountFix != this._data.AmountInitial)
				return ResponseModel<int>.FailureResponse($"Amount total[{String.Format("{0:N}", this._data.AmountInitial)}] not equal to Fix[{String.Format("{0:N}", this._data.AmountFix)}] + Invest[{String.Format("{0:N}", this._data.AmountInvest)}].");

			if(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(this._data.CompanyId) == null)
				return ResponseModel<int>.FailureResponse("Company not found");

			var oldAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.Get(this._data.Id);
			if(oldAllocation == null)
			{
				return ResponseModel<int>.FailureResponse($"Allocation not found");
			}

			var supplements = Infrastructure.Data.Access.Tables.FNC.BudgetSupplementCompanyAccess.GetByCompaniesAndYear(new List<int> { this._data.CompanyId }, this._data.Year)?.Sum(x => x.AmountInitial) ?? 0m;
			if(this._data.AmountInitial + supplements < oldAllocation.AmountAllocatedToDepartments + oldAllocation.AmountAllocatedToProjects)
			{
				return ResponseModel<int>.FailureResponse($"New amount [{String.Format("{0:N}", this._data.AmountInitial + supplements)}] smaller than already allocated amount [{String.Format("{0:N}", oldAllocation.AmountAllocatedToDepartments + oldAllocation.AmountAllocatedToProjects)}]");
			}

			var allocations = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(this._data.CompanyId, this._data.Year);
			if(allocations?.CompanyId == this._data.CompanyId && allocations?.Year == this._data.Year && allocations?.Id != this._data.Id)
			{
				return ResponseModel<int>.FailureResponse($"Company already allocated for year {this._data.Year}");
			}

			var companyDepartmentIds = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompanies(new List<long> { this._data.CompanyId })?.Select(x => (int)x.Id)?.ToList();
			var companyDepartmentAllocations = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentsAndYear(companyDepartmentIds, this._data.Year);
			var departmentWAllocationIds = companyDepartmentAllocations.Select(x => x.DepartmentId)?.ToList();
			var departmentAllocationAmount = companyDepartmentAllocations.Select(x => x.AmountInitial)?.Sum() ?? 0m;

			// - Company's Leasings
			var leasingEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByYearAndCompanies(this._data.Year, new List<int> { this._data.CompanyId }, null)
				?.Where(x => departmentWAllocationIds?.Contains(x.DepartmentId ?? -1) != true) // - remove allocatedDepartment Leasing as it is in their AmountInitial
				?.Where(x => x.Level > (int)Enums.BudgetEnums.ValidationLevels.SiteDirector)?.ToList();
			var yearLeasingAmount = leasingEntities?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, this._data.Year))?.Sum() ?? 0;
			var purchaseAmount = Helpers.Processings.Budget.Order.GetPurchaseAmount_Company(this._data.CompanyId, this._data.Year);
			if(this._data.AmountInitial < yearLeasingAmount + departmentAllocationAmount)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Allocation not enough for current detached leasings [{String.Format("{0:N}", yearLeasingAmount)}] and departments [{String.Format("{0:N}", departmentAllocationAmount)}]");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
