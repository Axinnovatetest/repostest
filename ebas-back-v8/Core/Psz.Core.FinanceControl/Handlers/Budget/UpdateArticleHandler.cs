using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateArticleHandler: IHandle<Models.Budget.GetArticlesModel, ResponseModel<int>>
	{
		private Models.Budget.GetArticlesModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateArticleHandler(Models.Budget.GetArticlesModel data, Identity.Models.UserModel user)
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

				var Articleentity = new Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity()
				{
					Article_code = _data.Article_code,
					Article_designation1 = _data.Article_designation1,
					Article_designation2 = _data.Article_designation2,
					Article_number = _data.Article_number,
					Article_supplier = _data.Article_supplier,
					Creator_Bind = _data.Creator_Bind,
					Description = _data.Description,
					Editor_Bind = _user.Id,
					Id_Currency = _data.Id_Currency,
					Unit_Price = _data.Unit_Price,

				};
				var updatedArticles = Infrastructure.Data.Access.Tables.FNC.Budget_ArticleAccess.Update(Articleentity);
				return ResponseModel<int>.SuccessResponse(updatedArticles);
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
			var ArticleID = Infrastructure.Data.Access.Tables.FNC.Budget_ArticleAccess.Get(this._data.Article_number);
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(ArticleID == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Article not found"}
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
