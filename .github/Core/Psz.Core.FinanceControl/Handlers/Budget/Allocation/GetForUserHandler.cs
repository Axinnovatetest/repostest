using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetForUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Allocation.User.UpdateModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int? _data { get; set; }
		public GetForUserHandler(Identity.Models.UserModel user, int? year)
		{
			this._user = user;
			this._data = year;
		}

		public ResponseModel<List<Models.Budget.Allocation.User.UpdateModel>> Handle()
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
				var userDeptEntities = new List<Infrastructure.Data.Entities.Tables.COR.UserEntity> { userEntity };
				var allocationEntities =
					Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUsersAndYear(new List<int> { this._user.Id }/*, DateTime.Today.Year*/)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BudgetAllocationUserEntity>();

				var departments = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get(new List<long> { userEntity?.DepartmentId ?? -1 });

				// - Departments user is HeadOf
				var departmentsHeadOf = this._user.IsGlobalDirector
					? Infrastructure.Data.Access.Tables.STG.DepartmentAccess.Get()
				   : Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(this._user.Id);
				if(departmentsHeadOf != null && departmentsHeadOf.Count > 0)
				{
					departments.AddRange(departmentsHeadOf);
					userDeptEntities.AddRange(Infrastructure.Data.Access.Tables.COR.UserAccess.GetByDepartmentIds(departments?.Select(x => (int)x.Id)?.ToList()));
				}


				allocationEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BudgetAllocationUserAccess.GetByUsersAndYear(userDeptEntities?.Select(x => (int)x.Id)?.ToList()/*, DateTime.Today.Year*/));
				var companyEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(departments?.Select(x => x.CompanyId)?.ToList());


				allocationEntities = allocationEntities?.DistinctBy(x => x.Id)?.Where(x => !this._data.HasValue || (this._data.HasValue && x.Year == this._data)).ToList();
				var responseBody = new List<Models.Budget.Allocation.User.UpdateModel>();
				foreach(var allocationItem in allocationEntities)
				{
					var userItem = userDeptEntities?.Find(x => x.Id == allocationItem.UserId);
					var orderEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
					if(userItem != null)
					{
						var departmentItem = departments?.Find(x => x.Id == userItem.DepartmentId);
						if(departmentItem != null)
						{
							var companyItem = companyEntities?.Find(x => x.Id == departmentItem.CompanyId);

							// -
							var ordersInternal = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUserAndLevelAndPayementTypeAndYear(
								(int)Enums.BudgetEnums.ValidationLevels.Draft,
						  (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
								this._data.Value, allocationItem.UserId, Enums.BudgetEnums.ProjectTypes.Internal.GetDescription(), null)
								?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
							if(ordersInternal != null && ordersInternal.Count > 0)
								orderEntities.AddRange(ordersInternal);

							var ordersFinance = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUserAndLevelAndPayementTypeAndYear(
								(int)Enums.BudgetEnums.ValidationLevels.Draft,
						  (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
								this._data.Value, allocationItem.UserId, Enums.BudgetEnums.ProjectTypes.Finance.GetDescription(), null)
								?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
							if(ordersFinance != null && ordersFinance.Count > 0)
								orderEntities.AddRange(ordersFinance);

							var leasings = this._data.HasValue
								? Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByUserAndYear(this._data.Value, allocationItem.UserId, null)
								: new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
							//orderLeasingEntities.Where(x => x.IssuerId == allocationItem.UserId)?.ToList();
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
							allocationItem.AmountSpent = Helpers.Processings.Budget.Order.getItemsAmount(
								articleEntities/*.Where(x => orders?.Any(z => z.OrderId == x.OrderId) == true).ToList()*/, false, true, 0)
								+ // - add currentAllocation's user leasings
								(leasings?.Select(x => Helpers.Processings.Budget.Order.getLeasingAmount(x, allocationItem.Year))?.Sum() ?? 0m);

							responseBody.Add(new Models.Budget.Allocation.User.UpdateModel(allocationItem, companyItem, departmentItem));
						}
					}
				}

				return ResponseModel<List<Models.Budget.Allocation.User.UpdateModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.Allocation.User.UpdateModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Allocation.User.UpdateModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Models.Budget.Allocation.User.UpdateModel>>.FailureResponse("user not found");

			return ResponseModel<List<Models.Budget.Allocation.User.UpdateModel>>.SuccessResponse();
		}
	}
}