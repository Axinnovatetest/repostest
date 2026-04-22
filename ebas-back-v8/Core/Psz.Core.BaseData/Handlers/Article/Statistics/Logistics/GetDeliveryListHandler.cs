using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Logistics
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class GetDeliveryListHandler: IHandle<UserModel, ResponseModel<Models.Article.Statistics.Logistics.DeliveryListResponseModel>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Statistics.Logistics.DeliveryListRequestModel _data { get; set; }
		public GetDeliveryListHandler(UserModel user, Models.Article.Statistics.Logistics.DeliveryListRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Models.Article.Statistics.Logistics.DeliveryListResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var statisticsEntities = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetDeliveryList(this._data.searchTerm, this._data.RequestedPage, this._data.ItemsPerPage);
				if(statisticsEntities != null && statisticsEntities.Count > 0)
				{
					var allCount = Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Logistics.GetDeliveryList_count(this._data.searchTerm);
					var results = new List<Models.Article.Statistics.Logistics.DeliveryListResponseModel>();
					return ResponseModel<Models.Article.Statistics.Logistics.DeliveryListResponseModel>.SuccessResponse(
						new Models.Article.Statistics.Logistics.DeliveryListResponseModel
						{
							AllCount = allCount,
							AllPagesCount = (int)Math.Ceiling((decimal)allCount / this._data.ItemsPerPage),
							ItemsPerPage = this._data.ItemsPerPage,
							RequestedPage = this._data.RequestedPage,
							DeliveryLists = statisticsEntities.Select(x => new Models.Article.Statistics.Logistics.LogisticsDeliveryListModel(x)).ToList()
						});
				}

				return ResponseModel<Models.Article.Statistics.Logistics.DeliveryListResponseModel>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<Models.Article.Statistics.Logistics.DeliveryListResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Article.Statistics.Logistics.DeliveryListResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<Models.Article.Statistics.Logistics.DeliveryListResponseModel>.SuccessResponse();
		}
	}
}
