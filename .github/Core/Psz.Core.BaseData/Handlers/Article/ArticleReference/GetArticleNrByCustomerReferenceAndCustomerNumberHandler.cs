using Infrastructure.Data.Entities.Tables.BSD;
using Psz.Core.BaseData.Models.Article.ArticleReference;
using Psz.Core.SharedKernel.Interfaces;
using System;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Linq;
using Psz.Core.BaseData.Models.Article.Statistics.CustomerService;

namespace Psz.Core.BaseData.Handlers.Article.ArticleReference
{
	public class GetArticleNrByCustomerReferenceAndCustomerNumberHandler: IHandle<UserModel, ResponseModel<ArticleInformationMinimalModel>>
	{
		private UserModel _user { get; set; }
		public GetLikeCustomerArticleReferenceRequestModel _data { get; set; }
		public GetArticleNrByCustomerReferenceAndCustomerNumberHandler(UserModel user, GetLikeCustomerArticleReferenceRequestModel Filter)
		{
			this._user = user;
			this._data = Filter;
		}
		public ResponseModel<ArticleInformationMinimalModel> Handle()
		{

			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var dataentity = new ArtikelCustomerReferencesAndCustomerIDLikeEntity() { CustomerId = _data.CustomerId, CustomerReference = _data.CustomerReference, supplierName = _data.supplierName };
				var dbentity = Infrastructure.Data.Access.Tables.BSD.ArtikelCustomerReferencesAccess.GetArticleNrByReferencesAndCustomerIDs(dataentity);

				if(dbentity is null)
				{
					return ResponseModel<ArticleInformationMinimalModel>.NotFoundResponse();
				}
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(dbentity.ArticleId ?? -1);
				return ResponseModel<ArticleInformationMinimalModel>.SuccessResponse(new ArticleInformationMinimalModel() { ArticleId = dbentity.ArticleId, Bezeichnung1 = articleEntity.Bezeichnung1 });

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<ArticleInformationMinimalModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<ArticleInformationMinimalModel>.AccessDeniedResponse();
			}
			return ResponseModel<ArticleInformationMinimalModel>.SuccessResponse();
		}

	}
}
