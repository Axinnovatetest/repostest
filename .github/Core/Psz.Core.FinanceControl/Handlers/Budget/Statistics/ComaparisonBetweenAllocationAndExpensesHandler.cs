using Infrastructure.Data.Access.Tables.FNC;
using Infrastructure.Services.Reporting.Models.MTM;
using Psz.Core.FinanceControl.Enums;
using Psz.Core.FinanceControl.Models.Budget.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Psz.Core.FinanceControl.Helpers.Processings.Budget;

namespace Psz.Core.FinanceControl.Handlers.Budget.Statistics
{

	// -- To do 11/10/2023  
	public class ComaparisonBetweenAllocationAndExpensesHandler: IHandle<Psz.Core.Identity.Models.UserModel, ResponseModel<AllocationAndExpenseResponseModel>>
	{

		public Psz.Core.Identity.Models.UserModel _user { get; set; }

		public int _data { get; set; }

		public int _companyId { get; set; }

		public ComaparisonBetweenAllocationAndExpensesHandler(UserModel user, int year, int company)
		{
			_user = user;
			_data = year;
			_companyId = company;



		}

		public ResponseModel<AllocationAndExpenseResponseModel> Handle()
		{
			try
			{
				var ValidationResponse = this.Validate();

				if(!ValidationResponse.Success)
				{
					return ValidationResponse;
				}






				#region >> Departements
				// Get All Company

				//var companyentitiesAll = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();

				////Get Company IDs

				// List<int> companyIds = companyentitiesAll?.Select(x => x.CompanyId);	

				//// get all Department
				//var companyDepartementEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompanies((List<long>)companyIds);

				//  1- grouping Department per Campany ; We can combine it if you want
				//var DepartmentGroupList = companyDepartementEntities.GroupBy(x => x.CompanyId);

				//// 2- Projection on the list group
				//var DepartementsAssociatedWithCompany = DepartmentGroupList?.Select(group => new DepartmentassociatedWithSite
				//										{
				//											CompanyId = group.Key,
				//											Departments = group.ToList(),
				//										});


				// For each Company  check Allocation and Expense
				//-- by using companyId try to get Company Allocation at budgetCompanyAllocation 
				#endregion



				var companyAllocation = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompanyAndYear(_companyId, _data)
										?? new Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationCompanyEntity(); // at least prevents exceptions


				decimal initialAmount = companyAllocation.AmountInitial;


				decimal initialAmountInThousands = initialAmount / 1000;


				string formattedInitialAmount = initialAmountInThousands.ToString("N1") + "K";

				// REM : Maybe check if Company allocation has Amounts Supplements And Add it. 


				var getOrderPayementtypesAll = Enum.GetValues(typeof(BestellungenExtensionEnums.OrderPaymentTypes)).Cast<BestellungenExtensionEnums.OrderPaymentTypes>().ToList();

				var PurchaseOrderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyAndYear(_companyId, _data, (BestellungenExtensionEnums.OrderPaymentTypes)Enum.GetValues(typeof(BestellungenExtensionEnums.OrderPaymentTypes)).GetValue(0)).Where(x => x.ApprovalUserId != null);


				//var LeasingOrderEnties = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyAndYear(_companyId, _data, (BestellungenExtensionEnums.OrderPaymentTypes)Enum.GetValues(typeof(BestellungenExtensionEnums.OrderPaymentTypes)).GetValue(1));

				var LeasingOrderEnties = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByYearAndCompanies(_data, new List<int> { _companyId }).Where(x => x.ApprovalUserId != null);

				// if purchase get all Articles 

				var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(PurchaseOrderEntities.Select(x => x.OrderId).ToList());

				#region >>> processing

				//TotalAmountBeforeDiscount = orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
				//? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(articles, false)
				//: (orderEntity.LeasingTotalAmount ?? 0);
				//			TotalAmount = orderEntity.Discount.HasValue && orderEntity.Discount.Value > 0 ? (1 - orderEntity.Discount.Value / 100) * (TotalAmountBeforeDiscount ?? 0) : (TotalAmountBeforeDiscount ?? 0);

				//TotalAmountDefaultCurrencyBeforeDiscount = orderEntity.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
				//	? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(articles, false, true)
				//	: (orderEntity.LeasingTotalAmount ?? 0);
				//TotalAmountDefaultCurrency = orderEntity.Discount.HasValue && orderEntity.Discount.Value > 0 ? (1 - orderEntity.Discount.Value / 100) * (TotalAmountDefaultCurrencyBeforeDiscount ?? 0) : (TotalAmountDefaultCurrencyBeforeDiscount ?? 0);

				#endregion

				decimal? companyExpense = PurchaseOrderEntities.Sum(order =>
						order.Discount.HasValue && order.Discount.Value > 0
							? (1 - order.Discount.Value / 100) *
							  (order.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
								? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(order.OrderId), false, true, order.Discount.Value)
								: order.LeasingTotalAmount ?? 0)
							: (order.PoPaymentType == (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Purchase
								? Psz.Core.FinanceControl.Helpers.Processings.Budget.Order.getItemsAmount(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(order.OrderId), false, true)
								: order.LeasingTotalAmount ?? 0)) + (LeasingOrderEnties.Select(order => order.LeasingNbMonths * order.LeasingMonthAmount).Sum() ?? 0);


				//decimal companyExpense = (articleEntities.Select(article => article.TotalCostDefaultCurrency).Sum() ?? 0) + (LeasingOrderEnties.Select(order => order.LeasingNbMonths * order.LeasingMonthAmount).Sum() ?? 0);
				decimal companyExpenseInThousands = companyExpense.Value / 1000;
				string formattedCompanyExpense = companyExpenseInThousands.ToString("N1") + "K";

				var response = new AllocationAndExpenseResponseModel(formattedInitialAmount, formattedCompanyExpense, initialAmount, companyExpense.Value);

				return ResponseModel<AllocationAndExpenseResponseModel>.SuccessResponse(response);

			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<AllocationAndExpenseResponseModel> Validate()
		{
			if(this._user == null)
			{
				ResponseModel<AllocationAndExpenseResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<AllocationAndExpenseResponseModel>.SuccessResponse();
		}

		#region formatted devise 

		private string FormatAmountWithCurrency(decimal amount, string currencyName)
		{
			// Utilisation de la méthode de formatage pour éviter les problèmes avec ToString()
			string formattedAmount = $"{amount:N2}K {currencyName}";
			return formattedAmount;
		}
		#endregion
	}
}




