using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class GetAllDataArticleHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.AllBudgetArticleModel>>>
	{
		public Identity.Models.UserModel _user { get; set; }
		public GetAllDataArticleHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Budget.AllBudgetArticleModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var responseBody = new List<Models.Budget.AllBudgetArticleModel>();
				var alldataarticles_tableEntities = Infrastructure.Data.Access.Tables.FNC.Budget_ArticleAccess.GetAllDataArticle();


				foreach(var alldataarticle_tableEntity in alldataarticles_tableEntities)
				{
					responseBody.Add(new Models.Budget.AllBudgetArticleModel(alldataarticle_tableEntity));
				}

				return ResponseModel<List<Models.Budget.AllBudgetArticleModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Budget.AllBudgetArticleModel>> Validate()
		{

			return ResponseModel<List<Models.Budget.AllBudgetArticleModel>>.SuccessResponse();

		}
	}
}
