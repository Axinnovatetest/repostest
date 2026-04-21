using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Logistics
{
	using Psz.Core.BaseData.Models.Article.Statistics.Logistics;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetFaStatusHandler: IHandle<UserModel, ResponseModel<List<FaStatusResponseModel>>>
	{
		private UserModel _user { get; set; }
		private FaStatusRequestModel _data { get; set; }

		public GetFaStatusHandler(UserModel user, FaStatusRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<FaStatusResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetFaStatus(this._data.ArticleNr, this._data.FaStatus, this._data.SearchValue);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<FaStatusResponseModel>>.SuccessResponse(statisticsEntities
							.Select(x => new FaStatusResponseModel(x)).ToList());
				}

				return ResponseModel<List<FaStatusResponseModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<FaStatusResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<FaStatusResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleNr) == null)
				return ResponseModel<List<FaStatusResponseModel>>.FailureResponse("Article not found.");

			return ResponseModel<List<FaStatusResponseModel>>.SuccessResponse();
		}
	}
}
