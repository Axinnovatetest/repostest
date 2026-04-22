using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetProductivityHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Basics.ProductivityResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Basics.ProductivityRequestModel _data { get; set; }
		public GetProductivityHandler(UserModel user, Models.Article.Statistics.Basics.ProductivityRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Basics.ProductivityResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> resultsCZ = null;
				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> resultsTN = null;
				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> resultsAL = null;
				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> resultsWS = null;
				List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Productivity> resultsGZ = null;
				// -
				try
				{
					resultsCZ = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivity_CZ(this._data.ArticleNumber);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
				try
				{
					resultsTN = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivity_TN(this._data.ArticleNumber);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
				try
				{
					resultsAL = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivity_AL(this._data.ArticleNumber);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
				try
				{
					resultsWS = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivity_WS(this._data.ArticleNumber);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
				}
				try
				{
					resultsGZ = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Basics.GetProductivity_GZ(this._data.ArticleNumber);
				} catch(Exception ex)
				{
					Infrastructure.Services.Logging.Logger.Log(ex);
				}

				return ResponseModel<Models.Article.Statistics.Basics.ProductivityResponseModel>.SuccessResponse(
					new Models.Article.Statistics.Basics.ProductivityResponseModel()
					{
						CZ = resultsCZ == null || resultsCZ.Count <= 0 ? null : new Models.Article.Statistics.Basics.ProductivityResponseModel.Item(resultsCZ[0]),
						TN = resultsTN == null || resultsTN.Count <= 0 ? null : new Models.Article.Statistics.Basics.ProductivityResponseModel.Item(resultsTN[0]),
						AL = resultsAL == null || resultsAL.Count <= 0 ? null : new Models.Article.Statistics.Basics.ProductivityResponseModel.Item(resultsAL[0]),
						WS = resultsWS == null || resultsWS.Count <= 0 ? null : new Models.Article.Statistics.Basics.ProductivityResponseModel.Item(resultsWS[0]),
						GZ = resultsGZ == null || resultsGZ.Count <= 0 ? null : new Models.Article.Statistics.Basics.ProductivityResponseModel.Item(resultsGZ[0]),
					});
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Basics.ProductivityResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Basics.ProductivityResponseModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArticleNumber) == null)
				return ResponseModel<Models.Article.Statistics.Basics.ProductivityResponseModel>.FailureResponse("Article not found");

			return ResponseModel<Models.Article.Statistics.Basics.ProductivityResponseModel>.SuccessResponse();
		}
	}
}
