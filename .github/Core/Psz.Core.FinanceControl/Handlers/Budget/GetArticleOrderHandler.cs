using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetArticleOrderHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetArticleOrderModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetArticleOrderHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.GetArticleOrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetArticleOrderModel>();
				var artikel_order_tableEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.GetArtikelOrder();


				foreach(var artikel_order_tableEntity in artikel_order_tableEntities)
				{
					responseBody.Add(new Models.Budget.GetArticleOrderModel(artikel_order_tableEntity));
				}

				return ResponseModel<List<Models.Budget.GetArticleOrderModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.GetArticleOrderModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.GetArticleOrderModel>>.SuccessResponse();
		}
	}
}
