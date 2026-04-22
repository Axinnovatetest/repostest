using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Article
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Budget.Configuration.Article.GetResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Configuration.Article.GetParamsModel _data { get; set; }

		public GetHandler(Identity.Models.UserModel user, Models.Budget.Configuration.Article.GetParamsModel model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<Models.Budget.Configuration.Article.GetResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var articleEntites = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.GetAllDataArtikel(
						this._data.SearchTerm,
						this._data.SortFieldKey,
						this._data.SortDesc,
						this._data.SearchSupplierRef,
						this._data.RequestedPage,
						this._data.ItemsPerPage);
				var allCount = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.GetAllDataArtikel_Count(this._data.SearchTerm);

				return ResponseModel<Models.Budget.Configuration.Article.GetResponseModel>.SuccessResponse(
					new Models.Budget.Configuration.Article.GetResponseModel()
					{
						artikels = articleEntites?.Select(x => new Models.Budget.ArtikelBudgetModel(x))?.ToList(),
						RequestedPage = this._data.RequestedPage,
						ItemsPerPage = this._data.ItemsPerPage,
						AllCount = allCount,
						AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Budget.Configuration.Article.GetResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<Models.Budget.Configuration.Article.GetResponseModel>.AccessDeniedResponse();
			}

			// - 
			return ResponseModel<Models.Budget.Configuration.Article.GetResponseModel>.SuccessResponse();
		}
	}
}
