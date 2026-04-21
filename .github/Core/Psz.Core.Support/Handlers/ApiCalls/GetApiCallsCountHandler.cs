namespace Psz.Core.Support.Handlers.ApiCalls
{
	public class GetApiCallsCountHandler: IHandle<Core.Identity.Models.UserModel, ResponseModel<ApiCallCountResponseModel>>
	{
		private readonly UserModel _user;
		private readonly ApiCallCountRequestModel _data;
		public GetApiCallsCountHandler(Core.Identity.Models.UserModel user, ApiCallCountRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<ApiCallCountResponseModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortField))
				{
					var sortFieldName = "";
					switch(this._data.SortField.ToLower())
					{
						default:
						case "api":
							sortFieldName = "Api";
							break;
						case "url":
							sortFieldName = "Url";
							break;
						case "callcountall":
							sortFieldName = "calls_all";
							break;
						case "last 3 months":
							sortFieldName = "calls_last_3_months";
							break;
						case "last 6 months":
							sortFieldName = "calls_last_6_months";
							break;
						case "last 12 months":
							sortFieldName = "calls_last_12_months";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}
				#endregion

				var _list = Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_ApisCallsAccess.GetApiCalls(_data.SearchValue, dataSorting, dataPaging);
				var allCount = Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_ApisCallsAccess.GetApiCalls_Count(_data.SearchValue);

				return ResponseModel<ApiCallCountResponseModel>.SuccessResponse(
					new ApiCallCountResponseModel()
					{
						Items = _list?.Select(x => new ApiCallCountModel(x)).ToList(),
						PageRequested = this._data.RequestedPage,
						PageSize = this._data.PageSize,
						TotalCount = allCount > 0 ? allCount : 0,
						TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.PageSize)) : 0,
					});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<ApiCallCountResponseModel> Validate()
		{
			if(_user == null)
			{
				return ResponseModel<ApiCallCountResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<ApiCallCountResponseModel>.SuccessResponse();
		}
	}
}