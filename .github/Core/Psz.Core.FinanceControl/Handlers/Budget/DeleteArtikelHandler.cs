using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteArtikelHandler: IHandle<Models.Budget.ArtikelBudgetModel, ResponseModel<int>>
	{
		private int _ArticleID { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteArtikelHandler(int articleid, Identity.Models.UserModel user)
		{
			this._ArticleID = articleid;
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

				var artikelentity = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(_ArticleID);
				if(artikelentity == null)
				{
					return ResponseModel<int>.SuccessResponse();
				}

				Infrastructure.Data.Access.Tables.FNC.BestellnummernAccess.DeleteByArticle(artikelentity.Artikel_Nr);
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.DeleteArtikel(artikelentity.Artikel_Nr));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.ConfigDeleteArtikel)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
