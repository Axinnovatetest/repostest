using Psz.Core.FinanceControl.Models.Budget;
using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class GetOrdersHandler: IHandle<Identity.Models.UserModel,
		Common.Models.ResponseModel<List<Models.Budget.GetOrdersModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetOrdersHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public Common.Models.ResponseModel<List<GetOrdersModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var ordersEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Get();
				var projectsEntities = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get();
				//var currenciesEntities = Infrastructure.Data.Access.Tables.FNC.Currency_BudgetAccess.Get();

				//var addressesEntities = Infrastructure.Data.Access.Tables.FNC.AdressenAccess.GetWhereLieferantennummerIsNotNull();
				var response = new List<Models.Budget.GetOrdersModel>();

				foreach(var orderEntity in ordersEntities)
				{
					var projectEntity = projectsEntities.Find(e => e.Id == orderEntity.ProjectId);
					//var currencyEntity = currenciesEntities.Find(e => e.IdC == orderEntity.Id_Currency_Order) ;
					//var addressEntity = addressesEntities.Find(e => e.Lieferantennummer == orderEntity.Id_Supplier);
					//var order = new Models.Budget.GetOrdersModel(orderEntity, projectEntity );

					//response.Add(order);
				}


				return ResponseModel<List<Models.Budget.GetOrdersModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public Common.Models.ResponseModel<List<GetOrdersModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<GetOrdersModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetOrdersModel>>.SuccessResponse();
		}
	}
}
