using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetAllByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.OrderRequestModel _data { get; set; }

		public GetAllByUserHandler(Identity.Models.UserModel user, Models.Budget.Order.OrderRequestModel data)
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
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var ordersExtensionEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
				var userProfilEntity = new Models.Administration.AccessProfile.AccessProfileModel(
				Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(
					Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { this._user.Id })
					?.Select(x => x.AccessProfileId)
					?.ToList()));

				if(userEntity.SuperAdministrator || userEntity.IsGlobalDirector.HasValue && userEntity.IsGlobalDirector.Value == true
					|| (userProfilEntity?.CommandeExternalViewAllGroup == true && userProfilEntity?.CommandeInternalViewAllGroup == true))
				{
					ordersExtensionEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Get(this._data.Year, booked: this._data.ShowCompletelyBooked);
				}
				else
				{
					if(userProfilEntity?.CommandeExternalViewAllGroup == true)
					{
						ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetExternal(booked: this._data.ShowCompletelyBooked, this._data.Year)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}
					if(userProfilEntity?.CommandeInternalViewAllGroup == true)
					{
						ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetInternal(booked: this._data.ShowCompletelyBooked, this._data.Year)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}

					var siteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntity.Id });
					if(siteEntities != null && siteEntities.Count > 0)
					{
						ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(siteEntities.Select(x => x.Id).ToList(), booked: this._data.ShowCompletelyBooked, this._data.Year)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}

					// - User Purchase of site
					var pSiteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get((long)(userEntity.CompanyId ?? -1));
					if(pSiteEntities != null)
					{
						var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(pSiteEntities.Id);
						if(Helpers.Processings.Budget.HasPurchaseProfile(userEntity.Id, companyExtension))
						{
							ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(new List<int> { companyExtension.CompanyId }, booked: this._data.ShowCompletelyBooked, this._data.Year)
									?.Where(x => x.ApprovalUserId.HasValue)?.ToList()
									?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
						}
					}

					var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntity.Id);
					if(departmentEntities != null && departmentEntities.Count > 0)
					{
						ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(departmentEntities.Select(x => (int)x.Id).ToList(), booked: this._data.ShowCompletelyBooked)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}

					// - Departments
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(new List<int> { userEntity.DepartmentId ?? -1 }, booked: this._data.ShowCompletelyBooked)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());

					// - User
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUser(this._user.Id, booked: this._data.ShowCompletelyBooked)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}

				if(ordersExtensionEntities == null || ordersExtensionEntities.Count <= 0)
				{
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
				}
				ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();

				var orderIds = ordersExtensionEntities.Select(x => x.OrderId)?.ToList();
				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(orderIds);
				var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(ordersExtensionEntities.Select(x => x.ProjectId ?? -1)?.ToList());

				Helpers.Processings.Budget.Order.updateArticlePrices(orderIds); // - FIXME: HEAVY processing

				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);
				var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntites?.Select(x => x.BestellteArtikelNr)?.ToList());
				var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(orderIds);

				var companyExtensionEntities = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();

				var response = new List<Models.Budget.Order.OrderModel>();
				Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenCustomerEntity = null;
				ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();
				ordersExtensionEntities = ordersExtensionEntities.OrderByDescending(x => x.Id)?.ToList();
				foreach(var orderEntity in ordersExtensionEntities)
				{

					var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
					var projectEntity = projectsEntities?.Find(e => e.Id == orderEntity?.ProjectId);
					if(projectEntity != null)
					{
						var customer = (projectEntity.CustomerId.HasValue && projectEntity.Id_Type != 2)
					   ? Infrastructure.Data.Access.Tables.FNC.KundenAccess.Get(projectEntity.CustomerId.Value)
						  : null;

						if(customer != null)
						{
							adressenCustomerEntity = customer.Nummer.HasValue
							? Infrastructure.Data.Access.Tables.FNC.AdressenAccess.Get(customer.Nummer.Value)
							: null;
						}
					}

					var articleEntity = articleEntites?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();
					var bestellteArticelEntity = bestellteArticleEntities?.FindAll(x => articleEntity.Select(y => y.BestellteArtikelNr)?.ToList()?.Exists(a => a == x.Nr) == true)?.ToList();
					var articlesEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntity?.Select(x => x.ArticleId)?.ToList());
					var fileEntity = fileEntities?.FindAll(x => x.Id_Order == orderEntity.OrderId);

					// Validators
					var validators = Validators.getByOrderId(orderEntity.OrderId, out List<string> errors);
					if(errors != null && errors.Count > 0)
						ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse(string.Join(", ", errors));

					var uValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validators?.Select(x => x.Id_Validator)?.ToList());
					var orderCompany = companyExtensionEntities.Find(x => x.CompanyId == orderEntity.CompanyId);

					// Handle last VALIDATOR as Profile NOT User
					response.Add(new Models.Budget.Order.OrderModel(orderEntity, bestellungEntity, projectEntity, articleEntity, bestellteArticelEntity, articlesEntities, fileEntity, validators, uValidators,
						this._user, Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId_Count(orderEntity.OrderId)));
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
			if(this._user == null)
			{
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
		}

		public static List<Models.Budget.Order.OrderModel> GetOrders(Identity.Models.UserModel user, bool? booked, out List<string> errors, int? year)
		{
			errors = new List<string>();

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			var ordersExtensionEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			var userProfilEntity = new Models.Administration.AccessProfile.AccessProfileModel(
				Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(
					Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { user.Id })
					?.Select(x => x.AccessProfileId)
					?.ToList()));

			if(userEntity.SuperAdministrator || userEntity.IsGlobalDirector.HasValue && userEntity.IsGlobalDirector.Value == true
				|| (userProfilEntity?.CommandeExternalViewAllGroup == true && userProfilEntity?.CommandeInternalViewAllGroup == true))
			{
				ordersExtensionEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Get(year, booked: booked);
			}
			else
			{
				if(userProfilEntity?.CommandeExternalViewAllGroup == true)
				{
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetExternal(booked: booked, year)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}
				if(userProfilEntity?.CommandeInternalViewAllGroup == true)
				{
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetInternal(booked: booked, year)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}

				var siteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntity.Id });
				if(siteEntities != null && siteEntities.Count > 0)
				{
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(siteEntities.Select(x => x.Id).ToList(), booked: booked, year)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}

				// - User Purchase of site
				var pSiteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get((long)(userEntity.CompanyId ?? -1));
				if(pSiteEntities != null)
				{
					var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(pSiteEntities.Id);
					if(Helpers.Processings.Budget.HasPurchaseProfile(userEntity.Id, companyExtension))
					{
						ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(new List<int> { companyExtension.CompanyId }, booked: booked, year)
								?.Where(x => x.ApprovalUserId.HasValue)?.ToList()
								?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}
				}

				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntity.Id);
				if(departmentEntities != null && departmentEntities.Count > 0)
				{
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(departmentEntities.Select(x => (int)x.Id).ToList(), booked: booked, year)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}

				// - Departments
				ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(new List<int> { userEntity.DepartmentId ?? -1 }, booked: booked, year)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());

				// - User
				ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUser(user.Id, booked: booked, year)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
			}

			if(ordersExtensionEntities == null || ordersExtensionEntities.Count <= 0)
			{
				return new List<Models.Budget.Order.OrderModel>();
			}
			ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();

			return Helpers.Processings.Budget.Order.GetOrderModels(ordersExtensionEntities, user, out errors);
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetOrderExtensions(Identity.Models.UserModel user, bool? booked, out List<string> errors, int? year, bool ignoreSpecialAccess=false)
		{
			errors = new List<string>();

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(user.Id);
			var ordersExtensionEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			var userProfilEntity = new Models.Administration.AccessProfile.AccessProfileModel(
				Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(
					Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { user.Id })
					?.Select(x => x.AccessProfileId)
					?.ToList()));

			if(!ignoreSpecialAccess && (userEntity.SuperAdministrator || userEntity.IsGlobalDirector.HasValue && userEntity.IsGlobalDirector.Value == true
				|| (userProfilEntity?.CommandeExternalViewAllGroup == true && userProfilEntity?.CommandeInternalViewAllGroup == true)))
			{
				ordersExtensionEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Get(year, booked: booked);
			}
			else
			{
				if(!ignoreSpecialAccess && userProfilEntity?.CommandeExternalViewAllGroup == true)
				{
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetExternal(booked: booked, year)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}
				if(!ignoreSpecialAccess && userProfilEntity?.CommandeInternalViewAllGroup == true)
				{
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetInternal(booked: booked, year)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}

				var siteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { userEntity.Id });
				if(siteEntities != null && siteEntities.Count > 0)
				{
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(siteEntities.Select(x => x.Id).ToList(), booked: booked, year)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}

				// - User Purchase of site
				var pSiteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get((long)(userEntity.CompanyId ?? -1));
				if(pSiteEntities != null)
				{
					var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(pSiteEntities.Id);
					if(Helpers.Processings.Budget.HasPurchaseProfile(userEntity.Id, companyExtension))
					{
						ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(new List<int> { companyExtension.CompanyId }, booked: booked, year)
								?.Where(x => x.ApprovalUserId.HasValue)?.ToList()
								?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}
				}

				var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(userEntity.Id);
				if(departmentEntities != null && departmentEntities.Count > 0)
				{
					ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(departmentEntities.Select(x => (int)x.Id).ToList(), booked: booked, year)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}

				// - Departments
				ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(new List<int> { userEntity.DepartmentId ?? -1 }, booked: booked, year)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());

				// - User
				ordersExtensionEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUser(user.Id, booked: booked, year)
					?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
			}

			if(ordersExtensionEntities == null || ordersExtensionEntities.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
			ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();

			return ordersExtensionEntities;
		}
	}
}
