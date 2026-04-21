using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetVKSimulationInHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.VKSimulationInRequestModel _data { get; set; }
		public GetVKSimulationInHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.VKSimulationInRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel>>.SuccessResponse(GetData(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel>>.SuccessResponse();
		}

		public static List<Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel> GetData(Models.Article.Statistics.ControllingAnalysis.VKSimulationInRequestModel data)
		{
			var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetVKSimulationData(data.ArticleNumber, data.Anteil)
				?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_VKSimulationInData>();

			return statisticsEntities.Select(x =>
			new Models.Article.Statistics.ControllingAnalysis.VKSimulationInResponseModel(x)).ToList();
		}
	}
}
