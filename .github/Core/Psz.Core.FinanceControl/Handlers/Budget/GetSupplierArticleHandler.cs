using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetSupplierArticleHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.GetSupplierArticleModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public string _data { get; set; }
		public GetSupplierArticleHandler(Identity.Models.UserModel user, string search)
		{
			this._user = user;
			this._data = search;
		}
		public ResponseModel<List<Models.Budget.GetSupplierArticleModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.GetSupplierArticleModel>();
				var supplier_article_tableEntities = Infrastructure.Data.Access.Tables.FNC.LieferantenAccess.GetSupplierArticle(this._data);


				foreach(var supplier_article_tableEntity in supplier_article_tableEntities)
				{
					responseBody.Add(new Models.Budget.GetSupplierArticleModel(supplier_article_tableEntity));
				}

				return ResponseModel<List<Models.Budget.GetSupplierArticleModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.GetSupplierArticleModel>> Validate()
		{
			//if (this._user.Access.Purchase.AccessUpdate == true)
			//{

			//}
			return ResponseModel<List<Models.Budget.GetSupplierArticleModel>>.SuccessResponse();
		}
	}
}
