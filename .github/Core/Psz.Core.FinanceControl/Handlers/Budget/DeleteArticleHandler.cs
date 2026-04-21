using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteArticleHandler: IHandle<Models.Budget.GetArticlesModel, ResponseModel<int>>
	{
		private int _ArticleID { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteArticleHandler(int articleid, Identity.Models.UserModel user)
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

				var articleentity = Infrastructure.Data.Access.Tables.FNC.Budget_ArticleAccess.Get(_ArticleID);
				if(articleentity == null)
				{
					return ResponseModel<int>.SuccessResponse();
				}

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.Budget_ArticleAccess.Delete(articleentity.Article_number));
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
