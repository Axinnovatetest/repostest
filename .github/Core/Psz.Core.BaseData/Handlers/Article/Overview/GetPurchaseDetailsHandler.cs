using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class GetPurchaseDetailsHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public GetPurchaseDetailsHandler(Identity.Models.UserModel user, int articleId)
		{
			this._user = user;
			this._data = articleId;
		}

		public ResponseModel<Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel
				{
					ArticleId = this._data
				};
				// -
				var orderEntity = Infrastructure.Data.Access.Tables.BSD.BestellnummernAccess.GetByArticle(this._data, true);
				if(orderEntity != null && orderEntity.Count > 0)
				{
					var supplierAddressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(orderEntity[0]?.Lieferanten_Nr ?? -1);
					responseBody.OrderNumber = orderEntity[0]?.Bestell_Nr;
					responseBody.StandardPurchasePrice = orderEntity[0]?.Einkaufspreis.HasValue == true
						? Convert.ToDecimal(orderEntity[0]?.Einkaufspreis.Value)
						: null;
					responseBody.StandardSupplierId = orderEntity[0]?.Lieferanten_Nr ?? -1;
					responseBody.StandardSupplierName = supplierAddressEntity?.Name1;
				}

				// -
				return ResponseModel<Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel> Validate()
		{
			if(this._user == null/* this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data) == null)
				return ResponseModel<Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel>.FailureResponse("Article not found");

			// -
			return ResponseModel<Models.Article.ArticleOverviewModel.PurchaseDetailsResponseModel>.SuccessResponse();
		}
	}
}
