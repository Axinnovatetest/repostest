using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	public class UpdateArticlesOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.OrderModel2 _data { get; set; }

		public UpdateArticlesOrderHandler(Identity.Models.UserModel user, Models.Budget.Order.OrderModel2 model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Id_Order);
				Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.DeleteByOrderId(this._data.Id_Order);
				Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.DeleteByOrderId(this._data.Id_Order);

				var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity?.CompanyId ?? -1);
				var currencyEntity = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(orderEntity?.CurrencyId ?? -1)
					?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
				var n = 1;

				foreach(var item in this._data.Articles)
				{
					item.Id_Order = this._data.Id_Order;
					item.Location_Id = orderEntity?.StorageLocationId ?? -1;
					item.Location_Name = orderEntity?.StorageLocationName;
					item.Position = n;

					item.Id_Currency_Article = orderEntity?.CurrencyId;
					item.Currency_Article = orderEntity?.CurrencyName;

					item.DefaultCurrencyId = companyExtensionEntity?.DefaultCurrencyId;
					item.DefaultCurrencyName = companyExtensionEntity?.DefaultCurrencyName;
					item.DefaultCurrencyDecimals = currencyEntity?.Dezimalstellen;
					item.DefaultCurrencyRate = (decimal?)currencyEntity?.entspricht_DM;
					if(item.Id_Currency_Article > 0
						&& item.DefaultCurrencyId > 0
						&& item.Id_Currency_Article != item.DefaultCurrencyId)
					{
						item.UnitPriceDefaultCurrency = (bool)currencyEntity?.entspricht_DM.HasValue && currencyEntity?.entspricht_DM.Value > 0
							? item.Unit_Price * 1 / (decimal?)currencyEntity?.entspricht_DM
							: 1;
						item.TotalCostDefaultCurrency = item.UnitPriceDefaultCurrency * item.Quantity;
					}
					// - -- -
					var id = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Insert(item.ToBestellteArtikelEntity());
					var bestellteItem = item.ToBestellteExtensionEntity();
					bestellteItem.BestellteArtikelNr = id;
					Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Insert(bestellteItem);
					n++;
				}

				// Update Order
				return ResponseModel<int>.SuccessResponse();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Id_Order) == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
