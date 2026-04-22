using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Technic
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetQuickAreaBestand_TNHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>>>
	{
		private UserModel _user { get; set; }
		public GetQuickAreaBestand_TNHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Technic.GetQuickAreaBestand_TN();
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Technic.QuickAreaBestandResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Technic.QuickAreaBestandResponseModel>>.SuccessResponse();
		}
	}
}
