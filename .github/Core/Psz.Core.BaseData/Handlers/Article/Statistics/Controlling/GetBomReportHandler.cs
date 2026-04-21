using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Controlling
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetBomReportHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.Controlling.BomReportModel>>>
	{
		private UserModel _user { get; set; }
		private string _data { get; set; }
		public GetBomReportHandler(UserModel user, string number)
		{
			this._user = user;
			this._data = number;
		}
		public ResponseModel<List<Models.Article.Statistics.Controlling.BomReportModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Controlling.GetBomReport(this._data);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.Controlling.BomReportModel>>.SuccessResponse(statisticsEntities
							.Select(x => new Models.Article.Statistics.Controlling.BomReportModel(x)).ToList());
				}

				return ResponseModel<List<Models.Article.Statistics.Controlling.BomReportModel>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.Controlling.BomReportModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.Controlling.BomReportModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data) == null)
				return ResponseModel<List<Models.Article.Statistics.Controlling.BomReportModel>>.FailureResponse("Article not found.");

			return ResponseModel<List<Models.Article.Statistics.Controlling.BomReportModel>>.SuccessResponse();
		}
	}
}
