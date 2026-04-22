using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class SearchByNameHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.ArtikelBudgetModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Order.Article.SearchByNameModel _data { get; set; }

		public SearchByNameHandler(Identity.Models.UserModel user, Models.Budget.Order.Article.SearchByNameModel model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<List<Models.Budget.ArtikelBudgetModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var articleEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.SearchByName(this._data.Name, this._data.MaxResults, this._data.SupplierId);

				// 
				return ResponseModel<List<Models.Budget.ArtikelBudgetModel>>.SuccessResponse(articleEntities?.Select(x => new Models.Budget.ArtikelBudgetModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.ArtikelBudgetModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.ArtikelBudgetModel>>.AccessDeniedResponse();
			}

			// - 
			return ResponseModel<List<Models.Budget.ArtikelBudgetModel>>.SuccessResponse();
		}
	}
}
