using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetOpenFaEsdHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Basics.OpenFaEsdResponseModel>>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		public GetOpenFaEsdHandler(UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.Basics.OpenFaEsdResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetOpenFaEsd(this._data);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Basics.OpenFaEsdResponseModel>>.SuccessResponse(
						statisticsEntities.Select(x => new Models.Article.Statistics.Basics.OpenFaEsdResponseModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Basics.OpenFaEsdResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Basics.OpenFaEsdResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Basics.OpenFaEsdResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.Basics.OpenFaEsdResponseModel>>.SuccessResponse();
		}
	}
}
