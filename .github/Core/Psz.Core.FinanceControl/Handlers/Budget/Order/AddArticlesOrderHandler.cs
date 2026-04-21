using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	public class AddArticlesOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.OrderModel2 _data { get; set; }

		public AddArticlesOrderHandler(Identity.Models.UserModel user, Models.Budget.Order.OrderModel2 model)
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

				/// 

				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.Id_Order);
				var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity?.CompanyId ?? -1);
				var currencyEntity = Infrastructure.Data.Access.Tables.STG.WahrungenAccess.Get(orderEntity?.CurrencyId ?? -1)
					?? new Infrastructure.Data.Entities.Tables.STG.WahrungenEntity { };
				if(this._data.Articles != null && this._data.Articles.Count > 0)
				{
					for(int i = 0; i < this._data.Articles.Count; i++)
					{
						this._data.Articles[i].Id_Order = this._data.Id_Order;
						this._data.Articles[i].Location_Id = orderEntity?.StorageLocationId ?? -1;
						this._data.Articles[i].Location_Name = orderEntity?.StorageLocationName;

						this._data.Articles[i].Id_Currency_Article = orderEntity?.CurrencyId;
						this._data.Articles[i].Currency_Article = orderEntity?.CurrencyName;

						this._data.Articles[i].DefaultCurrencyId = companyExtensionEntity?.DefaultCurrencyId;
						this._data.Articles[i].DefaultCurrencyName = companyExtensionEntity?.DefaultCurrencyName;
						this._data.Articles[i].DefaultCurrencyDecimals = currencyEntity?.Dezimalstellen;
						this._data.Articles[i].DefaultCurrencyRate = (decimal?)currencyEntity?.entspricht_DM;
						if(this._data.Articles[i].Id_Currency_Article > 0
							&& this._data.Articles[i].DefaultCurrencyId > 0
							&& this._data.Articles[i].Id_Currency_Article != this._data.Articles[i].DefaultCurrencyId)
						{
							this._data.Articles[i].UnitPriceDefaultCurrency = (bool)currencyEntity?.entspricht_DM.HasValue && currencyEntity?.entspricht_DM.Value > 0
								? this._data.Articles[i].Unit_Price * 1 / (decimal?)currencyEntity?.entspricht_DM
								: 1;
							this._data.Articles[i].TotalCostDefaultCurrency = this._data.Articles[i].UnitPriceDefaultCurrency * this._data.Articles[i].Quantity;
						}
					}
				}

				// New Articles
				//var artikelEntites = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(this._data.Articles?.Select(x => x.Id_Article)?.ToList());
				//var articles = new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
				var j = 1;
				foreach(var item in this._data.Articles)
				{
					//var artikel = artikelEntites.Find(x => x.Artikel_Nr == item.Id_Article);
					//articles.Add(item.ToBestellteExtensionEntity(artikel));

					// - -- -
					var bestellteArtikelEntity = item.ToBestellteArtikelEntity();
					bestellteArtikelEntity.Position = j;
					//bestellteArtikelEntity.Bestellnummer = orderEntity.OrderNumber;
					var id = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Insert(bestellteArtikelEntity);
					var bestellteItem = item.ToBestellteExtensionEntity();
					bestellteItem.BestellteArtikelNr = id;
					Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Insert(bestellteItem);
					j++;
				}

				// Insert Order
				return ResponseModel<int>.SuccessResponse(this._data.Id_Order);
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
