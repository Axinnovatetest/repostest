using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetPricesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.ArtikelBudgetModel.Supplier>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetPricesHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Budget.ArtikelBudgetModel.Supplier>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 

				return ResponseModel<List<Models.Budget.ArtikelBudgetModel.Supplier>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.FNC.BestellnummernAccess.GetByArticles(new List<int> { this._data })
					?.Select(x => new Models.Budget.ArtikelBudgetModel.Supplier(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.ArtikelBudgetModel.Supplier>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.ArtikelBudgetModel.Supplier>>.AccessDeniedResponse();
			}

			// - 
			return ResponseModel<List<Models.Budget.ArtikelBudgetModel.Supplier>>.SuccessResponse();
		}
	}
}
