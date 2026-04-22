using System.Linq;
using Psz.Core.Apps.Budget.Handlers;
using Psz.Core.Logistics.Models.InverntoryStockModels;
using static Psz.Core.Logistics.Models.InverntoryStockModels.GetLogRequestModel;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class getLogsHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetLogResponseModel>>
	{
		private GetLogRequestModel _request;
		private Core.Identity.Models.UserModel _user;
		public getLogsHandler(GetLogRequestModel request, Core.Identity.Models.UserModel user)
		{
			_request = request;
			_user = user;
		}
		public ResponseModel<GetLogResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var DataIstList = new List<Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity>();
				string searchValue = this._request.SearchValue;

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "id":
							sortFieldName = "[Id]";
							break;
						case "msgtype":
							sortFieldName = "[LogDescription]";
							break;
						case "logtime":
							sortFieldName = "[LogTime]";
							break;
						case "loguserid":
							sortFieldName = "[LogUserId]";
							break;
						case "objectid":
							sortFieldName = "[ObjectId]";
							break;
						case "objectname":
							sortFieldName = "[ObjectName]";
							break;

					}
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
				}
				int totalCount = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.CountLogsData(searchValue, _request.LagerId ?? -1);
				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._request.PageSize > 0 ? (this._request.RequestedPage * this._request.PageSize) : 0,
					RequestRows = this._request.FullData ? totalCount : this._request.PageSize

				};

				DataIstList = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.GetLogsData(searchValue,_request.LagerId ?? -1, dataSorting, dataPaging);

				var r = this._request.PageSize > 0 ? totalCount / decimal.Parse((this._request.PageSize).ToString()) : 0;
				var result = DataIstList.Select(x => new GetLogItemResponseModel(x)).ToList();

				return ResponseModel<GetLogResponseModel>.SuccessResponse(new GetLogResponseModel
				{
					Items = result,
					PageRequested = this._request.RequestedPage,
					PageSize = this._request.PageSize,
					TotalCount = totalCount,
					TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<GetLogResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<GetLogResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<GetLogResponseModel>.SuccessResponse();
		}
	}
}
