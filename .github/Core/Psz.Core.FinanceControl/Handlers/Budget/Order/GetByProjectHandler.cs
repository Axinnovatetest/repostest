using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using MoreLinq;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetByProjectHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetByProjectHandler(Identity.Models.UserModel user, int idProject)
		{
			this._user = user;
			this._data = idProject;
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
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data);

				var ordersEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(projectEntity.Id);
				if(ordersEntities == null)
				{
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
				}

				var orderIds = ordersEntities.Select(x => x.OrderId)?.ToList();
				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(orderIds);

				Helpers.Processings.Budget.Order.updateArticlePrices(orderIds); // - HEAVY processing

				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);
				var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntites?.Select(x => x.BestellteArtikelNr)?.ToList());
				var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(orderIds);

				var response = new List<Models.Budget.Order.OrderModel>();

				ordersEntities = ordersEntities.OrderBy(x => x.Id, OrderByDirection.Descending)?.ToList();
				foreach(var orderEntity in ordersEntities)
				{
					var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
					var articleEntity = articleEntites?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();
					var articlesEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntity?.Select(x => x.ArticleId)?.ToList());
					var bestellteArticelEntity = bestellteArticleEntities?.FindAll(x => articleEntity.Select(y => y.BestellteArtikelNr)?.ToList()?.Exists(a => a == x.Nr) == true)?.ToList();
					var fileEntity = fileEntities?.FindAll(x => x.Id_Order == orderEntity.OrderId);

					// Validators
					var validators = Validators.getByOrderId(orderEntity.OrderId, out List<string> errors);
					if(errors != null && errors.Count > 0)
						ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse(string.Join(", ", errors));

					var uValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validators?.Select(x => x.Id_Validator)?.ToList());
					var orderCompany = companyExtensionEntities.Find(x => x.CompanyId == orderEntity.CompanyId);

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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse("Project not found");

			return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
		}
	}
}
