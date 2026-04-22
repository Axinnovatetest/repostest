using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetProjectMessageHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.ControllingAnalysis.ProjectMessageRequestModel _data { get; set; }
		public GetProjectMessageHandler(UserModel user, Models.Article.Statistics.ControllingAnalysis.ProjectMessageRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel>.SuccessResponse(GetData(this._data));
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel>.SuccessResponse();
		}

		public static Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel GetData(Models.Article.Statistics.ControllingAnalysis.ProjectMessageRequestModel data)
		{
			var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
			{
				FirstRowNumber = data.ItemsPerPage > 0 ? (data.RequestedPage * data.ItemsPerPage) : 0,
				RequestRows = data.ItemsPerPage
			};

			Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
			var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetProjectMessage(data.ArticleNumber, data.ProjectNumber, dataSorting, dataPaging);

			if(statisticsEntities != null && statisticsEntities.Count > 0)
			{
				var updatedResults = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.UpdateProjectMessage(statisticsEntities?.Select(x => x.ID)?.ToList());

				var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.ControllingAnalysis.GetProjectMessage_Count(data.ArticleNumber, data.ProjectNumber);

				return new Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel
				{
					AllCount = allCount,
					AllPagesCount = (int)Math.Ceiling(((decimal)allCount) / data.ItemsPerPage),
					ItemsPerPage = data.ItemsPerPage,
					RequestedPage = data.RequestedPage,
					data = updatedResults.Select(x => new Models.Article.Statistics.ControllingAnalysis.ProjectMessageItemModel(x))?.ToList()
				};
			}

			return new Models.Article.Statistics.ControllingAnalysis.ProjectMessageResponseModel
			{
				AllCount = 0,
				AllPagesCount = 0,
				ItemsPerPage = data.ItemsPerPage,
				RequestedPage = data.RequestedPage,
				data = null
			};
		}
	}
}
