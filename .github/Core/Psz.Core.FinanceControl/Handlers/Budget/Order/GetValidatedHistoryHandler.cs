using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetValidatedHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetValidatedHistoryHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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

				var ordersEntities = new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
				var projectsForValidation = Infrastructure.Data.Access.Tables.FNC.ProjectValidatorsAccess.GetUserProjects(this._user.Id);
				if(projectsForValidation != null && projectsForValidation.Count > 0)
				{
					ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProjectsMaxLevel(projectsForValidation.Select(x => new Tuple<int, int>(x.Id_Project ?? -1, x.Level ?? 0)).ToList(), false, false));
				}

				// - Internal
				var userDirectedDepartments = Infrastructure.Data.Access.Tables.STG.DepartmentAccess.GetByDirectorId(this._user.Id);
				if(userDirectedDepartments != null && userDirectedDepartments.Count > 0)
				{
					ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByDepartments(userDirectedDepartments.Select(x => (int)x.Id).ToList(), null)
						?.Where(x => x.Level <= (int)Enums.BudgetEnums.ValidationLevels.DepartmentDirector));
				}

				var userCompanies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetByDirectorId(new List<int> { this._user.Id });
				if(userCompanies != null && userCompanies.Count > 0)
				{
					ordersEntities.AddRange(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByCompanies(userCompanies.Select(x => (int)x.Id).ToList(), null)
						?.Where(x => x.Level <= (int)Enums.BudgetEnums.ValidationLevels.SiteDirector));
				}

				// - Complementary data
				if(ordersEntities == null || ordersEntities.Count <= 0)
				{
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
				}

				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(ordersEntities.Select(x => x.OrderId)?.ToList());
				var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(ordersEntities.Select(x => x.ProjectId ?? -1)?.ToList());

				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(ordersEntities.Select(x => x.OrderId)?.ToList());
				var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntites?.Select(x => x.BestellteArtikelNr)?.ToList());
				var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(ordersEntities.Select(x => x.OrderId)?.ToList());

				var response = new List<Models.Budget.Order.OrderModel>();
				//Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenCustomerEntity = null;
				foreach(var orderEntity in ordersEntities)
				{
					var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
					var projectEntity = projectsEntities?.Find(e => e.Id == orderEntity?.ProjectId);
					var articleEntity = articleEntites?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();
					var bestellteArticelEntity = bestellteArticleEntities?.FindAll(x => articleEntity.Select(y => y.BestellteArtikelNr)?.ToList()?.Exists(a => a == x.Nr) == true)?.ToList();
					//var locationEntities =;
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
