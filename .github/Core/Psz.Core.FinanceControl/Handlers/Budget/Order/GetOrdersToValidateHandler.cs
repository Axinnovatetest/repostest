using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetOrdersToValidateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private bool _data { get; set; }

		public GetOrdersToValidateHandler(Identity.Models.UserModel user, bool data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Budget.Order.OrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var projectsIds = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetByValidatorId(this._user.Id);
				var listIdProjects = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.FilterProjectByIds(projectsIds?.Select(x => x.Id_Project ?? -1)?.ToList());

				var ordersEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

				//if(listIdProjects != null && listIdProjects.Count > 0)
				//{
				//    foreach (var x in listIdProjects)
				//    {
				//        var level = Infrastructure.Data.Access.Tables.FNC.Validators_ProjectAccess.GetValidationLevel(x, this._user.Id);
				//        var ordersEntities2 = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOrdersByLevelValidation(x, level);
				//        ordersEntities.AddRange(ordersEntities2);
				//    }
				//}
				if(projectsIds != null && projectsIds.Count > 0)
				{
					foreach(var x in projectsIds)
					{
						var level = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetValidationLevel(x.Id_Project ?? -1, this._user.Id);
						var ordersEntities2 = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetOrdersByLevelValidation(x.Id_Project ?? -1, x.Level ?? -1);
						ordersEntities.AddRange(ordersEntities2);
					}
				}

				// Add Department orders if director
				var deptOrder = getDepartmentOrders();
				if(ordersEntities == null)
				{
					ordersEntities = deptOrder;
				}
				else
				{
					if(deptOrder != null)
					{
						ordersEntities.AddRange(deptOrder);
					}
				}

				// Add Sites orders if director
				var siteOrders = getSiteOrders(this._data);
				if(ordersEntities == null)
				{
					ordersEntities = siteOrders;
				}
				else
				{
					if(siteOrders != null)
					{
						ordersEntities.AddRange(siteOrders);
					}
				}

				// Add Purchase orders if on the profile
				var purchaseOrders = getPurchaseOrders();
				if(ordersEntities == null)
				{
					ordersEntities = purchaseOrders;
				}
				else
				{
					if(purchaseOrders != null)
					{
						ordersEntities.AddRange(purchaseOrders);
					}
				}

				//  souilmi 11/06/2024 --add super validation orders if super validator--
				var superValidatorsOrders = getSuperValidatorOrders();
				if(ordersEntities == null)
				{
					ordersEntities = superValidatorsOrders;
				}
				else
				{
					if(superValidatorsOrders != null)
					{
						ordersEntities.AddRange(superValidatorsOrders);
					}
				}
				// Add Finance orders if on the company
				var financeOrders = getFinanceOrders();
				if(ordersEntities == null)
				{
					ordersEntities = financeOrders;
				}
				else
				{
					if(financeOrders != null)
					{
						ordersEntities.AddRange(financeOrders);
					}
				}


				if(ordersEntities == null)
				{
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
				}
				ordersEntities = ordersEntities?.Where(x => x.BudgetYear == DateTime.Today.Year).Distinct()?.ToList();

				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(ordersEntities.Select(x => x.OrderId)?.ToList());
				var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();
				var responsableEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.Get();


				var supplierEntites = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.Get();
				var addressesEntities = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(supplierEntites?.Select(x => x.Nummer.HasValue ? x.Nummer.Value : -1)?.ToList());
				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(ordersEntities.Select(x => x.OrderId)?.ToList());
				var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntites?.Select(x => x.BestellteArtikelNr)?.ToList());
				var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(ordersEntities.Select(x => x.OrderId)?.ToList());

				var response = new List<Models.Budget.Order.OrderModel>();

				ordersEntities = ordersEntities?.DistinctBy(x => x.Id)?.ToList();
				ordersEntities = ordersEntities.OrderByDescending(x => x.Id)?.ToList();
				foreach(var orderEntity in ordersEntities)
				{
					var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
					var projectEntity = projectsEntities?.Find(e => e.Id == orderEntity?.ProjectId);
					var customer = (projectEntity != null && projectEntity.CustomerId.HasValue && projectEntity.Id_Type != 2)
					   ? Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(projectEntity.CustomerId.Value)
						  : null;

					Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenCustomerEntity = null;
					if(customer != null)
					{
						adressenCustomerEntity = customer.Nummer.HasValue
						? Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(customer.Nummer.Value)
						: null;
					}
					var articleEntity = articleEntites?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();
					var articlesEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntity?.Select(x => x.ArticleId)?.ToList());
					var bestellteArticelEntity = bestellteArticleEntities?.FindAll(x => articleEntity.Select(y => y.BestellteArtikelNr)?.ToList()?.Exists(a => a == x.Nr) == true)?.ToList();
					var fileEntity = fileEntities?.FindAll(x => x.Id_Order == orderEntity.OrderId);

					// Validators
					var validators = Validators.getByOrderId(orderEntity.OrderId, out List<string> errors);
					if(errors != null && errors.Count > 0)
						ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse(string.Join(", ", errors));

					var uValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validators?.Select(x => x.Id_Validator)?.ToList());

					response.Add(new Models.Budget.Order.OrderModel(orderEntity, bestellungEntity, projectEntity, articleEntity, bestellteArticelEntity, articlesEntities, fileEntity, validators, uValidators));
				}

				return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.OrderModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
		}

		List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> getDepartmentOrders()
		{
			var deptDirectorEntity = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(this._user.Id);
			if(deptDirectorEntity == null)
				return null;

			var ret = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			deptDirectorEntity.ForEach(x =>
				ret.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartmentIdAndLevel((int)x.Id, (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector)));

			return ret;
		}
		List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> getSiteOrders(bool strictLevel = false)
		{
			var siteDirector = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { this._user.Id });
			if(siteDirector == null)
				return null;

			var ret = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			siteDirector.ForEach(x => ret.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanyIdAndLevel(x.Id, (int)Enums.BudgetEnums.ValidationLevels.SiteDirector, strictLevel:strictLevel)?.ToList()));
			return ret;
		}
		List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> getSuperValidatorOrders()
		{
			var ret = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

			ret.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetBySuperValidators(_user.Id));
			return ret;
		}
		List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> getPurchaseOrders()
		{
			var orders = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			var userCompany = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(userEntity?.CompanyId ?? -1);
			if(userCompany == null || Helpers.Processings.Budget.HasPurchaseProfile(userEntity.Id, userCompany) != true)
				return null;

			var userCompanyExternalProjects = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.GetBySitesIds(new List<int> { userEntity.CompanyId ?? -1 }, (int)Enums.BudgetEnums.ProjectTypes.External);
			var userCompanyExternalOrders = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetForPurchaseUserExternal(userCompanyExternalProjects?.Select(x => x.Id)?.ToList());
			orders.AddRange(userCompanyExternalOrders);

			var orderEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetForPurchaseUser(userCompany.CompanyId, (int)Enums.BudgetEnums.ValidationLevels.Purchase)?.ToList();
			var externalProjectIds = orderEntities.Where(x => (x.ProjectId.HasValue && x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.External.GetDescription().ToLower()))
				?.Select(x => (int)x.ProjectId)?.ToList();

			if(externalProjectIds != null && externalProjectIds.Count > 0)
			{
				var userCompanyProjectEntites = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(externalProjectIds)
					?.Where(x => x.CompanyId == userEntity.CompanyId)?.ToList();
				orders.AddRange(orderEntities.Where(x => x.OrderType.ToLower() == Enums.BudgetEnums.ProjectTypes.Internal.GetDescription().ToLower()
			   || userCompanyProjectEntites.Exists(y => y.Id == x.ProjectId.Value))?.ToList());

				orders = orders.Distinct()?.ToList();

				return orders;
			}

			orders.AddRange(orderEntities);
			orders = orders.Distinct()?.ToList();

			return orders;
		}
		List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> getFinanceOrders()
		{
			var validatorCompanies = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByFinanceValidatorIds(new List<int> { this._user.Id });
			if(validatorCompanies == null)
				return null;

			var ret = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			validatorCompanies.ForEach(x => ret.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetFinanceByValidator(x.Id, Enums.BudgetEnums.ProjectTypes.Finance.GetDescription())?.ToList()));
			return ret;
		}
	}
}
