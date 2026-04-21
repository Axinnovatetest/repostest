using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models;

namespace Psz.Core.CRP.Handlers.Statistics
{
	public partial class CrpStatisticsService: ICrpStatisticsService
	{
		public ResponseModel<OrderProcessingSearchLogsModel> GetOrderProcessingLogs(Identity.Models.UserModel user, OPSearchLogsModel data)
		{
			var validationResponse = this.ValidateGetOrderProcessingLogs(user);
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			try
			{
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.ItemsPerPage > 0 ? (data.RequestedPage * data.ItemsPerPage) : 0,
					RequestRows = data.ItemsPerPage
				};
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(data.SortFieldKey.ToLower())
					{
						default:

						case "vorfallnr":
							sortFieldName = "[Vorfall-Nr]";
							break;
						case "doknr":
							sortFieldName = "[DokNr]";
							break;
						case "pos":
							sortFieldName = "[Pos]";
							break;
						case "artikelnummer":
							sortFieldName = "[PSZ#]";
							break;
						case "user":
							sortFieldName = "[User]";
							break;
						case "typ":
							sortFieldName = "TYP";
							break;
						case "log":
							sortFieldName = "[Log]";
							break;
						case "datetime":
							sortFieldName = "[DateTime]";
							break;

					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				var response = new List<OrderProcessingLogsModel>();
				var PackingListEntity = Infrastructure.Data.Access.Joins.CRP.CRPStatisticsAccess.GetOrderProcessingLogs(
					dataSorting,
					dataPaging, data.SearchValueVorfallNr, data.SearchValuePosition, data.SearchValueartikelnummer, data.SearchValueUsername
					, data.ListSearchType);
				int allCount = Infrastructure.Data.Access.Joins.CRP.CRPStatisticsAccess.GetOrderProcessingLogs_count(data.SearchValueVorfallNr, data.SearchValuePosition, data.SearchValueartikelnummer, data.SearchValueUsername
					, data.ListSearchType);

				if(PackingListEntity != null && PackingListEntity.Count > 0)
				{
					response = PackingListEntity.Select(k => new OrderProcessingLogsModel(k, allCount)).ToList();
				}

				return ResponseModel<OrderProcessingSearchLogsModel>.SuccessResponse(
						new OrderProcessingSearchLogsModel()
						{
							OrderProcessingLogsList = response,
							RequestedPage = data.RequestedPage,
							ItemsPerPage = data.ItemsPerPage,
							AllCount = allCount > 0 ? allCount : 0,
							AllPagesCount = data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / data.ItemsPerPage)) : 0,
						});
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw new NotImplementedException();
			}

		}
		public ResponseModel<OrderProcessingSearchLogsModel> ValidateGetOrderProcessingLogs(Identity.Models.UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<OrderProcessingSearchLogsModel>.AccessDeniedResponse();
			}

			return ResponseModel<OrderProcessingSearchLogsModel>.SuccessResponse();
		}
	}
}