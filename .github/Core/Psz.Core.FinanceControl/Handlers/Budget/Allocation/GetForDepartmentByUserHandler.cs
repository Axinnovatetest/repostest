using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetForDepartmentByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Allocation.Department.UpdateModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int? _data { get; set; }
		public GetForDepartmentByUserHandler(Identity.Models.UserModel user, int? year)
		{
			this._user = user;
			this._data = year;
		}

		public ResponseModel<List<Models.Budget.Allocation.Department.UpdateModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				// - Departments user is HeadOf
				var departments = this._user.IsGlobalDirector || this._user.IsCorporateDirector
					? Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get()
					: Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(this._user.Id)
						?? new List<Infrastructure.Data.Entities.Tables.STG.DepartmentEntity>();

				// - Departments user is site director of
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { this._user.Id });
				if(companyEntities != null && companyEntities.Count > 0)
				{
					foreach(var item in companyEntities)
					{
						departments.AddRange(Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByCompany(item.Id));
					}
				}

				if(companyEntities != null)
				{
					companyEntities.AddRange(Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(departments.Select(x => x.CompanyId)?.ToList()));
				}
				else
				{
					companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(departments.Select(x => x.CompanyId)?.ToList());
				}

				var allocationEntities = (Infrastructure.Data.Access.Tables.FNC.BudgetAllocationDepartmentAccess.GetByDepartmentsAndYear(departments?.Select(x => (int)x.Id)?.ToList()/*, DateTime.Today.Year*/)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationDepartmentEntity>()).Distinct().Where(x => !this._data.HasValue || (this._data.HasValue && x.Year == this._data)).ToList();



				var responseBody = new List<Models.Budget.Allocation.Department.UpdateModel>();
				foreach(var allocationItem in allocationEntities)
				{
					var departmentItem = departments?.Find(x => x.Id == allocationItem.DepartmentId);
					var orderEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
					if(departmentItem != null)
					{
						var companyItem = companyEntities?.Find(x => x.Id == departmentItem.CompanyId);
						// -
						var ordersInternal = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartementAndLevelAndPayementTypeAndYear(
							(int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector,
					   (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
							  this._data.Value, allocationItem.DepartmentId, Enums.BudgetEnums.ProjectTypes.Internal.GetDescription(), null)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
						if(ordersInternal != null && ordersInternal.Count > 0)
							orderEntities.AddRange(ordersInternal);

						var ordersFinance = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartementAndLevelAndPayementTypeAndYear(
							(int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector,
					   (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
							  this._data.Value, allocationItem.DepartmentId, Enums.BudgetEnums.ProjectTypes.Finance.GetDescription(), null)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
						if(ordersFinance != null && ordersFinance.Count > 0)
							orderEntities.AddRange(ordersFinance);

						var leasings = this._data.HasValue
							? Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByDepartemantAndYear(this._data.Value, allocationItem.DepartmentId, null)
							: new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
						//orderLeasingEntities.Where(x => x.DepartmentId == allocationItem.DepartmentId)?.ToList();
						var articleEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderEntities.Select(x => x.OrderId)?.ToList())
						   ?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
						// - 2024-03-22 use Discount
						foreach(var item in articleEntities)
						{
							var order = orderEntities?.FirstOrDefault(x => x.OrderId == item.OrderId);
							if(order != null && order.Discount.HasValue && order.Discount.Value > 0)
							{
								item.TotalCost = (1m - order.Discount / 100) * item.TotalCost;
								item.TotalCostDefaultCurrency = (1m - order.Discount / 100) * item.TotalCostDefaultCurrency;
								item.UnitPrice = (1m - order.Discount / 100) * item.UnitPrice;
								item.UnitPriceDefaultCurrency = (1m - order.Discount / 100) * item.UnitPriceDefaultCurrency;
							}
						}
						// -
						allocationItem.AmountSpent = Helpers.Processings.Budget.Order.getItemsAmount(
							articleEntities/*.Where(x => orders?	.Any(z => z.OrderId == x.OrderId) == true).ToList()*/, false, true, 0)
							+
							(leasings?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, allocationItem.Year))?.Sum() ?? 0m);

						responseBody.Add(new Models.Budget.Allocation.Department.UpdateModel(allocationItem, companyItem, leasings));
					}
				}

				return ResponseModel<List<Models.Budget.Allocation.Department.UpdateModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Allocation.Department.UpdateModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Allocation.Department.UpdateModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Models.Budget.Allocation.Department.UpdateModel>>.FailureResponse("user not found");

			return ResponseModel<List<Models.Budget.Allocation.Department.UpdateModel>>.SuccessResponse();
		}
	}
}
