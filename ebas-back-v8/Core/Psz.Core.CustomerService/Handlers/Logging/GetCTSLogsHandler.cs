using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Logging
{
	public class GetCTSLogsHandler: IHandle<Identity.Models.UserModel, ResponseModel<CTSLogResponseModel>>
	{
		private CSTLogSearchModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetCTSLogsHandler(Identity.Models.UserModel user, CSTLogSearchModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<CTSLogResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.ItemsPerPage > 0 ? (this._data.RequestedPage * this._data.ItemsPerPage) : 0,
					RequestRows = this._data.ItemsPerPage
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(this._data.SortFieldKey))
				{
					var sortFieldName = "";
					switch(this._data.SortFieldKey.ToLower())
					{
						default:
						case "angebotnr":
							sortFieldName = "[AngebotNr]";
							break;
						case "projektnr":
							sortFieldName = "[ProjektNr]";
							break;
						case "datetime":
							sortFieldName = "[DateTime]";
							break;
						case "username":
							sortFieldName = "[Username]";
							break;
						case "logobject":
							sortFieldName = "[LogObject]";
							break;
						case "logtext":
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
				var dataTypes = new List<string>();
				if(_data.Types != null)
				{
					foreach(var typeInt in _data.Types)
					{
						dataTypes.Add(Enums.OrderEnums.TypeToData((Enums.OrderEnums.Types)typeInt));
					}
				}
				var logs = new List<CTSLogModel>();
				int allCount = 0;
				var LogEntities = Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Search(
				   this._data.ProjectNr,
				   this._data.VorfallNr,
				   this._data.User,
				   dataTypes,
				   dataSorting,
				   dataPaging);

				if(LogEntities != null && LogEntities.Count > 0)
				{
					allCount = Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.SearchCount(
				   this._data.ProjectNr,
				   this._data.VorfallNr,
				   this._data.User,
				   dataTypes
				   );
					logs = LogEntities.Select(x => new CTSLogModel(x)).ToList();
				}
				var response = new Models.CTSLogResponseModel()
				{
					CTSLog = logs,
					RequestedPage = this._data.RequestedPage,
					ItemsPerPage = this._data.ItemsPerPage,
					AllCount = allCount > 0 ? allCount : 0,
					AllPagesCount = this._data.ItemsPerPage > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / this._data.ItemsPerPage)) : 0,
				};

				return ResponseModel<CTSLogResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);

				throw;
			}
		}
		public ResponseModel<CTSLogResponseModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<CTSLogResponseModel>.AccessDeniedResponse();
			}
			return ResponseModel<CTSLogResponseModel>.SuccessResponse();
		}
	}
}
