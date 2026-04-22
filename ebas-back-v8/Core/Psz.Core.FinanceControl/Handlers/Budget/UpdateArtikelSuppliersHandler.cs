using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateArtikelSuppliersHandler: IHandle<Models.Budget.ArtikelBudgetModel, ResponseModel<int>>
	{
		private Models.Budget.ArtikelBudgetModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateArtikelSuppliersHandler(Models.Budget.ArtikelBudgetModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
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

				// - 2023-12-07 - separate Suppliers update
				Infrastructure.Data.Access.Tables.FNC.BestellnummernAccess.DeleteByArticle(this._data.Artikel_Nr);
				var bestellnummerEntities = this._data.ToBestellnummernEntities();
				for(int i = 0; i < bestellnummerEntities.Count; i++)
				{
					bestellnummerEntities[i].Artikel_Nr = this._data.Artikel_Nr;
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BestellnummernAccess.Insert(bestellnummerEntities));
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.ConfigEditArtikel)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var ArticleID = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(this._data.Artikel_Nr);
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(ArticleID == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Artikel not found"}
					}
				};
			}
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
