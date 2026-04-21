using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Allocation
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetForCompanyByuserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Allocation.Company.UpdateModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int? _data { get; set; }
		public GetForCompanyByuserHandler(Identity.Models.UserModel user, int? year)
		{
			this._user = user;
			this._data = year;
		}

		public ResponseModel<List<Models.Budget.Allocation.Company.UpdateModel>> Handle()
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
				var userCompanies = this._user.IsGlobalDirector || this._user.IsCorporateDirector || this._user.Access?.Financial?.Budget?.AssignAllSites == true
					? Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get()
					: Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntity.Id });

				var companyIds = userCompanies?.Select(x => x.Id)?.ToList();
				var allocationEntities = Infrastructure.Data.Access.Tables.FNC.BudgetAllocationCompanyAccess.GetByCompaniesAndYear(companyIds, this._data);


				var responseBody = new List<Models.Budget.Allocation.Company.UpdateModel> { };
				if(allocationEntities != null && allocationEntities.Count > 0)
				{
					var years = allocationEntities.Select(x => x.Year)?.ToList();
					foreach(var allocationItem in allocationEntities)
					{
						var orderEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

						var ordersInternal = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyAndLevelAndPayementTypeAndYear(
							(int)Enums.BudgetEnums.ValidationLevels.SiteDirector,
					   (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
							 this._data.Value, allocationItem.CompanyId, Enums.BudgetEnums.ProjectTypes.Internal.GetDescription(), null)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
						if(ordersInternal != null && ordersInternal.Count > 0)
							orderEntities.AddRange(ordersInternal);

						var ordersFinance = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyAndLevelAndPayementTypeAndYear(
							(int)Enums.BudgetEnums.ValidationLevels.SiteDirector,
					   (int)Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionEnums.OrderPaymentTypes.Leasing,
							 this._data.Value, allocationItem.CompanyId, Enums.BudgetEnums.ProjectTypes.Finance.GetDescription(), null)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
						if(ordersFinance != null && ordersFinance.Count > 0)
							orderEntities.AddRange(ordersFinance);

						var leasings = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetLeasingByCompanyAndYear(this._data.Value, allocationItem.CompanyId, null)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
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


						responseBody.Add(new Models.Budget.Allocation.Company.UpdateModel(allocationItem, leasings));
					}
				}

				return ResponseModel<List<Models.Budget.Allocation.Company.UpdateModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Allocation.Company.UpdateModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<Models.Budget.Allocation.Company.UpdateModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id) == null)
				return ResponseModel<List<Models.Budget.Allocation.Company.UpdateModel>>.FailureResponse("user not found");

			var uCompanies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { this._user.Id });
			if(uCompanies?.Count <= 0 && !(this._user.IsGlobalDirector || this._user.IsCorporateDirector) && this._user.Access.Financial.Budget.AssignViewLand != true)
			{
				return ResponseModel<List<Models.Budget.Allocation.Company.UpdateModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Allocation.Company.UpdateModel>>.SuccessResponse();
		}
	}
}
