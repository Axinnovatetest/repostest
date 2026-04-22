using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Logging
{
	public class GetFaLogsHandler: IHandle<Identity.Models.UserModel, ResponseModel<CTSLoggingResponseModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private CTSLoggingRequestModel _data { get; set; }
		public GetFaLogsHandler(Identity.Models.UserModel user, CTSLoggingRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<CTSLoggingResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
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
						case "AngebotNr":
							sortFieldName = "[AngebotNr]";
							break;
						case "ProjektNr":
							sortFieldName = "[ProjektNr]";
							break;
						case "DateTime":
							sortFieldName = "[DateTime]";
							break;
						case "Username":
							sortFieldName = "[Username]";
							break;
						case "LogObject":
							sortFieldName = "[LogObject]";
							break;
						case "LogText":
							sortFieldName = "[LogText]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._data.SortDesc,
					};
				}
				#endregion

				CTSLoggingResponseModel response = new CTSLoggingResponseModel();
				var FALogs = Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Get(this._data.SearchTerms, this._data.ObjectType, dataSorting, dataPaging);
				if(FALogs != null && FALogs.Count > 0)
				{
					var count = Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Get_Count(this._data.SearchTerms, this._data.ObjectType);
					response.Items = FALogs.Select(x => new CTSLoggingModel(x)).OrderByDescending(y => y.DateTime).ToList();
					response.PageRequested = this._data.RequestedPage;
					response.PageSize = this._data.PageSize;
					response.TotalPageCount = (int)Math.Ceiling(((decimal)count) / this._data.PageSize);
					response.TotalCount = count;
				}

				return ResponseModel<CTSLoggingResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<CTSLoggingResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<CTSLoggingResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<CTSLoggingResponseModel>.SuccessResponse();
		}
	}
}
