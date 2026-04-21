using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Sales
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetArticleROHNeed_SupplierStufeHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel>>>
	{
		private UserModel _user { get; set; }
		public GetArticleROHNeed_SupplierStufeHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel>> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//-
				var currCW = Common.Helpers.DateHelpers.ExtractIsoWeek(DateTime.Today);
				return ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetAllSupplierAddresses()
							.Select(x => new Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel(currCW, x)).ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Statistics.Sales.ArticleROHNeedStock_SupplierStufeResponseModel>>.SuccessResponse();
		}
	}
}
