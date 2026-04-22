using System;
using System.Collections.Generic;
namespace Psz.Core.BaseData.Handlers.Article.Statistics.Sales
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetArticleROHNeed_SyncHandler: IHandle<UserModel, ResponseModel<KeyValuePair<int, DateTime>>>
	{
		private UserModel _user { get; set; }
		public GetArticleROHNeed_SyncHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<KeyValuePair<int, DateTime>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var syncData = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Sync();
				if(syncData.Key > 0)
				{
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Sync_PO();
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Sync_FA();
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.GetArticleROHNeedStock_Sync_Details();
				}
				//-
				return ResponseModel<KeyValuePair<int, DateTime>>.SuccessResponse(syncData);
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<KeyValuePair<int, DateTime>> Validate()
		{
			if(this._user == null || !this._user.SuperAdministrator)
			{
				return ResponseModel<KeyValuePair<int, DateTime>>.AccessDeniedResponse();
			}


			return ResponseModel<KeyValuePair<int, DateTime>>.SuccessResponse();
		}
	}
}
