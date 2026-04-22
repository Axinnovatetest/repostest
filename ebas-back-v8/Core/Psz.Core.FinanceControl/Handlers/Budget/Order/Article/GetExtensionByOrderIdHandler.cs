using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetExtensionByOrderIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.Article.ArticleExtensionModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetExtensionByOrderIdHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Budget.Order.Article.ArticleExtensionModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var articleExtEntities = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderId(this._data);

				return ResponseModel<List<Models.Budget.Order.Article.ArticleExtensionModel>>.SuccessResponse(articleExtEntities?.Select(x => new Models.Budget.Order.Article.ArticleExtensionModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.Article.ArticleExtensionModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.Article.ArticleExtensionModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Budget.Order.Article.ArticleExtensionModel>>.FailureResponse("Order not found");

			return ResponseModel<List<Models.Budget.Order.Article.ArticleExtensionModel>>.SuccessResponse();
		}
	}
}
