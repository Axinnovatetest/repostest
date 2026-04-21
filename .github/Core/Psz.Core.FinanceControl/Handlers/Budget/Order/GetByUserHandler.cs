using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetByUserHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private bool? _data { get; set; }
		public GetByUserHandler(Identity.Models.UserModel user, bool? showCompletelyBooked)
		{
			this._user = user;
			this._data = showCompletelyBooked;
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

				var ordersExtensionEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUser(this._user.Id, booked: this._data);
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

				var response = new List<Models.Budget.Order.OrderModel>();
				Infrastructure.Data.Entities.Tables.FNC.AdressenEntity adressenCustomerEntity = null;

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

		public static List<Models.Budget.Order.OrderModel> GetOrders(Identity.Models.UserModel user, bool? booked, out List<string> errors)
		{
			errors = new List<string>();

			var ordersExtensionEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUser(user.Id, booked: booked);
			if(ordersExtensionEntities == null || ordersExtensionEntities.Count <= 0)
			{
				return new List<Models.Budget.Order.OrderModel>();
			}
			ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();

			return Helpers.Processings.Budget.Order.GetOrderModels(ordersExtensionEntities, user, out errors);
		}
		public static List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> GetOrderExtensions(Identity.Models.UserModel user, bool? booked, out List<string> errors)
		{
			errors = new List<string>();

			var ordersExtensionEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByUser(user.Id, booked: booked);
			if(ordersExtensionEntities == null || ordersExtensionEntities.Count <= 0)
			{
				return new List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity>();
			}
			ordersExtensionEntities = ordersExtensionEntities?.DistinctBy(x => x.Id)?.ToList();

			return ordersExtensionEntities;
		}
	}
}
