using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Microsoft.VisualBasic;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetAllByArchivedUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Psz.Core.FinanceControl.Models.Budget.Order.Statistics.OrdersArchivedByUserRequestModel _data { get; set; }

		public GetAllByArchivedUserHandler(Identity.Models.UserModel user, Psz.Core.FinanceControl.Models.Budget.Order.Statistics.OrdersArchivedByUserRequestModel data)
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
				var currentUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var companyExtensionEntities = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();

				//var ordersEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUser(this._user.Id, null, isArchived: true);
				// - filter start
				var ordersEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
				var userProfilEntity = new Models.Administration.AccessProfile.AccessProfileModel(
				Infrastructure.Data.Access.Tables.FNC.AccessProfileAccess.Get(
					Infrastructure.Data.Access.Tables.FNC.UserAccessProfilesAccess.GetByUserId(new List<int> { this._user.Id })
					?.Select(x => x.AccessProfileId)
					?.ToList()));

				if(currentUser.SuperAdministrator || currentUser.IsGlobalDirector.HasValue && currentUser.IsGlobalDirector.Value == true
					|| (userProfilEntity?.CommandeExternalViewAllGroup == true && userProfilEntity?.CommandeInternalViewAllGroup == true))
				{
					ordersEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Get(booked: null, isArchived: true);
				}
				else
				{
					if(userProfilEntity?.CommandeExternalViewAllGroup == true)
					{
						ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetExternal(booked: null, isArchived: true)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}
					if(userProfilEntity?.CommandeInternalViewAllGroup == true)
					{
						ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetInternal(booked: null, isArchived: true)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}

					var siteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { currentUser.Id });
					if(siteEntities != null && siteEntities.Count > 0)
					{
						ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(siteEntities.Select(x => x.Id).ToList(), booked: null, isArchived: true)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}

					// - User Purchase of site
					var pSiteEntities = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get((long)(currentUser.CompanyId ?? -1));
					if(pSiteEntities != null)
					{
						var companyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(pSiteEntities.Id);
						if(Helpers.Processings.Budget.HasPurchaseProfile(currentUser.Id, companyExtension))
						{
							ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(new List<int> { companyExtension.CompanyId }, booked: null, isArchived: true)
									?.Where(x => x.ApprovalUserId.HasValue)?.ToList()
									?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
						}
					}

					var departmentEntities = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(currentUser.Id);
					if(departmentEntities != null && departmentEntities.Count > 0)
					{
						ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(departmentEntities.Select(x => (int)x.Id).ToList(), booked: null, isArchived: true)
							?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
					}

					// - Departments
					ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(new List<int> { currentUser.DepartmentId ?? -1 }, booked: null, isArchived: true)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());

					// - User
					ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUser(this._user.Id, booked: null, isArchived: true)
						?? new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>());
				}

				if(ordersEntities == null || ordersEntities.Count <= 0)
				{
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
				}
				ordersEntities = ordersEntities?.DistinctBy(x => x.Id)?.ToList();
				// - filter end

				if(ordersEntities == null || ordersEntities.Count <= 0)
				{
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
				}

				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(ordersEntities.Select(x => x.OrderId)?.ToList());
				var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(ordersEntities.Select(x => x.ProjectId ?? -1)?.ToList());

				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(ordersEntities.Select(x => x.OrderId)?.ToList());
				var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntites?.Select(x => x.BestellteArtikelNr)?.ToList());
				var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(ordersEntities.Select(x => x.OrderId)?.ToList());

				ordersEntities = ordersEntities.Where(x => CompareDate((int)_data.year, x.ArchiveTime) == true).ToList();

				var response = new List<Models.Budget.Order.OrderModel>();
				//Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenCustomerEntity = null;
				ordersEntities = ordersEntities.OrderByDescending(x => x.Id)?.ToList();
				foreach(var orderEntity in ordersEntities)
				{
					var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
					var projectEntity = projectsEntities?.Find(e => e.Id == orderEntity?.ProjectId);
					var articleEntity = articleEntites?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();
					var bestellteArticelEntity = bestellteArticleEntities?.FindAll(x => articleEntity.Select(y => y.BestellteArtikelNr)?.ToList()?.Exists(a => a == x.Nr) == true)?.ToList();
					//
					var articlesEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntity?.Select(x => x.ArticleId)?.ToList());
					var fileEntity = fileEntities?.FindAll(x => x.Id_Order == orderEntity.OrderId);

					// Validators
					var validators = Validators.getByOrderId(orderEntity.OrderId, out List<string> errors, true);
					if(errors != null && errors.Count > 0)
						ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse(string.Join(", ", errors));

					var uValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validators?.Select(x => x.Id_Validator)?.ToList());
					var orderCompany = companyExtensionEntities.Find(x => x.CompanyId == orderEntity.CompanyId);

					// Handle last VALIDATOR as Profile NOT User
					response.Add(new Models.Budget.Order.OrderModel(orderEntity, bestellungEntity, projectEntity, articleEntity, bestellteArticelEntity, articlesEntities, fileEntity, validators, uValidators,
						this._user,
						Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId_Count(orderEntity.OrderId)));
				}

				return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private bool CompareDate(int year, DateTime? time)
		{
			if(time is null)
				return false;

			return time.Value.Year == year;
		}
		public ResponseModel<List<Models.Budget.Order.OrderModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
		}
	}


}
