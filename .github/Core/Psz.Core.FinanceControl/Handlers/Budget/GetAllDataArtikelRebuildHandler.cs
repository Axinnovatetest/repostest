using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetAllDataArtikelRebuildHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.ArtikelBudgetModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetAllDataArtikelRebuildHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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

				var responseBody = new List<Models.Budget.ArtikelBudgetModel>();
				var articleEntites = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.GetAllDataArtikel();
				var supplierBestEntities = Infrastructure.Data.Access.Tables.FNC.BestellnummernAccess.GetByArticles(articleEntites?.Select(x => x.Artikel_Nr)?.ToList());

				foreach(var alldataartikel_tableEntity in articleEntites)
				{
					var supp = supplierBestEntities?.FindAll(x => x.Artikel_Nr == alldataartikel_tableEntity.Artikel_Nr);
					responseBody.Add(new Models.Budget.ArtikelBudgetModel(alldataartikel_tableEntity, supp));
				}

				return ResponseModel<List<Models.Budget.ArtikelBudgetModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.ArtikelBudgetModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.ArtikelBudgetModel>>.SuccessResponse();

		}
	}
}
