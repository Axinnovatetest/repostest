using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetHighRunnerHandler: IHandle<UserModel, ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.HighRunnerResponseModel>>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.HighRunnerRequestModel _data { get; set; }
		public GetHighRunnerHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.HighRunnerRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.HighRunnerResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.HighRunnerResponseModel>>.SuccessResponse(GetData(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.HighRunnerResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.HighRunnerResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.HighRunnerResponseModel>>.SuccessResponse();
		}

		public static List<Models.Article.Statistics.ControllingAnalysis.HighRunnerResponseModel> GetData(Models.Article.Statistics.ControllingAnalysis.HighRunnerRequestModel data)
		{

			return (Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetHighRunners(data.CustomerNumber, data.DateFrom, data.DateTill)
				?? new List<Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.ControllingAnalysis_HighRunner>())
				.Select(x => new Models.Article.Statistics.ControllingAnalysis.HighRunnerResponseModel(x))
				?.ToList();

		}
	}
}
