using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class CreateArticleHandler
	{
		private Models.Budget.GetArticlesModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public CreateArticleHandler(Models.Budget.GetArticlesModel data, Identity.Models.UserModel user)
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
				var ArticleEntity = new Infrastructure.Data.Entities.Tables.FNC.Budget_ArticleEntity()
				{

					Article_code = _data.Article_code,
					Article_designation1 = _data.Article_designation1,
					Article_designation2 = _data.Article_designation2,
					Article_number = _data.Article_number,
					Article_supplier = _data.Article_supplier,
					Creator_Bind = _user.Id,
					Description = _data.Description,
					Editor_Bind = _data.Editor_Bind,
					Id_Currency = _data.Id_Currency,
					Unit_Price = _data.Unit_Price,
				};
				var InsertedArticle = Infrastructure.Data.Access.Tables.FNC.Budget_ArticleAccess.InsertArticle(ArticleEntity);
				return ResponseModel<int>.SuccessResponse(InsertedArticle);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null || !this._user.Access.Financial.Budget.ConfigCreateArtikel)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
