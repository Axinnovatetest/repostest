using System;
using System.Collections.Generic;


namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using MoreLinq;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetOrdersbyProjectHandler: IHandle<Identity.Models.UserModel, Common.Models.ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public GetOrdersbyProjectHandler(Identity.Models.UserModel user, int value)
		{
			this._user = user;
			this._data = value;
		}

		public Common.Models.ResponseModel<List<Models.Budget.Order.OrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var currentUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var companyExtensionEntities = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();

				var ordersEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(this._data);

				if(ordersEntities.Count == 0)
				{
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
				}

				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(ordersEntities.Select(x => x.OrderId)?.ToList());
				var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();

				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(ordersEntities.Select(x => x.OrderId)?.ToList());
				var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntites?.Select(x => x.BestellteArtikelNr)?.ToList());
				var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(ordersEntities.Select(x => x.OrderId)?.ToList());

				var response = new List<Models.Budget.Order.OrderModel>();

				ordersEntities = ordersEntities.OrderBy(x => x.Id, OrderByDirection.Descending)?.ToList();
				foreach(var orderEntity in ordersEntities)
				{
					var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
					var projectEntity = projectsEntities?.Find(e => e.Id == orderEntity?.ProjectId);
					var customer = (projectEntity.CustomerId.HasValue && projectEntity.Id_Type != 2)
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
					var orderCompany = companyExtensionEntities.Find(x => x.CompanyId == orderEntity.CompanyId);

					//var addressEntity = addressesEntities?.Find(e => e.Lieferantennummer == orderEntity?.Id_Supplier);
					var order = new Models.Budget.Order.OrderModel(orderEntity, bestellungEntity, projectEntity, articleEntity, bestellteArticelEntity, articlesEntities, fileEntity, validators, uValidators,
						this._user,
						Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId_Count(orderEntity.OrderId));

					response.Add(order);
				}

				return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public Common.Models.ResponseModel<List<Models.Budget.Order.OrderModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(this._data) == null)
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse("Order not found");

			return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
		}
	}
}
