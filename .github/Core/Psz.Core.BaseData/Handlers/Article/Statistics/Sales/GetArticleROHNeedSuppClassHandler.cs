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

	public class GetArticleROHNeedSuppClassHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Sales.SupplierClassResponseModel>>>
	{
		private UserModel _user { get; set; }
		public GetArticleROHNeedSuppClassHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Statistics.Sales.SupplierClassResponseModel>> Handle()
		{
			try
			{

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//-
				return ResponseModel<List<Models.Article.Statistics.Sales.SupplierClassResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetSupplierClass()
							.Select(x => new Models.Article.Statistics.Sales.SupplierClassResponseModel(x))
							.ToList());
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Sales.SupplierClassResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Sales.SupplierClassResponseModel>>.AccessDeniedResponse();
			}


			return ResponseModel<List<Models.Article.Statistics.Sales.SupplierClassResponseModel>>.SuccessResponse();
		}
	}
}
