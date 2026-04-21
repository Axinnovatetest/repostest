using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order.Article
{
	public class UpdateDeliveryHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.Article.UpdateDeliveryModel _data { get; set; }

		public UpdateDeliveryHandler(Identity.Models.UserModel user, Models.Budget.Order.Article.UpdateDeliveryModel model)
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


				var articleExtEntity = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Get(this._data.ArticleId);
				var articleEntity = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleExtEntity?.BestellteArtikelNr ?? -1);
				//var articleEntity = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.GetByOrderIdAndArticleId(articleExtEntity?.OrderId ?? -1, articleExtEntity?.ArticleId ?? -1);

				if(articleEntity != null)
					Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Update(this._data.ToEntity(articleEntity));

				if(articleExtEntity != null)
					Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Update(this._data.ToExtensionEntity(articleExtEntity));

				return ResponseModel<int>.SuccessResponse(1);
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

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data.OrderId);
			if(orderEntity == null)
				return ResponseModel<int>.FailureResponse("Order not found");

			if(Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.Get(this._data.ArticleId) == null)
				return ResponseModel<int>.FailureResponse("Item not found");

			var companyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(orderEntity.CompanyId ?? -1);
			if(!this._user.IsGlobalDirector && this._user.Id != orderEntity.IssuerId
				&& companyExtensionEntity != null && companyExtensionEntity.PurchaseProfileId.HasValue && Helpers.Processings.Budget.HasPurchaseProfile(this._user.Id, companyExtensionEntity) != true)
				return ResponseModel<int>.FailureResponse("User cannot update delivery data");


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
